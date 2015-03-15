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

public partial class SpecClinicalData_AddInspectionLR : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";

    private const string strMainCPH = "cplMainContents";
    private const string strMainLabel = "lblDonorID";

    private const string STR_OTHER_SELECTION = "Other";
    private const string STR_UNKNOWN_SELECTION = "Unknown";
    private const string STR_YES_SELECTION = "YES";
    private const string STR_NO_SELECTION = "NO";

    private const string STR_DD_UNKNOWN_SELECTION = "0";
    private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";
    //static Random _random = new Random();

    #endregion

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (string.IsNullOrEmpty(Request.QueryString["TID"]))
                {
                    throw new Exception("Could not obtain TrialID.");
                }
                if (Request.QueryString["TID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID.");
                }

                ddPreservationModality.DataSource = XMLPreservationModalitiesDataSource;
                ddPreservationModality.DataBind();

                ddPreservationModality_R.DataSource = XMLPreservationModalitiesDataSource;
                ddPreservationModality_R.DataBind();


                rblRandomisationComplete.DataSource = XMLMainOptionsYNDataSource;
                rblRandomisationComplete.DataBind();
                //rblRandomisationComplete.SelectedValue = STR_UNKNOWN_SELECTION;

                rblRandomisationComplete_R.DataSource = XMLMainOptionsYNDataSource;
                rblRandomisationComplete_R.DataBind();
                //rblRandomisationComplete.SelectedValue = STR_UNKNOWN_SELECTION;

                rblKidneyTransplantable.DataSource = XMLMainOptionsYNDataSource;
                rblKidneyTransplantable.DataBind();
                //rblRandomisationComplete.SelectedValue = STR_UNKNOWN_SELECTION;

                rblKidneyTransplantable_R.DataSource = XMLMainOptionsYNDataSource;
                rblKidneyTransplantable_R.DataBind();

                string strSQLRandomised = "SELECT COUNT(*) CR FROM kidneyr WHERE TrialID=?TrialID ";

                Int32 intRandomised = Convert.ToInt32(GeneralRoutines.ReturnScalar(strSQLRandomised, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

                //if (intRandomised==0)
                //{
                //    cmdAddData.Enabled = false;
                //    cmdReset.Enabled = false;
                //    throw new Exception("The Kidneys from this donor have not been randomised. Please randomised the kidneys first.");

                //}
                if (intRandomised == 1)
                {
                    cmdAddData.Enabled = true;
                    cmdReset.Enabled = true;
                    //check if the role allows it
                    if (SessionVariablesAll.ViewRandomise==STR_YES_SELECTION)
                    {
                        AssignRandomisationData();

                        ddPreservationModality.Visible = true;
                        ddPreservationModality_R.Visible = true;

                    }
                    else
                    {
                        ddPreservationModality.Visible = false;
                        ddPreservationModality_R.Visible = false;
                    }
                    
                }
                if (intRandomised < 0)
                {
                    throw new Exception("An error occured while checking if the Kidneys from this donor have been randomised.");
                }
                
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                //if (ddPreservationModality.SelectedValue=="A")
                //{
                //    txtRemovalDate.Visible = false;
                //    txtRemovalTime.Visible = false;
                //}

                //if (ddPreservationModality.SelectedValue == "B")
                //{
                //    txtRemovalDate.Visible = true;
                //    txtRemovalTime.Visible = true;
                //}

                //if (ddPreservationModality_R.SelectedValue == "A")
                //{
                //    txtRemoval_RDate.Visible = false;
                //    txtRemoval_RTime.Visible = false;
                //}

                //if (ddPreservationModality_R.SelectedValue == "B")
                //{
                //    txtRemoval_RDate.Visible = true;
                //    txtRemoval_RTime.Visible = true;

                //}
                ddSide.DataSource = XMLKidneySidesDataSource;
                ddSide.DataBind();
                ddSide.SelectedValue = "Left";

                ddSide_R.DataSource = XMLKidneySidesDataSource;
                ddSide_R.DataBind();
                ddSide_R.SelectedValue = "Right";
                ////remove Unknown if exists
                //ListItem liDDSide = ddSide.Items.FindByValue(STR_UNKNOWN_SELECTION);
                //if (liDDSide != null)
                //{
                //    ddSide.Items.Remove(liDDSide);
                //}

                //liDDSide = ddSide.Items.FindByValue(STR_UNKNOWN_SELECTION);
                //if (liDDSide != null)
                //{
                //    ddSide.Items.Remove(liDDSide);
                //}

                

                //rblPreservationModality.SelectedValue = STR_UNKNOWN_SELECTION;

                cblArterialProblems.DataSource = XMLArterialProblemsDataSource;
                cblArterialProblems.DataBind();

                cblArterialProblems_R.DataSource = XMLArterialProblemsDataSource;
                cblArterialProblems_R.DataBind();

                

                ddWashoutPerfusion.DataSource = XMLWashoutPerfusionDataSource;
                ddWashoutPerfusion.DataBind();

                ddWashoutPerfusion_R.DataSource = XMLWashoutPerfusionDataSource;
                ddWashoutPerfusion_R.DataBind();
                //rblWashoutPerfusion.SelectedValue = STR_UNKNOWN_SELECTION;

                //ddVisiblePerfusionDefects.DataSource = XMLMainOptionsDataSource;
                //ddVisiblePerfusionDefects.DataBind();
                

                
                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;

                txtRemovalDate_CalendarExtender.EndDate = DateTime.Today;
                txtRemoval_RDate_CalendarExtender.EndDate = DateTime.Today;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

                AssignData();

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
                strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessage"];
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

            strSQL += "SELECT t1.*  ";
            //strSQL += "DATE_FORMAT(t1.DateBaggedColdStorage, '%d/%m/%Y') Date_BaggedColdStorage, ";
            //strSQL += "TIME_FORMAT(t1.TimeBaggedColdStorage, '%H:%i') Time_BaggedColdStorage, ";
            //strSQL += "DATE_FORMAT(t1.DateArrivalTransplantCentre, '%d/%m/%Y') Date_ArrivalTransplantCentre, ";
            //strSQL += "TIME_FORMAT(t1.TimeArrivalTransplantCentre, '%H:%i') Time_ArrivalTransplantCentre ";
            strSQL += "FROM kidneyinspection t1 ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Summrary of Kidney Inspection Data.";

            if (GV1.Rows.Count>0)
            {

            }
            else
            {

            }
            lblDescription.Text = "Add  Kidney Inspection Data for " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;

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

    //assign randomisation data
    protected void AssignRandomisationData()
    {
        try
        {
            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  kidneyr t1 ";
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
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["LeftRanCategory"]))
                            {
                                ddPreservationModality.SelectedValue = myDr["LeftRanCategory"].ToString();
                                rblRandomisationComplete.SelectedValue = STR_YES_SELECTION;

                            }
                            if (!DBNull.Value.Equals(myDr["RightRanCategory"]))
                            {
                                ddPreservationModality_R.SelectedValue = myDr["RightRanCategory"].ToString();
                                rblRandomisationComplete_R.SelectedValue = STR_YES_SELECTION;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assinging Radnomisation Data ";
        }
    }
     
    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  kidneyinspection t1 ";
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

                            //if (!DBNull.Value.Equals(myDr["Side"]))
                            //{
                            //    ddSide.SelectedValue = myDr["Side"].ToString();
                            //}

                            //if (!DBNull.Value.Equals(myDr["AnatomyOther"]))
                            //{
                            //    txtAnatomyOther.Text = myDr["AnatomyOther"].ToString();
                            //}

                            //if (!DBNull.Value.Equals(myDr["DateBaggedColdStorage"]))
                            //{
                            //    if (GeneralRoutines.IsDate(myDr["DateBaggedColdStorage"].ToString()))
                            //    {
                            //        txtDateBaggedColdStorage.Text = Convert.ToDateTime(myDr["DateBaggedColdStorage"]).ToString("dd/MM/yyyy");
                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["TimeBaggedColdStorage"]))
                            //{
                            //    if (myDr["TimeBaggedColdStorage"].ToString().Length >= 5)
                            //    {
                            //        txtTimeBaggedColdStorage.Text = myDr["TimeBaggedColdStorage"].ToString().Substring(0, 5);
                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["DateArrivalTransplantCentre"]))
                            //{

                            //    if (GeneralRoutines.IsDate(myDr["DateArrivalTransplantCentre"].ToString()))
                            //    {
                            //        txtDateArrivalTransplantCentre.Text = Convert.ToDateTime(myDr["DateArrivalTransplantCentre"]).ToString("dd/MM/yyyy");
                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["TimeArrivalTransplantCentre"]))
                            //{
                            //    if (myDr["TimeArrivalTransplantCentre"].ToString().Length >= 5)
                            //    {
                            //        txtTimeArrivalTransplantCentre.Text = myDr["TimeArrivalTransplantCentre"].ToString().Substring(0, 5);
                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["ColdStorageSolution"]))
                            //{
                            //    ddColdStorageSolution.SelectedValue = myDr["ColdStorageSolution"].ToString();
                            //}

                            //if (!DBNull.Value.Equals(myDr["KidneySuitableTransplant"]))
                            //{
                            //    rblKidneySuitableTransplant.SelectedValue = myDr["KidneySuitableTransplant"].ToString();
                            //}

                            if (!DBNull.Value.Equals(myDr["NumberRenalArteries"]))
                            {
                                txtNumberRenalArteries.Text = myDr["NumberRenalArteries"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["NumberRenalArteries_R"]))
                            {
                                txtNumberRenalArteries.Text = myDr["NumberRenalArteries_R"].ToString();
                            }

                            if (lblNumberRenalArteries.Font.Bold == true)
                            {
                                if (txtNumberRenalArteries.Text == string.Empty || txtNumberRenalArteries.Text == string.Empty)
                                {
                                    lblNumberRenalArteries.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArterialProblems"]))
                            {
                                //lblCentre.Text += myDr["ArterialProblems"].ToString();
                                string[] strSC_Sets = myDr["ArterialProblems"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    ListItem currentCheckBox = cblArterialProblems.Items.FindByText(strSC_Contents[0].ToString());

                                    if (currentCheckBox != null)
                                    {
                                        if (strSC_Contents[1].ToString() == STR_YES_SELECTION)
                                        {
                                            currentCheckBox.Selected = true;
                                        }

                                    }

                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArterialProblems_R"]))
                            {
                                //lblCentre.Text += myDr["ArterialProblems"].ToString();
                                string[] strSC_Sets = myDr["ArterialProblems_R"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    ListItem currentCheckBox = cblArterialProblems_R.Items.FindByText(strSC_Contents[0].ToString());

                                    if (currentCheckBox != null)
                                    {
                                        if (strSC_Contents[1].ToString() == STR_YES_SELECTION)
                                        {
                                            currentCheckBox.Selected = true;
                                        }

                                    }

                                }
                            }

                            if (lblArterialProblems.Font.Bold == true)
                            {
                                if (cblArterialProblems.SelectedIndex==-1 || cblArterialProblems_R.SelectedIndex==-1)
                                {
                                    lblArterialProblems.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["KidneyTransplantable"]))
                            {
                                rblKidneyTransplantable.SelectedValue = myDr["KidneyTransplantable"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["KidneyTransplantable_R"]))
                            {
                                rblKidneyTransplantable_R.SelectedValue = myDr["KidneyTransplantable_R"].ToString();
                            }

                            if (lblKidneyTransplantable.Font.Bold==true)
                            {
                                if (rblKidneyTransplantable.SelectedIndex==-1 || rblKidneyTransplantable_R.SelectedIndex==-1)
                                {
                                    lblKidneyTransplantable.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ReasonNotTransplantable"]))
                            {
                                txtReasonNotTransplantable.Text = myDr["ReasonNotTransplantable"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["ReasonNotTransplantable_R"]))
                            {
                                txtReasonNotTransplantable_R.Text = myDr["ReasonNotTransplantable_R"].ToString();
                            }


                            //if (lblReasonNotTransplantable.Font.Bold==true)
                            //{
                            //    if (txtReasonNotTransplantable.Text == string.Empty || txtReasonNotTransplantable_R.Text == string.Empty)
                            //    {
                            //        lblReasonNotTransplantable.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                            //    }
                            //}
                            
                            if (!DBNull.Value.Equals(myDr["WashoutPerfusion"]))
                            {
                                ddWashoutPerfusion.SelectedValue = myDr["WashoutPerfusion"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["WashoutPerfusion_R"]))
                            {
                                ddWashoutPerfusion_R.SelectedValue = myDr["WashoutPerfusion_R"].ToString();
                            }


                            if (lblWashoutPerfusion.Font.Bold == true)
                            {
                                if (ddWashoutPerfusion.SelectedIndex == -1 || ddWashoutPerfusion.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddWashoutPerfusion_R.SelectedIndex == -1 || ddWashoutPerfusion_R.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblWashoutPerfusion.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["RemovalDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["RemovalDate"].ToString()))
                                {
                                    txtRemovalDate.Text = Convert.ToDateTime(myDr["RemovalDate"].ToString()).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["RemovalTime"]))                                
                            {
                                if (myDr["RemovalTime"].ToString().Length >= 5)
                                {
                                    txtRemovalTime.Text = myDr["RemovalTime"].ToString().Substring(0,5);
                                }
                                
                            }

                            if (!DBNull.Value.Equals(myDr["Removal_RDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["Removal_RDate"].ToString()))
                                {
                                    txtRemoval_RDate.Text = Convert.ToDateTime(myDr["Removal_RDate"].ToString()).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Removal_RTime"]))
                            {
                                if (myDr["Removal_RTime"].ToString().Length >= 5)
                                {
                                    txtRemoval_RTime.Text = myDr["Removal_RTime"].ToString().Substring(0, 5);
                                }

                            }

                            if (lblRemoval.Font.Bold == true)
                            {
                                if (GeneralRoutines.IsDate(txtRemovalDate.Text) == false || GeneralRoutines.IsDate(txtRemovalTime.Text) == false || GeneralRoutines.IsDate(txtRemoval_RDate.Text) == false || GeneralRoutines.IsDate(txtRemoval_RTime.Text) == false)
                                {
                                    lblRemoval.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["ArterialProblems"]))
                            //{
                            //    string[] items = myDr["ArterialProblems"].ToString().Split(',');

                            //    for (int i = 0; i <= items.GetUpperBound(0); i++)
                            //    {
                            //        ListItem currentCheckBox = cblArterialProblems.Items.FindByValue(items[i].ToString());
                            //        if (currentCheckBox != null)
                            //        {
                            //            currentCheckBox.Selected = true;
                            //        }
                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["ArterialProblemsRemarks"]))
                            //{
                            //    txtArterialProblemsRemarks.Text = myDr["ArterialProblemsRemarks"].ToString();
                            //}

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = myDr["Comments"].ToString();
                            }
                            if (!DBNull.Value.Equals(myDr["Comments_R"]))
                            {
                                txtComments.Text = myDr["Comments_R"].ToString();
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
                rblKidneyTransplantable_SelectedIndexChanged(this, EventArgs.Empty);
                rblKidneyTransplantable_R_SelectedIndexChanged(this, EventArgs.Empty);
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }
            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while assigning data.";
            }

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
        }
    }
     //Reset Page
    protected void cmdReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.Url.AbsoluteUri, false);
            //lblUserMessages.Text = "yoooo";
        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while resetting the page.";
        }
    }
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //if (ddSide.SelectedValue == "0")
            //{
            //    throw new Exception("Please Select an option for Anatomy.");
            //}

            //if (rblRandomisationComplete.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select if 'Randomisation' has been completed.");
            //}

            //if (GeneralRoutines.IsNumeric(txtNumberRenalArteries.Text) == false)
            //{
            //    throw new Exception("Please Enter 'Number of Renal Arteries' in numeric format.");
            //}

            //if (rblDamage.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select an option for Damage."); 
            //}
            //loop through checkboxlist to see if at least one item has been selected

            if (rblKidneyTransplantable.SelectedValue==STR_NO_SELECTION)
            {
                Page.Validate("SecondaryLeft");
            }

            if (rblKidneyTransplantable_R.SelectedValue == STR_NO_SELECTION)
            {
                Page.Validate("SecondaryRight");
            }

            if (Page.IsValid==false)
            {
                throw new Exception("Please Check the data you have entered.");
            }
            Int32 iSelected = 0;
            foreach (ListItem listItem in cblArterialProblems.Items )
            {
                if (listItem.Selected)
                {
                    //do some work 
                    iSelected += 1;
                }
                else
                {
                    //do something else 
                }
            }

            if (iSelected==0)
            {
                throw new Exception("Please Select at least one option from " + lblArterialProblems.Text + " for Left kidney.");
            }

            ListItem li;
            //now check if None has been selected
            li = cblArterialProblems.Items.FindByValue("None");

            if (li.Selected == true)
            {
                if (iSelected > 1)
                {
                    throw new Exception("Since you have selected 'None' from " + lblArterialProblems.Text + " for Left kidney, only one selection is allowed. Please Tick only one CheckBox.");
                }
            }
            iSelected = 0;

            foreach (ListItem listItem in cblArterialProblems_R.Items)
            {
                if (listItem.Selected)
                {
                    //do some work 
                    iSelected += 1;
                }
                else
                {
                    //do something else 
                }
            }

            if (iSelected == 0)
            {
                throw new Exception("Please Select at least one option from " + lblArterialProblems.Text + " for Right kidney.");
            }


            li = cblArterialProblems_R.Items.FindByValue("None");

            if (li.Selected == true)
            {
                if (iSelected > 1)
                {
                    throw new Exception("Since you have selected 'None' from " + lblArterialProblems.Text + " for Right kidney, only one selection is allowed. Please Tick only one CheckBox.");
                }
            }

            if (txtRemovalDate.Text !=string.Empty)
            {
                if (GeneralRoutines.IsDate(txtRemovalDate.Text) == false)
                {
                    throw new Exception("Please enter 'Kidney Removal' Date.");
                }
                if (Convert.ToDateTime(txtRemovalDate.Text) > DateTime.Now)
                {
                    throw new Exception("Date when 'Left Kidney Removed' cannot be greater than Today's date.");
                }
            }


            if (txtRemovalTime.Text != string.Empty && txtRemovalTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtRemovalTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("Time Hour when 'Left Kidney was Removed' should be numeric.");
                }

                if (Convert.ToInt16(txtRemovalTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("Time Hour when 'Left Kidney was Removed' should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtRemovalTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("Time Minute when 'Left Kidney was Removed' should be numeric.");
                }

                if (Convert.ToInt16(txtRemovalTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("Time Minute when 'Left Kidney was Removed' should not be greater than 59.");
                }
            }


            if (txtRemoval_RDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtRemoval_RDate.Text) == false)
                {
                    throw new Exception("Please enter 'Right Kidney Removal' Date.");
                }
                if (Convert.ToDateTime(txtRemoval_RDate.Text) > DateTime.Now)
                {
                    throw new Exception("Date when Right 'Kidney Removed' cannot be greater than Today's date.");
                }
            }


            if (txtRemoval_RTime.Text != string.Empty && txtRemoval_RTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtRemoval_RTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("Time Hour when 'Right Kidney was Removed' should be numeric.");
                }

                if (Convert.ToInt16(txtRemoval_RTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("Time Hour when 'Right Kidney was Removed' should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtRemoval_RTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("Time Minute when 'Right Kidney was Removed' should be numeric.");
                }

                if (Convert.ToInt16(txtRemoval_RTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("Time Minute when 'Right Kidney was Removed' should not be greater than 59.");
                }
            }


            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(),  STRCONN));

            if (intCountFind > 1)
            {
                throw new Exception("Data already added for '" + Request.QueryString["TID"].ToString() + "'. Please Select existing data to Edit by clicking on the link below.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occcured when checking if data already added for '" + ddSide.SelectedValue + " Kidney'. ");
            }

            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }

            //now add data

            string STRSQL = string.Empty;
            STRSQL += "INSERT INTO kidneyinspection ";
            STRSQL += "(TrialID, PreservationModality, RandomisationComplete, NumberRenalArteries, ArterialProblems, ";
            STRSQL += "KidneyTransplantable, ReasonNotTransplantable, WashoutPerfusion, RemovalDate, RemovalTime,  ";
            STRSQL += "PreservationModality_R, RandomisationComplete_R, NumberRenalArteries_R, ArterialProblems_R, ";
            STRSQL += "KidneyTransplantable_R, ReasonNotTransplantable_R, WashoutPerfusion_R, Removal_RDate, Removal_RTime,";
            STRSQL += "Comments, Comments_R, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID,  ?PreservationModality, ?RandomisationComplete, ?NumberRenalArteries, ?ArterialProblems, ";
            STRSQL += "?KidneyTransplantable, ?ReasonNotTransplantable, ?WashoutPerfusion, ?RemovalDate, ?RemovalTime,  ";
            STRSQL += "?PreservationModality_R, ?RandomisationComplete_R, ?NumberRenalArteries_R,?ArterialProblems_R,     ";
            STRSQL += "?KidneyTransplantable_R, ?ReasonNotTransplantable_R, ?WashoutPerfusion_R,  ?Removal_RDate, ?Removal_RTime,";
            STRSQL += " ?Comments, ?Comments_R,?DateCreated, ?CreatedBy) ";
            STRSQL += " ";


            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE kidneyinspection SET ";
            STRSQL_UPDATE += "PreservationModality=?PreservationModality, RandomisationComplete=?RandomisationComplete, NumberRenalArteries=?NumberRenalArteries, ";
            STRSQL_UPDATE += "ArterialProblems=?ArterialProblems,  KidneyTransplantable=?KidneyTransplantable, ReasonNotTransplantable=?ReasonNotTransplantable,";
            STRSQL_UPDATE += "WashoutPerfusion=?WashoutPerfusion,RemovalDate=?RemovalDate, RemovalTime=?RemovalTime,";
            STRSQL_UPDATE += "PreservationModality_R=?PreservationModality_R, RandomisationComplete_R=?RandomisationComplete_R, NumberRenalArteries_R=?NumberRenalArteries_R,";
            STRSQL_UPDATE += "ArterialProblems_R=?ArterialProblems_R, KidneyTransplantable_R=?KidneyTransplantable_R, ReasonNotTransplantable_R=?ReasonNotTransplantable_R,  ";
            STRSQL_UPDATE += "WashoutPerfusion_R=?WashoutPerfusion_R, Removal_RDate=?Removal_RDate, Removal_RTime=?Removal_RTime,";
            STRSQL_UPDATE += "Comments=?Comments, Comments_R=?Comments_R, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


            //lock data
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE kidneyinspection SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE kidneyinspection SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;


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
                throw new Exception("More than One Donor Lab Results exists for this TrialID. Click on TrialID in the table below to select data to delete. ");
            }
            else
            {
                throw new Exception("An error occured while check if Donor Lab Results Data already exist in the database.");
            }

            //MyCMD.CommandType = CommandType.Text;
            //MyCMD.CommandText = STRSQL;


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            //if (ddSide.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;
            //}

            if (ddPreservationModality.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?PreservationModality", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PreservationModality", MySqlDbType.VarChar).Value = ddPreservationModality.SelectedValue;
            }

            if (ddPreservationModality_R.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?PreservationModality_R", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PreservationModality_R", MySqlDbType.VarChar).Value = ddPreservationModality_R.SelectedValue;
            }

            if (rblRandomisationComplete.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RandomisationComplete", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RandomisationComplete", MySqlDbType.VarChar).Value = rblRandomisationComplete.SelectedValue;
            }

            if (rblRandomisationComplete_R.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RandomisationComplete_R", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RandomisationComplete_R", MySqlDbType.VarChar).Value = rblRandomisationComplete_R.SelectedValue;
            }

            if (txtNumberRenalArteries.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NumberRenalArteries", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NumberRenalArteries", MySqlDbType.VarChar).Value = txtNumberRenalArteries.Text;
            }

            if (txtNumberRenalArteries_R.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NumberRenalArteries_R", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NumberRenalArteries_R", MySqlDbType.VarChar).Value = txtNumberRenalArteries_R.Text;
            }

            //append selection
            string strArterialProblems = string.Empty;
            //Set up connection and command objects
            //Open connection
            for (int i = 0; i < cblArterialProblems.Items.Count; i++)
            {
                strArterialProblems += cblArterialProblems.Items[i].Value + ":";
                if (cblArterialProblems.Items[i].Selected)
                {
                    strArterialProblems += STR_YES_SELECTION;
                }
                else
                {
                    strArterialProblems += STR_NO_SELECTION;
                }

                if (i < cblArterialProblems.Items.Count - 1)
                {
                    strArterialProblems += ",";
                }

            }

            if (strArterialProblems == string.Empty)
            {
                MyCMD.Parameters.Add("?ArterialProblems", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArterialProblems", MySqlDbType.VarChar).Value = strArterialProblems;
            }

            //append selection
            string strArterialProblems_R = string.Empty;
            //Set up connection and command objects
            //Open connection
            for (int i = 0; i < cblArterialProblems_R.Items.Count; i++)
            {
                strArterialProblems_R += cblArterialProblems_R.Items[i].Value + ":";
                if (cblArterialProblems_R.Items[i].Selected)
                {
                    strArterialProblems_R += STR_YES_SELECTION;
                }
                else
                {
                    strArterialProblems_R += STR_NO_SELECTION;
                }

                if (i < cblArterialProblems_R.Items.Count - 1)
                {
                    strArterialProblems_R += ",";
                }

            }

            if (strArterialProblems_R == string.Empty)
            {
                MyCMD.Parameters.Add("?ArterialProblems_R", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArterialProblems_R", MySqlDbType.VarChar).Value = strArterialProblems_R;
            }

            if (rblKidneyTransplantable.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?KidneyTransplantable", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?KidneyTransplantable", MySqlDbType.VarChar).Value = rblKidneyTransplantable.SelectedValue;
            }

            if (txtReasonNotTransplantable.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?ReasonNotTransplantable", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonNotTransplantable", MySqlDbType.VarChar).Value = txtReasonNotTransplantable.Text;
            }

            if (rblKidneyTransplantable_R.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?KidneyTransplantable_R", MySqlDbType.VarChar).Value = txtReasonNotTransplantable.Text;
            }
            else
            {
                MyCMD.Parameters.Add("?KidneyTransplantable_R", MySqlDbType.VarChar).Value = rblKidneyTransplantable_R.SelectedValue;
            }

            if (txtReasonNotTransplantable_R.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ReasonNotTransplantable_R", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonNotTransplantable_R", MySqlDbType.VarChar).Value = txtReasonNotTransplantable_R.Text;
            }


            if (ddWashoutPerfusion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?WashoutPerfusion", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?WashoutPerfusion", MySqlDbType.VarChar).Value = ddWashoutPerfusion.SelectedValue;
            }

            if (ddWashoutPerfusion_R.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?WashoutPerfusion_R", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?WashoutPerfusion_R", MySqlDbType.VarChar).Value = ddWashoutPerfusion_R.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtRemovalDate.Text) == false)
            {
                MyCMD.Parameters.Add("?RemovalDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RemovalDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtRemovalDate.Text);
            }

            if (txtRemovalTime.Text == string.Empty || txtRemovalTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?RemovalTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RemovalTime", MySqlDbType.VarChar).Value = txtRemovalTime.Text;
            }

            if (GeneralRoutines.IsDate(txtRemoval_RDate.Text) == false)
            {
                MyCMD.Parameters.Add("?Removal_RDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Removal_RDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtRemoval_RDate.Text);
            }

            if (txtRemoval_RTime.Text == string.Empty || txtRemoval_RTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?Removal_RTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Removal_RTime", MySqlDbType.VarChar).Value = txtRemoval_RTime.Text;
            }

            //if (rblVisiblePerfusionDefects.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?VisiblePerfusionDefects", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?VisiblePerfusionDefects", MySqlDbType.VarChar).Value = rblVisiblePerfusionDefects.SelectedValue;
            //}

            //if (GeneralRoutines.IsDate(txtMachinePerfusionStartDate.Text) == false)
            //{
            //    MyCMD.Parameters.Add("?MachinePerfusionStartDate", MySqlDbType.Date).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?MachinePerfusionStartDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtMachinePerfusionStartDate.Text);
            //}

            //if (txtMachinePerfusionStartTime.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?MachinePerfusionStartTime", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?MachinePerfusionStartTime", MySqlDbType.VarChar).Value = txtMachinePerfusionStartTime.Text;
            //}



            //if (txtRecipientTransplantationCentre.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?RecipientTransplantationCentre", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?RecipientTransplantationCentre", MySqlDbType.VarChar).Value = txtRecipientTransplantationCentre.Text;
            //}



            if (txtComments.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            if (txtComments_R.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Comments_R", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments_R", MySqlDbType.VarChar).Value = txtComments_R.Text;
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
                strSQLCOMPLETE += "IF(t2.ArterialProblems LIKE '%YES%'  ";
                strSQLCOMPLETE += "AND t2.ArterialProblems_R LIKE '%YES%' ";
                strSQLCOMPLETE += "AND t2.KidneyTransplantable IS NOT NULL AND IF(t2.KidneyTransplantable='NO', t2.ReasonNotTransplantable IS NOT NULL, t2.KidneyTransplantable IS NOT NULL)   ";
                strSQLCOMPLETE += "AND t2.KidneyTransplantable_R IS NOT NULL AND IF(t2.KidneyTransplantable_R='NO', t2.ReasonNotTransplantable_R IS NOT NULL, t2.KidneyTransplantable_R IS NOT NULL) ";
                //strSQLCOMPLETE += "AND t2.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM kidneyinspection t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialID=?TrialID ";
                strSQLCOMPLETE += "";

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialID", Request.QueryString["TID"], STRCONN);

                //lblDonorRiskIndex.Text = strSQLCOMPLETE;

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
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID ";

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
                if (String.IsNullOrEmpty(Request.QueryString["KidneyInspectionID"]))
                {
                    throw new Exception("More than one Record exists for deletion. Click on TrialID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM kidneyinspection ";
            STRSQL += "WHERE TrialID=?TrialID ";
            if (intCountFind > 1)
            {
                STRSQL += "AND KidneyInspectionID=?KidneyInspectionID ";
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

            if (!String.IsNullOrEmpty(Request.QueryString["KidneyInspectionID"]))
            {
                MyCMD.Parameters.Add("?KidneyInspectionID", MySqlDbType.VarChar).Value = Request.QueryString["KidneyInspectionID"].ToString();
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


    protected void rblKidneyTransplantable_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblKidneyTransplantable.SelectedValue==STR_NO_SELECTION)
            {
                txtReasonNotTransplantable.Visible = true ;

            }
            else
            {
                txtReasonNotTransplantable.Visible = false;
            }

            if (rblKidneyTransplantable.SelectedValue == STR_NO_SELECTION && rblKidneyTransplantable.SelectedValue == STR_NO_SELECTION)
            {
                pnlReasonNotTransplantable.Visible = true;
            }
            else
            {
                pnlReasonNotTransplantable.Visible = false;
            }
        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while selecting if Left Kidney is Transplantable.";
        }
    }

    protected void rblKidneyTransplantable_R_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblKidneyTransplantable_R.SelectedValue == STR_NO_SELECTION)
            {
                txtReasonNotTransplantable_R.Visible = true;

            }
            else
            {
                txtReasonNotTransplantable_R.Visible = false;
            }

            if (rblKidneyTransplantable.SelectedValue == STR_NO_SELECTION && rblKidneyTransplantable.SelectedValue == STR_NO_SELECTION)
            {
                pnlReasonNotTransplantable.Visible = true;
            }
            else
            {
                pnlReasonNotTransplantable.Visible = false;
            }
        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while selecting if Left Kidney is Transplantable.";
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

    //protected void ddSide_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;

    //        if (ddSide.SelectedValue == "Left")
    //        {
    //            string STRSQL_FindLeftSide = string.Empty;
    //            STRSQL_FindLeftSide = "SELECT KidneyLeftDonated FROM donor_identification WHERE TrialID=?TrialID ";

    //            string strLeftSide = GeneralRoutines.ReturnScalar(STRSQL_FindLeftSide, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN);


    //            if (strLeftSide == "-1")
    //            {
    //                throw new Exception("Could not check if Left Kidney was donated.");
    //            }
    //            else
    //            {
    //                if (strLeftSide == STR_YES_SELECTION) //assign preservation modality
    //                {
    //                    //find preservation modality from randomised data
    //                    string STRSQL_FindPreservationModality = string.Empty;
    //                    STRSQL_FindPreservationModality = "SELECT LeftRancategory FROM kidneyr WHERE TrialID=?TrialID ";

    //                    string strPreservationModality = GeneralRoutines.ReturnScalar(STRSQL_FindPreservationModality, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN);

    //                    if (strPreservationModality != "-1")
    //                    {
    //                        rblPreservationModality.SelectedValue = strPreservationModality;
    //                        rblRandomisationComplete.SelectedValue = STR_YES_SELECTION;
    //                    }
    //                    else
    //                    {
    //                        throw new Exception("Could not check Randomised Category for selected Kidney '" + ddSide.SelectedValue + "'");
    //                    }
    //                }
    //            }

    //        }

    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while selecting an item from the Drop Down box.";
    //    }
    //}
    
}