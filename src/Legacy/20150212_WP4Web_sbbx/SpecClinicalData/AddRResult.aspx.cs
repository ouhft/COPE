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

public partial class SpecClinicalData_AddRResult : System.Web.UI.Page
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
        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID="; 

    #endregion

    protected void Page_Load(object sender, EventArgs e)
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


                string STRSQLFIND = "SELECT COUNT(*) CR FROM kidneyr WHERE TrialID=?TrialID AND WPFour_RandomID IS NOT NULL ";

                int INT_COUNTRECORDS = 0;

                INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialID", Request.QueryString["TID"], STRCONN));

                if (INT_COUNTRECORDS == 1)
                { throw new Exception("Kidneys from this TrialID/Donor have already been randomised."); }

                if (INT_COUNTRECORDS > 1)
                { throw new Exception("More than one records exist for the selected TrialID."); }

                if (INT_COUNTRECORDS < 0)
                { throw new Exception("An error occured while checking if the Randomiation data already exists in the database."); }


                lblDescription.Text = "Please Enter Randomisation Details for " + Request.QueryString["TID"];
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdAddData_ConfirmButtonExtender.ConfirmText = "Please Click on OK if you wish to add Randomisation Details.";

                ddLeftKidneyPreservationModality.DataSource = XMLPreservationModalitiesDataSource;
                ddLeftKidneyPreservationModality.DataBind();

                ListItem li = ddLeftKidneyPreservationModality.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (li!=null)
                {
                    ddLeftKidneyPreservationModality.Items.Remove(li);
                }

                ddRightKidneyPreservationModality.DataSource = XMLPreservationModalitiesDataSource;
                ddRightKidneyPreservationModality.DataBind();

                li = ddRightKidneyPreservationModality.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (li != null)
                {
                    ddRightKidneyPreservationModality.Items.Remove(li);
                }

                txtDateCreated_CalendarExtender.EndDate = DateTime.Today;

                ViewState["SortField"] = "MainTrialID";
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

            strSQL += "SELECT t1.TrialID MainTrialID, t1.DonorID MainDonorID, t1.DateOfBirthDonor, t2.*,  ";
            //strSQL += "t3.KidneyLeftDonated, t3.KidneyRightDonated, ";
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
                //cmdReset.Enabled = false;
                //cmdAddData.Enabled = false;
                //lblDescription.Text = "Kidney(s) from " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID + " already randomised.";
                lblGV1.Text = "Sumamry of Kidney Randomisation Data.";
                AssignData();

            }
            else if (GV1.Rows.Count == 0)
            {
                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;
                //cmdReset.Enabled = true;
                //cmdAddData.Enabled = true;
                //cmdAddData.Text = "Randomise";
                //lblDescription.Text = "Randomise " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;
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

            STRSQL += "SELECT t1.TrialID, t1.DonorID, ";
            STRSQL += "t2.* ";
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
                            //if (!DBNull.Value.Equals(myDr["DonorID"]))
                            //{
                            //    txtDonorID.Text = (string)(myDr["DonorID"]);
                            //}



                            if (!DBNull.Value.Equals(myDr["LeftRanCategory"]))
                            {
                                ddLeftKidneyPreservationModality.SelectedValue = (string)(myDr["LeftRanCategory"]);
                            }

                            if (!DBNull.Value.Equals(myDr["RightRanCategory"]))
                            {
                                ddRightKidneyPreservationModality.SelectedValue = (string)(myDr["RightRanCategory"]);
                            }

                            if (!DBNull.Value.Equals(myDr["DateCreated"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateCreated"].ToString()))
                                {
                                    txtDateCreated.Text = Convert.ToDateTime(myDr["DateCreated"]).ToShortDateString();
                                    txtTimeCreated.Text = Convert.ToDateTime(myDr["DateCreated"]).ToShortTimeString();
                                }

                                
                            }

                            //if (!DBNull.Value.Equals(myDr["ConsentChecked"]))
                            //{
                            //    rblConsentChecked.SelectedValue = (string)(myDr["ConsentChecked"]);

                            //}

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

    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQLFIND = "SELECT COUNT(*) CR FROM kidneyr WHERE TrialID=?TrialID AND WPFour_RandomID IS NOT NULL ";

            int INT_COUNTRECORDS = 0;

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialID", Request.QueryString["TID"], STRCONN));

            if (INT_COUNTRECORDS == 1)
            { throw new Exception("Kidneys from this TrialID/Donor have already been randomised."); }

            if (INT_COUNTRECORDS > 1)
            { throw new Exception("More than one records exist for the selected TrialID."); }

            if (INT_COUNTRECORDS < 0)
            { throw new Exception("An error occured while checking if the Randomiation data already exists in the database."); }

            if (ddLeftKidneyPreservationModality.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Enter " + lblLeftKidney.Text + ". ");
            }

            if (ddRightKidneyPreservationModality.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Enter " + lblRightKidney.Text + ". ");
            }


            if (ddLeftKidneyPreservationModality.SelectedValue == ddRightKidneyPreservationModality.SelectedValue)
            {
                throw new Exception("Randomisation Arms for both the sides cannot be same.");
            }


            if (txtDateCreated.Text == string.Empty)
            {
                //throw new Exception("Please Enter 'Recipient's Date of Birth'");
                throw new Exception("Please Enter Date for " + lblDateCreated.Text);
            }


            if (GeneralRoutines.IsDate(txtDateCreated.Text) == false)
            {
                throw new Exception("Please Enter Date for '" + lblDateCreated.Text + "' as DD/MM/YYYY.");
            }


            if (txtTimeCreated.Text == string.Empty)
            {
                //throw new Exception("Please Enter 'Recipient's Date of Birth'");
                throw new Exception("Please Enter Time for " + lblDateCreated.Text);
            }


            if (GeneralRoutines.IsDate(txtDateCreated.Text + " " + txtTimeCreated.Text) == false)
            {
                throw new Exception("Please Enter Date/Time for '" + lblDateCreated.Text + "' in the correct format.");
            }


            if (Convert.ToDateTime(txtDateCreated.Text + " " + txtTimeCreated.Text) > DateTime.Now)
            {
                throw new Exception("Date/Time for '" + lblDateCreated.Text + "' " + Convert.ToDateTime(txtDateCreated.Text + " " + txtTimeCreated.Text) + " cannot be later than the current Date/Time " + DateTime.Now.ToString());
            }


            STRSQLFIND = "SELECT COUNT(*) CR FROM kidneyr WHERE TrialID=?TrialID ";

            INT_COUNTRECORDS = 0;

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialID", Request.QueryString["TID"], STRCONN));

            if (INT_COUNTRECORDS > 1)
            { throw new Exception("More than one records exist for the selected TrialID."); }

            if (INT_COUNTRECORDS < 0)
            { throw new Exception("An error occured while checking if the Randomiation data already exists in the database."); }


            string STRSQL_INSERT = String.Empty;
            STRSQL_INSERT += "INSERT INTO kidneyr ";
            STRSQL_INSERT += "(TrialID, LeftRanCategory, LeftRandomisationArm, RightRanCategory, RightRandomisationArm, Comments, DateCreated, CreatedBy) ";
            STRSQL_INSERT += "VALUES ";
            STRSQL_INSERT += "(?TrialID, ?LeftRanCategory, ?LeftRandomisationArm, ?RightRanCategory, ?RightRandomisationArm, ?Comments,  ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE = @"UPDATE kidneyr SET 
                                LeftRanCategory=?LeftRanCategory, LeftRandomisationArm=?LeftRandomisationArm, RightRanCategory=?RightRanCategory, RightRandomisationArm=?RightRandomisationArm,  
                                Comments=?Comments, DateCreated=?DateCreated, DateUpdated=?DateUpdated, UpdatedBy=?CreatedBy 
                                WHERE TrialID=?TrialID ";


            //lock data 
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE kidneyr SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE kidneyr SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            
            
            //assign appropriate SQL statement
            if (INT_COUNTRECORDS == 1)
            {
                MyCMD.CommandText = STRSQL_UPDATE;
            }
            else if (INT_COUNTRECORDS == 0)
            {
                MyCMD.CommandText = STRSQL_INSERT;
            }

            
            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            if (ddLeftKidneyPreservationModality.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?LeftRanCategory", MySqlDbType.VarChar).Value = DBNull.Value;
                MyCMD.Parameters.Add("?LeftRandomisationArm", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?LeftRanCategory", MySqlDbType.VarChar).Value = ddLeftKidneyPreservationModality.SelectedValue;
                MyCMD.Parameters.Add("?LeftRandomisationArm", MySqlDbType.VarChar).Value = ddLeftKidneyPreservationModality.SelectedItem.Text;
            }


            if (ddRightKidneyPreservationModality.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RightRanCategory", MySqlDbType.VarChar).Value = DBNull.Value;
                MyCMD.Parameters.Add("?RightRandomisationArm", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RightRanCategory", MySqlDbType.VarChar).Value = ddRightKidneyPreservationModality.SelectedValue;
                MyCMD.Parameters.Add("?RightRandomisationArm", MySqlDbType.VarChar).Value = ddRightKidneyPreservationModality.SelectedItem.Text;
            }
            if (txtComments.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = Convert.ToDateTime(txtDateCreated.Text + " " + txtTimeCreated.Text);

            MyCMD.Parameters.Add("?DateUpdated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();
            try
            {

                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                BindData();
                AssignData();

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

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while adding randomisation details.";
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