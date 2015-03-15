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

public partial class SpecClinicalData_AddDeceased : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";

        //access denied cannot randomise
        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx?EID=51";

        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_OTHER_SELECTION = "Other";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                {
                    throw new Exception("Could not obtain the TrialID (Recipient).");
                }

                               

                txtDeathDate_CalendarExtender.EndDate = DateTime.Today;

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected data will be deleted.";


                rblCauseDeath.DataSource = XMLDeathCausesDataSource;
                rblCauseDeath.DataBind();

                cblCauseDeathDetails.DataSource = XMLCauseDeathDetailsDataSource;
                cblCauseDeathDetails.DataBind();

                ViewState["SortField"] = "TrialIDRecipient";
                ViewState["SortDirection"] = "DESC";

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
                //AssignData();
            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }

    protected void BindData()
    {
        try
        {
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID (Recipient).");
            }
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t3.RecipientID, t3.DateOfBirth RecipientDateOfBirth, ";
            strSQL += "DATE_FORMAT(t3.DateOfBirth, '%d/%m/%Y') Recipient_DateOfBirth, ";
            strSQL += "DATE_FORMAT(t1.DeathDate, '%d/%m/%Y') Death_Date ";
            strSQL += "FROM r_deceased t1 ";
            strSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "LEFT JOIN r_identification t3 ON t1.TrialIDRecipient=t3.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];

            //lblCauseDeath.Text = strSQL;

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();


            if (GV1.Rows.Count == 1 || string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]) == false)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;

                lblGV1.Text = "Summary of Death Data for " + Request.QueryString["TID"];
                lblDescription.Text = "Update Patient Death Data for " + Request.QueryString["TID"];

                cmdAddData.Text = "Submit";
                AssignData();
            }

            else if (GV1.Rows.Count > 1)
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblGV1.Text = "Click on TrialID to Delete Patient Death Data.";
                lblDescription.Text = "More than one Patient Death records exist for " + Request.QueryString["TID"] + "  Select a TrialID to Delete a Record.";
            }

            else
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblGV1.Text = "Patient Death Data has not been added.";
                lblDescription.Text = "Add Patient Death Data for " + Request.QueryString["TID"];
            }
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
                if (String.IsNullOrEmpty(Request.QueryString["RDeceasedID"]) == false)
                {
                    {
                        if (drv["RDeceasedID"].ToString() == Request.QueryString["RDeceasedID"].ToString())
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
            STRSQL += "SELECT * FROM r_deceased WHERE TrialIDRecipient=?TrialIDRecipient ";

            if (!string.IsNullOrEmpty(Request.QueryString["RDeceasedID"]))
            {
                STRSQL += "AND RDeceasedID=?RDeceasedID ";
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

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            if (!string.IsNullOrEmpty(Request.QueryString["RDeceasedID"]))
            {
                MyCMD.Parameters.Add("?RDeceasedID", MySqlDbType.VarChar).Value = Request.QueryString["RDeceasedID"];
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
                            if (!DBNull.Value.Equals(myDr["DeathDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DeathDate"].ToString()))
                                {
                                    txtDeathDate.Text = Convert.ToDateTime(myDr["DeathDate"]).ToShortDateString();
                                }

                            }

                            
                            if (lblDeath.Font.Bold == true)
                            {
                                if (txtDeathDate.Text == string.Empty )
                                {
                                    lblDeath.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CauseDeath"]))
                            {
                                rblCauseDeath.SelectedValue = myDr["CauseDeath"].ToString();
                            }


                           

                            if (!DBNull.Value.Equals(myDr["CauseDeathDetails"]))
                            {
                                string[] strSC_Sets = myDr["CauseDeathDetails"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    ListItem currentCheckBox = cblCauseDeathDetails.Items.FindByValue(strSC_Contents[0].ToString());

                                    if (currentCheckBox != null)
                                    {
                                        if (strSC_Contents[1].ToString() == STR_YES_SELECTION)
                                        {
                                            currentCheckBox.Selected = true;
                                        }

                                    }
                                }
                            }

                            if (lblCauseDeathDetails.Font.Bold==true)
                            {
                                if (cblCauseDeathDetails.SelectedIndex==-1)
                                {
                                    lblCauseDeathDetails.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["CauseDeathDetailsOther"]))
                            {
                                txtCauseDeathDetailsOther.Text = myDr["CauseDeathDetailsOther"].ToString();
                            }

                            ListItem li = cblCauseDeathDetails.Items.FindByValue(STR_OTHER_SELECTION);
                            if (li!=null)
                            {
                                if (li.Selected==true)
                                {
                                    if (txtCauseDeathDetailsOther.Text==string.Empty)
                                    {
                                        lblCauseDeathDetailsOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                    }
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
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assigning data. ";
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
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }
            string STRSQL = String.Empty;

            STRSQL += "DELETE FROM r_deceased WHERE TrialIDRecipient=?TrialIDRecipient ";

            if (!string.IsNullOrEmpty(Request.QueryString["RDeceasedID"]))
            {
                STRSQL += "AND RDeceasedID=?RDeceasedID ";

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

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            if (!string.IsNullOrEmpty(Request.QueryString["RDeceasedID"]))
            {
                MyCMD.Parameters.Add("?RDeceasedID", MySqlDbType.VarChar).Value = Request.QueryString["RDeceasedID"];
            }

            MyCONN.Open();

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            try
            {
                MyCMD.ExecuteNonQuery();

                
                myTrans.Commit();

                BindData();
                lblUserMessages.Text = "Data Deleted";

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                //redirect to summary page
                Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
            }

            catch (System.Exception ex)
            {
                myTrans.Rollback();
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
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


            //throw error message
            if (Page.IsValid == false)
            {
                throw new Exception("Please check the data you have entered.");
            }

            string STRSQLFIND = string.Empty;

            int INT_COUNTRECORDS = 0;

            //check if trialID already has a donor
            STRSQLFIND = "SELECT COUNT(*) CR FROM r_deceased WHERE TrialIDRecipient=?TrialIDRecipient  ";

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));


            if (INT_COUNTRECORDS > 1)
            {
                throw new Exception("More than 1 'Patient Death Data' have been associated with this TrialID. Please delete one of the records.");
            }

            if (INT_COUNTRECORDS < 0)
            {
                throw new Exception("An error occured while checking if 'Patient Death Data' has already been added for this TrialID.");
            }

            //get datetime when TrialID was created
            string STRSQLRANDOMISE = "SELECT DateCreated FROM trialdetails WHERE TrialID=?TrialID ";
            string strTrialIDDateCreated = (GeneralRoutines.ReturnScalar(STRSQLRANDOMISE, "?TrialID", Request.QueryString["TID_R"], STRCONN));

            
            DateTime dteTrialIDDateCreated;
            if (GeneralRoutines.IsDate(strTrialIDDateCreated) == true)
            {
                dteTrialIDDateCreated = Convert.ToDateTime(strTrialIDDateCreated);
            }
            else
            {
                dteTrialIDDateCreated = DateTime.MinValue;
            }

            if (txtDeathDate.Text == string.Empty)
            {
                throw new Exception("Please enter Death Date");
            }

            if (txtDeathDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDeathDate.Text) == true)
                {
                    if (Convert.ToDateTime(txtDeathDate.Text) > DateTime.Today)
                    {
                        throw new Exception("'" + lblDeath.Text + "' cannot be greater than today's date.");
                    }
                }
                else
                {
                    throw new Exception("Please Enter '" + lblDeath.Text + "' as dd/mm/yyyy.");
                }

                if (Convert.ToDateTime(txtDeathDate.Text).Date < dteTrialIDDateCreated.Date)
                {
                    throw new Exception(lblDeath.Text + " cannot be earlier than " + dteTrialIDDateCreated.ToShortDateString() + ", when the TrialID was randomised.)");
                }

                //check if it is not earlier than 
                string STRSQL_OPERATIONDATE = "SELECT TransplantationDate FROM r_perioperative WHERE TrialIDRecipient=?TrialIDRecipient ";
                string strTransplantationDate = GeneralRoutines.ReturnScalar(STRSQL_OPERATIONDATE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                if (GeneralRoutines.IsDate(strTransplantationDate))
                {
                    if (Convert.ToDateTime(txtDeathDate.Text).Date < Convert.ToDateTime(strTransplantationDate).Date)
                    {
                        throw new Exception(lblDeath.Text + " cannot be earlier than " + Convert.ToDateTime(strTransplantationDate).ToShortDateString() + ", Transplantation Date.");
                    }
                }
                else
                {
                    throw new Exception("Please Enter Transplantation Date for the Recipient in the Peri Operative Page.");
                }

            }
            //if (txtDeathTime.Text != string.Empty && txtDeathTime.Text != "__:__")
            //{
            //    if (GeneralRoutines.IsNumeric(txtDeathTime.Text.Substring(0, 2)) == false)
            //    {
            //        throw new Exception("'Death' Time Hour should be numeric.");
            //    }

            //    if (Convert.ToInt16(txtDeathTime.Text.Substring(0, 2)) > 23)
            //    {
            //        throw new Exception("'Death' Time Hour should not be greater than 23.");
            //    }

            //    if (GeneralRoutines.IsNumeric(txtDeathTime.Text.Substring(3, 2)) == false)
            //    {
            //        throw new Exception("'Death' Time Minute should be numeric.");
            //    }

            //    if (Convert.ToInt16(txtDeathTime.Text.Substring(3, 2)) > 59)
            //    {
            //        throw new Exception("'Death' Time Hour Minute not be greater than 59.");
            //    }

            //    if (txtDeathDate.Text != string.Empty)
            //    {
            //        if (GeneralRoutines.IsDate(txtDeathDate.Text) == true)
            //        {
            //            if (Convert.ToDateTime(txtDeathDate.Text + " " + txtDeathTime.Text) < dteTrialIDDateCreated)
            //            {
            //                throw new Exception(lblDeath.Text + " cannot be earlier than " + dteTrialIDDateCreated.ToString() + ", when the TrialID was randomised.)");
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    txtDeathTime.Text = string.Empty;
            //}

            if (txtCauseDeathDetailsOther.Text == string.Empty)
            {
                ListItem li = cblCauseDeathDetails.Items.FindByValue(STR_OTHER_SELECTION);
                if (li != null)
                {
                    if (li.Selected == true)
                    {
                        throw new Exception("Since 'Other' has been selected for Cause of Death, please provide more details.");
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

            STRSQL += "INSERT INTO r_deceased ";
            STRSQL += "(TrialIDRecipient, DeathDate, CauseDeath, CauseDeathDetails, CauseDeathDetailsOther, ";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?DeathDate, ?CauseDeath, ?CauseDeathDetails, ?CauseDeathDetailsOther,  ";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = "";
            STRSQL_UPDATE += "UPDATE r_deceased SET ";
            STRSQL_UPDATE += "DeathDate=?DeathDate, CauseDeath=?CauseDeath, ";
            STRSQL_UPDATE += "CauseDeathDetails=?CauseDeathDetails, CauseDeathDetailsOther=?CauseDeathDetailsOther, ";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient ";
            if (!string.IsNullOrEmpty(Request.QueryString["RDeceasedID"]))
            {
                STRSQL_UPDATE += "AND RDeceasedID=?RDeceasedID ";
            }

           
            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE r_deceased SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            //STRSQL_LOCK += "AND DeathDate IS NOT NULL AND CauseDeath IS NOT NULL ";
            //STRSQL_LOCK += "AND CauseDeathDetails LIKE '%YES%' ";
            //STRSQL_LOCK += "AND IF(CauseDeathDetails LIKE '%Other:YES%', CauseDeathDetailsOther IS NOT NULL, CauseDeathDetails IS NOT NULL) ";

            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE r_deceased SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));

            if (INT_COUNTRECORDS > 1)
            {
                throw new Exception("More than 1 Deceased data records exist this TrialID (Recipient). Please delete one of the Records.");
            }

            if (INT_COUNTRECORDS < 0)
            {
                throw new Exception("An error occured while checking if Deceased data exists for this TrialID (Recipient).");
            }

            //assign appropriate SQL statement
            if (INT_COUNTRECORDS == 1)
            {
                MyCMD.CommandText = STRSQL_UPDATE;
            }
            else if (INT_COUNTRECORDS == 0)
            {
                MyCMD.CommandText = STRSQL;
            }

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["RDeceasedID"]))
            {
                MyCMD.Parameters.Add("?RDeceasedID", MySqlDbType.VarChar).Value = Request.QueryString["RDeceasedID"];
            }

            if (GeneralRoutines.IsDate(txtDeathDate.Text) == false)
            {
                MyCMD.Parameters.Add("?DeathDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DeathDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtDeathDate.Text);
            }

            //if (txtDeathTime.Text == string.Empty || txtDeathTime.Text == "__:__")
            //{
            //    MyCMD.Parameters.Add("?DeathTime", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?DeathTime", MySqlDbType.VarChar).Value = txtDeathTime.Text;
            //}

            if (rblCauseDeath.SelectedValue==STR_UNKNOWN_SELECTION || rblCauseDeath.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?CauseDeath", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CauseDeath", MySqlDbType.VarChar).Value = rblCauseDeath.SelectedValue;
            }

            //if (txtCauseDeath1b.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?CauseDeathDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?CauseDeathDetails", MySqlDbType.VarChar).Value = txtCauseDeath1b.Text;
            //}

            //append selection
            string strCauseDeathDetails = string.Empty;
            //populate document
            for (int i = 0; i < cblCauseDeathDetails.Items.Count; i++)
            {
                strCauseDeathDetails += cblCauseDeathDetails.Items[i].Value + ":";
                if (cblCauseDeathDetails.Items[i].Selected)
                {
                    strCauseDeathDetails += STR_YES_SELECTION;
                }
                else
                {
                    strCauseDeathDetails += STR_NO_SELECTION;
                }

                if (i < cblCauseDeathDetails.Items.Count - 1)
                {
                    strCauseDeathDetails += ",";
                }
            }

            if (strCauseDeathDetails == string.Empty)
            {
                MyCMD.Parameters.Add("?CauseDeathDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CauseDeathDetails", MySqlDbType.VarChar).Value = strCauseDeathDetails;
            }

            if (txtCauseDeathDetailsOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?CauseDeathDetailsOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CauseDeathDetailsOther", MySqlDbType.VarChar).Value = txtCauseDeathDetailsOther.Text;
            }

            
                                   

            if (txtComments.Text == string.Empty)
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

                myTrans.Commit();

                BindData();
                lblUserMessages.Text = "Data Deleted";

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                //redirect to summary page if data complete is not no
                string strSQLCOMPLETE = string.Empty;
                strSQLCOMPLETE += "SELECT ";
                strSQLCOMPLETE += "IF(t2.DeathDate IS NOT NULL AND t2.CauseDeath IS NOT NULL  ";
                strSQLCOMPLETE += "AND t2.CauseDeathDetails LIKE '%YES%' ";
                strSQLCOMPLETE += "AND IF(t2.CauseDeathDetails LIKE '%Other:YES%', t2.CauseDeathDetailsOther IS NOT NULL, t2.CauseDeathDetails IS NOT NULL) ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM r_deceased t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialIDRecipient=?TrialIDRecipient ";
                strSQLCOMPLETE += "";

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                //lblDonorRiskIndex.Text = strComplete;

                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
                }
                else
                {
                    if (Request.Url.AbsoluteUri.Contains("SCode=1"))
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
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }


        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding/updating data.";
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