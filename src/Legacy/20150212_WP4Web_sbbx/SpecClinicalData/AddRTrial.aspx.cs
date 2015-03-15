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

using System.Collections;

using System.Net;
using System.Net.Mail;

public partial class SpecClinicalData_AddRTrial : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";
        //static Random _random = new Random();

    #endregion

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (string.IsNullOrEmpty(SessionVariablesAll.Randomise))
                {
                    Response.Redirect(strAccessDenied, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                if (SessionVariablesAll.Randomise != STR_YES_SELECTION)
                {
                    Response.Redirect(strAccessDenied, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                if (Request.QueryString["TID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID.");
                }


                ViewState["InspectionComplete"] = string.Empty;
                ViewState["BothKidneysTransplantable"] = string.Empty;

                string strSQLCOUNT = string.Empty;

                int intCount = 0;


                strSQLCOUNT = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID ";

                intCount = Convert.ToInt16(GeneralRoutines.ReturnScalar(strSQLCOUNT, "?TrialID", Request.QueryString["TID"], STRCONN));

                

                if (intCount==0)
                {
                    

                    cmdReset.Enabled = false;
                    cmdAddData.Enabled = false;
                    throw new Exception("Please complete the Inspection Page.");
                }

                if (intCount < 0)
                {
                    throw new Exception("An error occured while checking if Inspection Page has been completed.");
                }

                ViewState["InspectionComplete"] = intCount.ToString();


                strSQLCOUNT = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID AND KidneyTransplantable='YES' AND KidneyTransplantable_R='YES'";

                intCount = Convert.ToInt16(GeneralRoutines.ReturnScalar(strSQLCOUNT, "?TrialID", Request.QueryString["TID"], STRCONN));

                if (intCount == 0)
                {
                    cmdReset.Enabled = false;
                    cmdAddData.Enabled = false;

                    throw new Exception("Both the Kidneys should be Transplantable for Randomisation to be complete.");
                }

                if (intCount < 0)
                {
                    throw new Exception("An error occured while checking if Both the Kindeys are transplantable.");
                }

                ViewState["BothKidneysTransplantable"] = intCount.ToString();


                lblDescription.Text = "Please Complete all the details and then Click on Randomise to Randomise Kidney .";
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdAddData_ConfirmButtonExtender.ConfirmText = "Please Click on OK if you wish to Randomise Kidney(s) from this Donor.";

                rblAgeOrDateOfBirth.DataSource = XmlAgeOrDateOfBirthDataSource;
                rblAgeOrDateOfBirth.DataBind();

                rv_txtDonorAge.MinimumValue = (ConstantsGeneral.intMaxDonorAge * -1).ToString();
                rv_txtDonorAge.MaximumValue = (ConstantsGeneral.intMinDonorAge * -1).ToString();
                rv_txtDonorAge.ErrorMessage = "Donor Age should be between " + (ConstantsGeneral.intMaxDonorAge * -1).ToString() + " and " + (ConstantsGeneral.intMinDonorAge * -1).ToString() + ".";



                rb_txtDonorDateOfBirth.MinimumValue = DateTime.Today.AddYears(ConstantsGeneral.intMinDonorAge).ToShortDateString();
                rb_txtDonorDateOfBirth.MaximumValue = DateTime.Today.AddYears(ConstantsGeneral.intMaxDonorAge).ToShortDateString();
                rb_txtDonorDateOfBirth.ErrorMessage = lblDonorDateOfBirth.Text + " should be between " + rb_txtDonorDateOfBirth.MinimumValue + " and " + rb_txtDonorDateOfBirth.MaximumValue;
                    


                lblDonorEligibilityCriteria.Text = "I confirm that this Donor meets the eligibility criteria for inclusion in this trial i.e. ";
                lblDonorEligibilityCriteria.Text += "<br/>•	Maastricht category III DCD donor aged 50 years or older from the collaborating donor regions reported to Eurotransplant (ET) / National Health Service Blood and Transplant (NHSBT).";
                lblDonorEligibilityCriteria.Text += "<br/";
                lblDonorEligibilityCriteria.Text += "<br/>•	Both kidneys are deemed transplantable by the procurement surgeon";

                



                rblKidneyLeftDonated.DataSource = XMLKidneyYNMainDataSource;
                rblKidneyLeftDonated.DataBind();
                //rblKidneyLeftDonated.SelectedValue = STR_UNKNOWN_SELECTION;

                rblKidneyRightDonated.DataSource = XMLKidneyYNMainDataSource;
                rblKidneyRightDonated.DataBind();
                //rblKidneyRightDonated.SelectedValue = STR_UNKNOWN_SELECTION;

                rblInclusionCriteriaChecked.DataSource = XMLKidneyYNMainDataSource;
                rblInclusionCriteriaChecked.DataBind();
                //rblInclusionCriteriaChecked.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblExclusionCriteriaChecked.DataSource = XMLKidneyYNMainDataSource;
                //rblExclusionCriteriaChecked.DataBind();
                ////rblExclusionCriteriaChecked.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblConsentChecked.DataSource = XMLKidneyYNMainDataSource;
                //rblConsentChecked.DataBind();
                //rblConsentChecked.SelectedValue = STR_UNKNOWN_SELECTION;

                if (SessionVariablesAll.CentreCode != "")
                {
                    ddCentreCode.SelectedValue = SessionVariablesAll.CentreCode.ToString();
                }


                string STRSQL_TransplantCentres = string.Empty;
                string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                //if adminsuperuser then all access
                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION || strIsServer == "0")
                {
                    STRSQL_TransplantCentres += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged  
                                    FROM cope_wp_four.lstcentres  t1  
                                    ORDER BY t1.CountryCode, t1.CentreCode ";
                }
                else
                {
                    //only where add/edit=YES
                    STRSQL_TransplantCentres += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM cope_wp_four.lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    AND (t2.AddEditRecipient='" + STR_YES_SELECTION + "' OR  t2.AddEditFollowUp='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode ";


                }


                SQLDS_CentreCode.SelectCommand = STRSQL_TransplantCentres;
                SQLDS_CentreCode.SelectParameters.Clear();
                SQLDS_CentreCode.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

                if (!string.IsNullOrEmpty(SessionVariablesAll.CentreCode))
                {

                    SQLDS_CentreCode.SelectParameters.Add("?CountryCode", SessionVariablesAll.CentreCode);
                    ddCentreCode.DataSource = SQLDS_CentreCode;
                    ddCentreCode.DataBind();
                    //ddCountry.SelectedValue = SessionVariablesAll.CentreCode;

                }

                ViewState["SortField"] = "MainTrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

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
                    strDonorID = strDonorID.Replace("D ", "");
                }
            }

            string strSQL = String.Empty;

            strSQL += "SELECT t1.TrialID MainTrialID, t1.DonorID MainDonorID, t1.DateOfBirthDonor, t2.*,  ";
            strSQL += "t3.KidneyLeftDonated, t3.kidneyRightDonated, ";
            strSQL += "DATE_FORMAT(t1.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            strSQL += "FROM trialdetails  t1 ";
            strSQL += "INNER JOIN kidneyr t2 ON t1.TrialID=t2.TrialID ";
            strSQL += "LEFT JOIN donor_identification t3 ON t1.TrialID=t3.TrialID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            //lblGV1.Text = "Click on TrialID to Edit Randomisation Data.";

            if (GV1.Rows.Count == 1)
            {
                //cmdDelete.Enabled = true;
                //cmdDelete.Visible = true;
                //cmdAddData.Text = "Update Data";
                cmdReset.Enabled = false;
                cmdAddData.Enabled = false;
                lblDescription.Text = "Paired Kidney from " + Request.QueryString["TID"].ToString() + " have already been randomised.";
                lblDescription.Text += "<br/> The Main Donor Details on this page are read only.";
                lblGV1.Text = "Summary of Kidney Randomisation Data.";
                AssignData();

            }
            else if (GV1.Rows.Count == 0)
            {
                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;
                cmdReset.Enabled = true;
                cmdAddData.Enabled = true;
                cmdAddData.Text = "Randomise";
                lblDescription.Text = "Click on Randomise to Randomise Kidneys from " + Request.QueryString["TID"].ToString() + "";
                lblDescription.Text += "<br/> The Main Donor Details on this page are read only.";
                AssignData();
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
            lblUserMessages.Text = "";

            string STRSQL = string.Empty;

            STRSQL += "SELECT t1.*,  ";
            STRSQL += "t2.LeftKidneyDonate, t2.RightKidneyDonate, t2.InclusionCriteriaChecked, t2.LeftRanCategory, t2.RightRanCategory ";
            STRSQL += "FROM trialdetails t1  LEFT JOIN kidneyr t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "LEFT JOIN donor_identification t3 ON t1.TrialID=t3.TrialID ";
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

                            ddCentreCode.Visible = false;

                            lblRetrievalTeamData.Text = ddCentreCode.SelectedItem.Text;

                            if (!DBNull.Value.Equals(myDr["DonorID"]))
                            {
                                txtDonorID.Text = (string)(myDr["DonorID"]);
                            }
                            
                            txtDonorID.Visible = false;
                            lblDonorIDDetails.Text = txtDonorID.Text;

                            if (!DBNull.Value.Equals(myDr["AgeOrDateOfBirth"]))
                            {
                                rblAgeOrDateOfBirth.SelectedValue = (string)(myDr["AgeOrDateOfBirth"]);
                            }

                            if (!DBNull.Value.Equals(myDr["DonorAge"]))
                            {
                                txtDonorAge.Text = (string)(myDr["DonorAge"]);
                            }

                            txtDonorAge.Visible = false;
                            lblDonorAgeDetails.Text = txtDonorAge.Text;

                            if (!DBNull.Value.Equals(myDr["DateofBirthDonor"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateofBirthDonor"].ToString()) == true)
                                {
                                    txtDonorDateOfBirth.Text = Convert.ToDateTime(myDr["DateofBirthDonor"]).ToShortDateString();
                                }

                            }

                            txtDonorDateOfBirth.Visible = false;
                            lblDonorDateOfBirthDetails.Text = txtDonorDateOfBirth.Text;

                            if (!DBNull.Value.Equals(myDr["LeftKidneyDonate"]))
                            {
                                rblKidneyLeftDonated.SelectedValue = (string)(myDr["LeftKidneyDonate"]);
                            }

                            if (!DBNull.Value.Equals(myDr["RightKidneyDonate"]))
                            {
                                rblKidneyRightDonated.SelectedValue = (string)(myDr["RightKidneyDonate"]);
                            }

                            if (!DBNull.Value.Equals(myDr["InclusionCriteriaChecked"]))
                            {
                                rblInclusionCriteriaChecked.SelectedValue = (string)(myDr["InclusionCriteriaChecked"]);
                            }

                            //if (!DBNull.Value.Equals(myDr["ExclusionCriteriaChecked"]))
                            //{
                            //    rblExclusionCriteriaChecked.SelectedValue = (string)(myDr["ExclusionCriteriaChecked"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["ConsentChecked"]))
                            //{
                            //    rblConsentChecked.SelectedValue = (string)(myDr["ConsentChecked"]);

                            //}

                            rblAgeOrDateOfBirth_SelectedIndexChanged(this, EventArgs.Empty);
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
                lblUserMessages.Text = ex.Message + " An error occured while executing Assign Query.";
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

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while resetting the page.";
        }
    }

    // add data
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = "";

            if (string.IsNullOrEmpty(Request.QueryString["TID"]))
            {
                throw new Exception("Could not obtain TrialID for the Donor.");
            }

            if (Request.QueryString["TID"].Length <= 6)
            {
                throw new Exception("Please check the TrialID you are trying to randomise. It is not of correct length.");
            }

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyr WHERE TrialID=?TrialID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

            if (intCountFind > 0)
            {
                throw new Exception("Kidneys from this Donor have already been Randomised.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An Error occured while checking if this Kidney from this Donor has already been Randomised.");
            }


            //check if randomsiation data exists in the radnmosiation database
            STRSQL_FIND = "SELECT COUNT(*) CR FROM cope_wpfour_random.wpfour_random WHERE TrialID=?TrialID ";

            intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

            if (intCountFind > 0)
            {
                throw new Exception("The Kidney from this Donor has already been Randomised in the Randomisation database.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An Error occured while checking if this Kidney from this Donor has already been Randomised in the Randomisation database.");
            }


            //if (ddCentreCode.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("The Centre Code is missing.");
            //}

            //if (txtDonorID.Text == string.Empty)
            //{
            //    throw new Exception("The DonorID is missing.");
            //}


            //if (rblAgeOrDateOfBirth.SelectedIndex==-1)
            //{
            //    throw new Exception("Please select an option for Age/date of Birth.");
            //}
            //if (rblAgeOrDateOfBirth.SelectedValue == "Age")
            //{
            //    if (txtDonorAge.Text==string.Empty)
            //    {
            //        throw new Exception("Please Enter Donor Age.");
            //    }

            //}

            //if (rblAgeOrDateOfBirth.SelectedValue == "DateOfBirth")
            //{
            //    if (txtDonorDateOfBirth.Text == string.Empty)
            //    {
            //        throw new Exception("The Donor Date of Birth is missing.");

            //    }
            //    if (GeneralRoutines.IsDate(txtDonorDateOfBirth.Text) == false)
            //    {
            //        throw new Exception("Donor Date of Birth is not in the correct format.");
            //    }
            //}
            

            if (rblKidneyLeftDonated.SelectedValue!=STR_YES_SELECTION)
            {
                throw new Exception("Please Select " + STR_YES_SELECTION + " for " + lblKidneyLeftDonated.Text + ".");
            }

            if (rblKidneyRightDonated.SelectedValue != STR_YES_SELECTION)
            {
                throw new Exception("Please Select " + STR_YES_SELECTION + " for " + lblKidneyRightDonated.Text + ".");
            }


            //if (rblInclusionCriteriaChecked.SelectedValue == STR_UNKNOWN_SELECTION )
            //{
            //    throw new Exception("Please Select if Inclusion Criteria has been Checked.");
            //}

            if (rblInclusionCriteriaChecked.SelectedValue != STR_YES_SELECTION)
            {
                throw new Exception("Please Select 'YES' for the inclusion criteria.");
            }

            //if (rblExclusionCriteriaChecked.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select if Exclusion Criteria has been Checked.");
            //}

            //if (rblExclusionCriteriaChecked.SelectedValue != STR_YES_SELECTION)
            //{
            //    throw new Exception("Exclusion Criteria Checked should be 'YES'.");
            //}


            //if (rblConsentChecked.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select if Consent has been Checked.");
            //}

            //if (rblConsentChecked.SelectedValue != STR_YES_SELECTION)
            //{
            //    throw new Exception("Consent Checked should be 'YES'.");
            //}


            //randomise

            //default fo date created to use across the table in the main database and the randomisation database.
            DateTime dteDateTimeCreated = DateTime.Now; //datetime to

            //check if randomisation data exists
            string STRSQLFIND = "SELECT COUNT(*) CR FROM cope_wpfour_random.wpfour_random WHERE CountryCode=?CountryCode AND TrialID IS NULL";

            Int32 intCount = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?CountryCode", Request.QueryString["TID"].ToString().Substring(3,1), STRCONN));

            if (intCount==0)
            {
                throw new Exception("Randomisation List does not exists for the Country TrialID " + Request.QueryString["TID"].ToString()  + " belongs to.");
            }

            string STRSQL_INSERT = string.Empty;
            STRSQL_INSERT += "INSERT INTO kidneyr ";
            STRSQL_INSERT += "(TrialID, LeftKidneyDonate, RightKidneyDonate, InclusionCriteriaChecked, ";
            STRSQL_INSERT += "LeftRanCategory, LeftRandomisationArm, RightRanCategory, RightRandomisationArm, ";
            STRSQL_INSERT += "WPFour_RandomID, DateCreated, CreatedBy)  ";
            STRSQL_INSERT += "SELECT TrialID, ?LeftKidneyDonate, ?RightKidneyDonate, ?InclusionCriteriaChecked,      ";
            STRSQL_INSERT += "BlockCodeLeft, TreatmentLeft, BlockCodeRight, TreatmentRight, ";
            STRSQL_INSERT += "WPFour_RandomID, ?DateCreated, ?CreatedBy ";
            STRSQL_INSERT += "FROM cope_wpfour_random.wpfour_random WHERE TrialID=?TrialID ";

            //update the randmoisation database
            string STRSQL_RANDOM = " ";

            STRSQL_RANDOM += "UPDATE cope_wpfour_random.wpfour_random t1 INNER JOIN ";
            STRSQL_RANDOM += "(SELECT WPFour_RandomID FROM cope_wpfour_random.wpfour_random ";
            STRSQL_RANDOM += "WHERE TrialID IS NULL AND CountryCode=?CountryCode ";
            STRSQL_RANDOM += "ORDER BY WPFour_RandomID  LIMIT 1) t2 ON ";
            STRSQL_RANDOM += "t1.WPFour_RandomID=t2.WPFour_RandomID ";
            STRSQL_RANDOM += "SET t1.TrialID=?TrialID, DonorID=?DonorID, DonorDateOfBirth=?DonorDateOfBirth, DonorCentre=?DonorCentre,   ";
            STRSQL_RANDOM += "DateCreated=?DateCreated, CreatedBy='" + SessionVariablesAll.UserName + "' ";
            //STRSQL_RANDOM += "";



            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL_RANDOM;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();
            MyCMD.Parameters.Add("?CountryCode", MySqlDbType.VarChar).Value = SessionVariablesAll.CentreCode.Substring(0, 1);

            MyCMD.Parameters.Add("?DonorCentre", MySqlDbType.VarChar).Value = ddCentreCode.SelectedValue;
            MyCMD.Parameters.Add("?DonorID", MySqlDbType.VarChar).Value = txtDonorID.Text;
            if (GeneralRoutines.IsDate(txtDonorDateOfBirth.Text)==false)
            {
                MyCMD.Parameters.Add("?DonorDateOfBirth", MySqlDbType.Date).Value =DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DonorDateOfBirth", MySqlDbType.Date).Value = Convert.ToDateTime(txtDonorDateOfBirth.Text);
            }
            

            MyCMD.Parameters.Add("?LeftKidneyDonate", MySqlDbType.VarChar).Value = rblKidneyLeftDonated.SelectedValue;
            MyCMD.Parameters.Add("?RightKidneyDonate", MySqlDbType.VarChar).Value = rblKidneyRightDonated.SelectedValue;

            MyCMD.Parameters.Add("?InclusionCriteriaChecked", MySqlDbType.VarChar).Value = rblInclusionCriteriaChecked.SelectedValue;
            //MyCMD.Parameters.Add("?ExclusionCriteriaChecked", MySqlDbType.VarChar).Value = rblExclusionCriteriaChecked.SelectedValue;
            //MyCMD.Parameters.Add("?ConsentChecked", MySqlDbType.VarChar).Value = rblConsentChecked.SelectedValue;

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.VarChar).Value = dteDateTimeCreated.ToString("yyyy-MM-dd H:mm:ss"); // DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;


            MyCONN.Open();

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            try
            {

                MyCMD.ExecuteNonQuery();

                MyCMD.CommandText = STRSQL_INSERT;
                MyCMD.ExecuteNonQuery();

                myTrans.Commit();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                // now update UniqueID from randomisation database to update the trialdetails table

                //lblUserMessages.Text = intTrialID.ToString();
                BindData();
                lblUserMessages.Text = "TrialID " + Request.QueryString["TID"].ToString() + " has been randomised.";

                ArrayList arlEmails = EmailAddresses();

                if (arlEmails.Count == 0)
                {
                    throw new Exception("Could not obtain Email Addresses from the database. Email can not be Sent");
                }

                int intEmailSent = SAEEmailSend("WP4 TrialID " + Request.QueryString["TID"] + " Randomised (Dummy Database)", Request.QueryString["TID"], arlEmails);

            }
            catch (System.Exception ex)
            {
                myTrans.Rollback();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = ex.Message + " An error occured while executing transaction.";
            }

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while randomising.";
        }
    }

    protected void rblAgeOrDateOfBirth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAgeOrDateOfBirth.SelectedValue == "Age")
        {
            pnlDonorAge.Visible = true;

            pnlDonorDateOfBirth.Visible = false;
            txtDonorDateOfBirth.Text = string.Empty;
        }

        if (rblAgeOrDateOfBirth.SelectedValue == "DateOfBirth")
        {
            pnlDonorDateOfBirth.Visible = true;

            pnlDonorAge.Visible = false;
            txtDonorAge.Text = string.Empty;
        }
    }

    protected ArrayList EmailAddresses()
    {
        ArrayList arlEmailList = new ArrayList();

        try
        {
            //get the email address of the sender
            string STR_SQLEMAIL = string.Empty;

            //STR_SQLEMAIL += "SELECT FirstName, LastName, Email FROM wp2_cope_other.listusers WHERE TrialIDCreated=?TrialIDCreated ";
            STR_SQLEMAIL += "SELECT t1.* FROM (";
            STR_SQLEMAIL += "SELECT FirstName, LastName, Email FROM copewpfourother.listusers WHERE TrialIDCreated=?TrialIDCreated ";
            STR_SQLEMAIL += "UNION ";
            STR_SQLEMAIL += "SELECT t1.FirstName, t1.LastName, t1.Email FROM copewpfourother.listusers t1 ";
            STR_SQLEMAIL += "INNER JOIN  copewpfourother.listdbuser t2 ON t1.ListusersID=t2.ListusersID AND t2.TrialIDCentre=?TrialIDCreated ";
            STR_SQLEMAIL += "AND t2.CentreCode=?CentreCode ";
            STR_SQLEMAIL += ") t1 ORDER BY t1.LastName ";

            //lblCentre.Text = STR_SQLEMAIL;

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STR_SQLEMAIL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDCreated", MySqlDbType.VarChar).Value = "YES";
            MyCMD.Parameters.Add("?CentreCode", MySqlDbType.VarChar).Value = ddCentreCode.SelectedValue;
            MyCONN.Open();
            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {

                            if (!DBNull.Value.Equals(myDr["Email"]))
                            {
                                arlEmailList.Add(myDr["Email"].ToString().Trim());
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
                { MyCONN.Close(); }

                lblUserMessages.Text = ex.Message + " An error occured while executing select email query. ";
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while obtaining the email address page.";
        }

        return arlEmailList;

    }


    protected int SAEEmailSend(string strEmailSubject, string strTrialID, ArrayList arlEmails)
    {
        int intEmailSent = 0;

        try
        {
            string strMessageBody = string.Empty;

            strMessageBody += "<p><b>TrialID - " + strTrialID + "</b></p>";

            string STRSQL = string.Empty;
            STRSQL += "SELECT t1.*, t2.DateCreated DateTimeRandomised, t2.LeftRandomisationArm, t2.RightRandomisationArm  ";
            STRSQL += "FROM trialdetails t1 INNER JOIN kidneyr t2 ON t1.TrialID=t2.TrialID ";
            //STRSQL += "LEFT JOIN lstcentres t2 ON t1.CentreCode=CONCAT(t2.CountryCode, t2.CentreCode) ";
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

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = strTrialID;

            MyCONN.Open();


            using (MySqlDataReader myDr = MyCMD.ExecuteReader())
            {
                if (myDr.HasRows)
                {
                    while (myDr.Read())
                    {



                        if (!DBNull.Value.Equals(myDr["DonorID"]))
                        {
                            strMessageBody += "<p><b>DonorID -</b> " + myDr["DonorID"].ToString() + "</p>";
                        }

                        if (!DBNull.Value.Equals(myDr["DonorAge"]))
                        {
                            strMessageBody += "<p><b>Donor Age -</b> " + myDr["DonorAge"].ToString() + "</p>";
                        }
                        if (!DBNull.Value.Equals(myDr["DateOfBirthDonor"]))
                        {

                            if (GeneralRoutines.IsDate(myDr["DateOfBirthDonor"].ToString()) && myDr["DateOfBirthDonor"].ToString().Length >= 10)
                            {
                                strMessageBody += "<p><b>Donor Month/Year Birth -</b> " + Convert.ToDateTime(myDr["DateOfBirthDonor"]).ToShortDateString().Substring(3, 7) + "</p>";
                            }

                        }
                        



                        if (!DBNull.Value.Equals(myDr["DateTimeRandomised"]))
                        {
                            if (GeneralRoutines.IsDate(myDr["DateTimeRandomised"].ToString()) == true)
                            {
                                strMessageBody += "<p><b>Date/Time Paired Kidney from Donor Randomised -</b> " + Convert.ToDateTime(myDr["DateTimeRandomised"]).ToString("dd/MM/yyyy HH:mm") + " (+1 Hour for  Central European Time)</p>";
                            }

                        }

                        strMessageBody += "<p><b>Retrieval Team -</b> " + ddCentreCode.SelectedItem.Text + "</p>";


                    }
                }
            }

            if (MyCONN.State == ConnectionState.Open)
            { MyCONN.Close(); }

            //strMessageBody += "<p><b>Transplant Centre " + ddCentreList.SelectedItem.Text + "<br/></b></p>" + Environment.NewLine; 


            string smtpAddress = "smtp.ox.ac.uk";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = "situtrial.serverauto@nds.ox.ac.uk";

            string STRSQL_EMAILCC = "SELECT Email FROM copewpfourother.listusers WHERE UserName=?UserName ";
            string strEmailCC = GeneralRoutines.ReturnScalar(STRSQL_EMAILCC, "?UserName", SessionVariablesAll.UserName, STRCONN);

            string emailCC = strEmailCC;

            //string emailTo2 = "rraajjeevv@yahoo.com";
            string subject = strEmailSubject;
            string body = strMessageBody;
            body += "<p><i>This message was generated by the server after a pair of kidneys were randomised.</i><p>";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom, "SITU Trial Server");
                //mail.To.Add(emailTo);
                //mail.To.Add(emailTo2);
                //mail.To.Add(emailTo);
                for (int i = 0; i < arlEmails.Count; i++)
                {
                    mail.To.Add(arlEmails[i].ToString());

                }

                if (IsValidEmail(emailCC) == true)
                {
                    mail.CC.Add(emailCC);
                }


                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;                // Can set to false, if you  sending pure text.


                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    //smtp.Credentials = new NetworkCredential(emailFrom, "");
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }


            intEmailSent = 1;

            return intEmailSent;
        }
        catch
        {
            intEmailSent = 0;

            return intEmailSent;
        }
    }

    //if email is valid
    protected bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}