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

using System.Net;
using System.Net.Mail;


public partial class SpecClinicalData_AddFUPostTransplant : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

        private const string strExcludeOccasion = "1-7 Days";

        private const string strEditRedirect = "~/SpecClinicalData/EditFUPostTransplant.aspx?TID=";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

        private const string strSpecimenContents = "SpecimenContents"; //content place holder in SpecClinicalMasterPage

        private const string strPanel = "pnlOccasionSelected";//main panel containing controls when Occasion is selected

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        
        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const string strDefaultFontColor="Black";


        private const string strRedirectCurrentPage = "~/SpecClinicalData/AddFUPostTransplant.aspx?TID=";
        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";

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


                lblDescription.Text = "Add  Recipient Follow Up Data for " + Request.QueryString["TID_R"].ToString() + ".";


                //MaintainScrollPositionOnPostBack = true;
                
                

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                ddOccasion.DataSource = XMLFollowupsDataSource;
                ddOccasion.DataBind();

                if (!string.IsNullOrEmpty(Request.QueryString["Occasion"]))
                {

                    ListItem liOccasion = ddOccasion.Items.FindByValue(Request.QueryString["Occasion"]);

                    if (liOccasion != null)
                    {
                        ddOccasion.SelectedIndex = -1;
                        liOccasion.Selected = true;
                    }
                }



                AssignDates();

                Int32 intMonths = 0;
                Double dblMinDays = 0;
                Double dblMaxDays = 0;

                if (ddOccasion.SelectedValue==STR_DD_UNKNOWN_SELECTION)
                {
                    intMonths = 0;
                    dblMinDays = 0;
                    dblMaxDays = 0;
                }
                else
                {
                    if (ddOccasion.SelectedValue=="3 Months")
                    {
                        intMonths = 3;
                        dblMinDays = ConstantsGeneral.intMin3Months;
                        dblMaxDays = ConstantsGeneral.intMax3Months;
                    }
                    else if (ddOccasion.SelectedValue == "6 Months")
                    {
                        intMonths = 6;
                        dblMinDays = ConstantsGeneral.intMin6Months;
                        dblMaxDays = ConstantsGeneral.intMax6Months;
                    }
                    else if (ddOccasion.SelectedValue == "1 Year")
                    {
                        intMonths = 12;
                        dblMinDays = ConstantsGeneral.intMin1Year;
                        dblMaxDays = ConstantsGeneral.intMax1Year;
                    }
                    else
                    {
                        intMonths = 0;
                        dblMinDays = 0;
                        dblMaxDays = 0;
                    }
                }

                DateTime dteBaselineFollowUpDate = DateTime.MinValue;


                if (intMonths!=0)
                {
                    if (ViewState["Day1FollowUpDate"] != null)
                    {
                        if (GeneralRoutines.IsDate(ViewState["Day1FollowUpDate"].ToString()))
                        {

                            dteBaselineFollowUpDate = Convert.ToDateTime(ViewState["Day1FollowUpDate"]).AddMonths(intMonths);
                                                       
                        }
                        
                    }
                    else
                    {
                        if (ViewState["OperationStartDate"] != null)
                        {
                            if (GeneralRoutines.IsDate(ViewState["OperationStartDate"].ToString()))
                            {
                                dteBaselineFollowUpDate = Convert.ToDateTime(ViewState["OperationStartDate"]).AddMonths(intMonths);
                            }
                            
                        }
                        else
                        {
                            if (GeneralRoutines.IsDate(ViewState["DateCreated"].ToString()))
                            {
                                dteBaselineFollowUpDate = Convert.ToDateTime(ViewState["DateCreated"]).AddMonths(intMonths);
                            }
                        }
                    }
                }
                else
                {
                    //do nothing
                }


                if (dteBaselineFollowUpDate!=DateTime.MinValue)
                {
                    rv_txtFollowUpDate.MinimumValue = dteBaselineFollowUpDate.AddDays(dblMinDays).ToShortDateString();

                    rv_txtFollowUpDate.MaximumValue = dteBaselineFollowUpDate.AddDays(dblMaxDays).ToShortDateString();
                }
                else
                {
                    rv_txtFollowUpDate.MinimumValue = "01/11/2014";

                    rv_txtFollowUpDate.MaximumValue = DateTime.Today.ToShortDateString();
                }
                rv_txtFollowUpDate.ErrorMessage = "Date should be between " + rv_txtFollowUpDate.MinimumValue + " and " + rv_txtFollowUpDate.MaximumValue + ". Please check the date you have entered.";

                //lblFollowUpDate.Text = rv_txtFollowUpDate.ErrorMessage;
                //rblKidneyDiscarded.SelectedValue = STR_UNKNOWN_SELECTION;

                pnlGraftFailure.Visible = false;
                pnlQOL.Visible = false;
                //remove 1-14 Days from the drop down box
                //ListItem remListItem = ddOccasion.Items.FindByText(strExcludeOccasion);
                //ddOccasion.Items.Remove(remListItem);

                rblGraftFailure.DataSource = XMLMainOptionsYNDataSource;
                rblGraftFailure.DataBind();
                //rblGraftFailure.SelectedValue = STR_UNKNOWN_SELECTION;


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
                //rblCreatinineUnits.SelectedValue = strCreatinineUnits;               

                rblUrineCreatinineUnits.DataSource = XMLUrineCreatinineUnitsDataSource;
                rblUrineCreatinineUnits.DataBind();

                rblCreatinineClearanceUnits.DataSource = XMLCreatinineClearanceUnitsDataSource;
                rblCreatinineClearanceUnits.DataBind();
                                
                cblPostTxImmunosuppressive.DataSource = XMLPostTxImmunosuppressiveDataSource;
                cblPostTxImmunosuppressive.DataBind();

                rblCurrentlyDialysis.DataSource = XMLMainOptionsDataSource;
                rblCurrentlyDialysis.DataBind();
                //rblCurrentlyDialysis.SelectedValue = STR_UNKNOWN_SELECTION;

                rblDialysisType.DataSource = XMLDialysisTypesDataSource;
                rblDialysisType.DataBind();
                //rblDialysisType.SelectedValue = STR_UNKNOWN_SELECTION;

                //ddInductionTherapy.DataSource = XMLInductionTherapiesDataSource;
                //ddInductionTherapy.DataBind();

                ddRejection.DataSource = XMLMainOptionsYNDataSource;
                ddRejection.DataBind();

                rblPostTxPrednisolon.DataSource = XMLMainOptionsDataSource;
                rblPostTxPrednisolon.DataBind();
                //rblPostTxPrednisolon.SelectedValue = STR_UNKNOWN_SELECTION;

                rblPostTxOther.DataSource = XMLMainOptionsDataSource;
                rblPostTxOther.DataBind();
                //rblPostTxOther.SelectedValue = STR_UNKNOWN_SELECTION;

                rblRejectionBiopsyProven.DataSource = XMLMainOptionsDataSource;
                rblRejectionBiopsyProven.DataBind();
                //rblRejectionBiopsyProven.SelectedValue = STR_UNKNOWN_SELECTION;

                rblCalcineurinInhibitorToxicity.DataSource = XMLMainOptionsDataSource;
                rblCalcineurinInhibitorToxicity.DataBind();
                //rblCalcineurinInhibitorToxicity.SelectedValue = STR_UNKNOWN_SELECTION;

                ddQOLFilledAt.DataSource = XMLQOLFilledOptionsDataSource;
                ddQOLFilledAt.DataBind();

                ddMobility.DataSource = XMLMobilityDataSource;
                ddMobility.DataBind();

                ddSelfCare.DataSource = XMLSelfCareDataSource;
                ddSelfCare.DataBind();

                ddUsualActivities.DataSource = XMLUsualActivitiesDataSource;
                ddUsualActivities.DataBind();

                ddPainDiscomfort.DataSource = XMLPainDiscomfortDataSource;
                ddPainDiscomfort.DataBind();

                ddAnxietyDepression.DataSource = XMLAnxietyDepressionDataSource;
                ddAnxietyDepression.DataBind();

                //rblICU.DataSource = XMLMainOptionsDataSource;
                //rblICU.DataBind();
                //rblICU.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblNeedDialysis.DataSource = XMLMainOptionsDataSource;
                //rblNeedDialysis.DataBind();
                //rblNeedDialysis.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblBiopsyTaken.DataSource = XMLMainOptionsDataSource;
                //rblBiopsyTaken.DataBind();
                //rblBiopsyTaken.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblSurgery.DataSource = XMLMainOptionsDataSource;
                //rblSurgery.DataBind();
                //rblSurgery.SelectedValue = STR_UNKNOWN_SELECTION;

                

                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;


                ViewState["SortField"] = "FollowUpDate";
                ViewState["SortDirection"] = "ASC";

                BindData();

                if (ddOccasion.SelectedValue!=STR_DD_UNKNOWN_SELECTION)
                {
                    AssignData();
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
            strSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "LEFT JOIN r_identification t3 ON t1.TrialIDRecipient=t3.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t1.Occasion<>?Occasion ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());
            SqlDataSource1.SelectParameters.Add("?Occasion", strExcludeOccasion);

            GV1.DataBind();
            lblGV1.Text = "Click on Follow Up Date to Edit Recipient Follow Up Data.";

            //lblDescription.Text = "Add  Recipient Follow Up Data for " + Request.QueryString["TID_R"].ToString() + " (Recipient) and DonorID " + strDonorID;
            //if (strRecipientID != string.Empty)
            //{ lblDescription.Text += " and RecipientID " + strRecipientID; }

            //loop though rows to highlight selected occasion
            Label lbl;

            foreach (GridViewRow row in GV1.Rows)
            {
                lbl = (Label)row.FindControl("lblOccasion");
                
                if (lbl !=null)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (String.IsNullOrEmpty(Request.QueryString["Occasion"]) == false)
                        {
                            if (lbl.Text == Request.QueryString["Occasion"])
                            {
                                row.BackColor = System.Drawing.Color.LightBlue;
                            }

                        }
                    }
                }
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
        string strDay1FollowUpDate = string.Empty;
        //string strKidneyReceived = string.Empty;

        try
        {
            string STRSQL = string.Empty;

            STRSQL = "SELECT t1.*, t3.OperationStartDate, t3.OperationStartTime,  t3.ReperfusionDate, t3.ReperfusionTime, t4.FollowUpDate ";
            STRSQL += "FROM trialdetails t1  ";
            STRSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "INNER JOIN r_perioperative t3 ON  t2.TrialIDRecipient=t3.TrialIDRecipient ";
            STRSQL += "LEFT JOIN r_fuposttreatment t4 ON  t2.TrialIDRecipient=t4.TrialIDRecipient AND Occasion='1-7 Days' ";
            STRSQL += "WHERE t3.TrialIDRecipient=?TrialIDRecipient ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            //MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];
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

                            if (!DBNull.Value.Equals(myDr["FollowUpDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["FollowUpDate"].ToString()) == true)
                                {
                                    strDay1FollowUpDate = myDr["FollowUpDate"].ToString();
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
                ViewState["Day1FollowUpDate"] = strDay1FollowUpDate;



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


    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;


            string STRSQL = "SELECT t1.*, t2.RecipientID FROM  r_fuposttreatment t1 ";
            STRSQL += "INNER JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t1.Occasion=?Occasion  ";
            //STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t1.RFUPostTreatmentID=?RFUPostTreatmentID";
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
            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

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
                                if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
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
                                if (rblGraftFailure.SelectedIndex == -1)
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

                            //if (lblGraftRemoval.Font.Bold == true)
                            //{
                            //    if (rblGraftRemoval.SelectedIndex == -1)
                            //    {
                            //        lblGraftRemoval.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                            //    }
                            //}

                            //if (rblGraftFailure.SelectedValue == STR_YES_SELECTION)
                            //{
                            //    pnlGraftFailure.Visible = true;
                            //}
                            //else
                            //{
                            //    txtDateGraftFailure.Text = string.Empty;
                            //    ddPrimaryCause.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                            //    txtPrimaryCauseOther.Text = string.Empty;

                            //    pnlGraftFailure.Visible = false;
                            //}


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

                            //if (lblGraftRemoval.Font.Bold == true)
                            //{
                            //    if (rblGraftRemoval.SelectedIndex == -1)
                            //    {
                            //        lblGraftRemoval.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                            //    }
                            //}

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


                            if (!DBNull.Value.Equals(myDr["SerumCreatinine"]))
                            {
                                txtSerumCreatinine.Text = (string)(myDr["SerumCreatinine"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CreatinineUnit"]))
                            {
                                rblCreatinineUnits.SelectedValue = (string)(myDr["CreatinineUnit"]);
                            }

                            if (lblSerumCreatinine.Font.Bold == true)
                            {
                                if (txtSerumCreatinine.Text == string.Empty || rblCreatinineUnits.SelectedIndex == -1)
                                {
                                    lblSerumCreatinine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["UrineCreatinine"]))
                            {
                                txtUrineCreatinine.Text = (string)(myDr["UrineCreatinine"]);
                            }

                            if (!DBNull.Value.Equals(myDr["UrineUnit"]))
                            {
                                rblUrineCreatinineUnits.SelectedValue = (string)(myDr["UrineUnit"]);
                            }


                            if (lblUrineCreatinine.Font.Bold == true)
                            {
                                if (txtUrineCreatinine.Text == string.Empty || rblUrineCreatinineUnits.SelectedIndex == -1)
                                {
                                    lblUrineCreatinine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CreatinineClearance"]))
                            {
                                txtCreatinineClearance.Text = (string)(myDr["CreatinineClearance"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CreatinineClearanceUnit"]))
                            {
                                rblCreatinineClearanceUnits.SelectedValue = (string)(myDr["CreatinineClearanceUnit"]);
                            }

                            if (lblCreatinineClearance.Font.Bold == true)
                            {
                                if (txtCreatinineClearance.Text == string.Empty || rblCreatinineClearanceUnits.SelectedIndex == -1)
                                {
                                    lblCreatinineClearance.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CurrentlyDialysis"]))
                            {
                                rblCurrentlyDialysis.SelectedValue = (string)(myDr["CurrentlyDialysis"]);
                            }

                            if (lblCurrentlyDialysis.Font.Bold == true)
                            {
                                if (rblCurrentlyDialysis.SelectedIndex == -1)
                                {
                                    lblCurrentlyDialysis.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

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


                            if (!DBNull.Value.Equals(myDr["DateLastDialysis"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateLastDialysis"].ToString()) == true)
                                {
                                    txtDateLastDialysis.Text = Convert.ToDateTime(myDr["DateLastDialysis"]).ToShortDateString();
                                }
                            }

                            if (lblDateLastDialysis.Font.Bold == true)
                            {
                                if (txtDateLastDialysis.Text == string.Empty)
                                {
                                    lblDateLastDialysis.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DialysisSessions"]))
                            {
                                txtDialysisSessions.Text = (string)(myDr["DialysisSessions"]);
                            }

                            if (lblDialysisSessions.Font.Bold == true)
                            {
                                if (txtDialysisSessions.Text == string.Empty)
                                {
                                    lblDialysisSessions.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["InductionTherapy"]))
                            //{
                            //    ddInductionTherapy.SelectedValue = (string)(myDr["InductionTherapy"]);
                            //}


                            //if (lblInductionTherapy.Font.Bold == true)
                            //{
                            //    if (ddInductionTherapy.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                            //    {
                            //        lblInductionTherapy.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                            //    }
                            //}

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
                                ddRejection.SelectedValue = (string)(myDr["Rejection"]);
                            }

                            if (lblRejection.Font.Bold == true)
                            {
                                if (ddRejection.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblRejection.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }

                            }

                            if (!DBNull.Value.Equals(myDr["RejectionTreatmentsPostTx"]))
                            {
                                txtRejectionTreatmentsPostTx.Text = (string)(myDr["RejectionTreatmentsPostTx"]);
                            }

                            if (lblRejectionTreatmentsPostTx.Font.Bold == true)
                            {
                                if (txtRejectionTreatmentsPostTx.Text==string.Empty)
                                {
                                    lblRejectionTreatmentsPostTx.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }

                            }

                            if (!DBNull.Value.Equals(myDr["PostTxPrednisolon"]))
                            {
                                rblPostTxPrednisolon.SelectedValue = (string)(myDr["PostTxPrednisolon"]);
                            }

                            if (lblPostTxPrednisolon.Font.Bold == true)
                            {
                                if (rblPostTxPrednisolon.SelectedIndex == -1)
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

                            //if (!DBNull.Value.Equals(myDr["ICU"]))
                            //{
                            //    rblICU.SelectedValue = (string)(myDr["ICU"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["NeedDialysis"]))
                            //{
                            //    rblNeedDialysis.SelectedValue = (string)(myDr["NeedDialysis"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["BiopsyTaken"]))
                            //{
                            //    rblBiopsyTaken.SelectedValue = (string)(myDr["BiopsyTaken"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["Surgery"]))
                            //{
                            //    rblSurgery.SelectedValue = (string)(myDr["Surgery"]);
                            //}
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

                            if (!DBNull.Value.Equals(myDr["ComplicationsGraftFunction"]))
                            {
                                txtComplicationsGraftFunction.Text = (string)(myDr["ComplicationsGraftFunction"]);
                            }

                            if (lblComplicationsGraftFunction.Font.Bold == true)
                            {
                                if (txtComplicationsGraftFunction.Text == string.Empty)
                                {
                                    lblComplicationsGraftFunction.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["QOLFilledAt"]))
                            {
                                ddQOLFilledAt.SelectedValue = myDr["QOLFilledAt"].ToString();
                            }

                            if (lblQOLFilledAt.Font.Bold == true)
                            {
                                if (ddQOLFilledAt.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddQOLFilledAt.SelectedIndex == -1)
                                {
                                    lblQOLFilledAt.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Mobility"]))
                            {
                                ddMobility.SelectedValue = myDr["Mobility"].ToString();
                            }

                            if (lblMobility.Font.Bold == true)
                            {
                                if (ddMobility.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddMobility.SelectedIndex == -1)
                                {
                                    lblMobility.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["SelfCare"]))
                            {
                                ddSelfCare.SelectedValue = myDr["SelfCare"].ToString();
                            }

                            if (lblSelfCare.Font.Bold == true)
                            {
                                if (ddSelfCare.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddSelfCare.SelectedIndex == -1)
                                {
                                    lblSelfCare.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["UsualActivities"]))
                            {
                                ddUsualActivities.SelectedValue = myDr["UsualActivities"].ToString();
                            }

                            if (lblUsualActivities.Font.Bold == true)
                            {
                                if (ddUsualActivities.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddUsualActivities.SelectedIndex == -1)
                                {
                                    lblUsualActivities.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PainDiscomfort"]))
                            {
                                ddPainDiscomfort.SelectedValue = myDr["PainDiscomfort"].ToString();
                            }

                            if (lblPainDiscomfort.Font.Bold == true)
                            {
                                if (ddPainDiscomfort.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddPainDiscomfort.SelectedIndex == -1)
                                {
                                    lblPainDiscomfort.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["AnxietyDepression"]))
                            {
                                ddAnxietyDepression.SelectedValue = myDr["AnxietyDepression"].ToString();
                            }

                            if (lblAnxietyDepression.Font.Bold == true)
                            {
                                if (ddAnxietyDepression.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddAnxietyDepression.SelectedIndex == -1)
                                {
                                    lblAnxietyDepression.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["VASScore"]))
                            {
                                txtVASScore.Text = myDr["VASScore"].ToString();
                            }

                            if (lblVASScore.Font.Bold == true)
                            {
                                if (txtVASScore.Text == string.Empty)
                                {
                                    lblVASScore.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
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

                            lblDescription.Text = "Update  Recipient Follow Up Data for " + Request.QueryString["TID_R"].ToString() + " (" + ddOccasion.SelectedValue + ").";
                            cmdDelete.Visible = true;
                            
                        }
                    }

                    else
                    {
                        ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

                        if (mpCPH != null)
                        {
                            ContentPlaceHolder mpCPH_SCMP = (ContentPlaceHolder)(mpCPH.FindControl(strSpecimenContents));

                            //get the panel

                            Panel pnl = (Panel)(mpCPH_SCMP.FindControl(strPanel));

                            if (pnl != null)
                            {

                                Label lbl;
                                TextBox tb;
                                RadioButtonList rbl;
                                Panel pnlNested;

                                foreach (Control c in pnl.Controls)
                                {

                                    if (c.GetType() == typeof(Label))
                                    {
                                        lbl = (Label)c;

                                        lbl.ForeColor = System.Drawing.Color.FromName(strDefaultFontColor);
                                        //lblComments.Text += tb.ID;
                                    }

                                    if (c.GetType() == typeof(TextBox))
                                    {
                                        tb = (TextBox)c;
                                        tb.Text = string.Empty;

                                        //lblComments.Text += tb.ID;
                                    }

                                    else if (c.GetType() == typeof(RadioButtonList))
                                    {
                                        rbl = (RadioButtonList)c;
                                        rbl.SelectedIndex = -1;

                                        //lblComments.Text += rbl.ID;
                                    }
                                    else if (c.GetType() == typeof(Panel))
                                    {
                                        pnlNested = (Panel)(c);

                                        if (pnlNested != null)
                                        {
                                                                                       
                                            
                                            foreach (Control cNested in pnlNested.Controls)
                                            {

                                                if (cNested.GetType() == typeof(Label))
                                                {
                                                    lbl = (Label)cNested;

                                                    lbl.ForeColor = System.Drawing.Color.FromName(strDefaultFontColor);
                                                    //lblComments.Text += tb.ID;
                                                }
                                                
                                                if (cNested.GetType() == typeof(TextBox))
                                                {
                                                    tb = (TextBox)cNested;
                                                    tb.Text = string.Empty;

                                                    //lblComments.Text += tb.ID;
                                                }

                                                else if (cNested.GetType() == typeof(RadioButtonList))
                                                {
                                                    rbl = (RadioButtonList)cNested;
                                                    rbl.SelectedIndex = -1;

                                                    //lblComments.Text += rbl.ID;
                                                }
                                                else
                                                {
                                                    //do nothing
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //do nothing
                                        //lblComments.Text += c.ID;
                                    }
                                    //lblComments.Text += c.ID + "<br/>";
                                }
                            }

                            lblDescription.Text = "Add  Recipient Follow Up Data for " + Request.QueryString["TID_R"].ToString() + " (" + ddOccasion.SelectedValue + ").";
                            cmdDelete.Visible = false;

                        }
                    }

                    rblGraftFailure_SelectedIndexChanged(this, EventArgs.Empty);
                    rblGraftRemoval_SelectedIndexChanged(this, EventArgs.Empty);
                    rblCurrentlyDialysis_SelectedIndexChanged(this, EventArgs.Empty);
                    ddRejection_SelectedIndexChanged(this, EventArgs.Empty);
                    rblGraftRemoval_SelectedIndexChanged(this, EventArgs.Empty);
                    HideShowQOL();
                    pnlOccasionSelected.Visible = true;

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
            lblUserMessages.Text = ex.Message + " An error occured while assigning data.";
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


            if (ddOccasion.SelectedValue!=STR_DD_UNKNOWN_SELECTION)
            {
                Page.Validate("Secondary");
            }

            if (rblGraftFailure.SelectedValue == STR_YES_SELECTION)
            {
                Page.Validate("GraftFailureYes");
            }

            if (rblGraftRemoval.SelectedValue == STR_YES_SELECTION)
            {
                Page.Validate("GraftRemovalYes");
            }

            if (Page.IsValid == false)
            {
                throw new Exception("Please check the data you have entered.");
            }

            
            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select an Occasion.");
            }

            

            if (GeneralRoutines.IsDate(txtFollowUpDate.Text) == false)
            {
                throw new Exception("Please enter Follow Up Date in the correct format");
            }

            //if (Convert.ToDateTime(txtFollowUpDate.Text) > DateTime.Today)
            //{
            //    throw new Exception("Date of Follow Up cannot be greater than Today's date.");
            //}  
            //if patient withdrwawn throw up an error message
            string STRSQL_WITHDRAWN = "SELECT DateWithdrawn FROM trialidwithdrawn WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strWithdrawn = GeneralRoutines.ReturnScalar(STRSQL_WITHDRAWN, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);
                
            if (GeneralRoutines.IsDate(strWithdrawn))
            {
                if (Convert.ToDateTime(strWithdrawn).Date < Convert.ToDateTime(txtFollowUpDate.Text).Date)
                {
                    throw new Exception("Follow Up Date " + Convert.ToDateTime(txtFollowUpDate.Text).Date.ToShortDateString() + " cannnot be after the date when the recipient was withdrawn i.e. " + Convert.ToDateTime(strWithdrawn).Date.ToShortDateString() + ".");
                }

            }

            //if patient deceased throw up an error message
            string STRSQL_DECEASED = "SELECT DeathDate FROM r_deceased WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strDeceased = GeneralRoutines.ReturnScalar(STRSQL_DECEASED, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

            if (GeneralRoutines.IsDate(strDeceased))
            {
                if (Convert.ToDateTime(strDeceased).Date < Convert.ToDateTime(txtFollowUpDate.Text).Date)
                {
                    throw new Exception("Follow Up Date " + Convert.ToDateTime(txtFollowUpDate.Text).Date.ToShortDateString() + " cannnot be after the date when the recipient was deceased i.e. " + Convert.ToDateTime(strDeceased).Date.ToShortDateString() + ".");
                }

            }


            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND FollowUpDate=?FollowUpDate AND Occasion <> ?Occasion";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarThree(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?FollowUpDate", Convert.ToDateTime(txtFollowUpDate.Text).ToString("yyyy-MM-dd"), "?Occasion", ddOccasion.SelectedValue, STRCONN));


            if (intCountFind > 1)
            {
                throw new Exception("There already exists more than onea Follow Up data for the date you have entered. Please Edit the existing data.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("Could not check if there already exists a Follow Up data for the date you have entered.");
            }


            intCountFind = 0;

            STRSQL_FIND = "SELECT COUNT(*) CR FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";

            intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?Occasion", ddOccasion.SelectedValue, STRCONN));

            if (intCountFind > 1)
            {
                throw new Exception("There already exists more than one Follow Up data for the Occasion you have Selected. Please Edit the existing data.");
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
                if (GeneralRoutines.IsDate(txtDateGraftFailure.Text) == false) throw new Exception("Since Graft failure is 'YES', Please Enter Date of Graft Failure");

                if (Convert.ToDateTime(txtDateGraftFailure.Text) > DateTime.Today)
                {
                    throw new Exception("Date of Graft Failure cannot be greater than Today's date.");
                }

                if (ddPrimaryCause.SelectedValue == STR_DD_UNKNOWN_SELECTION) throw new Exception("Please Select Primary Cause of Graft Failure");

                if (ddPrimaryCause.SelectedValue == STR_OTHER_SELECTION)
                {
                    if (txtPrimaryCauseOther.Text == string.Empty)
                    {
                        throw new Exception("Since you have selected Primary Cause of Graft Failure as 'Other', Please provide details of 'Primary Cause (If Other)'");
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

            if (txtDialysisSessions.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtDialysisSessions.Text) == false)
                {
                    throw new Exception("The value for 'Number of Dialysis Sessions' should be numeric.");
                }
            }

            if (txtRejectionTreatmentsPostTx.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtRejectionTreatmentsPostTx.Text) == false)
                {
                    throw new Exception("The value for 'Number of Rejection Treatments Post Transplant' should be numeric.");
                }
            }

            if (txtVASScore.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtVASScore.Text) == false)
                {
                    throw new Exception("Please Enter value for 'VAS Score' in Numeric Format.");
                }

                if (Convert.ToInt16(txtVASScore.Text) < 0)
                {
                    throw new Exception("'VAS Score' cannot be less than 0.");
                }

                if (Convert.ToInt16(txtVASScore.Text) > 100)
                {
                    if (Convert.ToInt16(txtVASScore.Text) != 999)
                    {
                        throw new Exception("'VAS Score' cannot be greater than 100 (Only exception is 999 for missing value).");
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

            //add data

            string STRSQL = string.Empty;

            STRSQL += "INSERT INTO r_fuposttreatment ";
            STRSQL += "(TrialIDRecipient, Occasion, FollowUpDate, GraftFailure, DateGraftFailure, PrimaryCause, PrimaryCauseOther,";
            STRSQL += "GraftRemoval, DateGraftRemoval, ";
            STRSQL += "SerumCreatinine, CreatinineUnit, UrineCreatinine, UrineUnit, CreatinineClearance, CreatinineClearanceUnit,  EGFR,";
            STRSQL += "CurrentlyDialysis, DialysisType, DateLastDialysis, DialysisSessions,";
            STRSQL += "PostTxImmunosuppressive, PostTxImmunosuppressiveOther, Rejection, RejectionTreatmentsPostTx, PostTxPrednisolon, PostTxOther, PostTxOtherDetails,";
            STRSQL += "RejectionBiopsyProven, CalcineurinInhibitorToxicity, ComplicationsGraftFunction,";
            STRSQL += "QOLFilledAt, Mobility, SelfCare, UsualActivities, PainDiscomfort, AnxietyDepression, VASScore,";
            STRSQL += "EventCode, ";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?Occasion, ?FollowUpDate, ?GraftFailure, ?DateGraftFailure, ?PrimaryCause, ?PrimaryCauseOther,";
            STRSQL += "?GraftRemoval, ?DateGraftRemoval,   ";
            STRSQL += "?SerumCreatinine, ?CreatinineUnit, ?UrineCreatinine, ?UrineUnit, ?CreatinineClearance, ?CreatinineClearanceUnit, ?EGFR,";
            STRSQL += "?CurrentlyDialysis, ?DialysisType, ?DateLastDialysis, ?DialysisSessions,  ";
            STRSQL += "?PostTxImmunosuppressive, ?PostTxImmunosuppressiveOther, ?Rejection, ?RejectionTreatmentsPostTx, ?PostTxPrednisolon, ?PostTxOther, ?PostTxOtherDetails, ";
            STRSQL += "?RejectionBiopsyProven,  ?CalcineurinInhibitorToxicity, ?ComplicationsGraftFunction, ";
            STRSQL += "?QOLFilledAt, ?Mobility, ?SelfCare, ?UsualActivities, ?PainDiscomfort, ?AnxietyDepression, ?VASScore,";
            STRSQL += "?EventCode, ";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = string.Empty;

            STRSQL_UPDATE += "UPDATE r_fuposttreatment SET ";
            STRSQL_UPDATE += "FollowUpDate=?FollowUpDate, GraftFailure=?GraftFailure, DateGraftFailure=?DateGraftFailure, PrimaryCause=?PrimaryCause, PrimaryCauseOther=?PrimaryCauseOther,";
            STRSQL_UPDATE += "GraftRemoval=?GraftRemoval, DateGraftRemoval=?DateGraftRemoval,";
            STRSQL_UPDATE += "SerumCreatinine=?SerumCreatinine, CreatinineUnit=?CreatinineUnit, UrineCreatinine=?UrineCreatinine, UrineUnit=?UrineUnit, ";
            STRSQL_UPDATE += "CreatinineClearance=?CreatinineClearance, CreatinineClearanceUnit=?CreatinineClearanceUnit,  EGFR=?EGFR,";
            STRSQL_UPDATE += "CurrentlyDialysis=?CurrentlyDialysis, DialysisType=?DialysisType, DateLastDialysis=?DateLastDialysis, ";
            STRSQL_UPDATE += "DialysisSessions=?DialysisSessions, ";
            STRSQL_UPDATE += "PostTxImmunosuppressive=?PostTxImmunosuppressive, PostTxImmunosuppressiveOther=?PostTxImmunosuppressiveOther, Rejection=?Rejection, ";
            STRSQL_UPDATE += "RejectionTreatmentsPostTx=?RejectionTreatmentsPostTx, PostTxPrednisolon=?PostTxPrednisolon, PostTxOther=?PostTxOther, PostTxOtherDetails=?PostTxOtherDetails,";
            STRSQL_UPDATE += "RejectionBiopsyProven=?RejectionBiopsyProven,  CalcineurinInhibitorToxicity=?CalcineurinInhibitorToxicity, ComplicationsGraftFunction=?ComplicationsGraftFunction,";
            STRSQL_UPDATE += "QOLFilledAt=?QOLFilledAt, ";
            STRSQL_UPDATE += "Mobility=?Mobility, SelfCare=?SelfCare, UsualActivities=?UsualActivities, PainDiscomfort=?PainDiscomfort, AnxietyDepression=?AnxietyDepression, VASScore=?VASScore,";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient AND  Occasion=?Occasion ";


            //lock data
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE r_fuposttreatment SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) AND Occasion=?Occasion ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE r_fuposttreatment SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";

            
            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            if (intCountFind == 0)
            {
                MyCMD.CommandText = STRSQL;
            }

            if (intCountFind == 1)
            {
                MyCMD.CommandText = STRSQL_UPDATE;
            }

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();

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

            if (rblGraftFailure.SelectedIndex==-1)
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

                        
            if (txtSerumCreatinine.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?SerumCreatinine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SerumCreatinine", MySqlDbType.VarChar).Value = txtSerumCreatinine.Text;
            }

            if (rblCreatinineUnits.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?CreatinineUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CreatinineUnit", MySqlDbType.VarChar).Value = rblCreatinineUnits.SelectedValue;
            }

            if (txtUrineCreatinine.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?UrineCreatinine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UrineCreatinine", MySqlDbType.VarChar).Value = txtUrineCreatinine.Text;
            }

            if (rblUrineCreatinineUnits.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?UrineUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UrineUnit", MySqlDbType.VarChar).Value = rblUrineCreatinineUnits.SelectedValue;
            }

            if (txtCreatinineClearance.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?CreatinineClearance", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CreatinineClearance", MySqlDbType.VarChar).Value = txtCreatinineClearance.Text;
            }

            if (rblCreatinineClearanceUnits.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?CreatinineClearanceUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CreatinineClearanceUnit", MySqlDbType.VarChar).Value = rblCreatinineClearanceUnits.SelectedValue;
            }


            string strEGFR = string.Empty;

            if (rblCreatinineUnits.SelectedIndex == -1 || txtSerumCreatinine.Text == string.Empty)
            {
                strEGFR = string.Empty;
            }
            else
            {
                strEGFR = GeneralRoutines.eGFR_Calculate(Request.QueryString["TID_R"], Convert.ToDouble(txtSerumCreatinine.Text), Convert.ToDateTime(txtFollowUpDate.Text), rblCreatinineUnits.SelectedValue);

            }

            //lblComments.Text = "strEGFR " + strEGFR;

            //calculate EGFR
            //if (txtSerumCreatinine.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
                
                
            //    MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = txtSerumCreatinine.Text;
            //}

            if (strEGFR == string.Empty || GeneralRoutines.IsNumeric(strEGFR)==false)
            {
                MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?EGFR", MySqlDbType.VarChar).Value = strEGFR;
            }


            if (rblCurrentlyDialysis.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?CurrentlyDialysis", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CurrentlyDialysis", MySqlDbType.VarChar).Value = rblCurrentlyDialysis.SelectedValue;
            }

            if (rblDialysisType.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?DialysisType", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DialysisType", MySqlDbType.VarChar).Value = rblDialysisType.SelectedValue;
            }

            
            if (GeneralRoutines.IsDate(txtDateLastDialysis.Text) == false)
            {
                MyCMD.Parameters.Add("?DateLastDialysis", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateLastDialysis", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateLastDialysis.Text);
            }

            if (txtDialysisSessions.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?DialysisSessions", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DialysisSessions", MySqlDbType.VarChar).Value = txtDialysisSessions.Text;
            }

            //if (ddInductionTherapy.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?InductionTherapy", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?InductionTherapy", MySqlDbType.VarChar).Value = ddInductionTherapy.SelectedValue;
            //}

            //append selection
            string strPostTxImmunosuppression = string.Empty;
            //Set up connection and command objects
            //Open connection

            for (int i = 0; i < cblPostTxImmunosuppressive.Items.Count; i++)
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

            if (ddRejection.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Rejection", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Rejection", MySqlDbType.VarChar).Value = ddRejection.SelectedValue;
            }

            if (txtRejectionTreatmentsPostTx.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RejectionTreatmentsPostTx", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RejectionTreatmentsPostTx", MySqlDbType.VarChar).Value = txtRejectionTreatmentsPostTx.Text;
            }

            if (rblPostTxPrednisolon.SelectedIndex==-1)
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


            if (txtComplicationsGraftFunction.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ComplicationsGraftFunction", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ComplicationsGraftFunction", MySqlDbType.VarChar).Value = txtComplicationsGraftFunction.Text;
            }

            if (ddQOLFilledAt.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?QOLFilledAt", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?QOLFilledAt", MySqlDbType.VarChar).Value = ddQOLFilledAt.SelectedValue;
            }

            if (ddMobility.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?Mobility", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Mobility", MySqlDbType.VarChar).Value = ddMobility.SelectedValue;
            }

            if (ddSelfCare.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?SelfCare", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SelfCare", MySqlDbType.VarChar).Value = ddSelfCare.SelectedValue;
            }

            if (ddUsualActivities.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?UsualActivities", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UsualActivities", MySqlDbType.VarChar).Value = ddUsualActivities.SelectedValue;
            }

            if (ddPainDiscomfort.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?PainDiscomfort", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PainDiscomfort", MySqlDbType.VarChar).Value = ddPainDiscomfort.SelectedValue;
            }

            if (ddAnxietyDepression.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?AnxietyDepression", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AnxietyDepression", MySqlDbType.VarChar).Value = ddAnxietyDepression.SelectedValue;
            }

            if (txtVASScore.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?VASScore", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?VASScore", MySqlDbType.VarChar).Value = txtVASScore.Text;
            }

            string strEventCode = "0";
            
            if (ddOccasion.SelectedValue=="3 Months")
            {
                strEventCode = "43";
            }

            if (ddOccasion.SelectedValue == "6 Months")
            {
                strEventCode = "44";
            }

            if (ddOccasion.SelectedValue == "1 Year")
            {
                strEventCode = "45";
            }

            MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = strEventCode;

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
                { 
                    MyCONN.Close(); 
                }

                lblUserMessages.Text = "Data Added.";
                BindData();

                string strSQLCOMPLETE = string.Empty;
                strSQLCOMPLETE += "SELECT ";
                strSQLCOMPLETE += "IF(t2.Occasion IS NOT NULL AND t2.FollowUpDate IS NOT NULL AND t2.GraftFailure IS NOT NULL AND t2.GraftRemoval IS NOT NULL   ";
                strSQLCOMPLETE += "AND t2.SerumCreatinine IS NOT NULL AND t2.CreatinineUnit IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.UrineCreatinine IS NOT NULL AND t2.UrineUnit IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.CreatinineClearance IS NOT NULL AND t2.CreatinineClearanceUnit IS NOT NULL ";
                strSQLCOMPLETE += "AND IF(t2.CurrentlyDialysis = 'YES', t2.DialysisType IS NOT NULL, t2.CurrentlyDialysis IS NOT NULL) ";
                strSQLCOMPLETE += "  ";
                strSQLCOMPLETE += "AND IF(t2.Rejection ='YES', t2.PostTxPrednisolon IS NOT NULL AND t2.PostTxOther IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.RejectionBiopsyProven IS NOT NULL,  t2.Rejection IS NOT NULL) ";
                //QOL Data
                if (ddOccasion.SelectedValue=="3 Months" || ddOccasion.SelectedValue=="1 Year")
                {
                    strSQLCOMPLETE += "AND t2.QOLFilledAt IS NOT NULL ";
                    strSQLCOMPLETE += "AND t2.Mobility IS NOT NULL AND t2.SelfCare IS NOT NULL AND t2.UsualActivities IS NOT NULL  ";
                    strSQLCOMPLETE += "AND t2.PainDiscomfort IS NOT NULL AND t2.AnxietyDepression IS NOT NULL AND t2.VASScore IS NOT NULL ";
                }
                
                //strSQLCOMPLETE += "AND t2.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM r_fuposttreatment t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialIDRecipient=?TrialIDRecipient AND Occasion='" + ddOccasion.SelectedValue + "' ";
                strSQLCOMPLETE += "";

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                //lblDonorRiskIndex.Text = strComplete;

                //strEGFR = eGFR_Calculate(Request.QueryString["TID_R"], Convert.ToDouble(txtSerumCreatinine.Text), Convert.ToDateTime(txtFollowUpDate.Text), rblCreatinineUnits.SelectedValue);
                //lblDateLastDialysis.Text = strEGFR;
                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
                }
                else
                {
                    string strUri = Request.Url.AbsoluteUri;

                    string[] splitString = strUri.Split('?');

                    string strBaseUri = splitString[0];

                    string strRedirectUri = strBaseUri + "?TID=" + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + "&Occasion=" + ddOccasion.SelectedValue + "&SCode=1";

                    //lblUserMessages.Text = strRedirectUri;

                    Response.Redirect(strRedirectUri, false); ;
                }

            }

            catch (System.Exception ex)
            {
                myTrans.Rollback();
                if (MyCONN.State == ConnectionState.Open)
                { 
                    MyCONN.Close();
                }
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
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?Occasion", ddOccasion.SelectedValue, STRCONN));

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
            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

            if (!String.IsNullOrEmpty(Request.QueryString["RFUPostTreatmentID"]))
            {
                MyCMD.Parameters.Add("?RFUPostTreatmentID", MySqlDbType.VarChar).Value = Request.QueryString["RFUPostTreatmentID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                BindData();

                lblUserMessages.Text = "Data Deleted.";

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



        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }
    

   
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
            if (rblGraftRemoval.SelectedValue == STR_YES_SELECTION)
            {
                pnlGraftRemoval.Visible = true;
            }
            else
            {
                txtDateGraftRemoval.Text = string.Empty;
                
                pnlGraftRemoval.Visible = false;
            }
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Graft Removal.";
        }
    }

    protected void rblCurrentlyDialysis_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if(rblCurrentlyDialysis.SelectedValue==STR_YES_SELECTION)
            {
                pnlCurrentlyDialysis.Visible = true;

            }
            else
            {
                pnlCurrentlyDialysis.Visible = false;
                rblDialysisType.SelectedIndex = -1;
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Currently on Dialysis.";
        }
    }

    protected void ddRejection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddRejection.SelectedValue == STR_DD_UNKNOWN_SELECTION || ddRejection.SelectedValue == "NO")
            {
                pnlRejection.Visible = false;
                txtRejectionTreatmentsPostTx.Text = string.Empty;
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

    protected void ddOccasion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {

            if (ddOccasion.SelectedValue != STR_DD_UNKNOWN_SELECTION)
            {
                pnlOccasionSelected.Visible = true;

                Response.Redirect(strRedirectCurrentPage + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + "&Occasion=" + ddOccasion.SelectedValue, false);
            }
            else
            {
                pnlOccasionSelected.Visible = false;

                throw new Exception("Please Select an Occasion.");
                //lblUserMessages.Text = "Please select an Occasion.";
            }
            AssignData();
            //string STRSQL_ID = "SELECT RFUPostTreatmentID FROM r_fuposttreatment WHERE Occasion=?Occasion AND TrialIDRecipient=?TrialIDRecipient ";

            //string strID = string.Empty;

            //strID = GeneralRoutines.ReturnScalarTwo(STRSQL_ID, "?Occasion", ddOccasion.SelectedValue, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

            //if (GeneralRoutines.IsNumeric(strID) == true)
            //{
            //    if (Convert.ToInt16(strID) > 0)
            //    {
            //        Response.Redirect(strEditRedirect + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + "&RFUPostTreatmentID=" + strID + "&EventCode=" + (ddOccasion.SelectedIndex + 40).ToString(), false);
            //    }
            //}
                       

            //if (ddOccasion.SelectedValue == "1 Month" || ddOccasion.SelectedValue == "6 Months")
            //{
            //    pnlQOL.Visible = true;
            //}
            //else
            //{
            //    pnlQOL.Visible = false;
            //    ddMobility.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            //    ddSelfCare.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            //    ddUsualActivities.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            //    ddPainDiscomfort.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            //    ddAnxietyDepression.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            //    txtVASScore.Text = string.Empty;


            //}
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Follow Up Occasion.";
        }
        
    }

    protected void HideShowQOL()
    {
        if ( ddOccasion.SelectedValue == "1 Year")
        {
            pnlQOL.Visible = true;
        }
        else
        {
            pnlQOL.Visible = false;
            ddMobility.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            ddSelfCare.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            ddUsualActivities.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            ddPainDiscomfort.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            ddAnxietyDepression.SelectedValue = STR_DD_UNKNOWN_SELECTION;
            txtVASScore.Text = string.Empty;


        }
    }


    protected string eGFR_Calculate(string strTrialIDRecipient, Double dblCreatinine, DateTime dteCreatinineDate, string strUnit)
    {

        string streGFR = "NA";
        string strError = string.Empty;

        try
        {

            //check if identification data added
            string STRSQLFIND = @"SELECT COUNT(*) CR FROM r_identification  
                                    WHERE TrialIDRecipient=?TrialIDRecipient";

            Int16 intCountRecipientData = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", strTrialIDRecipient, STRCONN));

            if (intCountRecipientData > 1)
            {
                throw new Exception("More than one Records exist in r_identification table.");
            }

            if (intCountRecipientData == 0)
            {
                throw new Exception("No Records exist in r_identification table.");
            }


            if (intCountRecipientData < 0)
            {
                throw new Exception("Error while checking if Records exist in r_identification table.");
            }

            //get DOB
            //            string STRSQL_DOB = @"SELECT DateOfBirth, FORMAT(DATEDIFF(TransplantationDate,DateOfBirth) / 365.25,2) DOB FROM r_identification t1
            //                                    INNER JOIN r_perioperative t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient 
            //                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

            string STRSQL_DOB = "SELECT FORMAT(DATEDIFF('" + dteCreatinineDate.ToString("yyyy-MM-dd") + "',DateOfBirth) / 365.25,2) DOB FROM r_identification t1 ";
            STRSQL_DOB += "  ";
            STRSQL_DOB += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";

            lblCurrentlyDialysis.Text = STRSQL_DOB;

            string strDOB = GeneralRoutines.ReturnScalar(STRSQL_DOB, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);

            if (GeneralRoutines.IsNumeric(strDOB) == false || strDOB == "-1")
            {
                throw new Exception("An error occured while calculating Date of Birth.");
            }


            //get ethnicity
            string STRSQL_BLACK = @"SELECT EthnicityBlack FROM r_identification  
                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strEthnicityBlack = GeneralRoutines.ReturnScalar(STRSQL_BLACK, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);


            //check if female
            string STRSQL_FEMALE = @"SELECT Sex FROM r_identification  
                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strFemale = GeneralRoutines.ReturnScalar(STRSQL_FEMALE, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);

            //now calculate
            Double dbleGFR;

            if (strUnit == "µmol/l")
            {
                dbleGFR = 32788;
            }
            else if (strUnit == "mg/dL")
            {
                dbleGFR = 186;
            }
            else
            {
                throw new Exception("Unknown Unit.");
            }

            dbleGFR = dbleGFR * Math.Pow(dblCreatinine, -1.154) * Math.Pow(Convert.ToDouble(strDOB), -0.203);

            if (strEthnicityBlack == STR_YES_SELECTION)
            {
                dbleGFR = dbleGFR * 1.212;
            }

            if (strFemale == "Female")
            {
                dbleGFR = dbleGFR * 0.742;
            }

            streGFR = Math.Round(dbleGFR, 2).ToString(); //round to two decimal places
        }
        catch (Exception ex)
        {
            strError = ex.Message + " An error occured while calulating eGFR.";
            lblCurrentlyDialysis.Text += ex.Message + " An error occured while calulating eGFR."; ;
            streGFR = "Error";
        }
        return streGFR;

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