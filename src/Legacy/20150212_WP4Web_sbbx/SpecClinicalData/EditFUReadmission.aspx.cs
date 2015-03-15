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


public partial class SpecClinicalData_EditFUReadmission : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

        private const string strExcludeOccasion = "1-7 Days";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

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

                if (Request.QueryString["TID_R"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID (Recipient).");
                }

                if (Request.QueryString["RReadmissionsID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain Unique Identifier.");
                }

                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                //ddOccasion.DataSource = XMLFollowupsDataSource;
                //ddOccasion.DataBind();
                //rblKidneyDiscarded.SelectedValue = STR_UNKNOWN_SELECTION;

                string STRSQL = "";
                STRSQL += "SELECT RFUPostTreatmentID UniqueID, CONCAT(DATE_FORMAT(FollowUpDate, '%d/%m/%Y'), ' (', Occasion, ')') FollowUp FROM  r_fuposttreatment ";
                STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion <> ?Occasion ";
                STRSQL += "ORDER BY FollowUpDate";

                SQLDS_Occasion.SelectCommand = STRSQL;
                SQLDS_Occasion.SelectParameters.Clear();
                SQLDS_Occasion.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());
                SQLDS_Occasion.SelectParameters.Add("?Occasion", strExcludeOccasion);

                ddOccasion.DataSource = SQLDS_Occasion;
                ddOccasion.DataBind();

                txtDateAdmission_CalendarExtender.EndDate = DateTime.Today;
                txtDateDischarge_CalendarExtender.EndDate = DateTime.Today;

                ddICU.DataSource = XMLMainOptionsDataYNSource;
                ddICU.DataBind();

                ddNeedDialysis.DataSource = XMLMainOptionsDataYNSource;
                ddNeedDialysis.DataBind();

                ddBiopsyTaken.DataSource = XMLMainOptionsDataYNSource;
                ddBiopsyTaken.DataBind();

                ddSurgery.DataSource = XMLMainOptionsDataYNSource;
                ddSurgery.DataBind();


                AssignData();

                ViewState["SortField"] = "DateAdmission";
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
                        //cmdUpdate.Enabled = true;
                        cmdUpdate.Enabled = true;
                        cmdDelete.Enabled = true;
                        //cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        //cmdUpdate.Enabled = false;
                        cmdUpdate.Enabled = false;
                        cmdDelete.Enabled = false;
                        //cmdReset.Enabled = false;

                    }
                }

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConstantsGeneral.MandatoryMessage;
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

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
                        strSCode1Message = ConstantsGeneral.SCode1;
                        lblUserMessages.Text = strSCode1Message;
                    }
                }


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

    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.* FROM  r_readmissions t1 ";
            STRSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t1.RReadmissionsID=?RReadmissionsID  ";

            //lblAllDataAdded.Text = STRSQL;

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
            MyCMD.Parameters.Add("?RReadmissionsID", MySqlDbType.Int32).Value = Request.QueryString["RReadmissionsID"];

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

                            if (!DBNull.Value.Equals(myDr["RFUPostTreatmentID"]))
                            {
                                ddOccasion.SelectedValue = myDr["RFUPostTreatmentID"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["DateAdmission"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateAdmission"].ToString())==true)
                                {
                                    txtDateAdmission.Text = Convert.ToDateTime((myDr["DateAdmission"])).ToShortDateString();
                                }
                            }

                            if (lblDateAdmission.Font.Bold==true)
                            {
                                if (txtDateAdmission.Text == string.Empty)
                                {
                                    lblDateAdmission.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DateDischarge"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateDischarge"].ToString()) == true)
                                {
                                    txtDateDischarge.Text = Convert.ToDateTime((myDr["DateDischarge"])).ToShortDateString();
                                }
                            }

                            if (lblDateDischarge.Font.Bold == true)
                            {
                                if (txtDateDischarge.Text == string.Empty)
                                {
                                    lblDateDischarge.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ICU"]))
                            {
                                ddICU.SelectedValue = myDr["ICU"].ToString();
                            }

                            if (lblICU.Font.Bold==true)
                            {
                                if (ddICU.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblICU.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NeedDialysis"]))
                            {
                                ddNeedDialysis.SelectedValue = myDr["NeedDialysis"].ToString();
                            }

                            if (lblNeedDialysis.Font.Bold == true)
                            {
                                if (ddNeedDialysis.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblNeedDialysis.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["BiopsyTaken"]))
                            {
                                ddBiopsyTaken.SelectedValue = myDr["BiopsyTaken"].ToString();
                            }

                            if (lblBiopsyTaken.Font.Bold == true)
                            {
                                if (ddBiopsyTaken.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblBiopsyTaken.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Surgery"]))
                            {
                                ddSurgery.SelectedValue = myDr["Surgery"].ToString();
                            }

                            if (lblSurgery.Font.Bold == true)
                            {
                                if (ddSurgery.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblSurgery.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["ReasonAdmission"]))
                            {
                                txtComments.Text = (string)(myDr["ReasonAdmission"]);
                            }

                            if (lblComments.Font.Bold == true)
                            {
                                if (txtComments.Text == string.Empty)
                                {
                                    lblComments.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
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
            lblUserMessages.Text = ex.Message + " An error occured while Assigning data.";
        }
    }
    protected void BindData()
    {
        try
        {
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }

            // get the DonorID
            string strDonorID = string.Empty;
            string strRecipientID = string.Empty;

            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

            if (mpCPH != null)
            {
                Label lblMainLabel = (Label)(mpCPH.FindControl(strMainLabel));

                if (lblMainLabel != null)
                {
                    strDonorID = lblMainLabel.Text.Replace("(", "");
                    strDonorID = strDonorID.Replace(")", "");
                }

                Label lblRecipientLabel = (Label)(mpCPH.FindControl(strRecipientLabel));

                if (lblRecipientLabel != null)
                {
                    strRecipientID = lblRecipientLabel.Text.Replace("(", "");
                    strRecipientID = strRecipientID.Replace(")", "");
                }

            }

            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.TrialID,  ";
            strSQL += "DATE_FORMAT(t1.DateAdmission, '%d/%m/%Y') Date_Admission, ";
            strSQL += "DATE_FORMAT(t1.DateDischarge, '%d/%m/%Y') Date_Discharge ";
            strSQL += "FROM r_readmissions t1 ";
            strSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on Date Admission to Edit Recipient Readmissions Data.";

            lblDescription.Text = "Update  Recipient Readmissions Data for " + Request.QueryString["TID"].ToString() + " ";
            //if (strRecipientID != string.Empty)
            //{ lblDescription.Text += " and RecipientID " + strRecipientID; }



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

    protected void GV1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)(e.Row.DataItem);
                if (String.IsNullOrEmpty(Request.QueryString["RReadmissionsID"]) == false)
                {
                    {
                        if (drv["RReadmissionsID"].ToString() == Request.QueryString["RReadmissionsID"].ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }

            }
        }
    }

    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select an Occasion.");
            }




            if (txtDateAdmission.Text == string.Empty)
            {
                throw new Exception("Please Enter Date of Admission.");
            }

            if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
            {
                throw new Exception("Please Enter Date of Admission in the correct format.");
            }

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_readmissions WHERE TrialIDRecipient=?TrialIDRecipient AND DateAdmission=?DateAdmission AND RReadmissionsID <> ?RReadmissionsID";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarThree(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?DateAdmission", Convert.ToDateTime(txtDateAdmission.Text).ToString("yyyy-MM-dd"), "?RReadmissionsID", Request.QueryString["RReadmissionsID"].ToString(), STRCONN));

            if (intCountFind > 0)
            {
                throw new Exception("There already exists an Admission data for the date you have entered. Please Edit the existing data.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("Could not check if there already exists na Admission Up data for the date you have entered.");
            }


            if (Convert.ToDateTime(txtDateAdmission.Text) > DateTime.Today)
            {
                throw new Exception("Date of Admission cannot be greater than Today's Date.");
            }

            if (txtDateDischarge.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
                {
                    throw new Exception("Please Enter Date of Discharge in the correct format.");
                }


                if (Convert.ToDateTime(txtDateDischarge.Text) > DateTime.Today)
                {
                    throw new Exception("Date of Discharge cannot be greater than Today's Date.");
                }

                if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(txtDateDischarge.Text))
                {
                    throw new Exception("Date of Admission cannot be greater than Date of Discharge.");
                }


            }

            if (txtComments.Text == string.Empty)
            {
                throw new Exception("Please Enter Reason for Readmission.");
            }


            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }

            //add the data
            string STRSQL = String.Empty;
            STRSQL += "UPDATE  r_readmissions SET ";
            STRSQL += "RFUPostTreatmentID=?RFUPostTreatmentID, Occasion=?Occasion, DateAdmission=?DateAdmission, DateDischarge=?DateDischarge, ";
            STRSQL += "ICU=?ICU, NeedDialysis=?NeedDialysis, BiopsyTaken=?BiopsyTaken, Surgery=?Surgery,";
            STRSQL += "ReasonModified=?ReasonModified, ";
            STRSQL += "ReasonAdmission=?ReasonAdmission, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND RReadmissionsID=?RReadmissionsID ";


            string STRSQL_LOCK = string.Empty;
            STRSQL_LOCK += "UPDATE r_readmissions SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "AND RReadmissionsID=?RReadmissionsID ";
            //STRSQL_LOCK += "AND Occasion IS NOT NULL AND DateAdmission IS NOT NULL AND DateDischarge IS NOT NULL ";
            //STRSQL_LOCK += "AND ICU IS NOT NULL AND NeedDialysis IS NOT NULL AND  BiopsyTaken IS NOT NULL AND Surgery IS NOT NULL ";
            //STRSQL_LOCK += "AND ReasonAdmission IS NOT NULL";
            //STRSQL_LOCK += "";
            //STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE r_readmissions SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient AND RReadmissionsID=?RReadmissionsID";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();
            MyCMD.Parameters.Add("?RReadmissionsID", MySqlDbType.VarChar).Value = Request.QueryString["RReadmissionsID"].ToString();
            MyCMD.Parameters.Add("?RFUPostTreatmentID", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedItem.Text;


            if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
            {
                MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateAdmission.Text);
            }

            if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
            {
                MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateDischarge.Text);
            }

            if (ddICU.SelectedIndex == -1 || ddICU.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ICU", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ICU", MySqlDbType.VarChar).Value = ddICU.SelectedValue;
            }

            if (ddNeedDialysis.SelectedIndex == -1 || ddNeedDialysis.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?NeedDialysis", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NeedDialysis", MySqlDbType.VarChar).Value = ddNeedDialysis.SelectedValue;
            }

            if (ddBiopsyTaken.SelectedIndex == -1 || ddBiopsyTaken.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?BiopsyTaken", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?BiopsyTaken", MySqlDbType.VarChar).Value = ddBiopsyTaken.SelectedValue;
            }

            if (ddSurgery.SelectedIndex == -1 || ddSurgery.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Surgery", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Surgery", MySqlDbType.VarChar).Value = ddSurgery.SelectedValue;
            }


            if (string.IsNullOrEmpty(txtComments.Text))
            {
                MyCMD.Parameters.Add("?ReasonAdmission", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonAdmission", MySqlDbType.VarChar).Value = txtComments.Text;
            }


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


                //redirect
                Response.Redirect(Request.Url.AbsoluteUri, false);


            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing update query.";
            }


           

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Adding Data.";
        }
    }
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_readmissions WHERE TrialIDRecipient=?TrialIDRecipient ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), STRCONN));

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Record exists for deletion.");
            }

            if (intCountFind == 0)
            {
                throw new Exception("No Record exists for deletion.");
            }

            //if (intCountFind > 1)
            //{
            //    throw new Exception("More than one Record exists for deletion.");
            //}

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM r_readmissions ";
            STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND RReadmissionsID=?RReadmissionsID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();
            MyCMD.Parameters.Add("?RReadmissionsID", MySqlDbType.VarChar).Value = Request.QueryString["RReadmissionsID"].ToString();

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