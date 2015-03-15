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

using System.Globalization;

public partial class AddEditData_AddWTrialID : System.Web.UI.Page
{

    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        //Redirect if Access Denied
        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx";

        private const string STR_DEFAULT_SELECTION = "Unknown";
        private const string STR_YES_SELECTION = "YES";

        
        string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");

    //static Random _random = new Random();

    //DateTime dteMinDateBirth = DateTime.Now.AddYears(100); //maximum age is 50
    //DateTime dteMaxDateBirth = DateTime.Now.AddYears(50); //minimum age is 50;

    #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    lblUserMessages.Text = string.Empty;

                    lblDescription.Text = "Please Complete the Main Donor Details and Click on Submit to Add a TrialID for a Donor Added Offline";
                    cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                    cmdAddData_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK Donor data will be added and a New TrialID added.";

                    rblAgeOrDateOfBirth.DataSource = XmlAgeOrDateOfBirthDataSource;
                    rblAgeOrDateOfBirth.DataBind();

                    rv_txtDonorAge.MinimumValue = (ConstantsGeneral.intMaxDonorAge * -1).ToString();
                    rv_txtDonorAge.MaximumValue = (ConstantsGeneral.intMinDonorAge * -1).ToString();
                    rv_txtDonorAge.ErrorMessage = "Donor Age should be between " + (ConstantsGeneral.intMaxDonorAge * -1).ToString() + " and " + (ConstantsGeneral.intMinDonorAge * -1).ToString() + ".";


                    rb_txtDonorDateOfBirth.MinimumValue = DateTime.Today.AddYears(ConstantsGeneral.intMinDonorAge).ToShortDateString();
                    rb_txtDonorDateOfBirth.MaximumValue = DateTime.Today.AddYears(ConstantsGeneral.intMaxDonorAge).ToShortDateString();
                    rb_txtDonorDateOfBirth.ErrorMessage = lblDonorDateOfBirth.Text + " should be between " + rb_txtDonorDateOfBirth.MinimumValue + " and " + rb_txtDonorDateOfBirth.MaximumValue;
                    //rblKidneySide.DataSource = XmlKidneySideDataSource;
                    //rblKidneySide.DataBind();
                    //rblKidneySide.SelectedValue = STR_DEFAULT_SELECTION;
                    lblDonorAccept.Text = "I confirm that this Donor meets the eligibility criteria for inclusion in this trial:";
                    lblDonorAccept.Text += "<br/>a Maastricht category III DCD donor, aged 50 years or older, with both kidneys registered for donation,";
                    lblDonorAccept.Text += "from the collaborating donor regions reported to Eurotransplant (ET) / National Health Service Blood and Transplant (NHSBT)";
                    //lblDonorAccept.Text += "<br/>";
                    //lblDonorAccept.Text += "<br/>I also confirm that the Donor ";
                    //lblDonorAccept.Text += "<br/>- Does not have an aortic patch too small for a reliable connection to the Kidney Assist. And that 2 sizes of cannula holders are available:: 7mm x 20mm, 9mm x 30mm. And that at the start of the trial a solution for multiple arteries on different patches will be available ";
                    //lblDonorAccept.Text += "<br/>- Does not have too many renal arteries preventing a safe connection of the kidney to the Kidney Assist";

                    rblDonorAccept.DataSource = XMLDonorAcceptDataSource;
                    rblDonorAccept.DataBind();
                    rblDonorAccept.SelectedValue = STR_DEFAULT_SELECTION;

