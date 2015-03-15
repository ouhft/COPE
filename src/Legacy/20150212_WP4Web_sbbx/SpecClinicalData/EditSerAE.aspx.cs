using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;

using System.Data;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;

using System.Net;
using System.Net.Mail;

public partial class SpecClinicalData_EditSerAE : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";

    private const string STR_UNKNOWN_SELECTION = "Unknown";
    private const string STR_DD_UNKNOWN_SELECTION = "0";
    private const string STR_YES_SELECTION = "YES";
    private const string STR_NO_SELECTION = "NO";

    private const int intMinimumCharactersDescriptionEvent = 20;

    private const string strMainCPH = "cplMainContents";
    private const string strSpecimenCPH = "SpecimenContents";

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

                if (string.IsNullOrEmpty(Request.QueryString["SerAEID"]))
                {
                    throw new Exception("Could not obtain the Unique Identifier.");

                }
                lblDescription.Text = "Update Serious Adverse Event Data for " + Request.QueryString["TID_R"].ToString();


                if (!string.IsNullOrEmpty(Request.QueryString["ECH"]))
                {
                    if (Request.QueryString["ECH"]=="0")
                    {
                        lblUserMessages.Text += "<br/> An error occured while sending Email.";
                    }
                    if (Request.QueryString["ECH"] == "1")
                    {
                        lblUserMessages.Text += "Email has been sent. Please check your email account " + lblEmail.Text;
                    }

                }

                
                txtDateOnset_CalendarExtender.EndDate = DateTime.Today;
                txtDateResolution_CalendarExtender.EndDate = DateTime.Today;

                rblOngoing.DataSource = XMLMainOptionsDataSource;
                rblOngoing.DataBind();
                rblOngoing.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                ListItem liRemove = rblOngoing.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblOngoing.Items.Remove(liRemove);
                }

                rblResultDeath.DataSource = XMLMainOptionsDataSource;
                rblResultDeath.DataBind();
                rblResultDeath.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                liRemove = rblResultDeath.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblResultDeath.Items.Remove(liRemove);
                }

                rblPermanentDisability.DataSource = XMLMainOptionsDataSource;
                rblPermanentDisability.DataBind();
                rblPermanentDisability.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                liRemove = rblPermanentDisability.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblPermanentDisability.Items.Remove(liRemove);
                }

                rblSignInterferenceUsualActivity.DataSource = XMLMainOptionsDataSource;
                rblSignInterferenceUsualActivity.DataBind();
                rblSignInterferenceUsualActivity.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                liRemove = rblSignInterferenceUsualActivity.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblSignInterferenceUsualActivity.Items.Remove(liRemove);
                }

                rblWorkIncapacityInability.DataSource = XMLMainOptionsDataSource;
                rblWorkIncapacityInability.DataBind();
                rblWorkIncapacityInability.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                liRemove = rblWorkIncapacityInability.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblWorkIncapacityInability.Items.Remove(liRemove);
                }

                rblResolvedWithNoSequelae.DataSource = XMLMainOptionsDataSource;
                rblResolvedWithNoSequelae.DataBind();
                rblResolvedWithNoSequelae.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                liRemove = rblResolvedWithNoSequelae.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblResolvedWithNoSequelae.Items.Remove(liRemove);
                }

                //lblDeviceDeficiency.Text = "Did this SAE arise from OrganOx <i>metra</i> Device Deficiency? *";

                rblDeviceDeficiency.DataSource = XMLMainOptionsDataSource;
                rblDeviceDeficiency.DataBind();
                rblDeviceDeficiency.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                liRemove = rblDeviceDeficiency.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblDeviceDeficiency.Items.Remove(liRemove);
                }


                //lblDeviceUserError.Text = "Did this SAE arise from OrganOx <i>metra</i> Device User Error? *";
                rblDeviceUserError.DataSource = XMLMainOptionsDataSource;
                rblDeviceUserError.DataBind();
                rblDeviceUserError.SelectedValue = STR_UNKNOWN_SELECTION;
                //remove Unknown
                liRemove = rblDeviceUserError.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liRemove != null)
                {
                    rblDeviceUserError.Items.Remove(liRemove);
                }

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected data will be deleted.";
                cmdUpdate_ConfirmButtonExtender.ConfirmText = "Please check your email to confirm if an email was sent for this Serious Adverse Event.";

                ViewState["SortField"] = "DateOnset";
                ViewState["SortDirection"] = "DESC";

                BindData();

                GetUserEmail();

                // add confirmation if email has been sent
                if (string.IsNullOrEmpty(Request.QueryString["SerAEID"]))
                {
                    throw new Exception("Could not obtain the Unique Identifier.");

                }
                lblDescription.Text = "Update Serious Adverse Event Data for " + Request.QueryString["TID_R"].ToString();


                AssignData();

                ////lock data
                //pnlReasonModified.Visible = false;
                //txtReasonModified.ToolTip = "Enter Reasons if you are Modifying any Data on this page";
                //lblReasonModifiedOldDetails.ToolTip = "Displays Reasons that have been entered for modifying data in the past.";
                //if (chkDataLocked.Checked == true || chkDataFinal.Checked == true)
                //{

                //    if (chkDataLocked.Checked == true && chkDataFinal.Checked == true)
                //    {
                //        string strDataLockedFinalMessage = ConfigurationManager.AppSettings["LockedFinalMessage"];
                //        lblDescription.Text += "<br/>" + strDataLockedFinalMessage;
                //    }
                //    else
                //    {
                //        if (chkDataFinal.Checked == true)
                //        {
                //            string strDataFinalMessage = ConfigurationManager.AppSettings["FinalMessage"];
                //            lblDescription.Text += "<br/>" + strDataFinalMessage;
                //        }
                //        else
                //        {
                //            string strDataLocked = ConfigurationManager.AppSettings["LockedMessage"];
                //            lblDescription.Text += "<br/>" + strDataLocked;
                //        }
                //    }


                //    if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                //    {
                //        if (chkDataLocked.Checked == true)
                //        {
                //            pnlReasonModified.Visible = true;
                //        }
                //        else
                //        {
                //            pnlReasonModified.Visible = false;
                //        }
                //        pnlFinal.Visible = true;
                //        cmdUpdate.Enabled = true;
                //        cmdDelete.Enabled = true;
                //        cmdReset.Enabled = true;


                //    }
                //    else
                //    {
                //        //chkDataLocked.Checked = false;
                //        pnlReasonModified.Visible = false;
                //        pnlFinal.Visible = false;
                //        cmdUpdate.Enabled = false;
                //        cmdDelete.Enabled = false;
                //        cmdReset.Enabled = false;

                //    }
                //}

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
            lblUserMessages.Text = ex.Message + " An error occured while binding the page.";
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
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*,  t2.TrialID, ";
            strSQL += "DATE_FORMAT(t1.DateOnset, '%d/%m/%Y') Date_Onset, ";
            strSQL += "DATE_FORMAT(t1.DateResolution, '%d/%m/%Y') Date_Resolution ";
            //strSQL += "t2.RecipientID,DATE_FORMAT(t2.RecipientDateOfBirth, '%d/%m/%Y') Recipient_DateOfBirth ";
            strSQL += "FROM r_serae t1 ";
            strSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();


            lblGV1.Text = "Click on TrialID (Recipient) to Edit a Serious Adverse Event Data.";

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
                if (String.IsNullOrEmpty(Request.QueryString["SerAEID"]) == false)
                {
                    {
                        if (drv["SerAEID"].ToString() == Request.QueryString["SerAEID"].ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }
            }
        }
    }

    protected void GetUserEmail()
    {

        try
        {
            //get the email address of the sender
            string STR_SQLEMAIL = string.Empty;

            STR_SQLEMAIL += "SELECT FirstName, LastName, Email FROM copewpfourother.listusers WHERE username=?UserName ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STR_SQLEMAIL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?UserName", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();
            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["FirstName"]))
                            {
                                lblName.Text = myDr["FirstName"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["LastName"]))
                            {

                                if (lblName.Text != string.Empty)
                                {
                                    lblName.Text += " ";
                                }
                                lblName.Text += myDr["LastName"].ToString();
                            }
                            if (!DBNull.Value.Equals(myDr["Email"]))
                            {
                                lblEmail.Text = myDr["Email"].ToString();
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

    }
    protected void AssignData()
    {
        try
        {
            string STRSQL = string.Empty;

            STRSQL += "SELECT * FROM r_serae WHERE TrialIDRecipient=?TrialIDRecipient AND SerAEID=?SerAEID ";

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

            MyCMD.Parameters.Add("?SerAEID", MySqlDbType.VarChar).Value = Request.QueryString["SerAEID"];

            //MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = Request.QueryString["EventCode"];


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
                            if (!DBNull.Value.Equals(myDr["DateOnset"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateOnset"].ToString()) == true)
                                {
                                    txtDateOnset.Text = Convert.ToDateTime(myDr["DateOnset"]).ToShortDateString();
                                }
                            }

                            if (lblDateOnset.Font.Bold==true)
                            {
                                if (txtDateOnset.Text == string.Empty)
                                {
                                    lblDateOnset.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                } 
                            }
                            if (!DBNull.Value.Equals(myDr["SerialNumber"]))
                            {
                                lblSerialNumber.Text = myDr["SerialNumber"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["Ongoing"]))
                            {
                                rblOngoing.SelectedValue = myDr["Ongoing"].ToString();
                            }

                            if (lblOngoing.Font.Bold == true)
                            {
                                if (rblOngoing.SelectedIndex==-1 || rblOngoing.SelectedValue==STR_UNKNOWN_SELECTION)
                                {
                                    lblOngoing.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DateResolution"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateResolution"].ToString()) == true)
                                {
                                    txtDateResolution.Text = Convert.ToDateTime(myDr["DateResolution"]).ToShortDateString();
                                }
                            }

                            if (lblDateResolution.Font.Bold == true)
                            {
                                if (txtDateResolution.Text == string.Empty)
                                {
                                    lblDateResolution.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            

                            if (!DBNull.Value.Equals(myDr["DescriptionEvent"]))
                            {
                                txtDescriptionEvent.Text = myDr["DescriptionEvent"].ToString();
                            }

                            if (lblDescriptionEvent.Font.Bold == true)
                            {
                                if (txtDescriptionEvent.Text == string.Empty)
                                {
                                    lblDescriptionEvent.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ActionTaken"]))
                            {
                                txtActionTaken.Text = myDr["ActionTaken"].ToString();
                            }

                            if (lblActionTaken.Font.Bold == true)
                            {
                                if (txtActionTaken.Text == string.Empty)
                                {
                                    lblActionTaken.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Outcome"]))
                            {
                                txtOutcome.Text= myDr["Outcome"].ToString();
                            }

                            if (lblOutcome.Font.Bold == true)
                            {
                                if (txtOutcome.Text == string.Empty)
                                {
                                    lblOutcome.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ContactName"]))
                            {
                                txtContactName.Text = myDr["ContactName"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["ContactPhone"]))
                            {
                                txtContactPhone.Text = myDr["ContactPhone"].ToString();
                            }

                            

                            if (!DBNull.Value.Equals(myDr["ContactEmail"]))
                            {
                                txtContactEmail.Text = myDr["ContactEmail"].ToString();
                            }

                            if (lblContactDetails.Font.Bold == true)
                            {
                                if (txtContactName.Text==string.Empty || txtContactPhone.Text==string.Empty ||  txtContactEmail.Text == string.Empty)
                                {
                                    lblContactDetails.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                           
                            if (!DBNull.Value.Equals(myDr["OtherDetails"]))
                            {
                                
                                string[] strSC_Sets = myDr["OtherDetails"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    ContentPlaceHolder mpMainCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
                                    if (mpMainCPH == null)
                                    {
                                        throw new Exception("Could not obtain 'Main Content Place Holder' ID.");
                                    }

                                    ContentPlaceHolder mpSpecimenCPH = (ContentPlaceHolder)(mpMainCPH.FindControl(strSpecimenCPH));

                                    if (mpSpecimenCPH == null)
                                    {
                                        throw new Exception("Could not obtain 'Specimen Content Place Holder' ID.");
                                    }

                                    RadioButtonList rdbList = (RadioButtonList)(mpSpecimenCPH.FindControl("rbl" + strSC_Contents[0].ToString())); 
                                                                      

                                    if (rdbList != null)
                                    {                                        
                                           rdbList.SelectedValue=strSC_Contents[1].ToString();                                       

                                    }
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


                            rblOngoing_SelectedIndexChanged(this, EventArgs.Empty);
                            rblResultDeath_SelectedIndexChanged(this, EventArgs.Empty);
                            
                        }
                    }
                }
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

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
                        cmdDelete.Enabled = true;
                        cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        cmdUpdate.Enabled = false;
                        cmdDelete.Enabled = false;
                        cmdReset.Enabled = false;

                    }
                }
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
            lblUserMessages.Text = ex.Message + " An error occured while assigning data.";
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

    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (txtDateOnset.Text == string.Empty)
            {
                throw new Exception("Please enter 'Date of Onset'.");
            }

            if (GeneralRoutines.IsDate(txtDateOnset.Text) == false)
            {
                throw new Exception("Please enter 'Date of Onset' as dd/mm/yyyy.");
            }

            //date of Onset cannnot be before the date liver was randomised
            string STRSQL_TRIALIDDATE = string.Empty;
            STRSQL_TRIALIDDATE = "SELECT DateCreated FROM trialdetails WHERE TrialID=?TrialID ";
            string strTrialIDDate = string.Empty;
            strTrialIDDate = GeneralRoutines.ReturnScalar(STRSQL_TRIALIDDATE, "?TrialID", Request.QueryString["TID"], STRCONN);

            if (GeneralRoutines.IsDate(strTrialIDDate)==false)
            {
                throw new Exception("Could not obtain the Date and Time when the TrialID was created.");
            }

            if (Convert.ToDateTime(txtDateOnset.Text).Date <Convert.ToDateTime(strTrialIDDate).Date)
            {
                throw new Exception("The 'Date Of Onset' can not be earlier than the 'Date' (" + Convert.ToDateTime(strTrialIDDate).ToShortDateString() + ") when the TrialId was created.");
            }

            //serious adverse event cannot be after datewithdrawn
            string STRSQL_DATEWITHDRAWN = string.Empty;
            STRSQL_DATEWITHDRAWN += "SELECT DateWithdrawn FROM trialidwithdrawn  WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strDateWithdrawn = string.Empty;
            strDateWithdrawn += GeneralRoutines.ReturnScalar(STRSQL_DATEWITHDRAWN, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

            if (GeneralRoutines.IsDate(strDateWithdrawn) == true)
            {
                if (Convert.ToDateTime(txtDateOnset.Text).Date > Convert.ToDateTime(strDateWithdrawn).Date)
                {
                    throw new Exception("The 'Date Of Onset' can not be after 'Date Withdrawn ' (" + Convert.ToDateTime(strDateWithdrawn).ToShortDateString() + ") of the recipient.");
                }
            }
            //serious adverse event cannnot be after date of death
            string STRSQL_DEATHDATE = "SELECT DeathDate FROM r_deceased WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strDeathDate = string.Empty;
            strDeathDate = GeneralRoutines.ReturnScalar(STRSQL_DEATHDATE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

            if (GeneralRoutines.IsDate(strDeathDate) == true)
            {
                if (Convert.ToDateTime(txtDateOnset.Text).Date > Convert.ToDateTime(strDeathDate).Date)
                {
                    throw new Exception("The 'Date Of Onset' can not be after 'Date of Death ' (" + Convert.ToDateTime(strDeathDate).ToShortDateString() + ") of the recipient.");
                }
            }

            if (Convert.ToDateTime(txtDateOnset.Text) > DateTime.Today)
            {
                throw new Exception("'Date of Onset' can not be greater than Today's Date.");
            }

            // not required
            ////check if data exists for the occasion selected
            //string STR_SQLFIND = "SELECT COUNT(*) CR FROM r_serae WHERE TrialID=?TrialID AND DateOnset=?DateOnset AND SerAEID<>SerAEID ";

            //int INT_COUNTRECORDS = 0;

            //INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalarThree(STR_SQLFIND, "?TrialID", Request.QueryString["TID"], "?DateOnset", Convert.ToDateTime(txtDateOnset.Text).ToString("yyyy-MM-dd"), "?SerAEID", Request.QueryString["SerAEID"], STRCONN));

            //if (INT_COUNTRECORDS < 0)
            //{
            //    throw new Exception("An error occured while checking if 'Serious Adverse Event' data has already been added for the 'Date of Onset' you are entering.");
            //}
            //if (INT_COUNTRECORDS > 0)
            //{
            //    throw new Exception("'Serious Adverse Event' data has already been added for the 'Date of Onset' you are entering. Please Edit/Delete existing data.");
            //}


            if (rblOngoing.SelectedValue == STR_UNKNOWN_SELECTION || rblOngoing.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' if the Serious Adverse Event is 'Still Ongoing'.");
            }
            else
            {
                if (rblOngoing.SelectedValue == STR_NO_SELECTION)
                {
                    if (rblResultDeath.SelectedValue != STR_YES_SELECTION) //ie person is alive
                    {
                        if (txtDateResolution.Text == string.Empty)
                        {
                            throw new Exception("As The Serious Adverse Event is Not Ongoing, please Enter '" + lblDateResolution.Text + "'.");
                        }
                    }
                }

                if (rblOngoing.SelectedValue == STR_YES_SELECTION)
                {
                    if (txtDateResolution.Text != string.Empty)
                    {
                        throw new Exception("As The Serious Adverse Event is Still Ongoing, '" + lblDateResolution.Text + "' should be empty.");
                    }
                }

            }

            if (txtDateResolution.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDateResolution.Text) == false)
                {
                    throw new Exception("Please enter 'Date of Resolution' as dd/mm/yyyy.");
                }

                if (Convert.ToDateTime(txtDateResolution.Text) > DateTime.Today)
                {
                    throw new Exception("'Date of Resolution' can not be greater than Today's Date.");
                }

                if (Convert.ToDateTime(txtDateResolution.Text) < Convert.ToDateTime(txtDateOnset.Text))
                {
                    throw new Exception("'Date of Resolution' can not be earlier than 'Date of Onset'.");
                }
            }

            if (txtDescriptionEvent.Text == string.Empty)
            {
                throw new Exception("Please Enter 'Description of Event'.");
            }

            //if (txtDescriptionEvent.Text.Length < intMinimumCharactersDescriptionEvent)
            //{
            //    throw new Exception("The minimum number of characters in 'Description of Event' should at least be " + intMinimumCharactersDescriptionEvent.ToString());
            //}

            if (txtContactName.Text == string.Empty)
            {
                throw new Exception("Please Enter 'Contact Name' of the person to contact for this Serious Adverse Event.");
            }

            if (txtContactPhone.Text == string.Empty)
            {
                throw new Exception("Please Enter 'Contact Phone' of the person to contact for this Serious Adverse Event.");
            }

            if (txtContactEmail.Text == string.Empty)
            {
                throw new Exception("Please Enter 'Contact Email' of the person to contact for this Serious Adverse Event.");
            }

            if (IsValidEmail(txtContactEmail.Text) == false)
            {
                throw new Exception("The Contact Email address you have entered is invalid. Please check the email address.'.");
            }

            if (rblResultDeath.SelectedValue == STR_UNKNOWN_SELECTION || rblResultDeath.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' for '" + lblResultDeath.Text + "'.");
            }

            if (rblPermanentDisability.SelectedValue == STR_UNKNOWN_SELECTION || rblPermanentDisability.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' for '" + lblPermanentDisability.Text + "'.");
            }

            if (rblSignInterferenceUsualActivity.SelectedValue == STR_UNKNOWN_SELECTION || rblSignInterferenceUsualActivity.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' for '" + lblSignInterferenceUsualActivity.Text + "'.");
            }

            if (rblWorkIncapacityInability.SelectedValue == STR_UNKNOWN_SELECTION || rblWorkIncapacityInability.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' for '" + lblWorkIncapacityInability.Text + "'.");
            }

            if (rblResolvedWithNoSequelae.SelectedValue == STR_UNKNOWN_SELECTION || rblResolvedWithNoSequelae.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' for '" + lblResolvedWithNoSequelae.Text + "'.");
            }

            if (rblResultDeath.SelectedValue != STR_YES_SELECTION)
            {
                if (rblPermanentDisability.SelectedValue == STR_UNKNOWN_SELECTION || rblPermanentDisability.SelectedIndex == -1)
                {
                    throw new Exception("Please Select 'YES/NO' for '" + lblPermanentDisability.Text + "'.");
                }

                if (rblSignInterferenceUsualActivity.SelectedValue == STR_UNKNOWN_SELECTION || rblSignInterferenceUsualActivity.SelectedIndex == -1)
                {
                    throw new Exception("Please Select 'YES/NO' for '" + lblSignInterferenceUsualActivity.Text + "'.");
                }

                if (rblWorkIncapacityInability.SelectedValue == STR_UNKNOWN_SELECTION || rblWorkIncapacityInability.SelectedIndex == -1)
                {
                    throw new Exception("Please Select 'YES/NO' for '" + lblWorkIncapacityInability.Text + "'.");
                }

                if (rblResolvedWithNoSequelae.SelectedValue == STR_UNKNOWN_SELECTION || rblResolvedWithNoSequelae.SelectedIndex == -1)
                {
                    throw new Exception("Please Select 'YES/NO' for '" + lblResolvedWithNoSequelae.Text + "'.");
                }
            }
            else
            {
                if (rblOngoing.SelectedValue == STR_YES_SELECTION)
                {
                    throw new Exception("Since '" + lblResultDeath.Text + "' is " + rblResultDeath.SelectedValue + " '" + lblOngoing.Text + "' should be NO");
                }
            }

            if (rblDeviceDeficiency.SelectedValue == STR_UNKNOWN_SELECTION || rblDeviceDeficiency.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' for '" + lblDeviceDeficiency.Text + "'.");
            }

            if (rblDeviceUserError.SelectedValue == STR_UNKNOWN_SELECTION || rblDeviceUserError.SelectedIndex == -1)
            {
                throw new Exception("Please Select 'YES/NO' for '" + lblDeviceUserError.Text + "'.");
            }


            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }

            // add data
            string STRSQL = string.Empty;
            STRSQL += "UPDATE r_serae SET ";
            STRSQL += "DateOnset=?DateOnset, Ongoing=?Ongoing, DateResolution=?DateResolution, DescriptionEvent=?DescriptionEvent, ";
            STRSQL += "ActionTaken=?ActionTaken, Outcome=?Outcome, ContactName=?ContactName, ContactPhone=?ContactPhone, ContactEmail=?ContactEmail, ";
            STRSQL += "OtherDetails=?OtherDetails,  ";
            STRSQL += "ReasonModified=?ReasonModified, ";
            STRSQL += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND SerAEID=?SerAEID ";

            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE r_serae SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "AND SerAEID=?SerAEID ";
            //STRSQL_LOCK += "AND DateOnset IS NOT NULL AND SerialNumber IS NOT NULL AND Ongoing IS NOT NULL AND DateResolution IS NOT NULL ";
            //STRSQL_LOCK += "AND DescriptionEvent IS NOT NULL  AND ActionTaken IS NOT NULL AND Outcome IS NOT NULL ";
            //STRSQL_LOCK += "AND ContactName IS NOT NULL AND ContactPhone IS NOT NULL AND ContactEmail  IS NOT NULL  ";

            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE r_serae SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient AND SerAEID=?SerAEID";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            MyCMD.Parameters.Add("?SerAEID", MySqlDbType.VarChar).Value = Request.QueryString["SerAEID"];


            if (GeneralRoutines.IsDate(txtDateOnset.Text) == false)
            {
                MyCMD.Parameters.Add("?DateOnset", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateOnset", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateOnset.Text);
            }


            if (rblOngoing.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Ongoing", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Ongoing", MySqlDbType.VarChar).Value = rblOngoing.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtDateResolution.Text) == false)
            {
                MyCMD.Parameters.Add("?DateResolution", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateResolution", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateResolution.Text);
            }

            if (txtDescriptionEvent.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?DescriptionEvent", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DescriptionEvent", MySqlDbType.VarChar).Value = txtDescriptionEvent.Text;
            }

            if (txtActionTaken.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ActionTaken", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ActionTaken", MySqlDbType.VarChar).Value = txtActionTaken.Text;
            }

            if (txtOutcome.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Outcome", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Outcome", MySqlDbType.VarChar).Value = txtOutcome.Text;
            }

            //if (txtContactDetails.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?ContactDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?ContactDetails", MySqlDbType.VarChar).Value = txtContactDetails.Text;
            //}

            if (txtContactName.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ContactName", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ContactName", MySqlDbType.VarChar).Value = txtContactName.Text;
            }

            if (txtContactPhone.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ContactPhone", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ContactPhone", MySqlDbType.VarChar).Value = txtContactPhone.Text;
            }

            if (txtContactEmail.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ContactEmail", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ContactEmail", MySqlDbType.VarChar).Value = txtContactEmail.Text;
            }

            ArrayList arlOtherDetails = new ArrayList();
            //arlOtherDetails.Add("SeriousEvent");
            arlOtherDetails.Add("ResultDeath");
            arlOtherDetails.Add("PermanentDisability");
            arlOtherDetails.Add("SignInterferenceUsualActivity");
            arlOtherDetails.Add("WorkIncapacityInability");
            arlOtherDetails.Add("ResolvedWithNoSequelae");
            arlOtherDetails.Add("DeviceDeficiency");
            arlOtherDetails.Add("DeviceUserError");

            ContentPlaceHolder mpMainCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
            if (mpMainCPH == null)
            {
                throw new Exception("Could not obtain 'Main Content Place Holder' ID.");
            }

            ContentPlaceHolder mpSpecimenCPH = (ContentPlaceHolder)(mpMainCPH.FindControl(strSpecimenCPH));

            if (mpSpecimenCPH == null)
            {
                throw new Exception("Could not obtain 'Specimen Content Place Holder' ID.");
            }

            

            string strOtherDetails = string.Empty;

            RadioButtonList rdbList;

            for (int i = 0; i < arlOtherDetails.Count; i++)
            {
                rdbList = (RadioButtonList)(mpSpecimenCPH.FindControl("rbl" + arlOtherDetails[i].ToString()));

                if (rdbList !=null)
                {
                    strOtherDetails += arlOtherDetails[i].ToString() + ":" + rdbList.SelectedValue;

                    if (i != arlOtherDetails.Count - 1)
                    {
                        strOtherDetails += ",";
                    }
                }
                

                //lblDateOnset.Text += ". rdbList " + rdbList.ID;
            }

            //lblDateOnset.Text = strOtherDetails;

            MyCMD.Parameters.Add("?OtherDetails", MySqlDbType.VarChar).Value = strOtherDetails;

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

                //close the connection
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                BindData();
                

                AssignData();

                lblUserMessages.Text = "Serious Adverse Event Updated";

                ArrayList arlEmails = EmailAddresses();

                if (arlEmails.Count == 0)
                {
                    throw new Exception("Could not obtain Email Addresses from the database. Email Could not be Sent");
                }

                int intEmailSent = SAEEmailSend("WP4 SAE - Serious Adverse Event Updated for " + Request.QueryString["TID_R"], lblSerialNumber.Text, arlEmails);

                if (intEmailSent == 0)
                {
                    lblUserMessages.Text += "<br/> An error occured while sending Email.";
                }
                else
                {
                    lblUserMessages.Text += "<br/> Email has been sent. Please check your email account "+ lblEmail.Text;
                }
                //Response.Redirect(Request.Url.AbsoluteUri + "&ESent=" + intEmailSent.ToString(), false);

            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }


            //finally //close connection
            //{
            //    if (MyCONN.State == ConnectionState.Open)
            //    {
            //        MyCONN.Close();
            //    }
            //}

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text += ex.Message + " An error occured while adding data.";
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

            STRSQL += "DELETE FROM r_serae WHERE TrialIDRecipient=?TrialIDRecipient AND SerAEID=?SerAEID ";

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

            MyCMD.Parameters.Add("?SerAEID", MySqlDbType.VarChar).Value = Request.QueryString["SerAEID"];


            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                BindData();

                lblUserMessages.Text = "Data Deleted";
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
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }

    protected int SAEEmailSend(string strEmailSubject, string strSAENumber, ArrayList arlEmails)
    {
        int intEmailSent = 0;

        try
        {
            string strMessageBody = string.Empty;

            strMessageBody += "<p><b>TrialID (Recipient)" + Request.QueryString["TID_R"].ToString() + Environment.NewLine + "<br/></b></p>";
            strMessageBody += "<p>SAE Number - " + strSAENumber + Environment.NewLine + "<br/></p>";
            strMessageBody += "<p>" + lblDateOnset.Text + " - " + Convert.ToDateTime(txtDateOnset.Text).ToShortDateString() + Environment.NewLine + "<br/></p>";
            strMessageBody += "<p>" + lblOngoing.Text + " - " + rblOngoing.SelectedValue + Environment.NewLine + "<br/></p>";

            if (GeneralRoutines.IsDate(txtDateResolution.Text) == true)
            {
                strMessageBody += "<p>" + lblDateResolution.Text + " - " + Convert.ToDateTime(txtDateResolution.Text).ToShortDateString() + Environment.NewLine + "<br/></p>";
            }

            if (txtDescriptionEvent.Text != string.Empty)
            {
                strMessageBody += "<p>" + lblDescriptionEvent.Text + " - " + txtDescriptionEvent.Text + Environment.NewLine + "<br/></p>";
            }

            if (txtActionTaken.Text != string.Empty)
            {
                strMessageBody += "<p>" + lblActionTaken.Text + " - " + txtActionTaken.Text + Environment.NewLine + "<br/></p>";
            }

            if (txtOutcome.Text != string.Empty)
            {
                strMessageBody += "<p>" + lblOutcome.Text + " - " + txtOutcome.Text + Environment.NewLine + "<br/></p>";
            }

            //if (txtContactDetails.Text != string.Empty)
            //{
            //    strMessageBody += "<p>" + lblContactDetails.Text + "-" + txtContactDetails.Text + Environment.NewLine + "<br/></p>";
            //}

            if (txtContactName.Text != string.Empty)
            {
                strMessageBody += "<p>" + lblContactName.Text + " - " + txtContactName.Text + Environment.NewLine + "<br/></p>";
            }

            if (txtContactPhone.Text != string.Empty)
            {
                strMessageBody += "<p>" + lblContactPhone.Text + " - " + txtContactPhone.Text + Environment.NewLine + "<br/></p>";
            }

            if (txtContactEmail.Text != string.Empty)
            {
                strMessageBody += "<p>" + lblContactEmail.Text + " - " + txtContactEmail.Text + Environment.NewLine + "<br/></p>";
            }

            //strMessageBody += "<p>" + lblSeriousEvent.Text + "-" + rblSeriousEvent.SelectedValue + Environment.NewLine + "<br/></p>";

            strMessageBody += "<p>" + lblResultDeath.Text + " - " + rblResultDeath.SelectedValue + Environment.NewLine + "<br/></p>";

            strMessageBody += "<p>" + lblPermanentDisability.Text + " - " + rblPermanentDisability.SelectedValue + Environment.NewLine + "<br/></p>";

            strMessageBody += "<p>" + lblSignInterferenceUsualActivity.Text + " - " + rblSignInterferenceUsualActivity.SelectedValue + Environment.NewLine + "<br/></p>";

            strMessageBody += "<p>" + lblWorkIncapacityInability.Text + " - " + rblWorkIncapacityInability.SelectedValue + Environment.NewLine + "<br/></p>";

            strMessageBody += "<p>" + lblResolvedWithNoSequelae.Text + " - " + rblResolvedWithNoSequelae.SelectedValue + Environment.NewLine + "<br/></p>";

            strMessageBody += "<p>" + lblDeviceDeficiency.Text + " - " + rblDeviceDeficiency.SelectedValue + Environment.NewLine + "<br/></p>";

            strMessageBody += "<p>" + lblDeviceUserError.Text + " - " + rblDeviceUserError.SelectedValue + Environment.NewLine + "<br/></p>";

            //email created by
            if (lblName.Text != string.Empty)
            {
                strMessageBody += "<br><p>" + "Serious Adverse Event SAE entered in the database by <b>" + lblName.Text + "</b></p>";
            }

            string smtpAddress = "smtp.ox.ac.uk";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = "situtrial.serverauto@nds.ox.ac.uk ";
            //string emailTo = "rraajjeevv@yahoo.com";
            string emailCC = lblEmail.Text;

            //string emailTo2 = "rraajjeevv@yahoo.com";
            string subject = strEmailSubject;
            string body = strMessageBody;
            body += "<p><i>This message was generated by the server after SAE information was entered in the 'Serious Adverse Event' page.</i><p>";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                //mail.To.Add(emailTo);
                //mail.To.Add(emailTo2);
                //mail.To.Add(emailTo);
                for (int i = 0; i < arlEmails.Count; i++)
                {
                    mail.To.Add(arlEmails[i].ToString());

                }

                if (string.IsNullOrEmpty(emailCC) == false)
                {
                    if (IsValidEmail(txtContactEmail.Text) == true)
                    {
                        mail.CC.Add(emailCC);
                    }


                }
                if (IsValidEmail(txtContactEmail.Text) == true)
                {
                    mail.CC.Add(txtContactEmail.Text);
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


    protected ArrayList EmailAddresses()
    {
        ArrayList arlEmailList = new ArrayList();

        try
        {
            //get the email address of the sender
            string STR_SQLEMAIL = string.Empty;

            //STR_SQLEMAIL += "SELECT FirstName, LastName, Email FROM copewpfourother.listusers WHERE SAEAllCentre=?SAEAllCentre ";
            STR_SQLEMAIL += "SELECT FirstName, LastName, Email FROM copewpfourother.listusers WHERE SAEAllCentre='YES' ORDER BY LastName";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STR_SQLEMAIL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?SAEAllCentre", MySqlDbType.VarChar).Value = "YES";

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
                                arlEmailList.Add(myDr["Email"].ToString());
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
    protected void SendAutoEmail(string strMainBody)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string smtpAddress = "smtp.ox.ac.uk";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = "situtrial.serverauto@nds.ox.ac.uk ";
            string emailTo = "rajeev.kumar@nds.ox.ac.uk";
            string emailTo2 = "rraajjeevv@yahoo.com";
            string subject = "WP4 SAE";
            string body = strMainBody;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.To.Add(emailTo2);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.


                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    //smtp.Credentials = new NetworkCredential(emailFrom, "");
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }


        }
        catch (System.Exception ex)
        {

            lblUserMessages.Text = ex.Message + " An error occured while selecting Non Tx Cause of Death.";
        }
    }

    protected void rblResultDeath_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblResultDeath.SelectedValue == STR_YES_SELECTION)
        {
            rblPermanentDisability.SelectedIndex = -1;
            rblPermanentDisability.Enabled = false;

            rblSignInterferenceUsualActivity.SelectedIndex = -1;
            rblSignInterferenceUsualActivity.Enabled = false;

            rblWorkIncapacityInability.SelectedIndex = -1;
            rblWorkIncapacityInability.Enabled = false;

            rblResolvedWithNoSequelae.SelectedIndex = -1;
            rblResolvedWithNoSequelae.Enabled = false;

            lblResultDeathDetails.Visible = true;
            lblResultDeathDetails.Text = "Except for Device errors, all other options have been disbaled.";
        }
        else
        {
            rblPermanentDisability.Enabled = true;
            rblSignInterferenceUsualActivity.Enabled = true;
            rblWorkIncapacityInability.Enabled = true;
            rblResolvedWithNoSequelae.Enabled = true;
            lblResultDeathDetails.Visible = false;
            lblResultDeathDetails.Text = string.Empty;
        }
    }

    protected void rblOngoing_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblOngoing.SelectedValue == STR_NO_SELECTION)
        {
            pnlDateResolution.Visible = true;
        }
        else
        {
            pnlDateResolution.Visible = false;
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