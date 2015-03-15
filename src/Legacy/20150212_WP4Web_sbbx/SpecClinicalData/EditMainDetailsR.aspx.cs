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

public partial class SpecClinicalData_EditMainDetailsR : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        //private const string strMainCPH = "cplMainContents";
        //private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const Int16 INT_ACTIVE = 1;//by default the recipient is active

        private const Int16 INT_DATALOCKED = 1;//by default this record is locked
        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";
    //static Random _random = new Random();

    #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                    {
                        throw new Exception("Couldn ot obtain the Unique Identifier.");
                    }

                    lblDescription.Text = "Please Update Transplant Centre for " + Request.QueryString["TID_R"];

                    
                    string STRSQL_TransplantCentres = string.Empty;
                    string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                    //if adminsuperuser then all access
                    if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION || strIsServer == "0")
                    {
                        STRSQL_TransplantCentres += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged  
                                    FROM lstcentres  t1  
                                    ORDER BY t1.CountryCode, t1.CentreCode ";
                    }
                    else
                    {
                        //only where add/edit=YES
                        STRSQL_TransplantCentres += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    AND (t2.AddEditRecipient='" + STR_YES_SELECTION + "' OR  t2.AddEditFollowUp='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode ";


                    }


                    sqldsCentreLists.SelectCommand = STRSQL_TransplantCentres;
                    sqldsCentreLists.SelectParameters.Clear();
                    sqldsCentreLists.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

                    if (!string.IsNullOrEmpty(SessionVariablesAll.CentreCode))
                    {

                        sqldsCentreLists.SelectParameters.Add("?CountryCode", SessionVariablesAll.CentreCode);
                        ddTransplantCentre.DataSource = sqldsCentreLists;
                        ddTransplantCentre.DataBind();
                        //ddCountry.SelectedValue = SessionVariablesAll.CentreCode;

                    }


                    cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                    //cmdAddData_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK a new Recipient will be created.";

                    rblKidneyReceived.DataSource = XMLKidneySidesDataSource;
                    rblKidneyReceived.DataBind();

                    ListItem li = rblKidneyReceived.Items.FindByValue(STR_UNKNOWN_SELECTION);

                    if (li != null)
                    {
                        rblKidneyReceived.Items.Remove(li);
                    }

                    rblRecipientInformedConsent.DataSource = XMLMainOptionsYNDataSource;
                    rblRecipientInformedConsent.DataBind();

                    rblRecipient18Year.DataSource = XMLMainOptionsYNDataSource;
                    rblRecipient18Year.DataBind();

                    rblRecipientMultipleDualTransplant.DataSource = XMLMainOptionsYNDataSource;
                    rblRecipientMultipleDualTransplant.DataBind();

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
                            //cmdDelete.Enabled = true;
                            cmdReset.Enabled = true;


                        }
                        else
                        {
                            //chkDataLocked.Checked = false;
                            pnlReasonModified.Visible = false;
                            pnlFinal.Visible = false;
                            cmdAddData.Enabled = false;
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
            catch (Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
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


        protected void cmdAddData_Click(object sender, EventArgs e)
        {
            try
            {

                if (Page.IsValid == false)
                {
                    throw new Exception("Please Check the data you have added.");
                }

                

                if (string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                    {
                        throw new Exception("Couldn ot obtain the Unique Identifier.");
                    }

                if (txtTrialID.Text == string.Empty)
                {
                    throw new Exception("Please Enter TrialID.");
                }


                txtTrialID.Text.ToUpper();

                if (rblKidneyReceived.SelectedValue == STR_UNKNOWN_SELECTION || rblKidneyReceived.SelectedIndex == -1)
                {
                    throw new Exception("Please Select Side of the Kidney received.");
                }

                if (rblRecipientInformedConsent.SelectedValue != STR_YES_SELECTION)
                {
                    throw new Exception("Please Select YES for " + lblRecipientInformedConsent.Text + ".");
                }

                if (rblRecipient18Year.SelectedValue != STR_YES_SELECTION)
                {
                    throw new Exception("Please Select YES for " + lblRecipient18Year.Text + ".");
                }

                if (rblRecipientMultipleDualTransplant.SelectedValue != STR_NO_SELECTION)
                {
                    throw new Exception("Please Select NO for " + lblRecipientMultipleDualTransplant.Text + ".");
                }

                //if (rblRecipientInformedConsent.SelectedValue==STR_NO_SELECTION)
                //{
                //    throw new Exception("Please Select YES for " + lblRecipientInformedConsent.Text + ".");
                //}

                //if (rblRecipient18Year.SelectedValue == STR_NO_SELECTION)
                //{
                //    throw new Exception("Please Select YES for " + lblRecipient18Year.Text + ".");
                //}

                //if (rblRecipientMultipleDualTransplant.SelectedValue == STR_NO_SELECTION)
                //{
                //    throw new Exception("Please Select NO for " + lblRecipientMultipleDualTransplant.Text + ".");
                //}


                //check if kidney has been assigned
                string STRSQL_FIND_TrialID = "SELECT COUNT(*) CR FROM trialdetails WHERE TrialID=?TrialID  ";
                int intCountFind_TrialID = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND_TrialID, "?TrialID", txtTrialID.Text, STRCONN));

                if (intCountFind_TrialID == 0)
                {
                    throw new Exception("The TrialID " + txtTrialID.Text + " does not exist in the database.");
                }

                if (intCountFind_TrialID > 1)
                {
                    throw new Exception("More than one TrialID " + txtTrialID.Text + " exist in the database.");
                }
                if (intCountFind_TrialID < 0)
                {
                    throw new Exception("An error occured while checking if TrialID " + txtTrialID.Text + " exists in the database.");
                }

                //check if a kidney from mainland europe is not assigned to a recipient in UK

                if (txtTrialID.Text.Substring(3, 1) == "1")
                {
                    if (txtTrialID.Text.Substring(3, 1) != ddTransplantCentre.SelectedValue.Substring(0, 1))
                    {
                        throw new Exception("Country Code for the TrialID (Donor) should match with the Country code of the selected Transplant Centre.");
                    }

                }

                if (txtTrialID.Text.Substring(3, 1) != "1")
                {
                    if (ddTransplantCentre.SelectedValue.Substring(0, 1) == "1")
                    {
                        throw new Exception("A kidney from mainland Europe cannot be transplanted in the UK.");
                    }

                }


                //check if kidney has been assigned
                string STRSQL_FIND = "SELECT COUNT(*) CR FROM trialdetails_recipient WHERE TrialID=?TrialID AND KidneyReceived=?KidneyReceived AND TrialIDRecipient<>?TrialIDRecipient";

                int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarThree(STRSQL_FIND, "?TrialID", txtTrialID.Text, "?KidneyReceived", rblKidneyReceived.SelectedValue,"?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));


                if (intCountFind > 0)
                {
                    STRSQL_FIND = "SELECT DonorID FROM trialdetails t1 INNER JOIN trialdetails_recipient t2 ON t1.TrialID=t2.TrialID WHERE t2.TrialID=?TrialID AND t2.KidneyReceived=?KidneyReceived ";
                    string strDonorID = GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", txtTrialID.Text, "?KidneyReceived", rblKidneyReceived.SelectedValue, STRCONN);
                    throw new Exception(" " + rblKidneyReceived.SelectedValue + " Kidney from DonorID " + strDonorID + " has already been added for TrialID " + txtTrialID.Text + ". Please Select another Donor/Kidney combination.");
                }

                if (intCountFind < 0)
                {
                    throw new Exception("An error occured while checking if Data for " + rblKidneyReceived.SelectedValue + " Kidney already exists for this TrialID.");
                }


                string strTrialIDRecipient = txtTrialID.Text + rblKidneyReceived.SelectedValue.Substring(0, 1);


                //check if kidney has been assigned
                STRSQL_FIND = "SELECT COUNT(*) CR FROM trialdetails_recipient WHERE TrialIDRecipient=?TrialIDRecipient ";

                intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", strTrialIDRecipient, STRCONN));

                if (intCountFind > 1)
                {
                    throw new Exception("More than oner TrialID Recipient " + strTrialIDRecipient + " have already been added.");
                }

                if (intCountFind == 0)
                {
                    throw new Exception("TrialID Recipient " + strTrialIDRecipient + " that you are trying to modify has not been added.");
                }


                if (intCountFind < 0)
                {
                    throw new Exception("An error occured while checking if " + strTrialIDRecipient + " has already been added to the database.");
                }


                if (chkDataLocked.Checked == true)
                {
                    if (txtReasonModified.Text == string.Empty)
                    {
                        throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                    }
                }

                //now add the data
                string STRSQL = String.Empty;
                STRSQL += "UPDATE trialdetails_recipient SET ";
                STRSQL += "TransplantCentre=?TransplantCentre, RecipientInformedConsent=?RecipientInformedConsent, Recipient18Year=?Recipient18Year,   ";
                STRSQL += "RecipientMultipleDualTransplant=?RecipientMultipleDualTransplant, ";
                STRSQL += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy, ";
                STRSQL += "ReasonModified=?ReasonModified ";
                STRSQL += "WHERE TrialID=?TrialID AND  KidneyReceived=?KidneyReceived ";


                //lock data locked in every case
                string STRSQL_LOCK = "";
                STRSQL_LOCK += "UPDATE trialdetails_recipient SET ";
                STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
                STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
                STRSQL_LOCK += "";
                STRSQL_LOCK += "";


                // mark final
                string STRSQL_FINAL = string.Empty;
                STRSQL_FINAL += "UPDATE trialdetails_recipient SET ";
                STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
                STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient ";


                string CS = string.Empty;
                CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

                MySqlConnection MyCONN = new MySqlConnection();
                MyCONN.ConnectionString = CS;

                MySqlCommand MyCMD = new MySqlCommand();
                MyCMD.Connection = MyCONN;

                MyCMD.CommandType = CommandType.Text;

                MyCMD.CommandText = STRSQL;



                MyCMD.Parameters.Clear();

                MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = txtTrialID.Text;
                MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];
                MyCMD.Parameters.Add("?KidneyReceived", MySqlDbType.VarChar).Value = rblKidneyReceived.SelectedValue;
                //MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = strTrialIDRecipient;
                //MyCMD.Parameters.Add("?TransplantCentre", MySqlDbType.VarChar).Value = txtTransplantCentre.Text.Trim();
                MyCMD.Parameters.Add("?TransplantCentre", MySqlDbType.VarChar).Value = ddTransplantCentre.SelectedValue;
                MyCMD.Parameters.Add("?RecipientInformedConsent", MySqlDbType.VarChar).Value = rblRecipientInformedConsent.SelectedValue;
                MyCMD.Parameters.Add("?Recipient18Year", MySqlDbType.VarChar).Value = rblRecipient18Year.SelectedValue;
                MyCMD.Parameters.Add("?RecipientMultipleDualTransplant", MySqlDbType.VarChar).Value = rblRecipientMultipleDualTransplant.SelectedValue;

                MyCMD.Parameters.Add("?Active", MySqlDbType.VarChar).Value = INT_ACTIVE.ToString();


                DateTime dteDateTimeNow = DateTime.Now;

                


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

                MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = INT_DATALOCKED.ToString();
                MyCMD.Parameters.Add("?DateLocked", MySqlDbType.DateTime).Value = dteDateTimeNow;
                MyCMD.Parameters.Add("?LockedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

                if (chkDataFinal.Checked == true)
                {
                    MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 1;
                }
                else
                {
                    MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 0;
                }

                MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = dteDateTimeNow;

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

                    BindData(strTrialIDRecipient);

                    lblUserMessages.Text = "Recipient Data Updated.";

                    //if (rblRecipientInformedConsent.SelectedValue == STR_NO_SELECTION || rblRecipient18Year.SelectedValue == STR_NO_SELECTION || rblRecipientMultipleDualTransplant.SelectedValue == STR_NO_SELECTION)
                    //{
                    //    lblGV1.Text = ConstantsGeneral.RecipientConsentMessage;
                    //}
                    //pnlMain.Visible = false;
                    //pnlTrialIDRecipient.Visible = true;

                }

                catch (System.Exception ex)
                {
                    if (MyCONN.State == ConnectionState.Open)
                    { MyCONN.Close(); }

                    lblUserMessages.Text = ex.Message + " An error occured while executing insert query.";
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


        protected void BindData(string strTrialIDRecipient)
        {
            try
            {
                string STRSQL = string.Empty;
                STRSQL += "SELECT * ";
                //STRSQL += "DATE_FORMAT(DateOfBirth, '%d/%m/%Y') Date_OfBirth ";
                STRSQL += "FROM trialdetails_recipient ";
                STRSQL += "WHERE TrialIDRecipient = ?TrialIDRecipient ";
                //STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

                sqldsGV1.SelectParameters.Clear();
                sqldsGV1.SelectParameters.Add("?TrialIDRecipient", strTrialIDRecipient);

                sqldsGV1.SelectCommand = STRSQL;
                GV1.DataSource = sqldsGV1;
                GV1.DataBind();
                lblGV1.Text = "Recipient Main Details. Please click on TrialID (Recipient) to add Recipient Data";
            }
            catch (System.Exception ex)
            {
                lblGV1.Text = ex.Message + " An error occured while binding data.";
            }

            //lblUserMessages.Text = strSQL;
        }

        protected void rblRecipientInformedConsent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblRecipientInformedConsent.SelectedValue == STR_NO_SELECTION)
            {
                lblRecipientInformedConsentMessage.Text = ConstantsGeneral.RecipientConsentMessage;

            }
            else
            {
                lblRecipientInformedConsentMessage.Text = string.Empty;
            }
        }
        protected void rblRecipient18Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblRecipient18Year.SelectedValue == STR_NO_SELECTION)
            {
                lblRecipient18YearMessage.Text = ConstantsGeneral.RecipientConsentMessage;
            }
            else
            {
                lblRecipient18YearMessage.Text = string.Empty;
            }
        }
        protected void rblRecipientMultipleDualTransplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblRecipientMultipleDualTransplant.SelectedValue == STR_NO_SELECTION)
            {
                lblRecipientMultipleDualTransplantMessage.Text = ConstantsGeneral.RecipientConsentMessage;
            }
            else
            {
                lblRecipientMultipleDualTransplantMessage.Text = string.Empty;
            }
        }



    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.* FROM  trialdetails_recipient  t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
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
                                txtTrialID.Text = (string)(myDr["TrialID"]);
                            }

                            if (!DBNull.Value.Equals(myDr["KidneyReceived"]))
                            {
                                rblKidneyReceived.SelectedValue = (string)(myDr["KidneyReceived"]);
                            }

                            if (!DBNull.Value.Equals(myDr["TransplantCentre"]))
                            {
                                ddTransplantCentre.SelectedValue = (string)(myDr["TransplantCentre"]);

                            }

                            if (!DBNull.Value.Equals(myDr["RecipientInformedConsent"]))
                            {
                                rblRecipientInformedConsent.SelectedValue = (string)(myDr["RecipientInformedConsent"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Recipient18Year"]))
                            {
                                rblRecipient18Year.SelectedValue = (string)(myDr["Recipient18Year"]);
                            }

                            if (!DBNull.Value.Equals(myDr["RecipientMultipleDualTransplant"]))
                            {
                                rblRecipientMultipleDualTransplant.SelectedValue = (string)(myDr["RecipientMultipleDualTransplant"]);
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