                    string STRSQL = string.Empty;
                    string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                    //if adminsuperuser then all access
                    if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION || strIsServer == "0")
                    {
                        STRSQL += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged  
                                    FROM cope_wp_four.lstcentres  t1  
                                    ORDER BY t1.CountryCode, t1.CentreCode ";
                    }
                    else
                    {
                        //only where add/edit=YES
                        STRSQL += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM cope_wp_four.lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    AND t2.AddEdit='" + STR_YES_SELECTION + "' ORDER BY t1.CountryCode, t1.CentreCode ";


                    }



                    sqldsCentreLists.SelectCommand = STRSQL;
                    sqldsCentreLists.SelectParameters.Clear();

                    sqldsCentreLists.SelectParameters.Add("?CountryCode", SessionVariablesAll.CentreCode);
                    sqldsCentreLists.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);
                    ddCountry.DataSource = sqldsCentreLists;
                    ddCountry.DataBind();


                    //if (!string.IsNullOrEmpty(SessionVariablesAll.CentreCode))
                    //{

                    //    sqldsCentreLists.SelectParameters.Add("?CountryCode", SessionVariablesAll.CentreCode);
                    //    ddCountry.DataSource = sqldsCentreLists;
                    //    ddCountry.DataBind();
                    //    //ddCountry.SelectedValue = SessionVariablesAll.CentreCode;

                    //}

                    pnlData.Visible = false;
                    pnlRandomise.Visible = true;
                    pnlCommandButtons.Visible = true;
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

        protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            DateTime d;
            e.IsValid = DateTime.TryParseExact(e.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        }

        protected void ddCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddCountry.SelectedValue=="0")
                {
                    throw new Exception("Please Select an option for " + lblCentre.Text + ".");
                }

                txtTrialID.Text = strTrialIDLeadingCharacters + ddCountry.SelectedValue + "9";
            }
            catch(Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while loading Selecting " + lblCentre.Text + ".";
            }
        }
        protected void cmdAddData_Click(object sender, EventArgs e)
        {
            try
            {
                lblUserMessages.Text = string.Empty;

                // validate data


                if (ddCountry.SelectedValue == "0")
                {
                    throw new Exception("Please Select " + lblCentre.Text + ".");
                }

                if (txtTrialID.Text==string.Empty)
                {
                    throw new Exception("Please Enter " + lblTrialID.Text + ". ");
                }

                if (txtTrialID.Text.Length!= ConstantsGeneral.intTrialIDMaximumLength)
                {
                    throw new Exception("Length of " + lblTrialID.Text + " should be " + ConstantsGeneral.intTrialIDMaximumLength.ToString() + ".");

                }

                if (txtTrialID.Text.Substring(0, 3) != strTrialIDLeadingCharacters)
                {
                    throw new Exception("The first " + strTrialIDLeadingCharacters.Length.ToString() + " characters of the TrialID " + txtTrialID.Text + " should be " + strTrialIDLeadingCharacters);
                }

                if (txtTrialID.Text.Substring(3, 2) != SessionVariablesAll.CentreCode)
                {
                    throw new Exception("The fourth and fifth characters of the TrialID " + txtTrialID.Text + " should match the Centre Code " + SessionVariablesAll.CentreCode);
                }

                if (txtTrialID.Text.Substring(5, 1) != "9")
                {
                    throw new Exception("The sixth character of the TrialID " + txtTrialID.Text + " should be 9.");
                }

                if (GeneralRoutines.IsNumeric(txtTrialID.Text.Substring(6, 2)) == false)
                {
                    throw new Exception("The last two characted of the TrialID " + txtTrialID.Text + " should be numeric.");
                }
                else
                {
                    if (txtTrialID.Text.Substring(6, 2)=="00")
                    {
                        throw new Exception("The last two characters of the TrialID should start from '01'.");
                    }
                }

                //check if trailid exists

                string STR_SQLFIND = "SELECT COUNT(*) CR FROM trialdetails WHERE TrialID=?TrialID ";

                int intTrialIDCount = Convert.ToInt16(GeneralRoutines.ReturnScalar(STR_SQLFIND, "?TrialID", txtTrialID.Text, STRCONN));

                if (intTrialIDCount>0)
                {
                    throw new Exception("TrialID " + txtTrialID.Text + " already exists in the database.");
                }


                if (intTrialIDCount < 0)
                {
                    throw new Exception("An error occured while checking if TrialID " + txtTrialID.Text + " already exists in the database.");
                }

                if (txtDonorID.Text == string.Empty)
                {
                    throw new Exception("Please Enter " + lblDonorID.Text + ".");
                }

                //if (GeneralRoutines.IsDate(txtDonorDateOfBirth.Text) == false)
                //{
                //    throw new Exception("Please Enter Date of Birth of in the correct format.");
                //}

                //if (Convert.ToDateTime(txtDonorDateOfBirth.Text) > DateTime.Today)
                //{
                //    throw new Exception("Date of Birth of Donor cannot be greater than Today's date.");
                //}

                //if (Convert.ToDateTime(txtDonorDateOfBirth.Text) > DateTime.Now.AddYears(-intMinAge))
                //{
                //    throw new Exception("The minimum age of the donor should at least be " + intMinAge.ToString() + " years.");
                //}

                if (rblDonorAccept.SelectedValue != "YES")
                {
                    throw new Exception("Please Select 'YES' for Donor Meeting the Eligibility Criteria.");
                }

                txtDonorID.Text.Trim(); //trim spaces if exist


                if (rblAgeOrDateOfBirth.SelectedValue == "Age")
                {
                    if (txtDonorAge.Text == string.Empty)
                    {
                        throw new Exception("Please enter " + lblDonorAge.Text + ".");

                    }

                    if (GeneralRoutines.IsNumeric(txtDonorAge.Text) == false)
                    {
                        throw new Exception(lblDonorAge.Text +  " should be numeric.");
                    }

                    Page.Validate("DonorAge");
                }

                if (rblAgeOrDateOfBirth.SelectedValue == "DateOfBirth")
                {

                    if (GeneralRoutines.IsDate(txtDonorDateOfBirth.Text) == false)
                    {
                        throw new Exception("Please Enter " + lblDonorDateOfBirth.Text + ".");
                    }
                    Page.Validate("DonorDateOfBirth");
                }

                if (Page.IsValid == false)
                {
                    throw new Exception("Please enter all the requried fields.");
                }

                string STRSQLFIND = string.Empty;

                //check if Donor has been added. DonorID is uqniue for UK and unique for Europe
                if (ddCountry.SelectedValue.Substring(0,1)=="1")
                {
                     STRSQLFIND = "SELECT COUNT(*) CR FROM trialdetails WHERE DonorID=?DonorID AND CentreCode LIKE '1%' ";
                }
                else
                {
                     STRSQLFIND = "SELECT COUNT(*) CR FROM trialdetails WHERE DonorID=?DonorID AND CentreCode NOT LIKE '1%' ";

                }
                

                int INT_COUNTRECORDS = 0;

                //INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalarTwo(STRSQLFIND, "?DonorID", txtDonorID.Text, "?CountryCode", ddCountry.SelectedValue.Substring(0, 1) + "%", STRCONN));

                INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?DonorID", txtDonorID.Text,  STRCONN));

                if (INT_COUNTRECORDS > 0)
                { throw new Exception("Please Check your Donor. The Donor '" + txtDonorID.Text + "' has already been added to the database."); }

                if (INT_COUNTRECORDS < 0)
                { throw new Exception("An error occured while checking if the Donor you have added already exists in the database"); }


                //check if the trialid being added exists
                string STRSQL_COUNTTRIALID = string.Empty;
                STRSQL_COUNTTRIALID = "SELECT COUNT(*) CR FROM trialdetails WHERE TrialID =?TrialID";

                int INT_COUNTTRIALID = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQL_COUNTTRIALID, "?TrialID", txtTrialID.Text, STRCONN));

                if (INT_COUNTRECORDS>0)
                {
                    throw new Exception(lblTrialID.Text + " " + txtTrialID.Text + " already exists in the database. Please check the TrialID you are trying to add.");
                }
                


                string STRSQL_INSERT = String.Empty;
                STRSQL_INSERT += "INSERT INTO trialdetails ";
                STRSQL_INSERT += "(TrialID, DonorID, CentreCode, AgeOrDateOfBirth, DonorAge, DateOfBirthDonor, DonorAccept,  DateCreated, CreatedBy) ";
                STRSQL_INSERT += "VALUES ";
                STRSQL_INSERT += "(?TrialID, ?DonorID, ?CentreCode, ?AgeOrDateOfBirth, ?DonorAge, ?DateOfBirthDonor, ?DonorAccept, ?DateCreated, ?CreatedBy) ";
                //lblUserMessages.Text = STRSQL_MAXTrialID;


                //lock data locked in every case
                string STRSQL_LOCK = "";
                STRSQL_LOCK += "UPDATE trialdetails SET ";
                STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
                STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
                STRSQL_LOCK += "";
                STRSQL_LOCK += "";

                string CS = string.Empty;
                CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

                MySqlConnection MyCONN = new MySqlConnection();
                MyCONN.ConnectionString = CS;

                MySqlCommand MyCMD = new MySqlCommand();
                MyCMD.Connection = MyCONN;

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQL_INSERT;


                MyCMD.Parameters.Clear();

                //should include the Work Package code
                MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = txtTrialID.Text.Trim();

                MyCMD.Parameters.Add("?CountryCode", MySqlDbType.VarChar).Value = strTrialIDLeadingCharacters + ddCountry.SelectedValue.Substring(0, 1) + "%";

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

                MyCMD.Parameters.Add("?DonorAccept", MySqlDbType.VarChar).Value = rblDonorAccept.SelectedValue;

                MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = 1;

                DateTime dteDateTimeCreated = DateTime.Now; //datetime to 

                MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = dteDateTimeCreated; // DateTime.Now;

                MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;


                MyCONN.Open();

                MySqlTransaction myTrans = MyCONN.BeginTransaction();

                MyCMD.Transaction = myTrans;

                try
                {
                    

                    MyCMD.CommandText = STRSQL_INSERT;
                    MyCMD.ExecuteNonQuery();


                    MyCMD.CommandText = STRSQL_LOCK;
                    MyCMD.ExecuteNonQuery();



                    myTrans.Commit();

                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }

                    // now update UniqueID from randomisation database to update the trialdetails table

                    //lblUserMessages.Text = intTrialID.ToString();

                    lblUserMessages.Text = "TrialID " + txtTrialID.Text + " has been added to the database.";

                    BindData(txtTrialID.Text);

                    pnlData.Visible = true;
                    pnlRandomise.Visible = false;
                    pnlCommandButtons.Visible = false;

                    ArrayList arlEmails = EmailAddresses();

                    if (arlEmails.Count == 0)
                    {
                        throw new Exception("Could not obtain Email Addresses from the database. Email can not be Sent");
                    }

                    int intEmailSent = SAEEmailSend("WP4 TrialID " + txtTrialID.Text + " Created/ Donor Added (Dummy Database)", txtTrialID.Text, arlEmails);


                }
                catch (System.Exception ex)
                {

                    if (MyCONN.State == ConnectionState.Open)
                    {
                        myTrans.Rollback();
                    }


                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }

                    lblUserMessages.Text = ex.Message + " An error occured while executing query.";
                }

            }
            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while Adding Donor to the database.";
            }
        }

        protected void BindData(string strTrialID)
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
                sqldsGV1.SelectParameters.Add("?TrialID", strTrialID);

                sqldsGV1.SelectCommand = STRSQL;
                GV1.DataSource = sqldsGV1;
                GV1.DataBind();

                if (GV1.Rows.Count > 0)
                {
                    lblGV1.Text = "Please Click on TrialID to Add Other Donor Data.";
                }
            }
            catch (System.Exception ex)
            {
                lblGV1.Text = ex.Message + " An error occured while binding data.";
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
                MyCMD.Parameters.Add("?CentreCode", MySqlDbType.VarChar).Value = ddCountry.SelectedValue;
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
                STRSQL += "SELECT t1.*  FROM trialdetails t1 ";
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






                            if (!DBNull.Value.Equals(myDr["DateCreated"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateCreated"].ToString()) == true)
                                {
                                    strMessageBody += "<p><b>Date/Time Donor Added -</b> " + Convert.ToDateTime(myDr["DateCreated"]).ToString("dd/MM/yyyy HH:mm") + " (+1 hour for  Central European Time)</p>";
                                }

                            }

                            strMessageBody += "<p><b>Retrieval Team -</b> " + ddCountry.SelectedItem.Text + "</p>";


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
                body += "<p><i>This message was generated by the server after a Donor was added in the 'Add Donor' page.</i><p>";

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