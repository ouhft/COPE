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
public partial class SpecClinicalData_MarkWithdrawn : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        //access denied cannot randomise
        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx?EID=51";

        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";
        
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
                    throw new Exception("Could not obtain the TrialID (Recipient)");
                }

                ddReasonWithdrawn.DataSource = XMLReasonWithdrawnDataSource;
                ddReasonWithdrawn.DataBind();

                ViewState["SortField"] = "TrialIDRecipient";
                ViewState["SortDirection"] = "DESC";

                BindData();

                txtDateWithdrawn_CalendarExtender.EndDate = DateTime.Today;

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected data will be deleted and the TrialID marked as not withdrawn.";


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
            if (Request.QueryString["TID"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t3.RecipientID, t3.DateOfBirth RecipientDateOfBirth, ";
            strSQL += "DATE_FORMAT(t3.DateOfBirth, '%d/%m/%Y') Recipient_DateOfBirth, ";
            strSQL += "DATE_FORMAT(t1.DateWithdrawn, '%d/%m/%Y') Date_Withdrawn ";
            strSQL += "FROM trialidwithdrawn t1 ";
            strSQL += "INNER JOIN trialdetails_Recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "LEFT JOIN r_identification t3 ON t1.TrialIDRecipient=t3.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();


            if (GV1.Rows.Count == 1 || string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]) == false)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;

                lblGV1.Text = "Summary of Withdrawn Data for " + Request.QueryString["TID"];
                lblDescription.Text = "Update Withdrawn Data for " + Request.QueryString["TID"];

                cmdAddData.Text = "Submit";
                AssignData();
            }

            else if (GV1.Rows.Count > 1)
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblGV1.Text = "Click on TrialID to Delete Withdrawn Data.";
                lblDescription.Text = "More than one Withdrawn Data records exist for " + Request.QueryString["TID"] + "  Select a TrialID to Delete a Record.";
            }

            else
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblGV1.Text = "Withdrawn Data has not been added.";
                lblDescription.Text = "Add Withdrawn Data for " + Request.QueryString["TID"];
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
                if (String.IsNullOrEmpty(Request.QueryString["TrialIDithdrawnID"]) == false)
                {
                    {
                        if (drv["TrialIDithdrawnID"].ToString() == Request.QueryString["TrialIDithdrawnID"].ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }
            }
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

            STRSQL += "DELETE FROM trialidwithdrawn WHERE TrialIDRecipient=?TrialIDRecipient ";

            if (!string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]))
            {
                STRSQL += "AND TrialIDWithdrawnID=?TrialIDWithdrawnID ";

            }

            string STRSQLUPDATE = String.Empty;
            STRSQLUPDATE += "UPDATE trialdetails_recipient ";
            STRSQLUPDATE += "SET Active=1 ";
            STRSQLUPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient ";
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

            if (!string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]))
            {
                MyCMD.Parameters.Add("?TrialIDWithdrawnID", MySqlDbType.VarChar).Value = Request.QueryString["TrialIDWithdrawnID"];
            }

            MyCONN.Open();

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            try
            {
                MyCMD.ExecuteNonQuery();

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQLUPDATE;
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

    protected void AssignData()
    {
        try
        {
            string STRSQL = string.Empty;
            STRSQL += "SELECT * FROM trialidwithdrawn WHERE TrialIDRecipient=?TrialIDRecipient ";

            if (!string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]))
            {
                STRSQL += "AND TrialIDWithdrawnID=?TrialIDWithdrawnID ";
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

            if (!string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]))
            {
                MyCMD.Parameters.Add("?TrialIDWithdrawnID", MySqlDbType.VarChar).Value = Request.QueryString["TrialIDWithdrawnID"];
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
                            if (!DBNull.Value.Equals(myDr["DateWithdrawn"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateWithdrawn"].ToString()))
                                {
                                    txtDateWithdrawn.Text = Convert.ToDateTime(myDr["DateWithdrawn"]).ToShortDateString();
                                }
                                
                            }

                            if (!DBNull.Value.Equals(myDr["ReasonWithdrawn"]))
                            {
                                ddReasonWithdrawn.SelectedValue = myDr["ReasonWithdrawn"].ToString();
                            }



                            if (!DBNull.Value.Equals(myDr["WithdrawnComments"]))
                            {
                                txtWithdrawnComments.Text = (string)(myDr["WithdrawnComments"]);
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
            lblUserMessages.Text = ex.Message + " An error occured while assigning data.";
        }
    }
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQLFIND = string.Empty;

            int INT_COUNTRECORDS = 0;

            //check if trialID already has a donor
            STRSQLFIND = "SELECT COUNT(*) CR FROM trialidwithdrawn WHERE TrialIDRecipient=?TrialIDRecipient  ";

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));


            if (INT_COUNTRECORDS > 1)
            {
                throw new Exception("More than 1 'Withdrawn Data' have been associated with this TrialID (Recipient). Please delete one of the records.");
            }

            if (INT_COUNTRECORDS < 0)
            {
                throw new Exception("An error occured while checking if 'Withdrawn Data' has already been added for this TrialID.");
            }

            //get datetime when TrialID was created
            string STRSQLRANDOMISE = "SELECT DateCreated FROM trialdetails WHERE TrialID=?TrialID ";
            string strTrialIDDateCreated = (GeneralRoutines.ReturnScalar(STRSQLRANDOMISE, "?TrialID", Request.QueryString["TID"], STRCONN));

            //DateTime dteTrialIDDateCreated = Convert.ToDateTime(GeneralRoutines.ReturnScalar(STRSQLRANDOMISE, "?TrialID", Request.QueryString["TID"], STRCONN));
            DateTime dteTrialIDDateCreated;
            if (GeneralRoutines.IsDate(strTrialIDDateCreated) == true)
            {
                dteTrialIDDateCreated = Convert.ToDateTime(strTrialIDDateCreated);
            }
            else
            {
                dteTrialIDDateCreated = DateTime.MinValue;
            }

            if (txtDateWithdrawn.Text==string.Empty)
            {
                throw new Exception("Please enter Date Withdrawn");
            }

            if (txtDateWithdrawn.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDateWithdrawn.Text) == true)
                {
                    if (Convert.ToDateTime(txtDateWithdrawn.Text) > DateTime.Today)
                    {
                        throw new Exception("'" + lblDateWithdrawn.Text + "' cannot be greater than today's date.");
                    }
                }
                else
                {
                    throw new Exception("Please Enter ''" + lblDateWithdrawn.Text + "' as dd/mm/yyyy.");
                }

                if (Convert.ToDateTime(txtDateWithdrawn.Text).Date < dteTrialIDDateCreated.Date)
                {
                    throw new Exception(lblDateWithdrawn.Text + " cannot be earlier than " + dteTrialIDDateCreated.ToShortDateString() + ", when the TrialID was randomised.)");
                }

            }

            if (ddReasonWithdrawn.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select an option for '" + lblReasonWithdrawn.Text + "' ");
            }

            if (ddReasonWithdrawn.SelectedValue=="Other")
            {
                if (txtReasonWithdrawnOther.Text==string.Empty)
                {
                    throw new Exception("Since '" + lblReasonWithdrawn.Text + "' is " + ddReasonWithdrawn.SelectedValue + ", please enter " + lblReasonWithdrawnOther.Text);
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

            STRSQL += "INSERT INTO trialidwithdrawn ";
            STRSQL += "(TrialIDRecipient, DateWithdrawn, ReasonWithdrawn, ReasonWithdrawnOther, ";
            STRSQL += " WithdrawnComments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?DateWithdrawn, ?ReasonWithdrawn, ?ReasonWithdrawnOther,  ";
            STRSQL += "?WithdrawnComments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = "";
            STRSQL_UPDATE += "UPDATE trialidwithdrawn SET ";
            STRSQL_UPDATE += "DateWithdrawn=?DateWithdrawn, ReasonWithdrawn=?ReasonWithdrawn, ReasonWithdrawnOther=?ReasonWithdrawnOther, ";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "WithdrawnComments=?WithdrawnComments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient ";
            if (!string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]))
            {
                STRSQL_UPDATE += "AND TrialIDWithdrawnID=?TrialIDWithdrawnID ";
            }

            //trialdetails table
            string STRSQLUPDATEMAIN = String.Empty;
            STRSQLUPDATEMAIN += "UPDATE trialdetails_recipient ";
            STRSQLUPDATEMAIN += "SET Active=0 "; //mark as Inactive
            STRSQLUPDATEMAIN += "WHERE TrialIDRecipient=?TrialIDRecipient ";

            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE trialidwithdrawn SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            //STRSQL_LOCK += "AND DateWithdrawn IS NOT NULL AND ReasonWithdrawn IS NOT NULL ";
            //STRSQL_LOCK += "AND IF(ReasonWithdrawn='Other', ReasonWithdrawnOther IS NOT NULL, ReasonWithdrawn IS NOT NULL) ";

            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE trialidwithdrawn SET ";
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
                throw new Exception("More than 1 Withdrawn data records exist this TrialID. Please delete one of the Records.");
            }

            if (INT_COUNTRECORDS < 0)
            {
                throw new Exception("An error occured while checking if Withdrawn data exists for this TrialID");
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

            if (!string.IsNullOrEmpty(Request.QueryString["TrialIDWithdrawnID"]))
            {
                MyCMD.Parameters.Add("?TrialIDWithdrawnID", MySqlDbType.VarChar).Value = Request.QueryString["TrialIDWithdrawnID"];
            }

            if (GeneralRoutines.IsDate(txtDateWithdrawn.Text) == false)
            {
                MyCMD.Parameters.Add("?DateWithdrawn", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateWithdrawn", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateWithdrawn.Text);
            }

            if (ddReasonWithdrawn.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ReasonWithdrawn", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonWithdrawn", MySqlDbType.VarChar).Value = ddReasonWithdrawn.SelectedValue;
            }

            if (txtReasonWithdrawnOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ReasonWithdrawnOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonWithdrawnOther", MySqlDbType.VarChar).Value = txtReasonWithdrawnOther.Text;
            }

            if (txtWithdrawnComments.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?WithdrawnComments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?WithdrawnComments", MySqlDbType.VarChar).Value = txtWithdrawnComments.Text;
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

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQLUPDATEMAIN;
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
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding/updating data.";
        }
    }
    protected void ddReasonWithdrawn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddReasonWithdrawn.SelectedValue=="Other")
            {
                pnlReasonWithdrawnOther.Visible = true;
            }
            else
            {
                pnlReasonWithdrawnOther.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting an option for '" + lblReasonWithdrawn.Text + "' ";
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