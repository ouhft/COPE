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

public partial class SpecClinicalData_AddRecipient : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const string STR_OCCASION = "0";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";


        //static Random _random = new Random();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsPostBack)
            {
                //if (String.IsNullOrEmpty(Request.QueryString["TID"]))
                //{
                //    throw new Exception("Could not obtain TrialID.");
                //}

                //string STRSQL = "SELECT t1.TrialID, CONCAT(t1.TrialID, ' --> DonorID ', t1.DonorID) TrialIDDetails FROM trialdetails t1 WHERE t1.TrialID LIKE ?TrialID ORDER BY t1.TrialID";

                string STRSQL = @"SELECT t1.*, CONCAT(t1.TrialID, ' --> DonorID ', t1.DonorID) TrialIDDetails FROM trialdetails t1
                            
                            ORDER BY TrialID";


                string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");

                sqldsTrialID.SelectCommand = STRSQL;
                sqldsTrialID.SelectParameters.Clear();
                //sqldsTrialID.SelectParameters.Add("?TrialID", strTrialIDLeadingCharacters + SessionVariablesAll.CentreCode + "%");

                ddTrialID.DataSource = sqldsTrialID;
                ddTrialID.DataBind();


                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                rblKidneyReceived.DataSource = XMLKidneySidesDataSource;
                rblKidneyReceived.DataBind();
                //rblKidneyReceived.SelectedValue = STR_UNKNOWN_SELECTION;

                ListItem li = rblKidneyReceived.Items.FindByValue(STR_UNKNOWN_SELECTION);

                if (li != null)
                {
                    rblKidneyReceived.Items.Remove(li);
                }

                //rv_txtRecipientDateOfBirth.MinimumValue = DateTime.Now.AddYears(ConstantsGeneral.intMinRecipientAge).ToShortDateString();
                //rv_txtRecipientDateOfBirth.MaximumValue = DateTime.Now.AddYears(ConstantsGeneral.intMaxRecipientAge).ToShortDateString();
                /////rv_txtRecipientDateOfBirth.MaximumValue = DateTime.Now.ToShortDateString();

                //rv_txtRecipientDateOfBirth.ErrorMessage = "Date should be between " + DateTime.Today.AddYears(ConstantsGeneral.intMinRecipientAge).ToShortDateString() + " and " + DateTime.Today.AddYears(ConstantsGeneral.intMaxRecipientAge).ToShortDateString();

                rv_txtRecipientDateOfBirth.MinimumValue = DateTime.Today.AddYears(ConstantsGeneral.intMinRecipientAge).ToShortDateString();
                rv_txtRecipientDateOfBirth.MaximumValue = DateTime.Today.AddYears(ConstantsGeneral.intMaxRecipientAge).ToShortDateString();
                rv_txtRecipientDateOfBirth.ErrorMessage = lblRecipientDateOfBirth.Text + " should be between " + rv_txtRecipientDateOfBirth.MinimumValue + " and " + rv_txtRecipientDateOfBirth.MaximumValue;

                rv_txtWeight.MinimumValue = ConstantsGeneral.dblMinDonorWeight.ToString();
                rv_txtWeight.MaximumValue = ConstantsGeneral.dblMaxDonorWeight.ToString();
                rv_txtWeight.ErrorMessage = "Value should be between " + ConstantsGeneral.dblMinDonorWeight.ToString() + " and " + ConstantsGeneral.dblMaxDonorWeight.ToString();
                txtWeight.ToolTip = "Recommended Value between " + ConstantsGeneral.dblMinDonorWeightRec.ToString() + " and " + ConstantsGeneral.dblMaxDonorWeightRec.ToString();

                rv_txtHeight.MinimumValue = ConstantsGeneral.intMinDonorHeight.ToString();
                rv_txtHeight.MaximumValue = ConstantsGeneral.intMaxDonorHeight.ToString();
                rv_txtHeight.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinDonorHeight.ToString() + " and " + ConstantsGeneral.intMaxDonorHeight.ToString();
                txtHeight.ToolTip = "Recommended Value between " + ConstantsGeneral.intMinDonorHeightRec.ToString() + " and " + ConstantsGeneral.intMaxDonorHeightRec.ToString();

                rblSex.DataSource = XMLSexDataSource;
                rblSex.DataBind();
                rblSex.SelectedValue = STR_UNKNOWN_SELECTION;

                //rfv_rblSex.InitialValue = STR_UNKNOWN_SELECTION;

                rblEthnicityBlack.DataSource = XMLMainOptionsDataSource;
                rblEthnicityBlack.DataBind();
                rblEthnicityBlack.SelectedValue = STR_UNKNOWN_SELECTION;

                //rfv_rblEthnicityBlack.InitialValue = STR_UNKNOWN_SELECTION;

                ddRenalDisease.DataSource = XMLRenalDiseasesDataSource;
                ddRenalDisease.DataBind();

                //rblDialysisType.DataSource = XMLDialysisTypesDataSource;
                //rblDialysisType.DataBind();
                //rblDialysisType.SelectedValue = STR_UNKNOWN_SELECTION;

                rv_txtPreTransplantDiuresis.MinimumValue = ConstantsGeneral.intMinDiuresis.ToString();
                rv_txtPreTransplantDiuresis.MaximumValue = ConstantsGeneral.intMaxDiuresis.ToString();
                rv_txtPreTransplantDiuresis.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinDiuresis.ToString() + " and " + ConstantsGeneral.intMaxDiuresis.ToString();

                rblBloodGroup.DataSource = XmlBloodGroupDataSource;
                rblBloodGroup.DataBind();
                rblBloodGroup.SelectedValue = STR_UNKNOWN_SELECTION;

                //rfv_rblBloodGroup.InitialValue = STR_UNKNOWN_SELECTION;

                //rblHLA_A.DataSource = XMLHLAMismatchDataSource;
                //rblHLA_A.DataBind();
                //rblHLA_A.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblHLA_B.DataSource = XMLHLAMismatchDataSource;
                //rblHLA_B.DataBind();
                //rblHLA_B.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblHLA_DR.DataSource = XMLHLAMismatchDataSource;
                //rblHLA_DR.DataBind();
                //rblHLA_DR.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblET_Urgency.DataSource = XMLETUrgenciesDataSource;
                //rblET_Urgency.DataBind();
                //rblET_Urgency.SelectedValue = STR_UNKNOWN_SELECTION;

                rblHLA_A_Mismatch.DataSource = XMLHLAMismatchDataSource;
                rblHLA_A_Mismatch.DataBind();
                rblHLA_A_Mismatch.SelectedValue = STR_UNKNOWN_SELECTION;
                //rfv_rblHLA_A_Mismatch.InitialValue = STR_UNKNOWN_SELECTION;

                rblHLA_B_Mismatch.DataSource = XMLHLAMismatchDataSource;
                rblHLA_B_Mismatch.DataBind();
                rblHLA_B_Mismatch.SelectedValue = STR_UNKNOWN_SELECTION;
                //rfv_rblHLA_B_Mismatch.InitialValue = STR_UNKNOWN_SELECTION;

                rblHLA_DR_Mismatch.DataSource = XMLHLAMismatchDataSource;
                rblHLA_DR_Mismatch.DataBind();
                rblHLA_DR_Mismatch.SelectedValue = STR_UNKNOWN_SELECTION;
                //rfv_rblHLA_DR_Mismatch.InitialValue = STR_UNKNOWN_SELECTION;

                txtDateQOLFIlled_CalendarExtender.EndDate = DateTime.Today;

                ddQOLFilledAt.DataSource = XMLQOLFilledOptionsDataSource;
                ddQOLFilledAt.DataBind();

                ddMobility.DataSource = XMLMobilityDataSource;
                ddMobility.DataBind();

                ddSelfCare.DataSource = XMLSelfCareDataSource;
                ddSelfCare.DataBind();

                ddUsualActivities.DataSource = XMLUsualActivitiesDataSource;
                ddUsualActivities.DataBind();

                ddPainDiscomfort.DataSource = XMLPainDiscomfortDataSource;
                ddPainDiscomfort.DataBind();

                ddAnxietyDepression.DataSource = XMLAnxietyDepressionDataSource;
                ddAnxietyDepression.DataBind();

                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;

                AssignMainDetails();

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

                lblDescription.Text = "Please Enter RecipientID Details for " + Request.QueryString["TID_R"];

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
            string STRSQL = string.Empty;
            STRSQL += "SELECT *, ";
            STRSQL += "DATE_FORMAT(DateOfBirth, '%d/%m/%Y') Date_OfBirth ";
            STRSQL += "FROM r_identification ";
            STRSQL += "WHERE TrialID=?TrialID ";
            STRSQL += "AND TrialIDRecipient = ?TrialIDRecipient ";
            //STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialID", Request.QueryString["TID"]);
            sqldsGV1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"]);
            sqldsGV1.SelectCommand = STRSQL;
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();
            lblGV1.Text = "Recipient ID Details";

            if (GV1.Rows.Count == 1)
            {

                AssignData();

                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                lblGV1.Text = "Summary of Recipient Identification Data";
            }
            else
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
            }
        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding data.";
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


    protected void AssignMainDetails()
    {
        try
        {
            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  trialdetails_recipient t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID AND t1.TrialIDRecipient=?TrialIDRecipient";

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

            if (string.IsNullOrEmpty(Request.QueryString["TID_R"]))
            {
                //do nothing
            }
            else
            {
                MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];
            }

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["TrialID"]))
                            {
                                ddTrialID.SelectedValue = (string)(myDr["TrialID"]);

                            }

                            if (!DBNull.Value.Equals(myDr["KidneyReceived"]))
                            {
                                rblKidneyReceived.SelectedValue = (string)(myDr["KidneyReceived"]);

                            }

                        }
                    }
                }

                if (MyCONN.State == ConnectionState.Open)
                { 
                    MyCONN.Close();
                
                }

            }
            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { 
                    MyCONN.Close(); 
                }
                
                lblUserMessages.Text = ex.Message + " An error occured while executing Assign query.";
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assigning main data.";
        }
    }
    protected void AssignData()
    {

        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  r_identification t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID AND t1.TrialIDRecipient=?TrialIDRecipient ";
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

            if (string.IsNullOrEmpty(Request.QueryString["TID_R"]))
            {
                //do nothing
            }
            else
            {
                MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];
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

                            //if (!DBNull.Value.Equals(myDr["KidneyReceived"]))
                            //{
                            //    rblKidneyReceived.SelectedValue = (string)(myDr["KidneyReceived"]);

                            //}

                            if (!DBNull.Value.Equals(myDr["RecipientID"]))
                            {
                                txtRecipientID.Text = (string)(myDr["RecipientID"]);

                            }

                            if (lblRecipientID.Font.Bold == true)
                            {
                                if (txtRecipientID.Text == string.Empty)
                                {
                                    lblRecipientID.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }



                            if (!DBNull.Value.Equals(myDr["DateOfBirth"]))
                            {

                                if (GeneralRoutines.IsDate(myDr["DateOfBirth"].ToString()))
                                {
                                    txtRecipientDateOfBirth.Text = Convert.ToDateTime(myDr["DateOfBirth"]).ToString("dd/MM/yyyy");

                                }

                            }

                            if (lblRecipientDateOfBirth.Font.Bold == true)
                            {
                                if (txtRecipientDateOfBirth.Text == string.Empty)
                                {
                                    lblRecipientDateOfBirth.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Sex"]))
                            {
                                rblSex.SelectedValue = (string)(myDr["Sex"]);

                            }
                            if (lblSex.Font.Bold == true)
                            {
                                if (rblSex.SelectedValue == STR_UNKNOWN_SELECTION || rblSex.SelectedIndex == -1)
                                {
                                    lblSex.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Weight"]))
                            {
                                txtWeight.Text = (string)(myDr["Weight"]);
                            }

                            if (lblWeight.Font.Bold == true)
                            {
                                if (txtWeight.Text == string.Empty)
                                {
                                    lblWeight.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }



                            if (!DBNull.Value.Equals(myDr["Height"]))
                            {
                                txtHeight.Text = (string)(myDr["Height"]);
                            }

                            if (lblHeight.Font.Bold == true)
                            {
                                if (txtHeight.Text == string.Empty)
                                {
                                    lblHeight.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["EthnicityBlack"]))
                            {
                                rblEthnicityBlack.SelectedValue = (string)(myDr["EthnicityBlack"]);
                            }

                            if (lblEthnicityBlack.Font.Bold == true)
                            {
                                if (rblEthnicityBlack.SelectedValue == STR_UNKNOWN_SELECTION || rblEthnicityBlack.SelectedIndex == -1)
                                {
                                    lblEthnicityBlack.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["RenalDisease"]))
                            {
                                ddRenalDisease.SelectedValue = (string)(myDr["RenalDisease"]);
                            }

                            if (!DBNull.Value.Equals(myDr["RenalDiseaseOther"]))
                            {
                                txtRenalDiseaseOther.Text = (string)(myDr["RenalDiseaseOther"]);
                            }

                            if (ddRenalDisease.SelectedValue == STR_UNKNOWN_SELECTION || ddRenalDisease.SelectedIndex == -1)
                            {
                                if (lblRenalDiseaseOther.Font.Bold == true)
                                {
                                    if (txtRenalDiseaseOther.Text == string.Empty)
                                    {
                                        lblRenalDiseaseOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                    }
                                }
                            }


                            //if (!DBNull.Value.Equals(myDr["DateOnDialysisSince"]))
                            //{
                            //    if (GeneralRoutines.IsDate(myDr["DateOnDialysisSince"].ToString()))
                            //    {
                            //        txtDateOnDialysisSince.Text = Convert.ToDateTime(myDr["DateOnDialysisSince"]).ToString("dd/MM/yyyy");

                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["DialysisType"]))
                            //{
                            //    rblDialysisType.SelectedValue = (string)(myDr["DialysisType"]);
                            //}

                            if (!DBNull.Value.Equals(myDr["PreTransplantDiuresis"]))
                            {
                                txtPreTransplantDiuresis.Text = (string)(myDr["PreTransplantDiuresis"]);
                            }


                            if (lblPreTransplantDiuresis.Font.Bold == true)
                            {
                                if (txtPreTransplantDiuresis.Text == string.Empty)
                                {
                                    lblPreTransplantDiuresis.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["PannelReactiveAntibodies"]))
                            //{
                            //    txtPannelReactiveAntibodies.Text = (string)(myDr["PannelReactiveAntibodies"]);
                            //}


                            if (!DBNull.Value.Equals(myDr["BloodGroup"]))
                            {
                                rblBloodGroup.SelectedValue = (string)(myDr["BloodGroup"]);

                            }

                            if (lblBloodGroup.Font.Bold == true)
                            {
                                if (rblBloodGroup.SelectedValue == STR_UNKNOWN_SELECTION || rblBloodGroup.SelectedIndex == -1)
                                {
                                    lblBloodGroup.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["NumberPreviousTransplants"]))
                            //{
                            //    txtNumberPreviousTransplants.Text = (string)(myDr["NumberPreviousTransplants"]);
                            //}


                            //if (!DBNull.Value.Equals(myDr["HLA_A"]))
                            //{
                            //    rblHLA_A.SelectedValue = (string)(myDr["HLA_A_Mismatch"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["HLA_B"]))
                            //{
                            //    rblHLA_B.SelectedValue = (string)(myDr["HLA_B"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["HLA_DR"]))
                            //{
                            //    rblHLA_DR.SelectedValue = (string)(myDr["HLA_DR"]);
                            //}

                            if (!DBNull.Value.Equals(myDr["HLA_A_Mismatch"]))
                            {
                                //txtHLA_A_Mismatch.Text = (string)(myDr["HLA_A_Mismatch"]);
                                rblHLA_A_Mismatch.SelectedValue = (string)(myDr["HLA_A_Mismatch"]);
                            }

                            if (lblHLA_A_Mismatch.Font.Bold == true)
                            {
                                if (rblHLA_A_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION || rblHLA_A_Mismatch.SelectedIndex == -1)
                                {
                                    lblHLA_A_Mismatch.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["HLA_B_Mismatch"]))
                            {
                                //txtHLA_B_Mismatch.Text = (string)(myDr["HLA_B_Mismatch"]);
                                rblHLA_B_Mismatch.SelectedValue = (string)(myDr["HLA_B_Mismatch"]);
                            }

                            if (lblHLA_B_Mismatch.Font.Bold == true)
                            {
                                if (rblHLA_B_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION || rblHLA_B_Mismatch.SelectedIndex == -1)
                                {
                                    lblHLA_B_Mismatch.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["HLA_DR_Mismatch"]))
                            {
                                //txtHLA_DR_Mismatch.Text = (string)(myDr["HLA_DR_Mismatch"]);
                                rblHLA_DR_Mismatch.SelectedValue = (string)(myDr["HLA_DR_Mismatch"]);
                            }

                            if (lblHLA_DR_Mismatch.Font.Bold == true)
                            {
                                if (rblHLA_DR_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION || rblHLA_DR_Mismatch.SelectedIndex == -1)
                                {
                                    lblHLA_DR_Mismatch.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DateQOLFilled"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateQOLFilled"].ToString()))
                                {
                                    txtDateQOLFIlled.Text = Convert.ToDateTime(myDr["DateQOLFilled"]).ToShortDateString();
                                }

                            }

                            if (lblDateQOLFIlled.Font.Bold == true)
                            {
                                if (txtDateQOLFIlled.Text == string.Empty)
                                {
                                    lblDateQOLFIlled.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["QOLFilledAt"]))
                            {
                                ddQOLFilledAt.SelectedValue = myDr["QOLFilledAt"].ToString();
                            }

                            if (lblQOLFilledAt.Font.Bold == true)
                            {
                                if (ddQOLFilledAt.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddQOLFilledAt.SelectedIndex == -1)
                                {
                                    lblQOLFilledAt.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Mobility"]))
                            {
                                ddMobility.SelectedValue = myDr["Mobility"].ToString();
                            }

                            if (lblMobility.Font.Bold == true)
                            {
                                if (ddMobility.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddMobility.SelectedIndex == -1)
                                {
                                    lblMobility.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["SelfCare"]))
                            {
                                ddSelfCare.SelectedValue = myDr["SelfCare"].ToString();
                            }

                            if (lblSelfCare.Font.Bold == true)
                            {
                                if (ddSelfCare.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddSelfCare.SelectedIndex == -1)
                                {
                                    lblSelfCare.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["UsualActivities"]))
                            {
                                ddUsualActivities.SelectedValue = myDr["UsualActivities"].ToString();
                            }

                            if (lblUsualActivities.Font.Bold == true)
                            {
                                if (ddUsualActivities.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddUsualActivities.SelectedIndex == -1)
                                {
                                    lblUsualActivities.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PainDiscomfort"]))
                            {
                                ddPainDiscomfort.SelectedValue = myDr["PainDiscomfort"].ToString();
                            }

                            if (lblPainDiscomfort.Font.Bold == true)
                            {
                                if (ddPainDiscomfort.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddPainDiscomfort.SelectedIndex == -1)
                                {
                                    lblPainDiscomfort.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["AnxietyDepression"]))
                            {
                                ddAnxietyDepression.SelectedValue = myDr["AnxietyDepression"].ToString();
                            }

                            if (lblAnxietyDepression.Font.Bold == true)
                            {
                                if (ddAnxietyDepression.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddAnxietyDepression.SelectedIndex == -1)
                                {
                                    lblAnxietyDepression.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["VASScore"]))
                            {
                                txtVASScore.Text = myDr["VASScore"].ToString();
                            }

                            if (lblVASScore.Font.Bold == true)
                            {
                                if (txtVASScore.Text == string.Empty)
                                {
                                    lblVASScore.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
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


    // reset page
    protected void cmdReset_Click(object sender, EventArgs e)
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

    // add data
    protected void cmdAddData_Click(object sender, EventArgs e)
    {

        try
        {
            lblUserMessages.Text = "";

            if (Page.IsValid == false)
            {
                throw new Exception("Please check the data you have entered.");
            }

            if (ddRenalDisease.SelectedValue == STR_OTHER_SELECTION)
            {
                Page.Validate("RenalDiseaseOther");

                if (Page.IsValid == false)
                {
                    throw new Exception("Please check the data you have entered.");
                }

            }


            if (ddTrialID.SelectedValue == "0")
            {
                throw new Exception("Please Select a Donor TrialID");
            }
            if (rblKidneyReceived.SelectedValue == STR_UNKNOWN_SELECTION)
            { throw new Exception("Please Select Side of the Kidney received."); }


            //if (txtRecipientID.Text == string.Empty)
            //{
            //    throw new Exception("Please enter the Recipient ID.");
            //}

            if (txtRecipientID.Text != string.Empty)
            {
                txtRecipientID.Text.Trim();
            }
           

            if (txtRecipientDateOfBirth.Text!=string.Empty)
            {
                if (GeneralRoutines.IsDate(txtRecipientDateOfBirth.Text) == false)
                {
                    throw new Exception("Please enter the Date of Birth of the Recipient as DD/MM/YYYY");
                }
            }
            

            //if (Convert.ToDateTime(txtRecipientDateOfBirth.Text) > DateTime.Today)
            //{
            //    throw new Exception("Date of Birth of Recipient cannot be greater than Today's date.");
            //}

            //if (Convert.ToDateTime(txtRecipientDateOfBirth.Text) < DateTime.Now.AddYears(ConstantsGeneral.intMinRecipientAge))
            //{
            //    throw new Exception("The minimum age of the Recipient should at least be " + intMinAge.ToString() + " years.");
            //}

            //if (rblSex.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select Recipient's Sex.");
            //}

            //if (txtHeight.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtHeight.Text) == false)
            //    {
            //        throw new Exception("Height should be numeric.");
            //    }
            //}

            //if (txtWeight.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtWeight.Text) == false)
            //    {
            //        throw new Exception("Weight should be numeric.");
            //    }
            //}


            //if (rblEthnicityBlack.SelectedValue == STR_OTHER_SELECTION)
            //{
            //    throw new Exception("Please Select YES/NO for Recipient's Ethnicity if Black.");
            //}

            //if (ddRenalDisease.SelectedValue == "0")
            //{
            //    throw new Exception("Please Select Renal Disease.");
            //}

            //if (txtNumberPreviousTransplants.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtNumberPreviousTransplants.Text) == false)
            //    {
            //        throw new Exception("Please Enter Number of previous transplants in numeric format.");

            //    }
            //}

            if (txtDateQOLFIlled.Text!=string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDateQOLFIlled.Text) == false)
                {
                    throw new Exception("Please Enter " + lblDateQOLFIlled.Text + " as DD/MM/YYYY.");
                }

                if (Convert.ToDateTime(txtDateQOLFIlled.Text).Date>DateTime.Today.Date)
                {
                    throw new Exception("" + lblDateQOLFIlled.Text + " cannot be later than Today's date.");
                }
            }
            
            if (txtVASScore.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtVASScore.Text) == false)
                {
                    throw new Exception("Please Enter value for 'VAS Score' in Numeric Format.");
                }

                if (Convert.ToInt16(txtVASScore.Text) < 0)
                {
                    throw new Exception("'VAS Score' cannot be less than 0.");
                }

                if (Convert.ToInt16(txtVASScore.Text) > 100)
                {
                    if (Convert.ToInt16(txtVASScore.Text) != 999)
                    {
                        throw new Exception("'VAS Score' cannot be greater than 100 (Only exception is 999 for missing value).");
                    }
                }

            }

            //if (rblHLA_A.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA A Typing.");
            //}

            //if (rblHLA_B.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA B Typing.");
            //}

            //if (rblHLA_DR.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA DR Typing.");
            //}

            //if (rblET_Urgency.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'ET Urgency");
            //}

            //if (rblHLA_A_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA A Mismatch");
            //}

            //if (rblHLA_B_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA B Mismatch");
            //}

            //if (rblHLA_DR_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA DR Mismatch");
            //}

            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }


            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_identification WHERE TrialIDRecipient=?TrialIDRecipient ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));


            if (intCountFind > 1)
            {
                //STRSQL_FIND = "SELECT RecipientID FROM r_identification WHERE TrialIDRecipient=?TrialIDRecipient  ";
                //string strRecipientID = GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);
                //throw new Exception("A Recipient " + strRecipientID + " has already been added for TrialID " + ddTrialID.SelectedValue + " with " + rblKidneyReceived.SelectedValue + " Kidney. Please Select another Donor/Kidney combination.");
                throw new Exception("More than one RecipientIDs have been associated with this TrialID.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Data for " + rblKidneyReceived.SelectedValue + " Kidney already exists for this TrialID.");
            }

            //check if recipient already exists in Europe if a Europe donor/ UK if a UK donor. Country Code for UK is 3

            if (txtRecipientID.Text!=string.Empty)
            {
                STRSQL_FIND = "SELECT COUNT(*) CR FROM r_identification WHERE RecipientID=?RecipientID AND TrialIDRecipient <> ?TrialIDRecipient ";

                if (SessionVariablesAll.CentreCode.Substring(0, 1) == "1") //UK centrecode
                {
                    STRSQL_FIND += "AND TrialIDRecipient LIKE 'WP41%' ";
                }
                else
                {
                    STRSQL_FIND += "AND TrialIDRecipient NOT LIKE 'WP41%' ";
                }

                intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?RecipientID", txtRecipientID.Text, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));

                if (intCountFind > 0)
                {
                    //get the trialid for the RecipientID
                    STRSQL_FIND = "SELECT TrialID FROM r_identification WHERE RecipientID=?RecipientID ";

                    string strTrialIDRecipientID = string.Empty;
                    strTrialIDRecipientID = GeneralRoutines.ReturnScalar(STRSQL_FIND, "?RecipientID", txtRecipientID.Text, STRCONN);
                    throw new Exception("RecipientID " + txtRecipientID.Text + " has already been associated with " + strTrialIDRecipientID + ". Please check the RecipientID you are adding.");
                }

                if (intCountFind < 0)
                {
                    throw new Exception("An error occured while checking if another RecipientID exists." + STRSQL_FIND);
                }
            }
            

            ////check if trialID recipient alrady exists
            //STRSQL_FIND = "SELECT COUNT(*) CR FROM r_identification WHERE TrialIDRecipient = ?TrialIDRecipient ";

            //intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));

            //if (intCountFind>1)
            //{
            //    throw new Exception("More than One recipients exists with the same TrialID (Recipient) identifier.");
            //}

            //if (intCountFind <0)
            //{
            //    throw new Exception("An error occured while checking if more than One recipients exists with the same TrialID (Recipient) identifier.");
            //}

            //now add the data
            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO r_identification ";
            STRSQL += "(TrialID, TrialIDRecipient, RecipientID, DateOfBirth, Sex, Weight,  Height, BMI, EthnicityBlack,  ";
            //STRSQL += "RenalDisease, RenalDiseaseOther, NumberPreviousTransplants, PretransplantDiuresis, BloodGroup,  ";
            STRSQL += "RenalDisease, RenalDiseaseOther, PretransplantDiuresis, BloodGroup,  ";
            STRSQL += "HLA_A_Mismatch, HLA_B_Mismatch, HLA_DR_Mismatch, ";
            STRSQL += "Occasion, DateQOLFIlled, QOLFIlledAt, Mobility, SelfCare, UsualActivities, PainDiscomfort, AnxietyDepression, VASScore,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?TrialIDRecipient, ?RecipientID, ?DateOfBirth, ?Sex, ?Weight, ?Height, ?BMI, ?EthnicityBlack,  ";
            //STRSQL += "?RenalDisease, ?RenalDiseaseOther, ?NumberPreviousTransplants, ?PretransplantDiuresis, ?BloodGroup,  ";
            STRSQL += "?RenalDisease, ?RenalDiseaseOther, ?PretransplantDiuresis, ?BloodGroup,  ";
            STRSQL += "?HLA_A_Mismatch, ?HLA_B_Mismatch, ?HLA_DR_Mismatch, ";
            STRSQL += "?Occasion, ?DateQOLFIlled, ?QOLFIlledAt, ?Mobility, ?SelfCare, ?UsualActivities, ?PainDiscomfort, ?AnxietyDepression, ?VASScore,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE r_identification SET ";
            STRSQL_UPDATE += "RecipientID=?RecipientID,  DateOfBirth=?DateOfBirth, Sex=?Sex, Weight=?Weight,   ";
            STRSQL_UPDATE += "Height=?Height, BMI=?BMI, EthnicityBlack=?EthnicityBlack, RenalDisease=?RenalDisease, RenalDiseaseOther=?RenalDiseaseOther, ";
            //STRSQL_UPDATE += "Height=?Height, EthnicityBlack=?EthnicityBlack, RenalDisease=?RenalDisease, RenalDiseaseOther=?RenalDiseaseOther, ";
            STRSQL_UPDATE += "PretransplantDiuresis=?PretransplantDiuresis, BloodGroup=?BloodGroup, ";
            //STRSQL_UPDATE += "HLA_A=?HLA_A, HLA_B=?HLA_B, HLA_DR=?HLA_DR,";
            STRSQL_UPDATE += "HLA_A_Mismatch=?HLA_A_Mismatch, HLA_B_Mismatch=?HLA_B_Mismatch, HLA_DR_Mismatch=?HLA_DR_Mismatch,Comments=?Comments,";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "Occasion=?Occasion, DateQOLFIlled=?DateQOLFIlled, QOLFIlledAt=?QOLFIlledAt, Mobility=?Mobility, SelfCare=?SelfCare, UsualActivities=?UsualActivities, ";
            STRSQL_UPDATE += "PainDiscomfort=?PainDiscomfort, AnxietyDepression=?AnxietyDepression, VASScore=?VASScore,";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient ";

            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE r_identification SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            //STRSQL_LOCK += "AND DateQOLFilled IS NOT NULL AND QOLFilledAt IS NOT NULL ";
            //STRSQL_LOCK += "AND Mobility IS NOT NULL AND SelfCare IS NOT NULL AND UsualActivities IS NOT NULL ";
            //STRSQL_LOCK += "AND PainDiscomfort IS NOT NULL AND AnxietyDepression IS NOT NULL AND VASScore IS NOT NULL ";

            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE r_identification SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient ";


            if (intCountFind == 1)
            {
                STRSQL = STRSQL_UPDATE;
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

            //MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = ddTrialID.SelectedValue;

            //MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = ddTrialID.SelectedValue + rblKidneyReceived.SelectedValue.Substring(0, 1);

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            //MyCMD.Parameters.Add("?KidneyReceived", MySqlDbType.VarChar).Value = rblKidneyReceived.SelectedValue;

            if (txtRecipientID.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?RecipientID", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientID", MySqlDbType.VarChar).Value = txtRecipientID.Text;
            }
            
            if (GeneralRoutines.IsDate(txtRecipientDateOfBirth.Text)==false)
            {
                MyCMD.Parameters.Add("?DateOfBirth", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateOfBirth", MySqlDbType.Date).Value = Convert.ToDateTime(txtRecipientDateOfBirth.Text);
            }
            

            if (rblSex.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?Sex", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {

                MyCMD.Parameters.Add("?Sex", MySqlDbType.VarChar).Value = rblSex.SelectedValue;
            }
            
            if (txtHeight.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?Height", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Height", MySqlDbType.VarChar).Value = txtHeight.Text;
            }

            

            if (txtWeight.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Weight", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Weight", MySqlDbType.VarChar).Value = txtWeight.Text;
            }

            //if (txtAdmissionWeight.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?AdmissionWeight", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?AdmissionWeight", MySqlDbType.VarChar).Value = txtAdmissionWeight.Text;
            //}

            //bmi uses admission weight
            if (txtHeight.Text != string.Empty && GeneralRoutines.IsNumeric(txtHeight.Text) && txtWeight.Text != string.Empty && GeneralRoutines.IsNumeric(txtWeight.Text))
            { MyCMD.Parameters.Add("?BMI", MySqlDbType.VarChar).Value = Math.Round(Convert.ToDouble(txtWeight.Text) / (Convert.ToDouble(txtHeight.Text) / 100 * Convert.ToDouble(txtHeight.Text) / 100), 3); }
            else
            { MyCMD.Parameters.Add("?BMI", MySqlDbType.VarChar).Value = DBNull.Value; }

            

            if (rblEthnicityBlack.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?EthnicityBlack", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?EthnicityBlack", MySqlDbType.VarChar).Value = rblEthnicityBlack.SelectedValue;
            }

            if (ddRenalDisease.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RenalDisease", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RenalDisease", MySqlDbType.VarChar).Value = ddRenalDisease.SelectedValue;
            }

            if (txtRenalDiseaseOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RenalDiseaseOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RenalDiseaseOther", MySqlDbType.VarChar).Value = txtRenalDiseaseOther.Text;
            }

            //if (txtNumberPreviousTransplants.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?NumberPreviousTransplants", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?NumberPreviousTransplants", MySqlDbType.VarChar).Value = txtNumberPreviousTransplants.Text;
            //}

            if (txtPreTransplantDiuresis.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PreTransplantDiuresis", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PreTransplantDiuresis", MySqlDbType.VarChar).Value = txtPreTransplantDiuresis.Text;
            }

            if (rblBloodGroup.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?BloodGroup", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?BloodGroup", MySqlDbType.VarChar).Value = rblBloodGroup.SelectedValue;
            }

            //if (rblHLA_A.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?HLA_A", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HLA_A", MySqlDbType.VarChar).Value = rblHLA_A.SelectedValue;
            //}

            //if (rblHLA_B.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?HLA_B", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HLA_B", MySqlDbType.VarChar).Value = rblHLA_B.SelectedValue;
            //}

            //if (rblHLA_DR.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?HLA_DR", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HLA_DR", MySqlDbType.VarChar).Value = rblHLA_DR.SelectedValue;
            //}

            //if (rblET_Urgency.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?ET_Urgency", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?ET_Urgency", MySqlDbType.VarChar).Value = rblET_Urgency.SelectedValue;
            //}

            if (rblHLA_A_Mismatch.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?HLA_A_Mismatch", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_A_Mismatch", MySqlDbType.VarChar).Value = rblHLA_A_Mismatch.SelectedValue;
            }

            if (rblHLA_B_Mismatch.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?HLA_B_Mismatch", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_B_Mismatch", MySqlDbType.VarChar).Value = rblHLA_B_Mismatch.SelectedValue;
            }

            if (rblHLA_DR_Mismatch.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?HLA_DR_Mismatch", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_DR_Mismatch", MySqlDbType.VarChar).Value = rblHLA_DR_Mismatch.SelectedValue;
            }

            //Quality of life table
            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = STR_OCCASION;

            if (GeneralRoutines.IsDate(txtDateQOLFIlled.Text) == false)
            {
                MyCMD.Parameters.Add("?DateQOLFIlled", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateQOLFIlled", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateQOLFIlled.Text);
            }

            if (ddQOLFilledAt.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?QOLFilledAt", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?QOLFilledAt", MySqlDbType.VarChar).Value = ddQOLFilledAt.SelectedValue;
            }

            if (ddMobility.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?Mobility", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Mobility", MySqlDbType.VarChar).Value = ddMobility.SelectedValue;
            }

            if (ddSelfCare.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?SelfCare", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SelfCare", MySqlDbType.VarChar).Value = ddSelfCare.SelectedValue;
            }

            if (ddUsualActivities.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?UsualActivities", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UsualActivities", MySqlDbType.VarChar).Value = ddUsualActivities.SelectedValue;
            }

            if (ddPainDiscomfort.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?PainDiscomfort", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PainDiscomfort", MySqlDbType.VarChar).Value = ddPainDiscomfort.SelectedValue;
            }

            if (ddAnxietyDepression.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?AnxietyDepression", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AnxietyDepression", MySqlDbType.VarChar).Value = ddAnxietyDepression.SelectedValue;
            }

            if (txtVASScore.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?VASScore", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?VASScore", MySqlDbType.VarChar).Value = txtVASScore.Text;
            }

            if (string.IsNullOrEmpty(txtComments.Text))
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;


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
                lblUserMessages.Text = "Recipient Data Added.";


                //redirect to summary page if data complete is not no
                string strSQLCOMPLETE = string.Empty;

                strSQLCOMPLETE += "SELECT  ";
                strSQLCOMPLETE += "IF(t2.Weight IS NOT NULL AND t2.Height IS NOT NULL AND t2.Sex IS NOT NULL AND t2.EthnicityBlack IS NOT NULL ";
                strSQLCOMPLETE += "AND IF(t2.RenalDisease ='" + STR_OTHER_SELECTION + "',  t2.RenalDiseaseOther IS NOT NULL, t2.RenalDisease IS NOT NULL) ";
                strSQLCOMPLETE += "AND t2.PreTransplantDiuresis IS NOT NULL AND t2.BloodGroup IS NOT NULL AND t2.HLA_A_Mismatch IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.HLA_B_Mismatch IS NOT NULL AND t2.HLA_DR_Mismatch IS NOT NULL ";
                strSQLCOMPLETE += "";
                strSQLCOMPLETE += "AND t2.DateQOLFilled IS NOT NULL AND t2.QOLFilledAt IS NOT NULL AND ";
                strSQLCOMPLETE += "t2.Mobility IS NOT NULL AND t2.SelfCare IS NOT NULL AND t2.UsualActivities IS NOT NULL AND t2.PainDiscomfort IS NOT NULL AND t2.AnxietyDepression IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.VASScore IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM r_identification t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialIDRecipient=?TrialIDRecipient ";

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                //redirect to summary page
                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
                }
                else
                {
                    if (Request.Url.AbsoluteUri.Contains("&SCode=1"))
                    {
                        Response.Redirect(Request.Url.AbsoluteUri, false);
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
                myTrans.Rollback();
                if (MyCONN.State == ConnectionState.Open)
                { 
                    MyCONN.Close();
                }
                
                lblUserMessages.Text = ex.Message + " An error occured while executing insert/update query.";
            }


            

            //BindData(ddTrialID.SelectedValue + rblKidneyReceived.SelectedValue.Substring(0, 1));
            

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }

    //protected void ddTrialID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;

    //        lblDonorIDDetails.Text = string.Empty;

    //        if (ddTrialID.SelectedValue != "0")
    //        {

    //            pnlTrialIDDetails.Visible = true;
    //            ////populate txtDonorID
    //            //string STRSQL = string.Empty; 
    //            //STRSQL +="SELECT t1.TrialID, t1.DonorID, t1.DateOfBirthDonor, t2.KidneyLeftDonated, t2.KidneyRightDonated ";
    //            //STRSQL += "FROM trialdetails t1  ";
    //            //STRSQL += "LEFT JOIN donor_identification t2 ON t1.TrialID=t2.TrialID ";
    //            //STRSQL += "WHERE t1.TrialID=?TrialID ";

    //            //string CS = string.Empty;
    //            //CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

    //            //MySqlConnection MyCONN = new MySqlConnection();
    //            //MyCONN.ConnectionString = CS;

    //            //MySqlCommand MyCMD = new MySqlCommand();
    //            //MyCMD.Connection = MyCONN;

    //            //MyCMD.CommandType = CommandType.Text;
    //            //MyCMD.CommandText = STRSQL;

    //            //MyCMD.Parameters.Clear();

    //            //MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = ddTrialID.SelectedValue;

    //            //MyCONN.Open();

    //            //try
    //            //{
    //            //    using (MySqlDataReader myDr = MyCMD.ExecuteReader())
    //            //    {
    //            //        if (myDr.HasRows)
    //            //        {
    //            //            while (myDr.Read())
    //            //            {
    //            //                lblDonorIDDetails.Text = "DonorID- ";
    //            //                if (!DBNull.Value.Equals(myDr["DonorID"]))
    //            //                {
    //            //                    lblDonorIDDetails.Text +="<b>" + (string)(myDr["DonorID"]) + "</b>";
    //            //                }
    //            //                lblDonorIDDetails.Text += "; Left - ";
    //            //                if (!DBNull.Value.Equals(myDr["KidneyLeftDonated"]))
    //            //                {
    //            //                    lblDonorIDDetails.Text += "<b>" + (string)(myDr["KidneyLeftDonated"]) + "</b>";
    //            //                }
    //            //                else
    //            //                {
    //            //                    lblDonorIDDetails.Text += "<b>Not Entered</b>";
    //            //                }

    //            //                lblDonorIDDetails.Text += "; Right - ";
    //            //                if (!DBNull.Value.Equals(myDr["KidneyRightDonated"]))
    //            //                {
    //            //                    lblDonorIDDetails.Text += "<b>" + (string)(myDr["KidneyRightDonated"]) + "</b>";
    //            //                }
    //            //                else
    //            //                {
    //            //                    lblDonorIDDetails.Text += "<b>Not Entered</b>";
    //            //                }
    //            //            }
    //            //        }
    //            //    }

    //            //    if (MyCONN.State == ConnectionState.Open)
    //            //    { 
    //            //        MyCONN.Close(); 
    //            //    }
    //            //}

    //            //catch (System.Exception ex)
    //            //{
    //            //    if (MyCONN.State == ConnectionState.Open)
    //            //    { 
    //            //        MyCONN.Close(); 
    //            //    }
    //            //    lblUserMessages.Text = ex.Message + " An error occured while assiging Donor Data";
    //            //}

    //            //recipient details add

    //            string STRSQL_R = string.Empty;
    //            STRSQL_R += "SELECT t1.TrialID, t1.DonorID, t2.TrialIDRecipient, t2.RecipientID, t2.KidneyReceived ";
    //            STRSQL_R += "FROM trialdetails t1  ";
    //            STRSQL_R += "INNER JOIN r_identification t2 ON t1.TrialID=t2.TrialID ";
    //            STRSQL_R += "WHERE t1.TrialID=?TrialID ORDER BY KidneyReceived ";

    //            string CS = string.Empty;
    //            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

    //            MySqlConnection MyCONN = new MySqlConnection();
    //            MyCONN.ConnectionString = CS;

    //            MySqlCommand MyCMD = new MySqlCommand();
    //            MyCMD.Connection = MyCONN;

    //            MyCMD.CommandType = CommandType.Text;
    //            MyCMD.CommandText = STRSQL_R;

    //            MyCMD.Parameters.Clear();

    //            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = ddTrialID.SelectedValue;

    //            MyCONN.Open();

    //            //MyCMD.CommandText = STRSQL_R;

    //            if (MyCONN.State == ConnectionState.Closed)
    //            {
    //                MyCONN.Open();
    //            }


    //            try
    //            {
    //                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
    //                {
    //                    if (myDr.HasRows)
    //                        lblDonorIDDetails.Text += " ; Recipient- ";
    //                    {
    //                        int iLoop = 0;

    //                        while (myDr.Read())
    //                        {

    //                            if (!DBNull.Value.Equals(myDr["TrialIDRecipient"]))
    //                            {
    //                                lblDonorIDDetails.Text += " " + (string)(myDr["TrialIDRecipient"]) + " ";
    //                            }

    //                            if (!DBNull.Value.Equals(myDr["RecipientID"]))
    //                            {
    //                                lblDonorIDDetails.Text += " " + (string)(myDr["RecipientID"]) + " ";
    //                            }

    //                            if (!DBNull.Value.Equals(myDr["KidneyReceived"]))
    //                            {
    //                                lblDonorIDDetails.Text += "<b>" + (string)(myDr["KidneyReceived"]) + "</b>";
    //                            }

    //                            if (iLoop == 0)
    //                            {
    //                                lblDonorIDDetails.Text += "; ";
    //                            }
    //                            iLoop += 1;
    //                        }
    //                    }
    //                }

    //                if (MyCONN.State == ConnectionState.Open)
    //                {
    //                    MyCONN.Close();
    //                }
    //            }

    //            catch (System.Exception ex)
    //            {
    //                if (MyCONN.State == ConnectionState.Open)
    //                {
    //                    MyCONN.Close();
    //                }
    //                lblUserMessages.Text += ex.Message + " An error occured while assiging Recipient Details";
    //            }

    //        }
    //        else
    //        {
    //            lblDonorIDDetails.Text = string.Empty;
    //            pnlTrialIDDetails.Visible = false;
    //        }
    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while selecting a TrialID.";
    //    }
    //}
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_identification WHERE TrialID=?TrialID AND TrialIDRecipient=?TrialIDRecipient";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?TrialIDRecipient", Request.QueryString["TrialIDRecipient"].ToString(), STRCONN));

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
                //if (String.IsNullOrEmpty(Request.QueryString["RIdentificationID"]))
                //{
                //    throw new Exception("More than one Record exists for deletion.");
                //}
                throw new Exception("More than one Record exists for deletion.");
            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM r_identification ";
            STRSQL += "WHERE TrialID=?TrialID AND TrialIDRecipient=?TrialIDRecipient ";

            if (!string.IsNullOrEmpty(Request.QueryString["RIdentificationID"]))
            {
                STRSQL += "AND RIdentificationID=?RIdentificationID ";
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
            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();

            if (!String.IsNullOrEmpty(Request.QueryString["RIdentificationID"]))
            {
                MyCMD.Parameters.Add("?RIdentificationID", MySqlDbType.VarChar).Value = Request.QueryString["RIdentificationID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                if (MyCONN.State == ConnectionState.Open)
                { 
                    MyCONN.Close(); 
                }

                BindData();

                lblUserMessages.Text = "Data Deleted.";

                Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);

            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing delete query.";
            }


            //finally //close connection
            //{
            //    if (MyCONN.State == ConnectionState.Open)
            //    { MyCONN.Close(); }
            //}

            

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