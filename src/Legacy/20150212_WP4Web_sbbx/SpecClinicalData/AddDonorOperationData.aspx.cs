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

public partial class SpecClinicalData_AddDonorOperationData : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_DD_OTHER_SELECTION = "Other";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";

    #endregion

    //at load complete
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (Request.QueryString["TID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID.");
                }

                lblDescription.Text = "Add Operation Data for " + Request.QueryString["TID"].ToString();

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                rblHeparin.DataSource = XMLMainOptionsYNDataSource;
                rblHeparin.DataBind();
                //rblHeparin.SelectedValue = STR_UNKNOWN_SELECTION;

                ddSystemicFlushSolutionUsed.DataSource = XmlFlushSolutionsDataSource;
                ddSystemicFlushSolutionUsed.DataBind();

                ddPreservationSolutionColdPerfusion.DataSource = XmlFlushSolutionsDataSource;
                ddPreservationSolutionColdPerfusion.DataBind();

                txtWithdrawlLifeSupportTreatmentDate_CalendarExtender.EndDate = DateTime.Today;
                txtSystolicArterialPressureBelow50Date_CalendarExtender.EndDate = DateTime.Today;
                txtStartNoTouchPeriodDate_CalendarExtender.EndDate = DateTime.Today;
                txtCirculatoryArrestDate_CalendarExtender.EndDate = DateTime.Today;
                txtConfirmationDeathDate_CalendarExtender.EndDate = DateTime.Today;
                txtStartInSituColdPerfusionDate_CalendarExtender.EndDate = DateTime.Today;
                //txtConfirmationDeathDate_CalendarExtender.EndDate = DateTime.Today;
                //txtStartInSituColdPerfusionDate_CalendarExtender.EndDate = DateTime.Today;

                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;

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

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessageNoAsterisk"];
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

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

            strSQL += "SELECT t1.*,  ";
            strSQL += "CONCAT(IF(t1.WithdrawlLifeSupportTreatmentDate IS NULL, NULL, DATE_FORMAT(t1.WithdrawlLifeSupportTreatmentDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.WithdrawlLifeSupportTreatmentTime IS NULL, NULL, DATE_FORMAT(t1.WithdrawlLifeSupportTreatmentTime, '%H:%i')))  ";
            strSQL += "WithdrawlLifeSupportTreatment, ";
            strSQL += "CONCAT(IF(t1.SystolicArterialPressureBelow50Date IS NULL, NULL, DATE_FORMAT(t1.SystolicArterialPressureBelow50Date, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.SystolicArterialPressureBelow50Time IS NULL, NULL, DATE_FORMAT(t1.SystolicArterialPressureBelow50Time, '%H:%i')))  ";
            strSQL += "SystolicArterialPressureBelow50 ";
            strSQL += "FROM donor_operationdata t1 ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            

            if (GV1.Rows.Count == 1)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                //cmdAddData.Text = "Update Data";
                lblDescription.Text = "Update Operation Data for " + Request.QueryString["TID"].ToString() + "";

                AssignData();
                lblGV1.Text = "Summary of  Operation Data";
            }
            else if (GV1.Rows.Count == 0)
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblDescription.Text = "Add Operation Data for " + Request.QueryString["TID"].ToString() + "";
            }
            else
            {
                throw new Exception("More than one Records exist.");
            }





        }
        catch (System.Exception excep)
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
            string STRSQL = "";

            STRSQL += "SELECT t1.*, t2.DonorID Donor FROM  donor_operationdata t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID ";
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

                            if (!DBNull.Value.Equals(myDr["WithdrawlLifeSupportTreatmentDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["WithdrawlLifeSupportTreatmentDate"].ToString()) == true)
                                {
                                    txtWithdrawlLifeSupportTreatmentDate.Text = Convert.ToDateTime(myDr["WithdrawlLifeSupportTreatmentDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["WithdrawlLifeSupportTreatmentTime"]))
                            {
                                if (myDr["WithdrawlLifeSupportTreatmentTime"].ToString().Length >= 5)
                                {
                                    txtWithdrawlLifeSupportTreatmentTime.Text = myDr["WithdrawlLifeSupportTreatmentTime"].ToString().Substring(0,5);
                                }
                            }

                            if (lblWithdrawlLifeSupportTreatment.Font.Bold == true)
                            {
                                if (txtWithdrawlLifeSupportTreatmentDate.Text == string.Empty || txtWithdrawlLifeSupportTreatmentTime.Text==string.Empty )
                                {
                                    lblWithdrawlLifeSupportTreatment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["SystolicArterialPressureBelow50Date"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["SystolicArterialPressureBelow50Date"].ToString()) == true)
                                {
                                    txtSystolicArterialPressureBelow50Date.Text = Convert.ToDateTime(myDr["SystolicArterialPressureBelow50Date"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["SystolicArterialPressureBelow50Time"]))
                            {
                                if (myDr["SystolicArterialPressureBelow50Time"].ToString().Length >= 5)
                                {
                                    txtSystolicArterialPressureBelow50Time.Text = myDr["SystolicArterialPressureBelow50Time"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblSystolicArterialPressureBelow50.Font.Bold == true)
                            {
                                if (txtSystolicArterialPressureBelow50Date.Text == string.Empty || txtSystolicArterialPressureBelow50Time.Text == string.Empty)
                                {
                                    lblSystolicArterialPressureBelow50.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["StartNoTouchPeriodDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["StartNoTouchPeriodDate"].ToString()) == true)
                                {
                                    txtStartNoTouchPeriodDate.Text = Convert.ToDateTime(myDr["StartNoTouchPeriodDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["StartNoTouchPeriodTime"]))
                            {
                                if (myDr["StartNoTouchPeriodTime"].ToString().Length >= 5)
                                {
                                    txtStartNoTouchPeriodTime.Text = myDr["StartNoTouchPeriodTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblStartNoTouchPeriod.Font.Bold == true)
                            {
                                if (txtStartNoTouchPeriodDate.Text == string.Empty || txtStartNoTouchPeriodTime.Text == string.Empty)
                                {
                                    lblStartNoTouchPeriod.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["CirculatoryArrestDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["CirculatoryArrestDate"].ToString()) == true)
                                {
                                    txtCirculatoryArrestDate.Text = Convert.ToDateTime(myDr["CirculatoryArrestDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CirculatoryArrestTime"]))
                            {
                                if (myDr["CirculatoryArrestTime"].ToString().Length >= 5)
                                {
                                    txtCirculatoryArrestTime.Text = myDr["CirculatoryArrestTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblCirculatoryArrest.Font.Bold == true)
                            {
                                if (txtCirculatoryArrestDate.Text == string.Empty || txtCirculatoryArrestTime.Text == string.Empty)
                                {
                                    lblCirculatoryArrest.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["LengthNoTouchPeriod"]))
                            {
                                txtLengthNoTouchPeriod.Text = myDr["LengthNoTouchPeriod"].ToString();
                            }

                            if (lblLengthNoTouchPeriod.Font.Bold == true)
                            {
                                if (txtLengthNoTouchPeriod.Text==string.Empty)
                                {
                                    lblLengthNoTouchPeriod.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["ConfirmationDeathDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ConfirmationDeathDate"].ToString()) == true)
                                {
                                    txtConfirmationDeathDate.Text = Convert.ToDateTime(myDr["ConfirmationDeathDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ConfirmationDeathTime"]))
                            {
                                if (myDr["ConfirmationDeathTime"].ToString().Length >= 5)
                                {
                                    txtConfirmationDeathTime.Text = myDr["ConfirmationDeathTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblConfirmationDeath.Font.Bold == true)
                            {
                                if (txtConfirmationDeathDate.Text == string.Empty || txtConfirmationDeathTime.Text == string.Empty)
                                {
                                    lblConfirmationDeath.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["StartInSituColdPerfusionDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["StartInSituColdPerfusionDate"].ToString()) == true)
                                {
                                    txtStartInSituColdPerfusionDate.Text = Convert.ToDateTime(myDr["StartInSituColdPerfusionDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["StartInSituColdPerfusionTime"]))
                            {
                                if (myDr["StartInSituColdPerfusionTime"].ToString().Length >= 5)
                                {
                                    txtStartInSituColdPerfusionTime.Text = myDr["StartInSituColdPerfusionTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblStartInSituColdPerfusion.Font.Bold == true)
                            {
                                if (txtStartInSituColdPerfusionDate.Text == string.Empty || txtStartInSituColdPerfusionTime.Text == string.Empty)
                                {
                                    lblStartInSituColdPerfusion.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);

                                }
                            }
                            //if (!DBNull.Value.Equals(myDr["StartInSituColdPerfusionDate"]))
                            //{
                            //    if (GeneralRoutines.IsDate(myDr["StartInSituColdPerfusionDate"].ToString()) == true)
                            //    {
                            //        txtStartInSituColdPerfusionDate.Text = Convert.ToDateTime(myDr["StartInSituColdPerfusionDate"]).ToShortDateString();
                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["StartInSituColdPerfusionTime"]))
                            //{
                            //    if (myDr["StartInSituColdPerfusionTime"].ToString().Length >= 5)
                            //    {
                            //        txtStartInSituColdPerfusionTime.Text = myDr["StartInSituColdPerfusionTime"].ToString().Substring(0, 5);
                            //    }
                            //}

                            if (!DBNull.Value.Equals(myDr["SystemicFlushSolutionUsed"]))
                            {
                                ddSystemicFlushSolutionUsed.SelectedValue = (string)(myDr["SystemicFlushSolutionUsed"]);
                            }

                            if (lblSystemicFlushSolutionUsed.Font.Bold == true)
                            {
                                if (ddSystemicFlushSolutionUsed.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddSystemicFlushSolutionUsed.SelectedIndex==-1)
                                {
                                    lblSystemicFlushSolutionUsed.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);

                                }
                            }

                            if (!DBNull.Value.Equals(myDr["SystemicFlushSolutionUsedOther"]))
                            {
                                txtSystemicFlushSolutionUsedOther.Text = (string)(myDr["SystemicFlushSolutionUsedOther"]);
                            }

                            if (lblSystemicFlushSolutionUsedOther.Font.Bold == true)
                            {
                                if (txtSystemicFlushSolutionUsedOther.Text == String.Empty)
                                {
                                    lblSystemicFlushSolutionUsedOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);

                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PreservationSolutionColdPerfusion"]))
                            {
                                ddPreservationSolutionColdPerfusion.SelectedValue = (string)(myDr["PreservationSolutionColdPerfusion"]);
                            }

                            if (lblPreservationSolutionColdPerfusion.Font.Bold == true)
                            {
                                if (ddPreservationSolutionColdPerfusion.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddPreservationSolutionColdPerfusion.SelectedIndex == -1)
                                {
                                    lblPreservationSolutionColdPerfusion.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);

                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PreservationSolutionColdPerfusionOther"]))
                            {
                                txtPreservationSolutionColdPerfusionOther.Text = (string)(myDr["PreservationSolutionColdPerfusionOther"]);
                            }

                            if (lblPreservationSolutionColdPerfusionOther.Font.Bold == true)
                            {
                                if (txtPreservationSolutionColdPerfusionOther.Text == String.Empty)
                                {
                                    lblPreservationSolutionColdPerfusionOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);

                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["VolumeSolutionColdPerfusion"]))
                            //{
                            //    txtVolumeSolutionColdPerfusion.Text = myDr["VolumeSolutionColdPerfusion"].ToString();
                            //}


                            //if (lblVolumeSolutionColdPerfusion.Font.Bold == true)
                            //{
                            //    if (txtVolumeSolutionColdPerfusion.Text == String.Empty)
                            //    {
                            //        lblVolumeSolutionColdPerfusion.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);

                            //    }
                            //}
                            
                            if (!DBNull.Value.Equals(myDr["Heparin"]))
                            {
                                rblHeparin.SelectedValue = (string)(myDr["Heparin"]);
                            }

                            if (lblHeparin.Font.Bold == true)
                            {
                                if (rblHeparin.SelectedIndex==-1)
                                {
                                    lblHeparin.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);

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
                lblUserMessages.Text = ex.Message + " An error occured while executing Assign query.";
            }

        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while resetting the page.";
        }
    }

    // reset page
    protected void cmdReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.Url.AbsoluteUri, false);
            //lblUserMessages.Text = "yoooo";
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

            if ( string.IsNullOrEmpty(Request.QueryString["TID"]))
            {
                throw new Exception("Could not obtain TrialID.");
            }

            string strCountry = Request.QueryString["TID"].Substring(0, 4);

            string strUKTrialID = ConfigurationManager.AppSettings["TrialIDLeadingCharactersUK"];

            DateTime dteNow = DateTime.Now; //no uk time is 1 hour ahead
            DateTime dteNowPlus1 = DateTime.Now.AddHours(1); //no uk time is 1 hour ahead

            if (ddSystemicFlushSolutionUsed.SelectedValue == STR_DD_OTHER_SELECTION)
            {
                Page.Validate("ValSystemicFlushSolutionUsedOther");
            }

            if (ddPreservationSolutionColdPerfusion.SelectedValue == STR_DD_OTHER_SELECTION)
            {
                Page.Validate("ValPreservationSolutionColdPerfusion");
            }

            if (Page.IsValid == false)
            {
                throw new Exception("Please check the data you have entered.");
            }

            DateTime dteWithdrawlLifeSupportTreatment = DateTime.MinValue;
            DateTime dateWithdrawlLifeSupportTreatment = DateTime.MinValue;
            
            DateTime dteSystolicArterialPressureBelow50 = DateTime.MinValue;
            DateTime dateSystolicArterialPressureBelow50 = DateTime.MinValue;
            
            DateTime dteStartNoTouchPeriod = DateTime.MinValue;
            DateTime dateStartNoTouchPeriod = DateTime.MinValue;
            
            DateTime dteCirculatoryArrest = DateTime.MinValue;
            DateTime dateCirculatoryArrest = DateTime.MinValue;
            
            DateTime dteConfirmationDeath = DateTime.MinValue;
            DateTime dateConfirmationDeath = DateTime.MinValue;
            
            DateTime dteStartInSituColdPerfusion = DateTime.MinValue;
            DateTime dateStartInSituColdPerfusion = DateTime.MinValue;
            
            if (txtWithdrawlLifeSupportTreatmentDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtWithdrawlLifeSupportTreatmentDate.Text) == false)
                {
                    throw new Exception("Please enter " + lblWithdrawlLifeSupportTreatment.Text + " Date as DD/MM/YYYY");

                }

                if (Convert.ToDateTime(txtWithdrawlLifeSupportTreatmentDate.Text) > DateTime.Today)
                {
                    throw new Exception(lblWithdrawlLifeSupportTreatment.Text + " cannot be later than Today's date.");
                }
                
                dateWithdrawlLifeSupportTreatment = Convert.ToDateTime(txtWithdrawlLifeSupportTreatmentDate.Text);
            }


            if (txtWithdrawlLifeSupportTreatmentTime.Text != string.Empty && txtWithdrawlLifeSupportTreatmentTime.Text!="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblWithdrawlLifeSupportTreatment.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblWithdrawlLifeSupportTreatment.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblWithdrawlLifeSupportTreatment.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblWithdrawlLifeSupportTreatment.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtWithdrawlLifeSupportTreatmentDate.Text) && GeneralRoutines.IsDate(txtWithdrawlLifeSupportTreatmentTime.Text))
            {
                //dteWithdrawlLifeSupportTreatment = new DateTime();
                dteWithdrawlLifeSupportTreatment = Convert.ToDateTime(txtWithdrawlLifeSupportTreatmentDate.Text + " " + txtWithdrawlLifeSupportTreatmentTime.Text);
                //check if the time is not in the future

                if (strCountry==strUKTrialID)
                {
                    if (dteWithdrawlLifeSupportTreatment > dteNow )
                    {
                        throw new Exception("Please Check '" + lblWithdrawlLifeSupportTreatment.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
                else
                {
                    if (dteWithdrawlLifeSupportTreatment > dteNowPlus1)
                    {
                        throw new Exception("Please Check '" + lblWithdrawlLifeSupportTreatment.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
                
            }
            

            

            //if (dteWithdrawlLifeSupportTreatment > DateTime.Now)
            //{
            //    throw new Exception("'Withdrawl Life Support Treatment' Time cannot be greater than current time.");
            //}

            //lblWithdrawlLifeSupportTreatment.Text = dteWithdrawlLifeSupportTreatment.ToString();

            //Systolic Arterial Pressure Below 50 mm Hg
            if (txtSystolicArterialPressureBelow50Date.Text != string.Empty)
            {

                if (GeneralRoutines.IsDate(txtSystolicArterialPressureBelow50Date.Text) == false)
                {
                    throw new Exception("Please enter '" + lblSystolicArterialPressureBelow50.Text + "' Date as DD/MM/YYYY.");

                }

                if (Convert.ToDateTime(txtSystolicArterialPressureBelow50Date.Text) > DateTime.Today)
                {
                    throw new Exception(lblSystolicArterialPressureBelow50.Text + " cannot be later than Today's date.");
                }

                dateSystolicArterialPressureBelow50 = Convert.ToDateTime(txtSystolicArterialPressureBelow50Date.Text);
            }

            

            if (txtSystolicArterialPressureBelow50Time.Text != string.Empty && txtSystolicArterialPressureBelow50Time.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtSystolicArterialPressureBelow50Time.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'S" + lblSystolicArterialPressureBelow50.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtSystolicArterialPressureBelow50Time.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblSystolicArterialPressureBelow50.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtSystolicArterialPressureBelow50Time.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblSystolicArterialPressureBelow50.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtSystolicArterialPressureBelow50Time.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblSystolicArterialPressureBelow50.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtSystolicArterialPressureBelow50Date.Text) && GeneralRoutines.IsDate(txtSystolicArterialPressureBelow50Time.Text))
            {
                //dteSystolicArterialPressureBelow50 = new DateTime();
                dteSystolicArterialPressureBelow50 = Convert.ToDateTime(txtSystolicArterialPressureBelow50Date.Text + " " + txtSystolicArterialPressureBelow50Time.Text);
            }
            

            //lblSystolicArterialPressureBelow50.Text = dteSystolicArterialPressureBelow50.ToString();

            if (dateWithdrawlLifeSupportTreatment != DateTime.MinValue && dateSystolicArterialPressureBelow50 != DateTime.MinValue)
            {
                if (dateWithdrawlLifeSupportTreatment.Date > dateSystolicArterialPressureBelow50.Date)
                {
                    throw new Exception("The '" + lblSystolicArterialPressureBelow50.Text + "' Date cannnot be earlier than '" + lblWithdrawlLifeSupportTreatment.Text + "' Date.");
                }
            }

            if (dteWithdrawlLifeSupportTreatment != DateTime.MinValue && dteSystolicArterialPressureBelow50 != DateTime.MinValue)
            {
                if (dteWithdrawlLifeSupportTreatment > dteSystolicArterialPressureBelow50)
                {
                    throw new Exception("The '" + lblSystolicArterialPressureBelow50.Text + "' Date Time cannnot be earlier than '" + lblWithdrawlLifeSupportTreatment.Text + "' Date Time");
                }

                if (strCountry == strUKTrialID)
                {
                    if (dteSystolicArterialPressureBelow50 > dteNow)
                    {
                        throw new Exception("Please Check '" + lblSystolicArterialPressureBelow50.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
                else
                {
                    if (dteSystolicArterialPressureBelow50 > dteNowPlus1)
                    {
                        throw new Exception("Please Check '" + lblSystolicArterialPressureBelow50.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
            }
            

            //Start of No Touch Period
            if (txtStartNoTouchPeriodDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtStartNoTouchPeriodDate.Text) == false)
                {
                    throw new Exception("Please enter '" + lblStartNoTouchPeriod.Text + "' Date as DD/MM/YYYY.");

                }

                
                if (Convert.ToDateTime(txtStartNoTouchPeriodDate.Text) > DateTime.Today)
                {
                    throw new Exception(lblStartNoTouchPeriod.Text + " cannot be later than Today's date.");
                }

                dateStartNoTouchPeriod = Convert.ToDateTime(txtStartNoTouchPeriodDate.Text);
            }

            

            if (txtStartNoTouchPeriodTime.Text != string.Empty && txtStartNoTouchPeriodTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtStartNoTouchPeriodTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'Start of No Touch Period' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtStartNoTouchPeriodTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'Start of No Touch Period' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtStartNoTouchPeriodTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'Start of No Touch Period' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtStartNoTouchPeriodTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'Start of No Touch Period' Time Hour Minute can not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtStartNoTouchPeriodDate.Text) && GeneralRoutines.IsDate(txtStartNoTouchPeriodTime.Text))
            {
                //dteStartNoTouchPeriod = new DateTime();
                dteStartNoTouchPeriod = Convert.ToDateTime(txtStartNoTouchPeriodDate.Text + " " + txtStartNoTouchPeriodTime.Text);

                if (strCountry == strUKTrialID)
                {
                    if (dteStartNoTouchPeriod > dteNow)
                    {
                        throw new Exception("Please Check '" + lblStartNoTouchPeriod.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
                else
                {
                    if (dteStartNoTouchPeriod > dteNowPlus1)
                    {
                        throw new Exception("Please Check '" + lblStartNoTouchPeriod.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }

            }
            

            //lblStartNoTouchPeriod.Text = dteStartNoTouchPeriod.ToString();

            if (dateStartNoTouchPeriod != DateTime.MinValue)
            {
                
                if (dateSystolicArterialPressureBelow50 != DateTime.MinValue)
                {
                    if (dateSystolicArterialPressureBelow50.Date > dateStartNoTouchPeriod.Date)
                    {
                        throw new Exception("The '" + lblStartNoTouchPeriod.Text + "' Date cannnot be earlier than '" + lblSystolicArterialPressureBelow50.Text + "' Date.");
                    }
                }

                if (dateWithdrawlLifeSupportTreatment != DateTime.MinValue)
                {
                    if (dateWithdrawlLifeSupportTreatment.Date > dateStartNoTouchPeriod.Date)
                    {
                        throw new Exception("The '" + lblWithdrawlLifeSupportTreatment.Text + "' Date cannnot be earlier than '" + lblSystolicArterialPressureBelow50.Text + "' Date.");
                    }

                }
                
            }

            if (dteStartNoTouchPeriod != DateTime.MinValue  )
            {

                if (dteSystolicArterialPressureBelow50 != DateTime.MinValue)
                {
                    if (dteSystolicArterialPressureBelow50 > dteStartNoTouchPeriod)
                    {
                        throw new Exception("The '" + lblStartNoTouchPeriod.Text + "' Date Time cannnot be earlier than '" + lblSystolicArterialPressureBelow50.Text + "' Date Time.");
                    }
                }

                if (dteWithdrawlLifeSupportTreatment != DateTime.MinValue)
                {
                    if (dteWithdrawlLifeSupportTreatment.Date > dateStartNoTouchPeriod.Date)
                    {
                        throw new Exception("The '" + lblWithdrawlLifeSupportTreatment.Text + "' Date Time cannnot be earlier than '" + lblSystolicArterialPressureBelow50.Text + "' Date Time.");
                    }

                }

                
            }
            

            //Circulatory Arrest
            if (txtCirculatoryArrestDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtCirculatoryArrestDate.Text) == false)
                {
                    throw new Exception("Please enter '" + lblCirculatoryArrest.Text + "' Date as DD/MM/YYYY.");

                }

                

                if (Convert.ToDateTime(txtCirculatoryArrestDate.Text) > DateTime.Today)
                {
                    throw new Exception(lblCirculatoryArrest.Text + " cannot be later than Today's date.");
                }

                dateCirculatoryArrest = Convert.ToDateTime(txtCirculatoryArrestDate.Text);
            }

            

            if (txtCirculatoryArrestTime.Text != string.Empty && txtCirculatoryArrestTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtCirculatoryArrestTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblCirculatoryArrest.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtCirculatoryArrestTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblCirculatoryArrest.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtCirculatoryArrestTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblCirculatoryArrest.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtCirculatoryArrestTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblCirculatoryArrest.Text + "' Time Hour Minute can not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtCirculatoryArrestDate.Text) && GeneralRoutines.IsDate(txtCirculatoryArrestTime.Text))
            {
                //dteCirculatoryArrest = new DateTime();
                dteCirculatoryArrest = Convert.ToDateTime(txtCirculatoryArrestDate.Text + " " + txtCirculatoryArrestTime.Text);

                if (strCountry == strUKTrialID)
                {
                    if (dteCirculatoryArrest > dteNow)
                    {
                        throw new Exception("Please Check '" + lblCirculatoryArrest.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
                else
                {
                    if (dteCirculatoryArrest > dteNowPlus1)
                    {
                        throw new Exception("Please Check '" + lblCirculatoryArrest.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }

            }
            

            //lblCirculatoryArrest.Text = dteCirculatoryArrest.ToString();
            if (dateCirculatoryArrest != DateTime.MinValue)
            {

                if (dateStartNoTouchPeriod != DateTime.MinValue)
                {
                    if (dateStartNoTouchPeriod > dateCirculatoryArrest)
                    {
                        throw new Exception("The '" + lblCirculatoryArrest.Text + "' Date cannnot be earlier than '" + lblStartNoTouchPeriod.Text + "' Date.");
                    }
                }

            }

            if (dteCirculatoryArrest != DateTime.MinValue)
            {
                
                if (dteStartNoTouchPeriod != DateTime.MinValue)
                {
                    if (dteStartNoTouchPeriod > dteCirculatoryArrest)
                    {
                        throw new Exception("The '" + lblCirculatoryArrest.Text + "' Date Time cannnot be earlier than '" + lblStartNoTouchPeriod.Text + "' Date Time");
                    }
                }
                
            }
            

            //Length of No Touch Period
            if (txtLengthNoTouchPeriod.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtLengthNoTouchPeriod.Text) == false)
                {
                    throw new Exception("Please enter 'Length of No Touch Period' in numeric format");
                }


            }

            //Confirmation of Death
            if (txtConfirmationDeathDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtConfirmationDeathDate.Text) == false)
                {
                    throw new Exception("Please enter '" + lblConfirmationDeath.Text + "' Date as DD/MM/YYYY.");

                }


                if (Convert.ToDateTime(txtConfirmationDeathDate.Text) > DateTime.Today)
                {
                    throw new Exception(lblConfirmationDeath.Text + " cannot be later than Today's date.");
                }
            }


            if (txtConfirmationDeathTime.Text != string.Empty && txtConfirmationDeathTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtConfirmationDeathTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblConfirmationDeath.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtConfirmationDeathTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblConfirmationDeath.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtConfirmationDeathTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblConfirmationDeath.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtConfirmationDeathTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblConfirmationDeath.Text + "' Time Hour Minute can not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtConfirmationDeathDate.Text) && GeneralRoutines.IsDate(txtConfirmationDeathTime.Text))
            {
                //dteConfirmationDeath = new DateTime();
                dteConfirmationDeath = Convert.ToDateTime(txtConfirmationDeathDate.Text + " " + txtConfirmationDeathTime.Text);

                if (strCountry == strUKTrialID)
                {
                    if (dteConfirmationDeath > dteNow)
                    {
                        throw new Exception("Please Check '" + lblConfirmationDeath.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
                else
                {
                    if (dteConfirmationDeath > dteNowPlus1)
                    {
                        throw new Exception("Please Check '" + lblConfirmationDeath.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
            }
            

            //lblConfirmationDeath.Text = dteConfirmationDeath.ToString();
            if (dteCirculatoryArrest != DateTime.MinValue && dteConfirmationDeath != DateTime.MinValue)
            {
                if (dteCirculatoryArrest > dteConfirmationDeath)
                {
                    throw new Exception("The '" + lblConfirmationDeath.Text + "' Date Time cannnot be earlier than '" + lblCirculatoryArrest.Text + "' Date Time");
                }
            }
            

            //Start In Situ Cold Perfusion
            if (txtStartInSituColdPerfusionDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtStartInSituColdPerfusionDate.Text) == false)
                {
                    throw new Exception("Please enter '" + lblStartInSituColdPerfusion.Text + "' Date in the correct format.");
                }

                if (Convert.ToDateTime(txtStartInSituColdPerfusionDate.Text) > DateTime.Today)
                {
                    throw new Exception("'" + lblStartInSituColdPerfusion.Text + "' cannot be later than Today's date.");
                }
            }


            if (txtStartInSituColdPerfusionTime.Text != string.Empty && txtStartInSituColdPerfusionTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtStartInSituColdPerfusionTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblStartInSituColdPerfusion.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtStartInSituColdPerfusionTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblStartInSituColdPerfusion.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtStartInSituColdPerfusionTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblStartInSituColdPerfusion.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtStartInSituColdPerfusionTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblStartInSituColdPerfusion.Text + "' Time Hour Minute can not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtStartInSituColdPerfusionDate.Text) && GeneralRoutines.IsDate(txtStartInSituColdPerfusionTime.Text))
            {
                //dteStartInSituColdPerfusion = new DateTime();
                dteStartInSituColdPerfusion = Convert.ToDateTime(txtStartInSituColdPerfusionDate.Text + " " + txtStartInSituColdPerfusionTime.Text);

                if (strCountry == strUKTrialID)
                {
                    if (dteStartInSituColdPerfusion > dteNow)
                    {
                        throw new Exception("Please Check '" + lblStartInSituColdPerfusion.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }
                else
                {
                    if (dteStartInSituColdPerfusion > dteNowPlus1)
                    {
                        throw new Exception("Please Check '" + lblStartInSituColdPerfusion.Text + "' Date/Time. It cannot be later than current Date/Time.");
                    }
                }

            }
            

            //lblStartInSituColdPerfusion.Text = dteStartInSituColdPerfusion.ToString();
            if (dteConfirmationDeath != DateTime.MinValue && dteStartInSituColdPerfusion != DateTime.MinValue)
            {
                if (dteConfirmationDeath > dteStartInSituColdPerfusion)
                {
                    throw new Exception("The '" + lblStartInSituColdPerfusion.Text + "' Date Time cannnot be earlier than '" + lblConfirmationDeath.Text + "' Date Time");
                }
            }            


            //Length of No Touch Period
            if (txtLengthNoTouchPeriod.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtLengthNoTouchPeriod.Text) == false)
                {
                    throw new Exception("Please enter '" + lblLengthNoTouchPeriod.Text + "' in numeric format");
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

            STRSQL += "INSERT INTO donor_operationdata ";
            STRSQL += "(TrialID, WithdrawlLifeSupportTreatmentDate, WithdrawlLifeSupportTreatmentTime, SystolicArterialPressureBelow50Date,  ";
            STRSQL += "SystolicArterialPressureBelow50Time, StartNoTouchPeriodDate, StartNoTouchPeriodTime, CirculatoryArrestDate,";
            STRSQL += "CirculatoryArrestTime, LengthNoTouchPeriod, ConfirmationDeathDate, ConfirmationDeathTime,";
            STRSQL += "StartInSituColdPerfusionDate, StartInSituColdPerfusionTime, SystemicFlushSolutionUsed,";
            STRSQL += "SystemicFlushSolutionUsedOther, PreservationSolutionColdPerfusion, PreservationSolutionColdPerfusionOther,";
            //STRSQL += "VolumeSolutionColdPerfusion, Heparin,";
            STRSQL += "Heparin,";
            STRSQL += "TotalWarmIschaemicPeriod, WithdrawalPeriod,FunctionalWarmIschaemicPeriod,AsystolicWarmIschaemicPeriod,";
            STRSQL += " Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?WithdrawlLifeSupportTreatmentDate, ?WithdrawlLifeSupportTreatmentTime, ?SystolicArterialPressureBelow50Date, ";
            STRSQL += "?SystolicArterialPressureBelow50Time, ?StartNoTouchPeriodDate, ?StartNoTouchPeriodTime, ?CirculatoryArrestDate,";
            STRSQL += "?CirculatoryArrestTime, ?LengthNoTouchPeriod, ?ConfirmationDeathDate, ?ConfirmationDeathTime,";
            STRSQL += "?StartInSituColdPerfusionDate, ?StartInSituColdPerfusionTime, ?SystemicFlushSolutionUsed,";
            STRSQL += "?SystemicFlushSolutionUsedOther, ?PreservationSolutionColdPerfusion, ?PreservationSolutionColdPerfusionOther,";
            //STRSQL += "?VolumeSolutionColdPerfusion, ?Heparin,";
            STRSQL += "?Heparin,";
            STRSQL += "?TotalWarmIschaemicPeriod, ?WithdrawalPeriod,?FunctionalWarmIschaemicPeriod,?AsystolicWarmIschaemicPeriod,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";


            string STRSQL_UPDATE = "";
            STRSQL_UPDATE += "UPDATE donor_operationdata SET ";
            STRSQL_UPDATE += "WithdrawlLifeSupportTreatmentDate=?WithdrawlLifeSupportTreatmentDate, WithdrawlLifeSupportTreatmentTime=?WithdrawlLifeSupportTreatmentTime, ";
            STRSQL_UPDATE += "SystolicArterialPressureBelow50Date=?SystolicArterialPressureBelow50Date,SystolicArterialPressureBelow50Time=?SystolicArterialPressureBelow50Time ,";
            STRSQL_UPDATE += "StartNoTouchPeriodDate=?StartNoTouchPeriodDate,StartNoTouchPeriodTime=?StartNoTouchPeriodTime,";
            STRSQL_UPDATE += "CirculatoryArrestDate=?CirculatoryArrestDate,CirculatoryArrestTime=?CirculatoryArrestTime, LengthNoTouchPeriod=?LengthNoTouchPeriod,";
            STRSQL_UPDATE += "ConfirmationDeathDate=?ConfirmationDeathDate,ConfirmationDeathTime=?ConfirmationDeathTime,";
            STRSQL_UPDATE += "StartInSituColdPerfusionDate=?StartInSituColdPerfusionDate,StartInSituColdPerfusionTime=?StartInSituColdPerfusionTime,";
            STRSQL_UPDATE += "SystemicFlushSolutionUsed=?SystemicFlushSolutionUsed,SystemicFlushSolutionUsedOther=?SystemicFlushSolutionUsedOther,";
            STRSQL_UPDATE += "PreservationSolutionColdPerfusion=?PreservationSolutionColdPerfusion,PreservationSolutionColdPerfusionOther=?PreservationSolutionColdPerfusionOther,";
            //STRSQL_UPDATE += "VolumeSolutionColdPerfusion=?VolumeSolutionColdPerfusion,Heparin=?Heparin,";
            STRSQL_UPDATE += "Heparin=?Heparin,";
            STRSQL_UPDATE += "TotalWarmIschaemicPeriod=?TotalWarmIschaemicPeriod,WithdrawalPeriod=?WithdrawalPeriod,";
            STRSQL_UPDATE += "FunctionalWarmIschaemicPeriod=?FunctionalWarmIschaemicPeriod,AsystolicWarmIschaemicPeriod=?AsystolicWarmIschaemicPeriod,";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


            //lock data
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE donor_operationdata SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE donor_operationdata SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_operationdata WHERE TrialID=?TrialID ";

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

            
            if (GeneralRoutines.IsDate(txtWithdrawlLifeSupportTreatmentDate.Text) == false)
            {
                MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtWithdrawlLifeSupportTreatmentDate.Text);
            }

            if (txtWithdrawlLifeSupportTreatmentTime.Text == String.Empty || txtWithdrawlLifeSupportTreatmentTime.Text=="__:__")
            {
                MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentTime", MySqlDbType.VarChar).Value = txtWithdrawlLifeSupportTreatmentTime.Text;
            }

            if (GeneralRoutines.IsDate(txtSystolicArterialPressureBelow50Date.Text) == false)
            {
                MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Date", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Date", MySqlDbType.Date).Value = Convert.ToDateTime(txtSystolicArterialPressureBelow50Date.Text);
            }

            if (txtSystolicArterialPressureBelow50Time.Text == String.Empty || txtSystolicArterialPressureBelow50Time.Text=="__:__")
            {
                MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Time", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Time", MySqlDbType.VarChar).Value = txtSystolicArterialPressureBelow50Time.Text;
            }

            if (GeneralRoutines.IsDate(txtStartNoTouchPeriodDate.Text) == false)
            {
                MyCMD.Parameters.Add("?StartNoTouchPeriodDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?StartNoTouchPeriodDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtStartNoTouchPeriodDate.Text);
            }

            if (txtStartNoTouchPeriodTime.Text == String.Empty || txtStartNoTouchPeriodTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?StartNoTouchPeriodTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?StartNoTouchPeriodTime", MySqlDbType.VarChar).Value = txtStartNoTouchPeriodTime.Text;
            }

            if (GeneralRoutines.IsDate(txtCirculatoryArrestDate.Text) == false)
            {
                MyCMD.Parameters.Add("?CirculatoryArrestDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CirculatoryArrestDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtCirculatoryArrestDate.Text);
            }

            if (txtCirculatoryArrestTime.Text == String.Empty || txtCirculatoryArrestTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?CirculatoryArrestTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CirculatoryArrestTime", MySqlDbType.VarChar).Value = txtCirculatoryArrestTime.Text;
            }

            if (txtLengthNoTouchPeriod.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?LengthNoTouchPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?LengthNoTouchPeriod", MySqlDbType.VarChar).Value = txtLengthNoTouchPeriod.Text;
            }

            if (GeneralRoutines.IsDate(txtConfirmationDeathDate.Text) == false)
            {
                MyCMD.Parameters.Add("?ConfirmationDeathDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ConfirmationDeathDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtConfirmationDeathDate.Text);
            }

            if (txtConfirmationDeathTime.Text == String.Empty || txtConfirmationDeathTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?ConfirmationDeathTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ConfirmationDeathTime", MySqlDbType.VarChar).Value = txtConfirmationDeathTime.Text;
            }

            if (GeneralRoutines.IsDate(txtStartInSituColdPerfusionDate.Text) == false)
            {
                MyCMD.Parameters.Add("?StartInSituColdPerfusionDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?StartInSituColdPerfusionDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtStartInSituColdPerfusionDate.Text);
            }

            if (txtStartInSituColdPerfusionTime.Text == String.Empty || txtStartInSituColdPerfusionTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?StartInSituColdPerfusionTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?StartInSituColdPerfusionTime", MySqlDbType.VarChar).Value = txtStartInSituColdPerfusionTime.Text;
            }

            if (ddSystemicFlushSolutionUsed.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?SystemicFlushSolutionUsed", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SystemicFlushSolutionUsed", MySqlDbType.VarChar).Value = ddSystemicFlushSolutionUsed.SelectedValue;
            }

            if (txtSystemicFlushSolutionUsedOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?SystemicFlushSolutionUsedOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SystemicFlushSolutionUsedOther", MySqlDbType.VarChar).Value = txtSystemicFlushSolutionUsedOther.Text;
            }

            if (ddPreservationSolutionColdPerfusion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?PreservationSolutionColdPerfusion", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PreservationSolutionColdPerfusion", MySqlDbType.VarChar).Value = ddPreservationSolutionColdPerfusion.SelectedValue;
            }

            if (txtPreservationSolutionColdPerfusionOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PreservationSolutionColdPerfusionOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PreservationSolutionColdPerfusionOther", MySqlDbType.VarChar).Value = txtPreservationSolutionColdPerfusionOther.Text;
            }

            //if (txtVolumeSolutionColdPerfusion.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?VolumeSolutionColdPerfusion", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?VolumeSolutionColdPerfusion", MySqlDbType.VarChar).Value = txtVolumeSolutionColdPerfusion.Text;
            //}

            if (rblHeparin.SelectedIndex==1)
            {
                MyCMD.Parameters.Add("?Heparin", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Heparin", MySqlDbType.VarChar).Value = rblHeparin.SelectedValue;
            }

            //derived values

            TimeSpan tsGetSpan;

            //TotalWarmIschaemicPeriod
            if (dteStartInSituColdPerfusion != DateTime.MinValue && dteWithdrawlLifeSupportTreatment != DateTime.MinValue)
            {
                tsGetSpan = Convert.ToDateTime(dteStartInSituColdPerfusion) - Convert.ToDateTime(dteWithdrawlLifeSupportTreatment);

                MyCMD.Parameters.Add("?TotalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
            }
            else
            {
                MyCMD.Parameters.Add("?TotalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
            }

            //WithdrawalPeriod
            if (dteCirculatoryArrest != DateTime.MinValue && dteWithdrawlLifeSupportTreatment != DateTime.MinValue)
            {
                tsGetSpan = Convert.ToDateTime(dteCirculatoryArrest) - Convert.ToDateTime(dteWithdrawlLifeSupportTreatment);

                MyCMD.Parameters.Add("?WithdrawalPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
            }
            else
            {
                MyCMD.Parameters.Add("?WithdrawalPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
            }

            //FunctionalWarmIschaemicPeriod
            if (dteStartInSituColdPerfusion != DateTime.MinValue && dteSystolicArterialPressureBelow50 != DateTime.MinValue)
            {
                tsGetSpan = Convert.ToDateTime(dteStartInSituColdPerfusion) - Convert.ToDateTime(dteSystolicArterialPressureBelow50);

                MyCMD.Parameters.Add("?FunctionalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
            }
            else
            {
                MyCMD.Parameters.Add("?FunctionalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
            }

            //AsystolicWarmIschaemicPeriod
            if (dteStartInSituColdPerfusion != DateTime.MinValue && dteCirculatoryArrest != DateTime.MinValue)
            {
                tsGetSpan = Convert.ToDateTime(dteStartInSituColdPerfusion) - Convert.ToDateTime(dteCirculatoryArrest);

                MyCMD.Parameters.Add("?AsystolicWarmIschaemicPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
            }
            else
            {
                MyCMD.Parameters.Add("?AsystolicWarmIschaemicPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
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
                { MyCONN.Close(); }

                BindData();

                string strSQLCOMPLETE = string.Empty;
                strSQLCOMPLETE += "SELECT ";
                strSQLCOMPLETE += "IF(t2.WithdrawlLifeSupportTreatmentDate IS NOT NULL AND t2.WithdrawlLifeSupportTreatmentTime IS NOT NULL  ";
                //strSQLCOMPLETE += "AND t2.SystolicArterialPressureBelow50Date IS NOT NULL AND t2.StartNoTouchPeriodDate IS NOT NULL AND t2.StartNoTouchPeriodTime IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.StartNoTouchPeriodDate IS NOT NULL AND t2.StartNoTouchPeriodTime IS NOT NULL ";
                //strSQLCOMPLETE += "AND t2.CirculatoryArrestDate IS NOT NULL AND t2.CirculatoryArrestTime IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.LengthNoTouchPeriod IS NOT NULL  AND t2.ConfirmationDeathDate IS NOT NULL AND t2.ConfirmationDeathTime IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.StartInSituColdPerfusionDate IS NOT NULL  AND t2.StartInSituColdPerfusionTime IS NOT NULL   ";
                strSQLCOMPLETE += "AND IF(t2.SystemicFlushSolutionUsed ='Other', t2.SystemicFlushSolutionUsedOther IS NOT NULL,t2.SystemicFlushSolutionUsed IS NOT NULL) ";
                strSQLCOMPLETE += "AND IF(t2.PreservationSolutionColdPerfusion ='Other', t2.PreservationSolutionColdPerfusionOther IS NOT NULL,t2.PreservationSolutionColdPerfusion IS NOT NULL) ";
                //strSQLCOMPLETE += "AND t2.VolumeSolutionColdPerfusion IS NOT NULL  AND t2.Heparin IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.Heparin IS NOT NULL  ";
                //strSQLCOMPLETE += "AND t2.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM donor_operationdata t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialID=?TrialID ";
                strSQLCOMPLETE += "";

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialID", Request.QueryString["TID"], STRCONN);

                //lblDonorRiskIndex.Text = strComplete;

                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);
                }
                else
                {
                    if (Request.Url.AbsoluteUri.Contains("&SCode=1"))
                    {
                        Response.Redirect(Request.Url.AbsoluteUri);

                    }
                    else
                    {
                        Response.Redirect(Request.Url.AbsoluteUri + "&SCode=1", false);
                    }
                    lblUserMessages.Text = "Data Submitted";
                }


            }

            catch (System.Exception ex)
            {

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing add/update query.";
            }

            //finally //close connection
            //{
            //    if (MyCONN.State == ConnectionState.Open)
            //    { MyCONN.Close(); }
            //}




        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_operationdata WHERE TrialID=?TrialID ";

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
                if (String.IsNullOrEmpty(Request.QueryString["DonorOperatationDataID"]))
                {
                    throw new Exception("More than one Record exists for deletion. Click on TrialID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM donor_operationdata ";
            STRSQL += "WHERE TrialID=?TrialID ";
            if (intCountFind > 1)
            {
                STRSQL += "AND DonorOperatationDataID=?DonorOperatationDataID ";
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

            if (!String.IsNullOrEmpty(Request.QueryString["DonorLabResultsID"]))
            {
                MyCMD.Parameters.Add("?DonorOperatationDataID", MySqlDbType.VarChar).Value = Request.QueryString["DonorOperatationDataID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

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

            BindData();

            lblUserMessages.Text = "Data Deleted.";

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }

    protected void ddSystemicFlushSolutionUsed_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddSystemicFlushSolutionUsed.SelectedValue == STR_DD_OTHER_SELECTION)
        {
            pnlSystemicFlushSolutionUsedOther.Visible = true;
        }
        else
        {
            pnlSystemicFlushSolutionUsedOther.Visible = false;
            txtSystemicFlushSolutionUsedOther.Text = string.Empty;
        }
    }

    protected void ddPreservationSolutionColdPerfusion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddPreservationSolutionColdPerfusion.SelectedValue==STR_DD_OTHER_SELECTION)
        {
            pnlPreservationSolutionColdPerfusionOther.Visible = true;
        }
        else
        {
            pnlPreservationSolutionColdPerfusionOther.Visible = false;
            txtPreservationSolutionColdPerfusionOther.Text = string.Empty;
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

    //protected void cmdAddData_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;

    //        DateTime? dteWithdrawlLifeSupportTreatment = null;
    //        DateTime? dteSystolicArterialPressureBelow50 = null;
    //        DateTime? dteStartNoTouchPeriod = null;
    //        DateTime? dteCirculatoryArrest = null;
    //        DateTime? dteConfirmationDeath = null;
    //        DateTime? dteStartInSituColdPerfusion = null;

    //        if (txtWithdrawlLifeSupportTreatmentDate.Text == string.Empty)
    //        {
    //            throw new Exception("Please enter 'Withdrawl of Life Support Treatment' Date.");
    //        }

    //        if (GeneralRoutines.IsDate(txtWithdrawlLifeSupportTreatmentDate.Text) == false)
    //        {
    //            throw new Exception("Please enter 'Withdrawl of Life Support Treatment' Date in the correct format.");

    //        }
    //        else
    //        {
    //            if (Convert.ToDateTime(txtWithdrawlLifeSupportTreatmentDate.Text) > DateTime.Now)
    //            {
    //                throw new Exception("'Withdrawl of Life Support Treatment' Date can not be greater than Today's date.");
    //            }
    //        }


    //        if (txtWithdrawlLifeSupportTreatmentTime.Text == string.Empty)
    //        {
    //            throw new Exception("Please enter 'Withdrawl Life Support Treatment' Time. ");
    //        }
    //        else
    //        {
    //            if (GeneralRoutines.IsNumeric(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(0, 2)) == false)
    //            {
    //                throw new Exception("'Withdrawl Life Support Treatment' Time Hour should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(0, 2)) > 23)
    //            {
    //                throw new Exception("'Withdrawl Life Support Treatment' Time Hour should not be greater than 23.");
    //            }

    //            if (GeneralRoutines.IsNumeric(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(3, 2)) == false)
    //            {
    //                throw new Exception("'Withdrawl Life Support Treatment' Time Minute should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtWithdrawlLifeSupportTreatmentTime.Text.Substring(3, 2)) > 59)
    //            {
    //                throw new Exception("'Withdrawl Life Support Treatment' Time Hour Minute not be greater than 59.");
    //            }
    //        }

    //        dteWithdrawlLifeSupportTreatment = new DateTime();

    //        dteWithdrawlLifeSupportTreatment = Convert.ToDateTime(txtWithdrawlLifeSupportTreatmentDate.Text + " " + txtWithdrawlLifeSupportTreatmentTime.Text);

    //        //if (dteWithdrawlLifeSupportTreatment > DateTime.Now)
    //        //{
    //        //    throw new Exception("'Withdrawl Life Support Treatment' Time cannot be greater than current time.");
    //        //}

    //        //lblWithdrawlLifeSupportTreatment.Text = dteWithdrawlLifeSupportTreatment.ToString();

    //        //Systolic Arterial Pressure Below 50 mm Hg
    //        if (txtSystolicArterialPressureBelow50Date.Text == string.Empty)
    //        {
    //            txtSystolicArterialPressureBelow50Date.Text = txtWithdrawlLifeSupportTreatmentDate.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtSystolicArterialPressureBelow50Date.Text) == false)
    //        {
    //            throw new Exception("Please enter 'Systolic Arterial Pressure Below 50 mm Hg' Date in the correct format.");

    //        }
    //        else
    //        {
    //            if (Convert.ToDateTime(txtSystolicArterialPressureBelow50Date.Text) > DateTime.Now)
    //            {
    //                throw new Exception("'Systolic Arterial Pressure Below 50 mm Hg' Date can not be greater than Today's date.");
    //            }
    //        }

    //        if (txtSystolicArterialPressureBelow50Time.Text == string.Empty)
    //        {
    //            throw new Exception("Please enter 'Systolic Arterial Pressure Below 50 mm Hg' Time. ");
    //        }
    //        else
    //        {
    //            if (GeneralRoutines.IsNumeric(txtSystolicArterialPressureBelow50Time.Text.Substring(0, 2)) == false)
    //            {
    //                throw new Exception("'Systolic Arterial Pressure Below 50 mm Hg' Time Hour should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtSystolicArterialPressureBelow50Time.Text.Substring(0, 2)) > 23)
    //            {
    //                throw new Exception("'Systolic Arterial Pressure Below 50 mm Hg' Time Hour should not be greater than 23.");
    //            }

    //            if (GeneralRoutines.IsNumeric(txtSystolicArterialPressureBelow50Time.Text.Substring(3, 2)) == false)
    //            {
    //                throw new Exception("'Systolic Arterial Pressure Below 50 mm Hg' Time Minute should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtSystolicArterialPressureBelow50Time.Text.Substring(3, 2)) > 59)
    //            {
    //                throw new Exception("'Systolic Arterial Pressure Below 50 mm Hg' Time Hour Minute not be greater than 59.");
    //            }
    //        }

    //        dteSystolicArterialPressureBelow50 = new DateTime();
    //        dteSystolicArterialPressureBelow50 = Convert.ToDateTime(txtSystolicArterialPressureBelow50Date.Text + " " + txtSystolicArterialPressureBelow50Time.Text);

    //        //lblSystolicArterialPressureBelow50.Text = dteSystolicArterialPressureBelow50.ToString();

    //        if (dteWithdrawlLifeSupportTreatment > dteSystolicArterialPressureBelow50)
    //        {
    //            throw new Exception("The 'Systolic Arterial Pressure Below 50 mm Hg' Date Time cannnot be earlier than 'Withdrawl Life Support Treatment' Date Time");
    //        }

    //        //Start of No Touch Period
    //        if (txtStartNoTouchPeriodDate.Text == string.Empty)
    //        {
    //            txtStartNoTouchPeriodDate.Text = txtSystolicArterialPressureBelow50Date.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtStartNoTouchPeriodDate.Text) == false)
    //        {
    //            throw new Exception("Please enter 'Start of No Touch Period' Date in the correct format.");
    //        }
    //        else
    //        {
    //            if (Convert.ToDateTime(txtStartNoTouchPeriodDate.Text) > DateTime.Now)
    //            {
    //                throw new Exception("'Start of No Touch Period' Date can not be greater than Today's date.");
    //            }
    //        }

    //        if (txtStartNoTouchPeriodTime.Text == string.Empty)
    //        {
    //            throw new Exception("Please enter 'Start of No Touch Period' Time. ");
    //        }
    //        else
    //        {
    //            if (GeneralRoutines.IsNumeric(txtStartNoTouchPeriodTime.Text.Substring(0, 2)) == false)
    //            {
    //                throw new Exception("'Start of No Touch Period' Time Hour should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtStartNoTouchPeriodTime.Text.Substring(0, 2)) > 23)
    //            {
    //                throw new Exception("'Start of No Touch Period' Time Hour should not be greater than 23.");
    //            }

    //            if (GeneralRoutines.IsNumeric(txtStartNoTouchPeriodTime.Text.Substring(3, 2)) == false)
    //            {
    //                throw new Exception("'Start of No Touch Period' Time Minute should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtStartNoTouchPeriodTime.Text.Substring(3, 2)) > 59)
    //            {
    //                throw new Exception("'Start of No Touch Period' Time Hour Minute can not be greater than 59.");
    //            }
    //        }


    //        dteStartNoTouchPeriod = new DateTime();
    //        dteStartNoTouchPeriod = Convert.ToDateTime(txtStartNoTouchPeriodDate.Text + " " + txtStartNoTouchPeriodTime.Text);

    //        //lblStartNoTouchPeriod.Text = dteStartNoTouchPeriod.ToString();

    //        if (dteSystolicArterialPressureBelow50 > dteStartNoTouchPeriod)
    //        {
    //            throw new Exception("The 'Start of No Touch Period' Date Time cannnot be earlier than 'Systolic Arterial Pressure Below 50 mm Hg' Date Time");
    //        }

    //        //Circulatory Arrest
    //        if (txtCirculatoryArrestDate.Text == string.Empty)
    //        {
    //            txtCirculatoryArrestDate.Text = txtStartNoTouchPeriodDate.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtCirculatoryArrestDate.Text) == false)
    //        {
    //            throw new Exception("Please enter 'Circulatory Arrest' Date in the correct format.");
    //        }
    //        else
    //        {
    //            if (Convert.ToDateTime(txtCirculatoryArrestDate.Text) > DateTime.Now)
    //            {
    //                throw new Exception("'Circulatory Arrest' Date can not be greater than Today's date.");
    //            }
    //        }

    //        if (txtCirculatoryArrestTime.Text == string.Empty)
    //        {
    //            throw new Exception("Please enter 'Circulatory Arrest' Time. ");


    //        }
    //        else
    //        {
    //            if (GeneralRoutines.IsNumeric(txtCirculatoryArrestTime.Text.Substring(0, 2)) == false)
    //            {
    //                throw new Exception("'Circulatory Arrest' Time Hour should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtCirculatoryArrestTime.Text.Substring(0, 2)) > 23)
    //            {
    //                throw new Exception("'Circulatory Arrest' Time Hour should not be greater than 23.");
    //            }

    //            if (GeneralRoutines.IsNumeric(txtCirculatoryArrestTime.Text.Substring(3, 2)) == false)
    //            {
    //                throw new Exception("'Circulatory Arrest' Time Minute should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtCirculatoryArrestTime.Text.Substring(3, 2)) > 59)
    //            {
    //                throw new Exception("'Circulatory Arrest' Time Hour Minute can not be greater than 59.");
    //            }
    //        }

    //        dteCirculatoryArrest = new DateTime();
    //        dteCirculatoryArrest = Convert.ToDateTime(txtCirculatoryArrestDate.Text + " " + txtCirculatoryArrestTime.Text);

    //        //lblCirculatoryArrest.Text = dteCirculatoryArrest.ToString();

    //        if (dteStartNoTouchPeriod > dteCirculatoryArrest)
    //        {
    //            throw new Exception("The 'Circulatory Arrest' Date Time cannnot be earlier than 'Start of No Touch Period' Date Time");
    //        }

    //        //Length of No Touch Period
    //        if (txtLengthNoTouchPeriod.Text != string.Empty)
    //        {
    //            if (GeneralRoutines.IsNumeric(txtLengthNoTouchPeriod.Text) == false)
    //            {
    //                throw new Exception("Please enter 'Length of No Touch Period' in numeric format");
    //            }
    //        }

    //        //Confirmation of Death
    //        if (txtConfirmationDeathDate.Text == string.Empty)
    //        {
    //            txtConfirmationDeathDate.Text = txtCirculatoryArrestDate.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtConfirmationDeathDate.Text) == false)
    //        {
    //            throw new Exception("Please enter 'Confirmation Death' Date in the correct format.");
    //        }
    //        else
    //        {
    //            if (Convert.ToDateTime(txtConfirmationDeathDate.Text) > DateTime.Now)
    //            {
    //                throw new Exception("'Confirmation Death' Date can not be greater than Today's date.");
    //            }
    //        }


    //        if (txtConfirmationDeathTime.Text == string.Empty)
    //        {
    //            throw new Exception("Please enter 'Confirmation Death' Time. ");


    //        }
    //        else
    //        {
    //            if (GeneralRoutines.IsNumeric(txtConfirmationDeathTime.Text.Substring(0, 2)) == false)
    //            {
    //                throw new Exception("'Confirmation Death' Time Hour should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtConfirmationDeathTime.Text.Substring(0, 2)) > 23)
    //            {
    //                throw new Exception("'Confirmation Death' Time Hour should not be greater than 23.");
    //            }

    //            if (GeneralRoutines.IsNumeric(txtConfirmationDeathTime.Text.Substring(3, 2)) == false)
    //            {
    //                throw new Exception("'Confirmation Death' Time Minute should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtConfirmationDeathTime.Text.Substring(3, 2)) > 59)
    //            {
    //                throw new Exception("'Confirmation Death' Time Hour Minute can not be greater than 59.");
    //            }
    //        }

    //        dteConfirmationDeath = new DateTime();
    //        dteConfirmationDeath = Convert.ToDateTime(txtConfirmationDeathDate.Text + " " + txtConfirmationDeathTime.Text);

    //        //lblConfirmationDeath.Text = dteConfirmationDeath.ToString();

    //        if (dteCirculatoryArrest > dteConfirmationDeath)
    //        {
    //            throw new Exception("The 'Confirmation of  Death' Date Time cannnot be earlier than 'Circulatory Arrest' Date Time");
    //        }

    //        //Start In Situ Cold Perfusion
    //        if (txtStartInSituColdPerfusionDate.Text == string.Empty)
    //        {
    //            txtStartInSituColdPerfusionDate.Text = txtConfirmationDeathDate.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtStartInSituColdPerfusionDate.Text) == false)
    //        {
    //            throw new Exception("Please enter 'Start In Situ Cold Perfusion' Date in teh correct format.");
    //        }
    //        else
    //        {
    //            if (Convert.ToDateTime(txtStartInSituColdPerfusionDate.Text) > DateTime.Now)
    //            {
    //                throw new Exception("'Start In Situ Cold Perfusion' Date can not be greater than Today's date.");
    //            }
    //        }


    //        if (txtStartInSituColdPerfusionTime.Text == string.Empty)
    //        {
    //            throw new Exception("Please enter 'Start In Situ Cold Perfusion' Time. ");
    //        }
    //        else
    //        {
    //            if (GeneralRoutines.IsNumeric(txtStartInSituColdPerfusionTime.Text.Substring(0, 2)) == false)
    //            {
    //                throw new Exception("'Start In Situ Cold Perfusion' Time Hour should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtStartInSituColdPerfusionTime.Text.Substring(0, 2)) > 23)
    //            {
    //                throw new Exception("'Start In Situ Cold Perfusion' Time Hour should not be greater than 23.");
    //            }

    //            if (GeneralRoutines.IsNumeric(txtStartInSituColdPerfusionTime.Text.Substring(3, 2)) == false)
    //            {
    //                throw new Exception("'Start In Situ Cold Perfusion' Time Minute should be numeric.");
    //            }

    //            if (Convert.ToInt16(txtStartInSituColdPerfusionTime.Text.Substring(3, 2)) > 59)
    //            {
    //                throw new Exception("'Start In Situ Cold Perfusion' Time Hour Minute can not be greater than 59.");
    //            }
    //        }

    //        dteStartInSituColdPerfusion = new DateTime();
    //        dteStartInSituColdPerfusion = Convert.ToDateTime(txtStartInSituColdPerfusionDate.Text + " " + txtStartInSituColdPerfusionTime.Text);

    //        //lblStartInSituColdPerfusion.Text = dteStartInSituColdPerfusion.ToString();

    //        if (dteConfirmationDeath > dteStartInSituColdPerfusion)
    //        {
    //            throw new Exception("The 'Start In Situ Cold Perfusion' Date Time cannnot be earlier than 'Confirmation of  Death' Date Time");
    //        }


    //        //Length of No Touch Period
    //        if (txtLengthNoTouchPeriod.Text != string.Empty)
    //        {
    //            if (GeneralRoutines.IsNumeric(txtLengthNoTouchPeriod.Text) == false)
    //            {
    //                throw new Exception("Please enter 'Length of No Touch Period' in numeric format");
    //            }
    //        }

    //        if (chkDataLocked.Checked == true)
    //        {
    //            if (txtReasonModified.Text == string.Empty)
    //            {
    //                throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
    //            }
    //        }

    //        string STRSQL = "";

    //        STRSQL += "INSERT INTO donor_operationdata ";
    //        STRSQL += "(TrialID, WithdrawlLifeSupportTreatmentDate, WithdrawlLifeSupportTreatmentTime, SystolicArterialPressureBelow50Date,  ";
    //        STRSQL += "SystolicArterialPressureBelow50Time, StartNoTouchPeriodDate, StartNoTouchPeriodTime, CirculatoryArrestDate,";
    //        STRSQL += "CirculatoryArrestTime, LengthNoTouchPeriod, ConfirmationDeathDate, ConfirmationDeathTime,";
    //        STRSQL += "StartInSituColdPerfusionDate, StartInSituColdPerfusionTime, SystemicFlushSolutionUsed,";
    //        STRSQL += "SystemicFlushSolutionUsedOther, PreservationSolutionColdPerfusion, PreservationSolutionColdPerfusionOther,";
    //        STRSQL += "VolumeSolutionColdPerfusion, Heparin,";
    //        STRSQL += "TotalWarmIschaemicPeriod, WithdrawalPeriod,FunctionalWarmIschaemicPeriod,AsystolicWarmIschaemicPeriod,";
    //        STRSQL += " Comments, DateCreated, CreatedBy) ";
    //        STRSQL += "VALUES ";
    //        STRSQL += "(?TrialID, ?WithdrawlLifeSupportTreatmentDate, ?WithdrawlLifeSupportTreatmentTime, ?SystolicArterialPressureBelow50Date, ";
    //        STRSQL += "?SystolicArterialPressureBelow50Time, ?StartNoTouchPeriodDate, ?StartNoTouchPeriodTime, ?CirculatoryArrestDate,";
    //        STRSQL += "?CirculatoryArrestTime, ?LengthNoTouchPeriod, ?ConfirmationDeathDate, ?ConfirmationDeathTime,";
    //        STRSQL += "?StartInSituColdPerfusionDate, ?StartInSituColdPerfusionTime, ?SystemicFlushSolutionUsed,";
    //        STRSQL += "?SystemicFlushSolutionUsedOther, ?PreservationSolutionColdPerfusion, ?PreservationSolutionColdPerfusionOther,";
    //        STRSQL += "?VolumeSolutionColdPerfusion, ?Heparin,";
    //        STRSQL += "?TotalWarmIschaemicPeriod, ?WithdrawalPeriod,?FunctionalWarmIschaemicPeriod,?AsystolicWarmIschaemicPeriod,";
    //        STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";


    //        string STRSQL_UPDATE = "";
    //        STRSQL_UPDATE += "UPDATE donor_operationdata SET ";
    //        STRSQL_UPDATE += "WithdrawlLifeSupportTreatmentDate=?WithdrawlLifeSupportTreatmentDate, WithdrawlLifeSupportTreatmentTime=?WithdrawlLifeSupportTreatmentTime, ";
    //        STRSQL_UPDATE += "SystolicArterialPressureBelow50Date=?SystolicArterialPressureBelow50Date,SystolicArterialPressureBelow50Time=?SystolicArterialPressureBelow50Time ,";
    //        STRSQL_UPDATE += "StartNoTouchPeriodDate=?StartNoTouchPeriodDate,StartNoTouchPeriodTime=?StartNoTouchPeriodTime,";
    //        STRSQL_UPDATE += "CirculatoryArrestDate=?CirculatoryArrestDate,CirculatoryArrestTime=?CirculatoryArrestTime, LengthNoTouchPeriod=?LengthNoTouchPeriod,";
    //        STRSQL_UPDATE += "ConfirmationDeathDate=?ConfirmationDeathDate,ConfirmationDeathTime=?ConfirmationDeathTime,";
    //        STRSQL_UPDATE += "StartInSituColdPerfusionDate=?StartInSituColdPerfusionDate,StartInSituColdPerfusionTime=?StartInSituColdPerfusionTime,";
    //        STRSQL_UPDATE += "SystemicFlushSolutionUsed=?SystemicFlushSolutionUsed,SystemicFlushSolutionUsedOther=?SystemicFlushSolutionUsedOther,";
    //        STRSQL_UPDATE += "PreservationSolutionColdPerfusion=?PreservationSolutionColdPerfusion,PreservationSolutionColdPerfusionOther=?PreservationSolutionColdPerfusionOther,";
    //        STRSQL_UPDATE += "VolumeSolutionColdPerfusion=?VolumeSolutionColdPerfusion,Heparin=?Heparin,";
    //        STRSQL_UPDATE += "TotalWarmIschaemicPeriod=?TotalWarmIschaemicPeriod,WithdrawalPeriod=?WithdrawalPeriod,";
    //        STRSQL_UPDATE += "FunctionalWarmIschaemicPeriod=?FunctionalWarmIschaemicPeriod,AsystolicWarmIschaemicPeriod=?AsystolicWarmIschaemicPeriod,";
    //        STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
    //        STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


    //        //lock data
    //        string STRSQL_LOCK = "";
    //        STRSQL_LOCK += "UPDATE donor_operationdata SET ";
    //        STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
    //        STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
    //        STRSQL_LOCK += "";
    //        STRSQL_LOCK += "";


    //        // mark final
    //        string STRSQL_FINAL = string.Empty;
    //        STRSQL_FINAL += "UPDATE donor_operationdata SET ";
    //        STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
    //        STRSQL_FINAL += "WHERE TrialID=?TrialID ";

    //        string CS = string.Empty;
    //        CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

    //        MySqlConnection MyCONN = new MySqlConnection();
    //        MyCONN.ConnectionString = CS;

    //        MySqlCommand MyCMD = new MySqlCommand();
    //        MyCMD.Connection = MyCONN;

    //        MyCMD.CommandType = CommandType.Text;

    //        string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_operationdata WHERE TrialID=?TrialID ";

    //        int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

    //        if (intCountFind == 1)
    //        {
    //            MyCMD.CommandText = STRSQL_UPDATE;
    //        }
    //        else if (intCountFind == 0)
    //        {
    //            MyCMD.CommandText = STRSQL;
    //        }
    //        else if (intCountFind > 1)
    //        {
    //            throw new Exception("More than One Donor Operation Data exists for this TrialID. Click on TrialID in the table below to select data to delete. ");
    //        }
    //        else
    //        {
    //            throw new Exception("An error occured while check if Donor Operation Data already exist in the database.");
    //        }


    //        MyCMD.Parameters.Clear();

    //        MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

    //        if (GeneralRoutines.IsDate(txtWithdrawlLifeSupportTreatmentDate.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentDate", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtWithdrawlLifeSupportTreatmentDate.Text);
    //        }

    //        if (txtWithdrawlLifeSupportTreatmentTime.Text == String.Empty)
    //        {
    //            MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentTime", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?WithdrawlLifeSupportTreatmentTime", MySqlDbType.VarChar).Value = txtWithdrawlLifeSupportTreatmentTime.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtSystolicArterialPressureBelow50Date.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Date", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Date", MySqlDbType.Date).Value = Convert.ToDateTime(txtSystolicArterialPressureBelow50Date.Text);
    //        }

    //        if (txtSystolicArterialPressureBelow50Time.Text == String.Empty)
    //        {
    //            MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Time", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?SystolicArterialPressureBelow50Time", MySqlDbType.VarChar).Value = txtSystolicArterialPressureBelow50Time.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtStartNoTouchPeriodDate.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?StartNoTouchPeriodDate", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?StartNoTouchPeriodDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtStartNoTouchPeriodDate.Text);
    //        }

    //        if (txtStartNoTouchPeriodTime.Text == String.Empty)
    //        {
    //            MyCMD.Parameters.Add("?StartNoTouchPeriodTime", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?StartNoTouchPeriodTime", MySqlDbType.VarChar).Value = txtStartNoTouchPeriodTime.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtCirculatoryArrestDate.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?CirculatoryArrestDate", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?CirculatoryArrestDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtCirculatoryArrestDate.Text);
    //        }

    //        if (txtCirculatoryArrestTime.Text == String.Empty)
    //        {
    //            MyCMD.Parameters.Add("?CirculatoryArrestTime", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?CirculatoryArrestTime", MySqlDbType.VarChar).Value = txtCirculatoryArrestTime.Text;
    //        }

    //        if (txtLengthNoTouchPeriod.Text == string.Empty)
    //        {
    //            MyCMD.Parameters.Add("?LengthNoTouchPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?LengthNoTouchPeriod", MySqlDbType.VarChar).Value = txtLengthNoTouchPeriod.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtConfirmationDeathDate.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?ConfirmationDeathDate", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?ConfirmationDeathDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtConfirmationDeathDate.Text);
    //        }

    //        if (txtConfirmationDeathTime.Text == String.Empty)
    //        {
    //            MyCMD.Parameters.Add("?ConfirmationDeathTime", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?ConfirmationDeathTime", MySqlDbType.VarChar).Value = txtConfirmationDeathTime.Text;
    //        }

    //        if (GeneralRoutines.IsDate(txtStartInSituColdPerfusionDate.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?StartInSituColdPerfusionDate", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?StartInSituColdPerfusionDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtStartInSituColdPerfusionDate.Text);
    //        }

    //        if (txtStartInSituColdPerfusionTime.Text == String.Empty)
    //        {
    //            MyCMD.Parameters.Add("?StartInSituColdPerfusionTime", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?StartInSituColdPerfusionTime", MySqlDbType.VarChar).Value = txtStartInSituColdPerfusionTime.Text;
    //        }

    //        if (ddSystemicFlushSolutionUsed.SelectedValue == STR_DD_UNKNOWN_SELECTION)
    //        {
    //            MyCMD.Parameters.Add("?SystemicFlushSolutionUsed", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?SystemicFlushSolutionUsed", MySqlDbType.VarChar).Value = ddSystemicFlushSolutionUsed.SelectedValue;
    //        }

    //        if (txtSystemicFlushSolutionUsedOther.Text == string.Empty)
    //        {
    //            MyCMD.Parameters.Add("?SystemicFlushSolutionUsedOther", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?SystemicFlushSolutionUsedOther", MySqlDbType.VarChar).Value = txtSystemicFlushSolutionUsedOther.Text;
    //        }

    //        if (ddPreservationSolutionColdPerfusion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
    //        {
    //            MyCMD.Parameters.Add("?PreservationSolutionColdPerfusion", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?PreservationSolutionColdPerfusion", MySqlDbType.VarChar).Value = ddPreservationSolutionColdPerfusion.SelectedValue;
    //        }

    //        if (txtPreservationSolutionColdPerfusionOther.Text == string.Empty)
    //        {
    //            MyCMD.Parameters.Add("?PreservationSolutionColdPerfusionOther", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?PreservationSolutionColdPerfusionOther", MySqlDbType.VarChar).Value = txtPreservationSolutionColdPerfusionOther.Text;
    //        }

    //        if (txtVolumeSolutionColdPerfusion.Text == string.Empty)
    //        {
    //            MyCMD.Parameters.Add("?VolumeSolutionColdPerfusion", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?VolumeSolutionColdPerfusion", MySqlDbType.VarChar).Value = txtVolumeSolutionColdPerfusion.Text;
    //        }

    //        if (rblHeparin.SelectedValue == STR_UNKNOWN_SELECTION)
    //        {
    //            MyCMD.Parameters.Add("?Heparin", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?Heparin", MySqlDbType.VarChar).Value = rblHeparin.SelectedValue;
    //        }

    //        //derived values

    //        TimeSpan tsGetSpan;

    //        //TotalWarmIschaemicPeriod
    //        if (dteStartInSituColdPerfusion.HasValue && dteWithdrawlLifeSupportTreatment.HasValue)
    //        {
    //            tsGetSpan = Convert.ToDateTime(dteStartInSituColdPerfusion) - Convert.ToDateTime(dteWithdrawlLifeSupportTreatment);

    //            MyCMD.Parameters.Add("?TotalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?TotalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }

    //        //WithdrawalPeriod
    //        if (dteCirculatoryArrest.HasValue && dteWithdrawlLifeSupportTreatment.HasValue)
    //        {
    //            tsGetSpan = Convert.ToDateTime(dteCirculatoryArrest) - Convert.ToDateTime(dteWithdrawlLifeSupportTreatment);

    //            MyCMD.Parameters.Add("?WithdrawalPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?WithdrawalPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }

    //        //FunctionalWarmIschaemicPeriod
    //        if (dteStartInSituColdPerfusion.HasValue && dteSystolicArterialPressureBelow50.HasValue)
    //        {
    //            tsGetSpan = Convert.ToDateTime(dteStartInSituColdPerfusion) - Convert.ToDateTime(dteSystolicArterialPressureBelow50);

    //            MyCMD.Parameters.Add("?FunctionalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?FunctionalWarmIschaemicPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }

    //        //AsystolicWarmIschaemicPeriod
    //        if (dteStartInSituColdPerfusion.HasValue && dteCirculatoryArrest.HasValue)
    //        {
    //            tsGetSpan = Convert.ToDateTime(dteStartInSituColdPerfusion) - Convert.ToDateTime(dteCirculatoryArrest);

    //            MyCMD.Parameters.Add("?AsystolicWarmIschaemicPeriod", MySqlDbType.VarChar).Value = tsGetSpan.TotalMinutes;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?AsystolicWarmIschaemicPeriod", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }


    //        if (txtComments.Text == string.Empty)
    //        {
    //            MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
    //        }

    //        if (txtReasonModified.Text == string.Empty)
    //        {
    //            MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = DBNull.Value;

    //        }
    //        else
    //        {
    //            string strReasonModified = "";
    //            strReasonModified += DateTime.Now.ToString() + " Modified By " + SessionVariablesAll.UserName + System.Environment.NewLine;
    //            strReasonModified += txtReasonModified.Text;
    //            if (lblReasonModifiedOldDetails.Text != string.Empty)
    //            {
    //                strReasonModified += System.Environment.NewLine + lblReasonModifiedOldDetails.Text;
    //            }

    //            MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = strReasonModified;
    //        }

    //        MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = 1;

    //        if (chkDataFinal.Checked == true)
    //        {
    //            MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 1;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 0;
    //        }


    //        MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

    //        MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

    //        MyCONN.Open();

    //        try
    //        {
    //            MyCMD.ExecuteNonQuery();

    //            //lock data
    //            if (chkAllDataAdded.Checked == true)
    //            {
    //                MyCMD.CommandType = CommandType.Text;
    //                MyCMD.CommandText = STRSQL_LOCK;
    //                MyCMD.ExecuteNonQuery();

    //            }

    //            if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
    //            {
    //                MyCMD.CommandType = CommandType.Text;
    //                MyCMD.CommandText = STRSQL_FINAL;
    //                MyCMD.ExecuteNonQuery();
    //            }


    //            if (MyCONN.State == ConnectionState.Open)
    //            { MyCONN.Close(); }

    //            BindData();

    //            string strSQLCOMPLETE = string.Empty;
    //            strSQLCOMPLETE += "SELECT ";
    //            strSQLCOMPLETE += "IF(t2.WithdrawlLifeSupportTreatmentDate IS NOT NULL AND t2.WithdrawlLifeSupportTreatmentTime IS NOT NULL  ";
    //            strSQLCOMPLETE += "AND t2.SystolicArterialPressureBelow50Date IS NOT NULL AND t2.StartNoTouchPeriodDate IS NOT NULL AND t2.StartNoTouchPeriodTime IS NOT NULL ";
    //            strSQLCOMPLETE += "AND t2.CirculatoryArrestDate IS NOT NULL AND t2.CirculatoryArrestTime IS NOT NULL AND t2.LengthNoTouchPeriod IS NOT NULL ";
    //            strSQLCOMPLETE += "AND t2.ConfirmationDeathDate IS NOT NULL AND t2.ConfirmationDeathTime IS NOT NULL ";
    //            strSQLCOMPLETE += "AND t2.StartInSituColdPerfusionDate IS NOT NULL  AND t2.StartInSituColdPerfusionTime IS NOT NULL   ";
    //            strSQLCOMPLETE += "AND IF(t2.SystemicFlushSolutionUsed ='Other', t2.SystemicFlushSolutionUsedOther IS NOT NULL,t2.SystemicFlushSolutionUsed IS NOT NULL) ";
    //            strSQLCOMPLETE += "AND IF(t2.PreservationSolutionColdPerfusion ='Other', t2.PreservationSolutionColdPerfusionOther IS NOT NULL,t2.PreservationSolutionColdPerfusion IS NOT NULL) ";
    //            strSQLCOMPLETE += "AND t2.VolumeSolutionColdPerfusion IS NOT NULL  AND t2.Heparin IS NOT NULL ";
    //            strSQLCOMPLETE += " ";
    //            //strSQLCOMPLETE += "AND t2.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
    //            strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
    //            strSQLCOMPLETE += "FROM donor_operationdata t2 ";
    //            strSQLCOMPLETE += "WHERE t2.TrialID=?TrialID ";
    //            strSQLCOMPLETE += "";

    //            string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialID", Request.QueryString["TID"], STRCONN);

    //            //lblDonorRiskIndex.Text = strComplete;

    //            if (strComplete == "Complete")
    //            {
    //                Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);
    //            }
    //            else
    //            {
    //                if (Request.Url.AbsoluteUri.Contains("&SCode=1"))
    //                {
    //                    Response.Redirect(Request.Url.AbsoluteUri);

    //                }
    //                else
    //                {
    //                    Response.Redirect(Request.Url.AbsoluteUri + "&SCode=1", false);
    //                }
    //                lblUserMessages.Text = "Data Submitted";
    //            }


    //        }

    //        catch (System.Exception ex)
    //        {

    //            if (MyCONN.State == ConnectionState.Open)
    //            { MyCONN.Close(); }
    //            lblUserMessages.Text = ex.Message + " An error occured while executing add/update query.";
    //        }

    //        //finally //close connection
    //        //{
    //        //    if (MyCONN.State == ConnectionState.Open)
    //        //    { MyCONN.Close(); }
    //        //}




    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while adding data.";
    //    }
    //}
    
}