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


public partial class SpecClinicalData_AddFU1to14 : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";
        
        private const string strIncludeOccasion = "1-7 Days";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

        private const string strSpecimenContentsCPH = "SpecimenContents";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_DD_REJECTION_SELECTION = "-1";

        private const string STR_NOT_REQUIRED = "Not Required";

        private const string strTxtSerumCreatinine = "txtSerumCreatinine";
        private const string strCreatinineUnits = "µmol/l";

        private const int intDefaultEventCode = 41;

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";


        
    //static Random _random = new Random();

    #endregion

    //protected void Page_Load(object sender, EventArgs e)
    //{

    //    if (!Page.IsPostBack)
    //    {
    //        Page.MaintainScrollPositionOnPostBack = "true";
    //    }
    //}

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

                //MaintainScrollPositionOnPostBack = true;
                AssignDates();


                if (ViewState["OperationStartDate"]!=null)
                {
                    if (GeneralRoutines.IsDate(ViewState["OperationStartDate"].ToString()))
                    {
                        rv_txtFollowUpDate.MinimumValue = Convert.ToDateTime(ViewState["OperationStartDate"]).ToShortDateString();

                        rv_txtFollowUpDate.MaximumValue = Convert.ToDateTime(ViewState["OperationStartDate"]).AddDays(2).ToShortDateString();

                        //rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();

                        rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue +  ".";
                        //lblFollowUpDate.Text = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ".";
                    }
                    else
                    {
                        if (ViewState["DateCreated"]!=null)
                        {
                            if (GeneralRoutines.IsDate(ViewState["DateCreated"].ToString()))
                            {
                                rv_txtFollowUpDate.MinimumValue = Convert.ToDateTime(ViewState["DateCreated"]).ToShortDateString();

                                rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();

                                rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ".";
                            }
                            else
                            {
                                rv_txtFollowUpDate.MinimumValue = Convert.ToDateTime("01/01/2014").ToShortDateString();

                                rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();

                                rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ".";
                            }
                           
                        }

                        else
                        {
                            rv_txtFollowUpDate.MinimumValue = Convert.ToDateTime("01/01/2014").ToShortDateString();

                            rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();

                            rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ".";
                        }
                                                
                        //rv_txtFollowUpDate.MinimumValue=DateTime
                    }
                }
                else
                {
                    if (ViewState["DateCreated"] != null)
                    {
                        if (GeneralRoutines.IsDate(ViewState["DateCreated"].ToString()))
                        {
                            rv_txtFollowUpDate.MinimumValue = Convert.ToDateTime(ViewState["DateCreated"]).ToShortDateString();

                            rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();

                            rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ".";
                        }
                        else
                        {
                            rv_txtFollowUpDate.MinimumValue = Convert.ToDateTime("01/01/2014").ToShortDateString();

                            rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();

                            rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ".";
                        }
                    }

                    else
                    {
                        rv_txtFollowUpDate.MinimumValue = Convert.ToDateTime("01/01/2014").ToShortDateString();

                        rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();

                        rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ".";
                    }
                }


                

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";


                
                pnlGraftFailure.Visible = false;

                ddOccasion.DataSource = XMLFollowUpFirstDataSource;
                ddOccasion.DataBind();
                ddOccasion.SelectedValue = strIncludeOccasion;
                //rblKidneyDiscarded.SelectedValue = STR_UNKNOWN_SELECTION;

                txtFollowUpDate_CalendarExtender.EndDate = DateTime.Today;
                txtDateGraftFailure_CalendarExtender.EndDate = DateTime.Today;
                txtDateGraftRemoval_CalendarExtender.EndDate = DateTime.Today;
                txtDatePrimaryPostTxDischarge_CalendarExtender.EndDate = DateTime.Today;

                rblGraftFailure.DataSource = XMLMainOptionsYNDataSource;
                rblGraftFailure.DataBind();
                //rblGraftFailure.SelectedValue = STR_UNKNOWN_SELECTION;

                rblDialysisType.DataSource = XMLDialysisTypesDataSource;
                rblDialysisType.DataBind();
                //rblDialysisType.SelectedValue = STR_UNKNOWN_SELECTION;

                rblDialysisType.Items.Add(new ListItem(STR_NOT_REQUIRED, STR_NOT_REQUIRED));
                //rblDialysisType.Items.Insert("0," new ListItem("Not Required", "Not Required"));

                rblRequiredHyperkalemia.DataSource = XMLMainOptionsYNDataSource;
                rblRequiredHyperkalemia.DataBind();
                rblRequiredHyperkalemia.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblHypotensivePeriod1.DataSource = XMLMainOptionsYNDataSource;
                //rblHypotensivePeriod1.DataBind();
                //rblHypotensivePeriod1.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblHypotensivePeriod2.DataSource = XMLMainOptionsYNDataSource;
                //rblHypotensivePeriod2.DataBind();
                //rblHypotensivePeriod2.SelectedValue = STR_UNKNOWN_SELECTION;

                ddPrimaryCause.DataSource = XMLPrimaryCausesDataSource;
                ddPrimaryCause.DataBind();

                rblGraftRemoval.DataSource = XMLMainOptionsYNDataSource;
                rblGraftRemoval.DataBind();
                //rblGraftRemoval.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblDeath.DataSource = XMLMainOptionsDataSource;
                //rblDeath.DataBind();
                //rblDeath.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblCauseDeath.DataSource = XMLDeathCausesDataSource;
                //rblCauseDeath.DataBind();
                //rblCauseDeath.SelectedValue = STR_UNKNOWN_SELECTION;

                rblCreatinineUnits.DataSource = XMLCreatinineUnitsDataSource;
                rblCreatinineUnits.DataBind();
                rblCreatinineUnits.SelectedValue = strCreatinineUnits;

                rv_txtSerumCreatinineAdmission.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinineAdmission.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinineAdmission.ErrorMessage = "Values should be between " + rv_txtSerumCreatinineAdmission.MinimumValue + " and " + rv_txtSerumCreatinineAdmission.MaximumValue + ".";


                rv_txtSerumCreatinine1.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinine1.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinine1.ErrorMessage = "Value should be between " + rv_txtSerumCreatinine1.MinimumValue + " and " + rv_txtSerumCreatinine1.MaximumValue + ".";


                rv_txtSerumCreatinine2.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinine2.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinine2.ErrorMessage = "Value should be between " + rv_txtSerumCreatinine2.MinimumValue + " and " + rv_txtSerumCreatinine2.MaximumValue + ".";

                rv_txtSerumCreatinine3.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinine3.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinine3.ErrorMessage = "Value should be between " + rv_txtSerumCreatinine3.MinimumValue + " and " + rv_txtSerumCreatinine3.MaximumValue + ".";

                rv_txtSerumCreatinine4.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinine4.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinine4.ErrorMessage = "Value should be between " + rv_txtSerumCreatinine4.MinimumValue + " and " + rv_txtSerumCreatinine4.MaximumValue + ".";

                rv_txtSerumCreatinine5.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinine5.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinine5.ErrorMessage = "Value should be between " + rv_txtSerumCreatinine5.MinimumValue + " and " + rv_txtSerumCreatinine5.MaximumValue + ".";


                rv_txtSerumCreatinine6.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinine6.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinine6.ErrorMessage = "Value should be between " + rv_txtSerumCreatinine6.MinimumValue + " and " + rv_txtSerumCreatinine6.MaximumValue + ".";

                rv_txtSerumCreatinine7.MinimumValue = ConstantsGeneral.intMinSerumCreatinine.ToString();
                rv_txtSerumCreatinine7.MaximumValue = ConstantsGeneral.intMaxSerumCreatinine.ToString();
                rv_txtSerumCreatinine7.ErrorMessage = "Value should be between " + rv_txtSerumCreatinine7.MinimumValue + " and " + rv_txtSerumCreatinine7.MaximumValue + ".";


                ddInductionTherapy.DataSource = XMLInductionTherapiesDataSource;
                ddInductionTherapy.DataBind();
                
                cblPostTxImmunosuppressive.DataSource = XMLPostTxImmunosuppressiveDataSource;
                cblPostTxImmunosuppressive.DataBind();


                //rv_txtRejectionTreatmentsPostTx.MinimumValue = ConstantsGeneral.intMinNumberAcuteRejections.ToString();
                //rv_txtRejectionTreatmentsPostTx.MaximumValue = ConstantsGeneral.intMaxNumberAcuteRejections.ToString();
                //rv_txtRejectionTreatmentsPostTx.ErrorMessage = "Value should be between " + rv_txtRejectionTreatmentsPostTx.MinimumValue + " and " + rv_txtRejectionTreatmentsPostTx.MaximumValue + ".";

                ////add values to dropdown
                //for (int i = 0; i <= ConstantsGeneral.intMaxNumberAcuteRejections; i++ )
                //{
                //    ddRejection.Items.Add(new ListItem(i.ToString(), i.ToString()));
                //}

                ddRejection.DataSource = XMLMainOptionsYNDataSource;
                ddRejection.DataBind();

                rblPostTxPrednisolon.DataSource = XMLMainOptionsYNDataSource;
                rblPostTxPrednisolon.DataBind();
                //rblPostTxPrednisolon.SelectedValue = STR_UNKNOWN_SELECTION;

                rblPostTxOther.DataSource = XMLMainOptionsYNDataSource;
                rblPostTxOther.DataBind();
                //rblPostTxOther.SelectedValue = STR_UNKNOWN_SELECTION;

                rblRejectionBiopsyProven.DataSource = XMLMainOptionsYNDataSource;
                rblRejectionBiopsyProven.DataBind();
                //rblRejectionBiopsyProven.SelectedValue = STR_UNKNOWN_SELECTION;

                rblCalcineurinInhibitorToxicity.DataSource = XMLMainOptionsYNDataSource;
                rblCalcineurinInhibitorToxicity.DataBind();
                //rblCalcineurinInhibitorToxicity.SelectedValue = STR_UNKNOWN_SELECTION;
                
                cblDialysisRequirementInitial.DataSource = XMLDialysisDaysDataSource;
                cblDialysisRequirementInitial.DataBind();


                

                

                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;


                ViewState["SortField"] = "FollowUpDate";
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


                //if kidney discarded throw up an error message

                string STRSQL_DISCARDED = "SELECT COUNT(*) CR FROM r_perioperative WHERE TrialIDRecipient=?TrialIDRecipient AND KidneyDiscarded='YES'";

                int intDiscarded = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQL_DISCARDED, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));


                if (intDiscarded > 1)
                {
                    throw new Exception("More than one Peri Operative data exists for this Recipient.");
                }

                if (intDiscarded == 1)
                {
                    throw new Exception("This Kidney was discarded. Follow Up Data cannnot be added.");
                }

                if (intDiscarded < 0)
                {
                    throw new Exception("An error occured while checking if this Kidney was discarded. ");
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
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID (Recipient).");
            }

            //// get the DonorID
            //string strDonorID = string.Empty;
            //string strRecipientID = string.Empty;

            //ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

            //if (mpCPH != null)
            //{
            //    Label lblMainLabel = (Label)(mpCPH.FindControl(strMainLabel));

            //    if (lblMainLabel != null)
            //    {
            //        strDonorID = lblMainLabel.Text.Replace("(", "");
            //        strDonorID = strDonorID.Replace(")", "");
            //    }

            //    Label lblRecipientLabel = (Label)(mpCPH.FindControl(strRecipientLabel));

            //    if (lblRecipientLabel != null)
            //    {
            //        strRecipientID = lblRecipientLabel.Text.Replace("(", "");
            //        strRecipientID = strRecipientID.Replace(")", "");
            //    }

            //}

            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.TrialID,  ";
            strSQL += "DATE_FORMAT(t1.FollowUpDate, '%d/%m/%Y') FollowUp_Date, ";
            strSQL += "DATE_FORMAT(t1.DateGraftFailure, '%d/%m/%Y') Date_GraftFailure, ";
            strSQL += "DATE_FORMAT(t1.DateGraftRemoval, '%d/%m/%Y') Date_GraftRemoval, ";
            strSQL += "DATE_FORMAT(t1.DateDeath, '%d/%m/%Y') Date_Death, ";
            strSQL += "DATE_FORMAT(t1.DateDialysisSince, '%d/%m/%Y') Date_DialysisSince, ";
            strSQL += "DATE_FORMAT(t1.DateLastDialysis, '%d/%m/%Y') Date_LastDialysis ";
            strSQL += "FROM r_fuposttreatment t1 ";
            strSQL += "LEFT JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "INNER JOIN trialdetails_recipient t3 ON t1.TrialIDRecipient=t3.TrialIDRecipient ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t1.Occasion=?Occasion ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());
            SqlDataSource1.SelectParameters.Add("?Occasion", strIncludeOccasion);


            GV1.DataBind();
            lblGV1.Text = "Click on Follow Up Date to Edit Recipient Follow Up Data (Days 1 to 14).";

            if (GV1.Rows.Count == 1)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                //cmdAddData.Text = "Update Data";
                //lblDescription.Text = "Update  Recipient Follow Up Data (Days 1 to 7) for " + Request.QueryString["TID_R"].ToString() + " (Recipient) and DonorID " + strDonorID;
                lblDescription.Text = "Update  Recipient Follow Up Data (Days 1 to 7) for " + Request.QueryString["TID_R"].ToString() + ".";
                //if (strRecipientID != string.Empty)
                //{ lblDescription.Text += " and RecipientID " + strRecipientID; }
                AssignData();

            }
            else if (GV1.Rows.Count == 0)
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                //lblDescription.Text = "Add  Recipient Follow Up Data (Days 1 to 7) for " + Request.QueryString["TID_R"].ToString() + " (Recipient) and DonorID " + strDonorID;
                lblDescription.Text = "Add  Recipient Follow Up Data (Days 1 to 7) for " + Request.QueryString["TID_R"].ToString() + ".";
                //if (strRecipientID != string.Empty)
                //{ lblDescription.Text += " and RecipientID " + strRecipientID; }
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


    //get old values 
    protected void AssignDates()
    {
        

        string strDateCreated = string.Empty;
        string strOperationStartDate = string.Empty;
        string strOperationStartTime = string.Empty;

        string strReperfusionDate = string.Empty;
        string strReperfusionTime = string.Empty;

        //string strKidneyReceived = string.Empty;

        try
        {
            string STRSQL = string.Empty;

            STRSQL = "SELECT t1.*, t3.OperationStartDate, t3.OperationStartTime,  t3.ReperfusionDate, t3.ReperfusionTime ";
            STRSQL += "FROM trialdetails t1  ";
            STRSQL += "LEFT JOIN trialdetails_recipient t2 ON t1.TrialID=t2.TrialID AND t2.TrialIDRecipient=?TrialIDRecipient ";
            STRSQL += "LEFT JOIN r_perioperative t3 ON  t2.TrialIDRecipient=t3.TrialIDRecipient AND t3.TrialIDRecipient=?TrialIDRecipient ";
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
                            if (!DBNull.Value.Equals(myDr["DateCreated"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateCreated"].ToString()) == true)
                                {
                                    strDateCreated = Convert.ToDateTime(myDr["DateCreated"]).ToString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["OperationStartDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["OperationStartDate"].ToString()) == true)
                                {
                                    strOperationStartDate = myDr["OperationStartDate"].ToString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["OperationStartTime"]))
                            {
                                if (myDr["OperationStartTime"].ToString().Length >= 5)
                                {
                                    strOperationStartTime = myDr["OperationStartTime"].ToString().Substring(0, 5);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["ReperfusionDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ReperfusionDate"].ToString()) == true)
                                {
                                    strReperfusionDate = myDr["ReperfusionDate"].ToString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ReperfusionTime"]))
                            {
                                if (myDr["ReperfusionTime"].ToString().Length >= 5)
                                {
                                    strReperfusionTime = myDr["ReperfusionTime"].ToString().Substring(0, 5);
                                }
                            }

                            
                           

                        }
                    }
                }


                ViewState["DateCreated"] = strDateCreated;
                ViewState["OperationStartDate"] = strOperationStartDate;
                ViewState["OperationStartTime"] = strOperationStartTime;
                ViewState["ReperfusionDate"] = strReperfusionDate;
                ViewState["ReperfusionTime"] = strReperfusionTime;



                //lblFollowUpDate.Text += "DateCreated " + ViewState["DateCreated"].ToString();
                //lblFollowUpDate.Text += "OperationStartDate " + ViewState["OperationStartDate"].ToString();
                //lblFollowUpDate.Text += "OperationStartTime " + ViewState["OperationStartTime"].ToString();
                //lblFollowUpDate.Text += "ReperfusionDate " + ViewState["ReperfusionDate"].ToString();
                //lblFollowUpDate.Text += "ReperfusionTime " + ViewState["ReperfusionTime"].ToString();
                



                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }

            catch (System.Exception)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                //do nothing

            }

        }
        catch (System.Exception)
        {
            //do nothing
        }
    }


    //assign data
    protected void AssignData()
    {
        try
        {
            //lblUserMessages.Text=string.Empty
            string STRSQL = string.Empty;

            STRSQL += "SELECT t1.*, t2.TrialID TrialID FROM  r_fuposttreatment t1 ";
            STRSQL += "INNER JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient  ";
            STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t1.Occasion=?Occasion ";
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
            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = strIncludeOccasion;;

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
                            if (!DBNull.Value.Equals(myDr["FollowUpDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["FollowUpDate"].ToString()) == true)
                                {
                                    txtFollowUpDate.Text = Convert.ToDateTime(myDr["FollowUpDate"]).ToShortDateString();
                                }
                            }

                            if (lblFollowUpDate.Font.Bold == true)
                            {
                                if (txtFollowUpDate.Text == string.Empty)
                                {
                                    lblFollowUpDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Occasion"]))
                            {
                                ddOccasion.SelectedValue = (string)(myDr["Occasion"]);
                            }

                            if (lblOccasion.Font.Bold == true)
                            {
                                if (ddOccasion.SelectedValue==STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblOccasion.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["GraftFailure"]))
                            {
                                rblGraftFailure.SelectedValue = (string)(myDr["GraftFailure"]);
                            }

                            if (lblGraftFailure.Font.Bold == true)
                            {
                                if (rblGraftFailure.SelectedIndex==-1)
                                {
                                    lblGraftFailure.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DateGraftFailure"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateGraftFailure"].ToString()) == true)
                                {
                                    txtDateGraftFailure.Text = Convert.ToDateTime(myDr["DateGraftFailure"]).ToShortDateString();
                                }
                            }

                            if (lblDateGraftFailure.Font.Bold == true)
                            {
                                if (txtDateGraftFailure.Text == string.Empty)
                                {
                                    lblDateGraftFailure.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PrimaryCause"]))
                            {
                                ddPrimaryCause.SelectedValue = (string)(myDr["PrimaryCause"]);
                            }

                            if (lblPrimaryCause.Font.Bold == true)
                            {
                                if (ddPrimaryCause.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblPrimaryCause.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PrimaryCauseOther"]))
                            {
                                txtPrimaryCauseOther.Text = (string)(myDr["PrimaryCauseOther"]);
                            }

                            if (lblPrimaryCauseOther.Font.Bold == true)
                            {
                                if (txtPrimaryCauseOther.Text == string.Empty)
                                {
                                    lblPrimaryCauseOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["GraftRemoval"]))
                            {
                                rblGraftRemoval.SelectedValue = (string)(myDr["GraftRemoval"]);
                            }

                            if (lblGraftRemoval.Font.Bold == true)
                            {
                                if (rblGraftRemoval.SelectedIndex == -1)
                                {
                                    lblGraftRemoval.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DateGraftRemoval"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateGraftRemoval"].ToString()) == true)
                                {
                                    txtDateGraftRemoval.Text = Convert.ToDateTime(myDr["DateGraftRemoval"]).ToShortDateString();
                                }
                            }

                            if (lblDateGraftRemoval.Font.Bold == true)
                            {
                                if (txtDateGraftRemoval.Text == string.Empty)
                                {
                                    lblDateGraftRemoval.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["Death"]))
                            //{
                            //    rblDeath.SelectedValue = (string)(myDr["Death"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["DateDeath"]))
                            //{
                            //    if (GeneralRoutines.IsDate(myDr["DateDeath"].ToString()) == true)
                            //    {
                            //        txtDateDeath.Text = Convert.ToDateTime(myDr["DateDeath"]).ToShortDateString();
                            //    }
                            //}


                            //if (!DBNull.Value.Equals(myDr["CauseDeath"]))
                            //{
                            //    rblCauseDeath.SelectedValue = (string)(myDr["CauseDeath"]);
                            //}


                            if (!DBNull.Value.Equals(myDr["DialysisType"]))
                            {
                                rblDialysisType.SelectedValue = (string)(myDr["DialysisType"]);
                            }

                            if (lblDialysisType.Font.Bold == true)
                            {
                                if (rblDialysisType.SelectedIndex == -1)
                                {
                                    lblDialysisType.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NeedDialysis"]))
                            {
                                
                                string[] strNeedDialysis_Sets = myDr["NeedDialysis"].ToString().Split(',');

                                for (int i = 0; i <= strNeedDialysis_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strNeedDialysis_Contents = strNeedDialysis_Sets[i].ToString().Split(':');

                                    ListItem currentCheckBox = cblDialysisRequirementInitial.Items.FindByValue(strNeedDialysis_Contents[0].ToString());

                                    if (currentCheckBox != null)
                                    {
                                        if (strNeedDialysis_Contents[1].ToString() == STR_YES_SELECTION)
                                        {
                                            currentCheckBox.Selected = true;
                                        }
                                        
                                    }

                                    
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CreatinineUnit"]))
                            {

                                ListItem liCreatinineUnit = rblRequiredHyperkalemia.Items.FindByValue(myDr["CreatinineUnit"].ToString());
                                if (liCreatinineUnit != null)
                                {
                                    rblRequiredHyperkalemia.SelectedValue = myDr["CreatinineUnit"].ToString();
                                }
                                
                            }


                            if (lblCreatinineUnit.Font.Bold == true)
                            {
                                if (rblCreatinineUnits.SelectedIndex == -1)
                                {
                                    lblCreatinineUnit.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["InductionTherapy"]))
                            {
                                ddInductionTherapy.SelectedValue = myDr["InductionTherapy"].ToString();
                            }

                            if (lblInductionTherapy.Font.Bold == true)
                            {
                                if (ddInductionTherapy.SelectedValue==STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblInductionTherapy.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["EGFR"]))
                            {
                                txtEGFR.Text = myDr["EGFR"].ToString();
                            }
                            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
                            ContentPlaceHolder mpSpecimeContentsCPH = (ContentPlaceHolder)(mpCPH.FindControl(strSpecimenContentsCPH));

                            if (!DBNull.Value.Equals(myDr["SerumCreatinine"]))
                            {
                                string[] strSC_Sets = myDr["SerumCreatinine"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    TextBox txtSerumCreatinine = (TextBox)(mpSpecimeContentsCPH.FindControl(strTxtSerumCreatinine + strSC_Contents[0].ToString()));
                                    txtSerumCreatinine.Text = strSC_Contents[1].ToString();
                                }
                            }


                            if (lblSerumCreatinineAdmission.Font.Bold == true)
                            {
                                if (txtSerumCreatinineAdmission.Text == string.Empty)
                                {
                                    lblSerumCreatinineAdmission.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (lblSerumCreatinine1.Font.Bold == true)
                            {
                                if (txtSerumCreatinine1.Text == string.Empty)
                                {
                                    lblSerumCreatinine1.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (lblSerumCreatinine2.Font.Bold == true)
                            {
                                if (txtSerumCreatinine2.Text == string.Empty)
                                {
                                    lblSerumCreatinine2.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (lblSerumCreatinine3.Font.Bold == true)
                            {
                                if (txtSerumCreatinine3.Text == string.Empty)
                                {
                                    lblSerumCreatinine3.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (lblSerumCreatinine4.Font.Bold == true)
                            {
                                if (txtSerumCreatinine4.Text == string.Empty)
                                {
                                    lblSerumCreatinine4.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (lblSerumCreatinine5.Font.Bold == true)
                            {
                                if (txtSerumCreatinine5.Text == string.Empty)
                                {
                                    lblSerumCreatinine5.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (lblSerumCreatinine6.Font.Bold == true)
                            {
                                if (txtSerumCreatinine6.Text == string.Empty)
                                {
                                    lblSerumCreatinine6.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (lblSerumCreatinine7.Font.Bold == true)
                            {
                                if (txtSerumCreatinine7.Text == string.Empty)
                                {
                                    lblSerumCreatinine7.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["RequiredHyperkalemia"]))
                            {
                                rblRequiredHyperkalemia.SelectedValue = myDr["RequiredHyperkalemia"].ToString();
                            }

                            


                            if (!DBNull.Value.Equals(myDr["PostTxImmunosuppressive"]))
                            {
                                //lblCentre.Text += myDr["OtherOrgansDonated"].ToString();
                                string[] strSC_Sets = myDr["PostTxImmunosuppressive"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    ListItem currentCheckBox = cblPostTxImmunosuppressive.Items.FindByValue(strSC_Contents[0].ToString());

                                    if (currentCheckBox != null)
                                    {
                                        if (strSC_Contents[1].ToString() == STR_YES_SELECTION)
                                        {
                                            currentCheckBox.Selected = true;
                                        }

                                    }

                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PostTxImmunosuppressiveOther"]))
                            {
                                txtPostTxImmunosuppressiveOther.Text = (string)(myDr["PostTxImmunosuppressiveOther"]);
                            }

                            if (lblPostTxImmunosuppressiveOther.Font.Bold == true)
                            {
                                if (txtPostTxImmunosuppressiveOther.Text == string.Empty)
                                {
                                    lblPostTxImmunosuppressiveOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Rejection"]))
                            {
                                //txtRejection.Text = (string)(myDr["Rejection"]);
                                ddRejection.SelectedValue = (string)(myDr["Rejection"]);
                            }

                            if (lblRejection.Font.Bold == true)
                            {
                                //if (txtRejectionTreatmentsPostTx.Text == string.Empty )
                                //{
                                //    lblRejectionTreatmentsPostTx.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                //}
                                if (ddRejection.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblRejection.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }

                            }

                            if (!DBNull.Value.Equals(myDr["PostTxPrednisolon"]))
                            {
                                rblPostTxPrednisolon.SelectedValue = (string)(myDr["PostTxPrednisolon"]);
                            }

                            if (lblPostTxPrednisolon.Font.Bold == true)
                            {
                                if (rblPostTxPrednisolon.SelectedIndex==-1)
                                {
                                    lblPostTxPrednisolon.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PostTxOther"]))
                            {
                                rblPostTxOther.SelectedValue = (string)(myDr["PostTxOther"]);
                            }

                            if (lblPostTxOther.Font.Bold == true)
                            {
                                if (rblPostTxOther.SelectedIndex == -1)
                                {
                                    lblPostTxOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PostTxOtherDetails"]))
                            {
                                txtPostTxOtherDetails.Text = (string)(myDr["PostTxOtherDetails"]);
                            }

                            if (lblPostTxOtherDetails.Font.Bold == true)
                            {
                                if (txtPostTxOtherDetails.Text == string.Empty)
                                {
                                    lblPostTxOtherDetails.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["RejectionBiopsyProven"]))
                            {
                                rblRejectionBiopsyProven.SelectedValue = (string)(myDr["RejectionBiopsyProven"]);
                            }


                            if (lblRejectionBiopsyProven.Font.Bold == true)
                            {
                                if (rblRejectionBiopsyProven.SelectedIndex == -1)
                                {
                                    lblRejectionBiopsyProven.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                           
                            if (!DBNull.Value.Equals(myDr["CalcineurinInhibitorToxicity"]))
                            {
                                rblCalcineurinInhibitorToxicity.SelectedValue = (string)(myDr["CalcineurinInhibitorToxicity"]);
                            }

                            if (lblCalcineurinInhibitorToxicity.Font.Bold == true)
                            {
                                if (rblCalcineurinInhibitorToxicity.SelectedIndex == -1)
                                {
                                    lblCalcineurinInhibitorToxicity.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DatePrimaryPostTxDischarge"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DatePrimaryPostTxDischarge"].ToString()) == true)
                                {
                                    txtDatePrimaryPostTxDischarge.Text = Convert.ToDateTime(myDr["DatePrimaryPostTxDischarge"]).ToShortDateString();
                                }
                            }

                            if (lblDatePrimaryPostTxDischarge.Font.Bold == true)
                            {
                                if (txtDatePrimaryPostTxDischarge.Text == string.Empty)
                                {
                                    lblDatePrimaryPostTxDischarge.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
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

                            rblGraftFailure_SelectedIndexChanged(this, EventArgs.Empty);
                            rblGraftRemoval_SelectedIndexChanged(this, EventArgs.Empty);


                            //rblHypotensivePeriod1_SelectedIndexChanged(this, EventArgs.Empty);
                            //rblHypotensivePeriod2_SelectedIndexChanged(this, EventArgs.Empty);

                            
                            ddRejection_SelectedIndexChanged(this, EventArgs.Empty);
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
                lblUserMessages.Text = ex.Message + " An error occured while executing Assign query.";
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Assigning data.";
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
            lblUserMessages.Text = string.Empty;

            string STRSQL_DISCARDED = "SELECT COUNT(*) CR FROM r_perioperative WHERE TrialIDRecipient=?TrialIDRecipient AND KidneyDiscarded='YES'";

            int intDiscarded = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQL_DISCARDED, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));


            if (intDiscarded > 1)
            {
                throw new Exception("More than one Peri Operative data exists for this Recipient.");
            }

            if (intDiscarded == 1)
            {
                throw new Exception("This Kidney was discarded. Follow Up Data cannnot be added.");
            }

            if (intDiscarded < 0)
            {
                throw new Exception("An error occured while checking if this Kidney was discarded. ");
            }

            if (rblGraftFailure.SelectedValue==STR_YES_SELECTION)
            {
                Page.Validate("GraftFailureYes");
            }

            if (rblGraftRemoval.SelectedValue == STR_YES_SELECTION)
            {
                Page.Validate("GraftRemovalYes");
            }

            if (!Page.IsValid)
            {
                throw new Exception("Please Check the values you have entered.");
            }

            

            int i = 0; //counter

                        
            //lblDescription.Text += " " + strCreatinineClearance;

            if (GeneralRoutines.IsDate(txtFollowUpDate.Text) == false)
            {
                throw new Exception("Please enter Follow Up Date in the correct format");
            }

            if (Convert.ToDateTime(txtFollowUpDate.Text) > DateTime.Today)
            {
                throw new Exception("Date of Follow Up cannot be greater than Today's date.");
            }

            
            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select Occasion.");
            }

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND FollowUpDate=?FollowUpDate AND Occasion <>'" + strIncludeOccasion + "' ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?FollowUpDate", Convert.ToDateTime(txtFollowUpDate.Text).ToString("yyyy-MM-dd"), STRCONN));


            if (intCountFind > 0)
            {
                throw new Exception("There already exists another Follow Up data for the date you have entered. Please Edit the existing data.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("Could not check if there already exists a Follow Up data for the date you have entered.");
            }


            STRSQL_FIND = "SELECT COUNT(*) CR FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";

            intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?Occasion", ddOccasion.SelectedValue, STRCONN));

            if (intCountFind > 1)
            {
                throw new Exception("There already exists more than one Follow Up data for the Occasion you have Selected. Please Edit/Delete the existing data.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("Could not check if there already exists a Follow Up data for the Occasion you have Selected.");
            }

            //Graft Failure
            if (rblGraftFailure.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please select an option for Graft Failure.");
            }

            if (rblGraftFailure.SelectedValue == STR_YES_SELECTION)
            {

                if (txtDateGraftFailure.Text!=string.Empty)
                {
                    if (GeneralRoutines.IsDate(txtDateGraftFailure.Text) == false)
                    {
                        throw new Exception("Since Graft failure is 'YES', Please Enter Date of Graft Failure");
                    }

                    if (Convert.ToDateTime(txtDateGraftFailure.Text) > DateTime.Today)
                    {
                        throw new Exception("Date of Graft Failure cannot be greater than Today's date.");
                    }
                }
                

                //if (ddPrimaryCause.SelectedValue == STR_DD_UNKNOWN_SELECTION) throw new Exception("Please Select Primary Cause of Graft Failure");

                if (ddPrimaryCause.SelectedValue == STR_OTHER_SELECTION)
                {
                    if (txtPrimaryCauseOther.Text == string.Empty)
                    {
                        throw new Exception("Since you have selected Primary Cause of Graft Failure as 'Other', Please provide details of 'Primary Cause (If Other)'");
                    }
                }
                else
                {
                    if (txtPrimaryCauseOther.Text != string.Empty)
                    {
                        throw new Exception("Since Primary Cause of Graft Failure is not 'Other', textbox 'Primary Cause (If Other)' should be empty.");
                    }
                }

            }
            // if Graft Failure is No
            else
            {
                if (txtDateGraftFailure.Text != string.Empty)
                {
                    throw new Exception("Since Graft failure is'NO', Date of Graft Failure should be empty.");
                }

                if (ddPrimaryCause.SelectedValue != STR_DD_UNKNOWN_SELECTION)
                {
                    throw new Exception("Since Graft failure is'NO', value for 'Primary Cause' should be empty.");
                }

                if (txtPrimaryCauseOther.Text != string.Empty)
                {
                    throw new Exception("Since Graft failure is'NO', value for 'Primary Cause Other' should be empty.");
                }
            }

            //Graft Removal
            if (rblGraftRemoval.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please select an option for Graft Removal.");
            }

            if (rblGraftRemoval.SelectedValue == STR_YES_SELECTION)
            {
                if (GeneralRoutines.IsDate(txtDateGraftRemoval.Text) == false) throw new Exception("Since Graft Removal is'YES', Please Enter Date of Graft Removal");

                if (Convert.ToDateTime(txtDateGraftRemoval.Text) > DateTime.Today)
                {
                    throw new Exception("Date of Graft Removal cannot be greater than Today's date.");
                }
            }

            // if Graft Removal is No
            else
            {
                if (txtDateGraftRemoval.Text != string.Empty)
                {
                    throw new Exception("Since Graft Removal is'NO', Date of Graft Failure should be empty.");
                }
            }


            ////Death
            //if (rblDeath.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please select an option for 'Death'.");
            //}

            //if (rblDeath.SelectedValue == STR_YES_SELECTION)
            //{
            //    if (GeneralRoutines.IsDate(txtDateDeath.Text) == false) throw new Exception("Since the option selected for 'Death' is 'YES', Please Enter Date of Death");

            //    if (Convert.ToDateTime(txtDateDeath.Text) > DateTime.Today)
            //    {
            //        throw new Exception("Date of Death cannot be greater than Today's date.");
            //    }
            //}

            //// if Death is No
            //else
            //{
            //    if (txtDateDeath.Text != string.Empty)
            //    {
            //        throw new Exception("Since the option selected for 'Death' is 'NO', Date of Death should be empty.");
            //    }

            //    if (rblCauseDeath.SelectedValue != STR_UNKNOWN_SELECTION)
            //    {
            //        throw new Exception("Since the option selected for 'Death' is 'NO', Cause of Death should be 'Unknown'.");
            //    }
            //}

            // serum creatinine
            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
            ContentPlaceHolder mpSpecimeContentsCPH = (ContentPlaceHolder)(mpCPH.FindControl(strSpecimenContentsCPH));

            //check if dialysis requirement is selected
            //check the options selected
            string strNeedDialysis = string.Empty;

            i = 0;//count the number of options selected
            
            for (i = 0; i < cblDialysisRequirementInitial.Items.Count; i++)
            {
                strNeedDialysis += cblDialysisRequirementInitial.Items[i].Value + ":";
                if (cblDialysisRequirementInitial.Items[i].Selected)
                {
                    strNeedDialysis += STR_YES_SELECTION;
                }
                else
                {
                    strNeedDialysis += STR_NO_SELECTION;
                }

                if (i < cblDialysisRequirementInitial.Items.Count - 1)
                {
                    strNeedDialysis += ",";
                }
            }

            if (cblDialysisRequirementInitial.SelectedIndex!=-1)
            {
                if (rblDialysisType.SelectedIndex!=-1)
                {
                    if (rblDialysisType.SelectedValue==STR_NOT_REQUIRED)
                    {
                        throw new Exception("Since at least one checkbox has been ticked for " + lblDialysisRequirementInitial.Text + ", " + lblDialysisType.Text + " cannot be  " + rblDialysisType.SelectedValue + ".");
                    }
                }
            }
            else
            {
                //no checkbox ticked
                if (rblDialysisType.SelectedIndex != -1)
                {
                    if (rblDialysisType.SelectedValue != STR_NOT_REQUIRED || rblDialysisType.SelectedValue != STR_UNKNOWN_SELECTION)
                    {
                        throw new Exception("Since at no checkbox has been ticked for " + lblDialysisRequirementInitial.Text + ", " + lblDialysisType.Text + " cannot be  " + rblDialysisType.SelectedValue + ".");
                    }
                }

            }
            string strSerumCreatinine = string.Empty;

            string strEGFR = string.Empty;
            string strCalulcatedEGFR = string.Empty;

            //serum creatinine Admission
            if (txtSerumCreatinineAdmission.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtSerumCreatinineAdmission.Text) == false)
                {
                    throw new Exception("Please enter Serum Creatinine in numeric format for 'Admission'");
                }

                //admisssion date get it
                strCalulcatedEGFR = string.Empty;
                
            }

            strSerumCreatinine += "Admission:" + txtSerumCreatinineAdmission.Text + ",";
            strEGFR += "Admission:" + strCalulcatedEGFR + ",";
            //string strTxtSerumCreatinine = "txtSerumCreatinine";
            for (i = 0; i < 7; i++)
            {

                strCalulcatedEGFR = string.Empty;

                TextBox txtSerumCreatinine = (TextBox)(mpSpecimeContentsCPH.FindControl(strTxtSerumCreatinine + (i + 1).ToString()));

                if (txtSerumCreatinine.Text != string.Empty)
                {
                    if (GeneralRoutines.IsNumeric(txtSerumCreatinine.Text) == false)
                    {
                        throw new Exception("Please enter Serum Creatinine in numeric format for Day " + (i + 1).ToString());
                    }

                    strCalulcatedEGFR = GeneralRoutines.eGFR_Calculate(Request.QueryString["TID_R"], Convert.ToDouble(txtSerumCreatinine.Text), Convert.ToDateTime(txtFollowUpDate.Text).AddDays(i), rblCreatinineUnits.SelectedValue);
                }
                else
                {
                    strCalulcatedEGFR = string.Empty;
                }

                
                strSerumCreatinine += (i + 1).ToString() + ":" + txtSerumCreatinine.Text ;

                strEGFR += (i + 1).ToString() + ":" + strCalulcatedEGFR;

                if (i < 6)
                {
                    strSerumCreatinine += ",";
                    strEGFR += ",";
                }

                
            }

            ListItem li = cblPostTxImmunosuppressive.Items.FindByValue(STR_OTHER_SELECTION);

            if (li !=null)
            {
                if (li.Selected==true)
                {
                    if (txtPostTxImmunosuppressiveOther.Text==string.Empty)
                    {
                        throw new Exception("Please provide details for " + lblPostTxImmunosuppressiveOther.Text + ", as " + STR_OTHER_SELECTION + " has been selected for " + lblPostTxImmunosuppressive.Text + ".");
                    }
                }
                else
                {
                    if (txtPostTxImmunosuppressiveOther.Text != string.Empty)
                    {
                        throw new Exception("" + lblPostTxImmunosuppressiveOther.Text + " should be empty as " + STR_OTHER_SELECTION + " has not been selected for " + lblPostTxImmunosuppressive.Text + ".");
                    }
                }
            }
            
                              

                       

            if (txtDatePrimaryPostTxDischarge.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDatePrimaryPostTxDischarge.Text) == false)
                {
                    throw new Exception("Please enter 'Date of Primary Post Tx Discharge' in the correct format.");
                }

                if (Convert.ToDateTime(txtDatePrimaryPostTxDischarge.Text) > DateTime.Today)
                {
                    throw new Exception("'Date of Primary Post Tx Discharge' cannot be greater than Today's date.");
                }

            }


            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }

            
            //add data

            string STRSQL = string.Empty;

            STRSQL += "INSERT INTO r_fuposttreatment ";
            STRSQL += "(TrialIDRecipient, Occasion, FollowUpDate, GraftFailure, DateGraftFailure, PrimaryCause, PrimaryCauseOther,";
            STRSQL += "GraftRemoval, DateGraftRemoval, ";
            STRSQL += "DialysisType, NeedDialysis,  RequiredHyperkalemia,  InductionTherapy, ";
            //STRSQL += "HypotensivePeriod1, HypotensivePeriod1Duration, HypotensivePeriod1LowestSystolicBloodPressure, HypotensivePeriod1LowestDiastolicBloodPressure,";
            //STRSQL += "HypotensivePeriod2,HypotensivePeriod2Duration, HypotensivePeriod2LowestSystolicBloodPressure, HypotensivePeriod2LowestDiastolicBloodPressure, ";
            STRSQL += "SerumCreatinine, CreatinineUnit, EGFR, ";
            STRSQL += "PostTxImmunosuppressive, PostTxImmunosuppressiveOther, Rejection, PostTxPrednisolon, PostTxOther, PostTxOtherDetails,";
            STRSQL += "RejectionBiopsyProven, CalcineurinInhibitorToxicity, DatePrimaryPostTxDischarge, EventCode,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?Occasion, ?FollowUpDate, ?GraftFailure, ?DateGraftFailure, ?PrimaryCause, ?PrimaryCauseOther,";
            STRSQL += "?GraftRemoval, ?DateGraftRemoval,  ";
            STRSQL += "?DialysisType, ?NeedDialysis,  ?RequiredHyperkalemia, ?InductionTherapy, ";
            //STRSQL += "?HypotensivePeriod1, ?HypotensivePeriod1Duration, ?HypotensivePeriod1LowestSystolicBloodPressure, ?HypotensivePeriod1LowestDiastolicBloodPressure,";
            //STRSQL += "?HypotensivePeriod2, ?HypotensivePeriod2Duration, ?HypotensivePeriod2LowestSystolicBloodPressure, ?HypotensivePeriod2LowestDiastolicBloodPressure, ";
            STRSQL += "?SerumCreatinine, ?CreatinineUnit, EGFR,";
            STRSQL += "?PostTxImmunosuppressive, ?PostTxImmunosuppressiveOther, ?Rejection, ?PostTxPrednisolon, ?PostTxOther, ?PostTxOtherDetails,";
            STRSQL += "?RejectionBiopsyProven, ?CalcineurinInhibitorToxicity, ?DatePrimaryPostTxDischarge, ?EventCode, ";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = string.Empty;

            STRSQL_UPDATE += "UPDATE r_fuposttreatment SET ";
            STRSQL_UPDATE += "Occasion=?Occasion, FollowUpDate=?FollowUpDate, GraftFailure=?GraftFailure, DateGraftFailure=?DateGraftFailure, ";
            STRSQL_UPDATE += "PrimaryCause=?PrimaryCause, PrimaryCauseOther=?PrimaryCauseOther,";
            STRSQL_UPDATE += "GraftRemoval=?GraftRemoval, DateGraftRemoval=?DateGraftRemoval,  ";
            STRSQL_UPDATE += "DialysisType=?DialysisType, NeedDialysis=?NeedDialysis,  RequiredHyperkalemia=?RequiredHyperkalemia,  InductionTherapy=?InductionTherapy, ";
            //STRSQL_UPDATE += "HypotensivePeriod1=?HypotensivePeriod1, HypotensivePeriod1Duration=?HypotensivePeriod1Duration, ";
            //STRSQL_UPDATE += "HypotensivePeriod1LowestSystolicBloodPressure=?HypotensivePeriod1LowestSystolicBloodPressure, ";
            //STRSQL_UPDATE += "HypotensivePeriod1LowestDiastolicBloodPressure=?HypotensivePeriod1LowestDiastolicBloodPressure,";
            //STRSQL_UPDATE += "HypotensivePeriod2=?HypotensivePeriod2,HypotensivePeriod2Duration=?HypotensivePeriod2Duration, ";
            //STRSQL_UPDATE += "HypotensivePeriod2LowestSystolicBloodPressure=?HypotensivePeriod2LowestSystolicBloodPressure, ";
            //STRSQL_UPDATE += "HypotensivePeriod2LowestDiastolicBloodPressure=?HypotensivePeriod2LowestDiastolicBloodPressure, ";
            STRSQL_UPDATE += "SerumCreatinine=?SerumCreatinine, CreatinineUnit=?CreatinineUnit, EGFR=?EGFR, ";
            STRSQL_UPDATE += "PostTxImmunosuppressive=?PostTxImmunosuppressive, PostTxImmunosuppressiveOther=?PostTxImmunosuppressiveOther,";
            STRSQL_UPDATE += "Rejection=?Rejection, PostTxPrednisolon=?PostTxPrednisolon, PostTxOther=?PostTxOther, PostTxOtherDetails=?PostTxOtherDetails, ";
            STRSQL_UPDATE += "RejectionBiopsyProven=?RejectionBiopsyProven,";
            STRSQL_UPDATE += "CalcineurinInhibitorToxicity=?CalcineurinInhibitorToxicity, ";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?DefaultOccasion ";


            //lock data
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE r_fuposttreatment SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) AND Occasion=?DefaultOccasion ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE r_fuposttreatment SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?DefaultOccasion ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            STRSQL_FIND = string.Empty;

            STRSQL_FIND = "SELECT COUNT(*) CR FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?DefaultOccasion ";

            intCountFind = 0;
            intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?DefaultOccasion", strIncludeOccasion, STRCONN));

            if (intCountFind == 1)
            {
                MyCMD.CommandText = STRSQL_UPDATE;
            }
            else if (intCountFind == 0)
            {
                MyCMD.CommandText = STRSQL;
            }

            else if (intCountFind > 1)
            {
                throw new Exception("More than One Follow Up (Day 1 to 14) Data exists for this TrialID. Please Delete an existing Follow Up (Day 1 to 14) Data.");
            }

            else
            {
                throw new Exception("An error occured while checking if Follow Up (Day 1 to 14) data already exist in the database.");
            }

                       


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();
            MyCMD.Parameters.Add("?DefaultOccasion", MySqlDbType.VarChar).Value = strIncludeOccasion;

            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtFollowUpDate.Text) == false)
            {
                MyCMD.Parameters.Add("?FollowUpDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?FollowUpDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtFollowUpDate.Text);
            }

            if (rblGraftFailure.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?GraftFailure", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?GraftFailure", MySqlDbType.VarChar).Value = rblGraftFailure.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtDateGraftFailure.Text) == false)
            {
                MyCMD.Parameters.Add("?DateGraftFailure", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateGraftFailure", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateGraftFailure.Text);
            }

            if (ddPrimaryCause.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?PrimaryCause", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PrimaryCause", MySqlDbType.VarChar).Value = ddPrimaryCause.SelectedValue;
            }

            if (txtPrimaryCauseOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PrimaryCauseOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PrimaryCauseOther", MySqlDbType.VarChar).Value = txtPrimaryCauseOther.Text;
            }

            if (rblGraftRemoval.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?GraftRemoval", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            {
                MyCMD.Parameters.Add("?GraftRemoval", MySqlDbType.VarChar).Value = rblGraftRemoval.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtDateGraftRemoval.Text) == false)
            {
                MyCMD.Parameters.Add("?DateGraftRemoval", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateGraftRemoval", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateGraftRemoval.Text);
            }

            //if (rblDeath.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?Death", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //{
            //    MyCMD.Parameters.Add("?Death", MySqlDbType.VarChar).Value = rblDeath.SelectedValue;
            //}

            //if (GeneralRoutines.IsDate(txtDateDeath.Text) == false)
            //{
            //    MyCMD.Parameters.Add("?DateDeath", MySqlDbType.Date).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?DateDeath", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateDeath.Text);
            //}

            //if (rblCauseDeath.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?CauseDeath", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?CauseDeath", MySqlDbType.VarChar).Value = rblCauseDeath.SelectedValue;
            //}

            if (rblDialysisType.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?DialysisType", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DialysisType", MySqlDbType.VarChar).Value = rblDialysisType.SelectedValue;
            }

            if (strNeedDialysis == string.Empty)
            {
                MyCMD.Parameters.Add("?NeedDialysis", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NeedDialysis", MySqlDbType.VarChar).Value = strNeedDialysis;
            }

            if (rblRequiredHyperkalemia.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?RequiredHyperkalemia", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RequiredHyperkalemia", MySqlDbType.VarChar).Value = rblRequiredHyperkalemia.SelectedValue;
            }

            if (ddInductionTherapy.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?InductionTherapy", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?InductionTherapy", MySqlDbType.VarChar).Value = ddInductionTherapy.SelectedValue;
            }

            //if (rblHypotensivePeriod1.SelectedIndex == -1)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1", MySqlDbType.VarChar).Value = rblHypotensivePeriod1.SelectedValue;
            //}

            //if (txtHypotensivePeriod1Duration.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1Duration", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1Duration", MySqlDbType.VarChar).Value = txtHypotensivePeriod1Duration.Text;
            //}

            //if (txtHypotensivePeriod1LowestSystolicBloodPressure.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1LowestSystolicBloodPressure", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1LowestSystolicBloodPressure", MySqlDbType.VarChar).Value = txtHypotensivePeriod1LowestSystolicBloodPressure.Text;
            //}

            //if (txtHypotensivePeriod1LowestDiastolicBloodPressure.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1LowestDiastolicBloodPressure", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod1LowestDiastolicBloodPressure", MySqlDbType.VarChar).Value = txtHypotensivePeriod1LowestDiastolicBloodPressure.Text;
            //}

            //if (rblHypotensivePeriod2.SelectedIndex == -1)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2", MySqlDbType.VarChar).Value = rblHypotensivePeriod2.SelectedValue;
            //}

            //if (txtHypotensivePeriod2Duration.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2Duration", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2Duration", MySqlDbType.VarChar).Value = txtHypotensivePeriod2Duration.Text;
            //}

            //if (txtHypotensivePeriod2LowestSystolicBloodPressure.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2LowestSystolicBloodPressure", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2LowestSystolicBloodPressure", MySqlDbType.VarChar).Value = txtHypotensivePeriod2LowestSystolicBloodPressure.Text;
            //}

            //if (txtHypotensivePeriod2LowestDiastolicBloodPressure.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2LowestDiastolicBloodPressure", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?HypotensivePeriod2LowestDiastolicBloodPressure", MySqlDbType.VarChar).Value = txtHypotensivePeriod2LowestDiastolicBloodPressure.Text;
            //}

            if (rblCreatinineUnits.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?CreatinineUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CreatinineUnit", MySqlDbType.VarChar).Value = rblCreatinineUnits.SelectedValue;
            }
            //string strSerumCreatinine = string.Empty;

            if (strSerumCreatinine == string.Empty)
            {
                MyCMD.Parameters.Add("?SerumCreatinine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SerumCreatinine", MySqlDbType.VarChar).Value = strSerumCreatinine;
            }
            
            //append selection
            string strPostTxImmunosuppression = string.Empty;
            //Set up connection and command objects
            //Open connection

            for ( i = 0; i < cblPostTxImmunosuppressive.Items.Count; i++)
            {
                strPostTxImmunosuppression += cblPostTxImmunosuppressive.Items[i].Value + ":";
                if (cblPostTxImmunosuppressive.Items[i].Selected)
                {
                    strPostTxImmunosuppression += STR_YES_SELECTION;
                }
                else
                {
                    strPostTxImmunosuppression += STR_NO_SELECTION;
                }

                if (i < cblPostTxImmunosuppressive.Items.Count - 1)
                {
                    strPostTxImmunosuppression += ",";
                }
            }

            ////calculate EGFR
            //if (txtSerumCreatinine7.Text==string.Empty)
            //{
            //    MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = txtSerumCreatinine7.Text;
            //}

            //calculate EGFR
            if (strEGFR == string.Empty)
            {
                MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = strEGFR;
            }

            if (strPostTxImmunosuppression == string.Empty)
            {
                MyCMD.Parameters.Add("?PostTxImmunosuppressive", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PostTxImmunosuppressive", MySqlDbType.VarChar).Value = strPostTxImmunosuppression;
            }

            if (txtPostTxImmunosuppressiveOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PostTxImmunosuppressiveOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PostTxImmunosuppressiveOther", MySqlDbType.VarChar).Value = txtPostTxImmunosuppressiveOther.Text;
            }

            if (ddRejection.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Rejection", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Rejection", MySqlDbType.VarChar).Value = ddRejection.SelectedValue;
            }

            if (rblPostTxPrednisolon.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?PostTxPrednisolon", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PostTxPrednisolon", MySqlDbType.VarChar).Value = rblPostTxPrednisolon.SelectedValue;
            }

            if (rblPostTxOther.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?PostTxOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PostTxOther", MySqlDbType.VarChar).Value = rblPostTxOther.SelectedValue;
            }

            if (txtPostTxOtherDetails.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PostTxOtherDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PostTxOtherDetails", MySqlDbType.VarChar).Value = txtPostTxOtherDetails.Text;
            }

            if (rblRejectionBiopsyProven.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?RejectionBiopsyProven", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RejectionBiopsyProven", MySqlDbType.VarChar).Value = rblRejectionBiopsyProven.SelectedValue;
            }

            if (rblCalcineurinInhibitorToxicity.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?CalcineurinInhibitorToxicity", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CalcineurinInhibitorToxicity", MySqlDbType.VarChar).Value = rblCalcineurinInhibitorToxicity.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtDatePrimaryPostTxDischarge.Text) == false)
            {
                MyCMD.Parameters.Add("?DatePrimaryPostTxDischarge", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DatePrimaryPostTxDischarge", MySqlDbType.Date).Value =Convert.ToDateTime(txtDatePrimaryPostTxDischarge.Text);
            }

            MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = intDefaultEventCode.ToString();


            if (string.IsNullOrEmpty(txtComments.Text))
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
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
                { MyCONN.Close(); }

                lblUserMessages.Text = "Data Added.";
                BindData();


                string strSQLCOMPLETE = string.Empty;
                strSQLCOMPLETE += "SELECT ";
                strSQLCOMPLETE += "IF(t2.Occasion IS NOT NULL AND t2.FollowUpDate IS NOT NULL AND t2.GraftFailure IS NOT NULL AND t2.GraftRemoval IS NOT NULL   ";
                //strSQLCOMPLETE += "AND t2.SerumCreatinine NOT LIKE '%:,%' AND t2.CreatinineUnit IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.CreatinineUnit IS NOT NULL ";
                strSQLCOMPLETE += "AND IF(t2.NeedDialysis LIKE '%YES%', t2.DialysisType IS NOT NULL, t2.NeedDialysis IS NOT NULL) ";
                strSQLCOMPLETE += "  ";
                strSQLCOMPLETE += "AND IF(t2.Rejection ='YES', t2.PostTxPrednisolon IS NOT NULL AND t2.PostTxOther IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.RejectionBiopsyProven IS NOT NULL,  t2.Rejection IS NOT NULL) ";
                strSQLCOMPLETE += " ";
                strSQLCOMPLETE += " ";
                //strSQLCOMPLETE += "AND t2.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM r_fuposttreatment t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialIDRecipient=?TrialIDRecipient AND Occasion='" + strIncludeOccasion + "' ";
                strSQLCOMPLETE += "";

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                //lblDonorRiskIndex.Text = strComplete;

                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"] +"&TID_R=" +  Request.QueryString["TID_R"], false);
                }
                else
                {
                    if (Request.Url.AbsoluteUri.Contains("&SCode=1"))
                    {
                        Response.Redirect(Request.Url.AbsoluteUri);

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

                Page.MaintainScrollPositionOnPostBack = false;

                lblUserMessages.Text = ex.Message + " An error occured while executing insert query.";
            }


            

            

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }


    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?Occasion", strIncludeOccasion, STRCONN));

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
                if (String.IsNullOrEmpty(Request.QueryString["RFUPostTreatmentID"]))
                {
                    throw new Exception("More than one Record exists for deletion.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM r_fuposttreatment ";
            STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";
            if (intCountFind > 1)
            {
                STRSQL += "AND RFUPostTreatmentID=?RFUPostTreatmentID ";
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

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();
            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = strIncludeOccasion;

            if (!String.IsNullOrEmpty(Request.QueryString["RFUPostTreatmentID"]))
            {
                MyCMD.Parameters.Add("?RFUPostTreatmentID", MySqlDbType.VarChar).Value = Request.QueryString["RFUPostTreatmentID"].ToString();
            }

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
    
    //protected void rblHypotensivePeriod1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (rblHypotensivePeriod1.SelectedValue == STR_YES_SELECTION)
    //        {
    //            pnlHypotensivePeriod1.Visible = true;
    //        }
    //        else
    //        {
    //            txtHypotensivePeriod1Duration.Text = string.Empty;
    //            txtHypotensivePeriod1LowestSystolicBloodPressure.Text = string.Empty;
    //            txtHypotensivePeriod1LowestDiastolicBloodPressure.Text = string.Empty;
    //            pnlHypotensivePeriod1.Visible = false;
    //        }
    //    }

    //    catch (System.Exception Exception)
    //    {
    //        lblUserMessages.Text = Exception.Message + " An error occured while Selecting Hypotensive Period I";
    //    }
        
    //}
    //protected void rblHypotensivePeriod2_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (rblHypotensivePeriod2.SelectedValue == STR_YES_SELECTION)
    //        {
    //            pnlHypotensivePeriod2.Visible = true;
    //        }
    //        else
    //        {
    //            txtHypotensivePeriod2Duration.Text = string.Empty;
    //            txtHypotensivePeriod2LowestSystolicBloodPressure.Text = string.Empty;
    //            txtHypotensivePeriod2LowestDiastolicBloodPressure.Text = string.Empty;
    //            pnlHypotensivePeriod2.Visible = false;
    //        }
    //    }

    //    catch (System.Exception Exception)
    //    {
    //        lblUserMessages.Text = Exception.Message + " An error occured while Selecting Hypotensive Period II";
    //    }
    //}
    protected void rblGraftFailure_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblGraftFailure.SelectedValue == STR_YES_SELECTION)
            {
                pnlGraftFailure.Visible = true;
            }
            else
            {
                txtDateGraftFailure.Text = string.Empty;
                ddPrimaryCause.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                txtPrimaryCauseOther.Text = string.Empty;

                pnlGraftFailure.Visible = false;
            }
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting graft Failure.";
        }
    }
    protected void rblGraftRemoval_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblGraftRemoval.SelectedValue==STR_YES_SELECTION)
            {
                pnlGraftRemoval.Visible = true;
            }
            else
            {
                pnlGraftRemoval.Visible = false;
                txtDateGraftRemoval.Text = string.Empty;
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Graft Removal.";
        }
    }
    protected void rblCreatinineUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Creatinine Unit.";
        }
    }

    protected void ddRejection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddRejection.SelectedValue==STR_DD_UNKNOWN_SELECTION || ddRejection.SelectedValue=="NO")
            {
                pnlRejection.Visible = false;
                rblPostTxPrednisolon.SelectedIndex = -1;
                rblPostTxOther.SelectedIndex = -1;
                txtPostTxOtherDetails.Text = string.Empty;
                rblRejectionBiopsyProven.SelectedIndex = -1;
                
            }
            else
            {
                pnlRejection.Visible = true;
                
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting " + lblRejection.Text + ". ";
        }
    }

    //protected void txtRejectionTreatmentsPostTx_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (GeneralRoutines.IsNumeric(txtRejectionTreatmentsPostTx.Text))
    //        {
    //            if (Convert.ToInt32(txtRejectionTreatmentsPostTx.Text)>0)
    //            {
    //                pnlRejection.Visible = true;
    //            }
    //            else
    //            {
    //                pnlRejection.Visible = false;
    //                rblPostTxPrednisolon.SelectedIndex = -1;
    //                rblPostTxOther.SelectedIndex = -1;
    //                txtPostTxOtherDetails.Text = string.Empty;
    //                rblRejectionBiopsyProven.SelectedIndex = -1;
    //            }
    //        }
    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while selecting " + lblRejectionTreatmentsPostTx.Text + ". ";
    //    }
    //}

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