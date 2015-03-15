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

public partial class SpecClinicalData_EditMainDetails : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        //Redirect if Access Denied
        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
            {
                if (!Page.IsPostBack)
                {
                    lblUserMessages.Text = string.Empty;

                    if (string.IsNullOrEmpty(Request.QueryString["TID"]))
                    {
                        throw new Exception("Could not obtain the TrialID for the Donor.");
                    }

                    lblDescription.Text = "Update Main Donor Details for " + Request.QueryString["TID"];



                    cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                    

                    string STRSQL = string.Empty;

                    
                    //STRSQL += "SELECT t1.CountryCode,  CONCAT(t2.CountryCode, t2.CentreCode) CentreCodeMerged, ";
                    //STRSQL += "CONCAT(t2.CentreName,', ', t1.Country, ' (', t2.CountryCode, t2.CentreCode, ')') CentreDetails ";
                    //STRSQL += "FROM lstcountries t1 INNER JOIN lstcentres t2 ON t1.CountryCode=t2.CountryCode ";
                    //STRSQL += "WHERE t1.CountryCode=?CountryCode ";
                    //STRSQL += "ORDER BY t2.CountryCode, t2.CentreCode ";

                    //if (!string.IsNullOrEmpty(SessionVariablesAll.CentreCode))
                    //{

                    //    sqldsCentreLists.SelectParameters.Add("?CountryCode", SessionVariablesAll.CentreCode);
                    //    ddCountry.DataSource = sqldsCentreLists;
                    //    ddCountry.DataBind();
                    //    //ddCountry.SelectedValue = SessionVariablesAll.CentreCode;

                    //}


                    rblAgeOrDateOfBirth.DataSource = XmlAgeOrDateOfBirthDataSource;
                    rblAgeOrDateOfBirth.DataBind();

                    rv_txtDonorAge.MinimumValue = (ConstantsGeneral.intMaxDonorAge * -1).ToString();
                    rv_txtDonorAge.MaximumValue = (ConstantsGeneral.intMinDonorAge * -1).ToString();
                    rv_txtDonorAge.ErrorMessage = "Donor Age should be between " + (ConstantsGeneral.intMaxDonorAge * -1).ToString() + " and " + (ConstantsGeneral.intMinDonorAge * -1).ToString() + ".";

                    

                    rb_txtDonorDateOfBirth.MinimumValue = DateTime.Today.AddYears(ConstantsGeneral.intMinDonorAge).ToShortDateString();
                    rb_txtDonorDateOfBirth.MaximumValue = DateTime.Today.AddYears(ConstantsGeneral.intMaxDonorAge).ToShortDateString();
                    rb_txtDonorDateOfBirth.ErrorMessage = lblDonorDateOfBirth.Text + " should be between " + rb_txtDonorDateOfBirth.MinimumValue + " and " + rb_txtDonorDateOfBirth.MaximumValue;


                    //if adminsuperuser then all access

                    string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                    if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION || strIsServer == "0")
                    {
                        STRSQL += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged  
                                    FROM lstcentres  t1  
                                    ORDER BY t1.CountryCode, t1.CentreCode ";
                    }
                    else
                    {
                        //only where add/edit=YES
                        STRSQL += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    AND (t2.AddEdit='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode ";


                    }
                    sqldsCentreLists.SelectCommand = STRSQL;
                    sqldsCentreLists.SelectParameters.Clear();
                    sqldsCentreLists.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);
                    ddCountry.DataSource = sqldsCentreLists;
                    ddCountry.DataBind();

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
                            cmdUpdate.Enabled = true;
                            //cmdDelete.Enabled = true;
                            cmdReset.Enabled = true;


                        }
                        else
                        {
                            //chkDataLocked.Checked = false;
                            pnlReasonModified.Visible = false;
                            pnlFinal.Visible = false;
                            cmdUpdate.Enabled = false;
                            //cmdDelete.Enabled = false;
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

                    

                    //ddCountry.SelectedValue = SessionVariablesAll.CentreCode;
                }
            }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }
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
    protected void BindData()
    {
        try
        {
            string STRSQL = string.Empty;
            STRSQL += "SELECT *, ";
            STRSQL += "DATE_FORMAT(DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            STRSQL += "FROM trialdetails ";
            STRSQL += "WHERE TrialID = ?TrialID ";
            //STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialID", Request.QueryString["TID"]);

            sqldsGV1.SelectCommand = STRSQL;
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();

            if (GV1.Rows.Count>0)
            {
                lblGV1.Text = "Summary of Main Details";
            }
            
        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding data.";
        }
    }

   

    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.* FROM  trialdetails t1 ";
            //STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
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
                            if (!DBNull.Value.Equals(myDr["CentreCode"]))
                            {
                                ddCountry.SelectedValue = (string)(myDr["CentreCode"]);
                            }

                            if (!DBNull.Value.Equals(myDr["DonorID"]))
                            {
                                txtDonorID.Text = (string)(myDr["DonorID"]);
                            }

                            if (!DBNull.Value.Equals(myDr["AgeOrDateOfBirth"]))
                            {
                                rblAgeOrDateOfBirth.SelectedValue = (string)(myDr["AgeOrDateOfBirth"]);
                            }

                            if (!DBNull.Value.Equals(myDr["DonorAge"]))
                            {
                                txtDonorAge.Text = (string)(myDr["DonorAge"]);
                            }

                            if (!DBNull.Value.Equals(myDr["DateofBirthDonor"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateofBirthDonor"].ToString()) == true)
                                {
                                    txtDonorDateOfBirth.Text = Convert.ToDateTime(myDr["DateofBirthDonor"]).ToShortDateString();
                                }
                                
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

                            //if (!DBNull.Value.Equals(myDr["ConsentAdditionalSamples"]))
                            //{
                            //    if (myDr["ConsentAdditionalSamples"].ToString() == STR_YES_SELECTION)
                            //    {
                            //        chkConsentAdditionalSamples.Checked = true;
                            //    }
                            //}

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

                lblUserMessages.Text = ex.Message + " An error occured while executing assign query. ";
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assiging data.";
        }
    }
    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddCountry.SelectedValue == "0")
            {
                throw new Exception("Please Select a Centre Code.");
            }

            if (txtDonorID.Text == string.Empty)
            {
                throw new Exception("Please Enter DonorID.");
            }

            //if (GeneralRoutines.IsDate(txtDonorDateOfBirth.Text) == false)
            //{
            //    throw new Exception("Please Enter Date of Birth of in the correct format.");
            //}

            

            txtDonorID.Text.Trim(); //trim spaces if exist

            if (rblAgeOrDateOfBirth.SelectedValue == "Age")
            {
                if (txtDonorAge.Text == string.Empty)
                {
                    throw new Exception("Please enter Donor Age.");

                }

                if (GeneralRoutines.IsNumeric(txtDonorAge.Text) == false)
                {
                    throw new Exception("DOnor Age should be numeric.");
                }

                Page.Validate("DonorAge");
            }

            if (rblAgeOrDateOfBirth.SelectedValue == "DateOfBirth")
            {

                if (GeneralRoutines.IsDate(txtDonorDateOfBirth.Text) == false)
                {
                    throw new Exception("Please Enter Date of Birth of in the correct format.");
                }
                Page.Validate("DonorDateOfBirth");
            }

            if (rblAgeOrDateOfBirth.SelectedValue == "Age")
            {
                Page.Validate("DonorAge");
            }

            if (rblAgeOrDateOfBirth.SelectedValue == "DateOfBirth")
            {
                Page.Validate("DonorDateOfBirth");
            }

            if (Page.IsValid == false)
            {
                throw new Exception("Please enter all the requried fields.");
            }

            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }

            //if (Convert.ToDateTime(txtDonorDateOfBirth.Text) > DateTime.Today)
            //{
            //    throw new Exception("Date of Birth of Donor cannot be greater than Today's date.");
            //}

            //if (Convert.ToDateTime(txtDonorDateOfBirth.Text) > DateTime.Now.AddYears(-intMinAge))
            //{
            //    throw new Exception("The minimum age of the donor should at least be " + intMinAge.ToString() + " years.");
            //}

            //check if DonorID exists
            txtDonorID.Text.Trim(); //trim spaces if exist

            string STRSQLFIND = "SELECT COUNT(*) CR FROM trialdetails WHERE DonorID=?DonorID AND TrialID LIKE ?TrialIDCentre AND TrialID <> ?TrialID";

            int INT_COUNTRECORDS = 0;

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalarThree(STRSQLFIND, "?DonorID", txtDonorID.Text, "?TrialIDCentre", ConstantsGeneral.LeadingChars + SessionVariablesAll.CentreCode + "%", "?TrialID", Request.QueryString["TID"] ,STRCONN));

            if (INT_COUNTRECORDS > 0)
            { throw new Exception("Please Check your Donor. The Donor '" + txtDonorID.Text + "' has already been added to the database."); }

            if (INT_COUNTRECORDS < 0)
            { throw new Exception("An error occured while checking if the Donor you have added already exists in the database"); }


            string STRSQL = String.Empty;
            STRSQL += "UPDATE trialdetails SET ";
            STRSQL += "DonorID=?DonorID, CentreCode=?CentreCode, ";
            STRSQL += "AgeOrDateOfBirth=?AgeOrDateOfBirth, DonorAge=?DonorAge, DateOfBirthDonor=?DateOfBirthDonor, ";
            STRSQL += "ReasonModified=?ReasonModified, ";
            STRSQL += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL += "WHERE TrialID=?TrialID ";


            //lock data locked in every case
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE trialdetails SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE trialdetails SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";

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

            //MyCMD.Parameters.Add("?CountryCode", MySqlDbType.VarChar).Value = ddCountry.SelectedValue.Substring(0, 1) + "%";

            MyCMD.Parameters.Add("?CentreCode", MySqlDbType.VarChar).Value = ddCountry.SelectedValue;

            MyCMD.Parameters.Add("?DonorID", MySqlDbType.VarChar).Value = txtDonorID.Text.Trim();

            MyCMD.Parameters.Add("?AgeOrDateOfBirth", MySqlDbType.VarChar).Value = rblAgeOrDateOfBirth.SelectedValue;

            if (txtDonorAge.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?DonorAge", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DonorAge", MySqlDbType.VarChar).Value = txtDonorAge.Text;
            }


            if (GeneralRoutines.IsDate(txtDonorDateOfBirth.Text) == false)
            {
                MyCMD.Parameters.Add("?DateOfBirthDonor", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateOfBirthDonor", MySqlDbType.Date).Value = Convert.ToDateTime(txtDonorDateOfBirth.Text);
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

                Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);

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
            lblUserMessages.Text = ex.Message + " An error occured while updating data."; 
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