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
public partial class SpecClinicalData_AddRPeriOperativeData : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

        private const string REDIRECTPAGE = "~/SpecClinicalData/EditRPeriOperativeData.aspx?";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";

        //static Random _random = new Random();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
                

            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_identification WHERE TrialIDRecipient=?TrialIDRecipient ";

                int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), STRCONN));

                if (intCountFind == 0)
                {
                    throw new Exception("Please add Recipient Identification Data First.");
                }


                string STRSQL = string.Empty;

                STRSQL += "SELECT COUNT(*) CR FROM r_perioperative WHERE TrialIDRecipient=?TrialIDRecipient ";

                int intFindSQL = 0;
                if (GeneralRoutines.IsNumeric(GeneralRoutines.ReturnScalar(STRSQL, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN)))
                {
                    intFindSQL = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQL, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));
                }
                else
                {
                    throw new Exception("Could not check if TrialID (Recipient) exists.");
                }

                if (intFindSQL > 1) throw new Exception("More than one TrialID (Recipient) exists for " + Request.QueryString["TID_R"]);
                    
                if (intFindSQL < 0) throw new Exception("An error occured while checking if " + Request.QueryString["TID_R"] + " (Recipient) exists in the database.");

                //if (intFindSQL == 1)
                //{
                //    STRSQL = string.Empty;
                //    STRSQL += "SELECT RPerioperativeID FROM r_perioperative WHERE TrialIDRecipient=?TrialIDRecipient ";

                //    string strRPerioperativeID = string.Empty;

                //    strRPerioperativeID = GeneralRoutines.ReturnScalar(STRSQL, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                //    if (!string.IsNullOrEmpty(strRPerioperativeID))
                //    {
                //        Response.Redirect(REDIRECTPAGE + "TID=" + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + "&RPerioperativeID=" + strRPerioperativeID);
                //    }

                //}


                //assign minimum/maximum dates to 

                    
                    
            }

                


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["TID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID.");
                }

                


                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";


                AssignDates();

                if (GeneralRoutines.IsDate(ViewState["DateCreated"].ToString()))
                {
                    rv_txtTransplantationDate.MinimumValue = Convert.ToDateTime(ViewState["DateCreated"]).ToShortDateString();
                }
                else
                {
                    rv_txtTransplantationDate.MinimumValue = DateTime.MinValue.ToShortDateString();
                }

                
                rv_txtTransplantationDate.MaximumValue = DateTime.Today.ToShortDateString();
                rv_txtTransplantationDate.ErrorMessage = "Date should be between " + rv_txtTransplantationDate.MinimumValue + " " + rv_txtTransplantationDate.MaximumValue + ".";


                txtTransplantationDate_CalendarExtender.StartDate = Convert.ToDateTime(rv_txtTransplantationDate.MinimumValue);
                txtTransplantationDate_CalendarExtender.EndDate = DateTime.Today.Date;

                if (ViewState["KidneyReceived"].ToString() == "Right")
                {
                    if (GeneralRoutines.IsDate(ViewState["KidneyRemovalRightDate"].ToString()))
                    {
                        rv_txtMachinePerfusionStopDate.MinimumValue = Convert.ToDateTime(ViewState["KidneyRemovalRightDate"]).ToShortDateString();
                    }
                    else
                    {
                        rv_txtMachinePerfusionStopDate.MinimumValue = DateTime.MinValue.ToShortDateString();
                    }
                    
                }
                else
                {
                    if (GeneralRoutines.IsDate(ViewState["KidneyRemovalLeftDate"].ToString()))
                    {
                        rv_txtMachinePerfusionStopDate.MinimumValue = Convert.ToDateTime(ViewState["KidneyRemovalLeftDate"]).ToShortDateString();
                    }
                    else
                    {
                        rv_txtMachinePerfusionStopDate.MinimumValue = DateTime.MinValue.ToShortDateString();
                    }

                    
                }
                //rv_txtMachinePerfusionStopDate.MinimumValue = DateTime.MinValue.ToShortDateString();
                rv_txtMachinePerfusionStopDate.MaximumValue = DateTime.Today.ToShortDateString();
                rv_txtMachinePerfusionStopDate.ErrorMessage = "Date should be between " + rv_txtMachinePerfusionStopDate.MinimumValue + " " + rv_txtMachinePerfusionStopDate.MaximumValue + ".";

                txtMachinePerfusionStopDate_CalendarExtender.EndDate = DateTime.Today;

                txtKidneyRemovedFromIceDate_CalendarExtender.EndDate = DateTime.Today;
                //lblTransplantationDate.Text = "Date should be between " + rv_txtTransplantationDate.MinimumValue + " " + rv_txtTransplantationDate.MaximumValue + ".";
                rblTapeBroken.DataSource = XMLMainOptionsYNDataSource;
                rblTapeBroken.DataBind();

                rblOxygenBottleFullAndTurnedOpen.DataSource = XMLMainOptionsYNDataSource;
                rblOxygenBottleFullAndTurnedOpen.DataBind();


                if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
                {

                    
                    pnlOxygenBottleFullOpen.Visible = true;
                }
                else
                {
                    

                    pnlOxygenBottleFullOpen.Visible = false;
                }

                rblKidneyDiscarded.DataSource = XMLMainOptionsYNDataSource;
                rblKidneyDiscarded.DataBind();
                //rblKidneyDiscarded.SelectedValue = STR_UNKNOWN_SELECTION;


                //assign minimum/maximum dates to Transplantation Date

                txtOperationStartDate_CalendarExtender.EndDate = DateTime.Today;

                rblMannitolUsed.DataSource = XMLMainOptionsDataSource;
                rblMannitolUsed.DataBind();
                //rblMannitolUsed.SelectedValue = STR_UNKNOWN_SELECTION;

                rblDiureticsUsed.DataSource = XMLMainOptionsDataSource;
                rblDiureticsUsed.DataBind();
                //rblDiureticsUsed.SelectedValue = STR_UNKNOWN_SELECTION;
                                        
                rblHypotensivePeriod.DataSource = XMLMainOptionsDataSource;
                rblHypotensivePeriod.DataBind();
                //rblHypotensivePeriod.SelectedValue = STR_UNKNOWN_SELECTION;

                rblIncision.DataSource = XMLIncisionsDataSource;
                rblIncision.DataBind();
                //rblIncision.SelectedValue = STR_UNKNOWN_SELECTION;
                    
                rblTransplantSide.DataSource = XMLKidneySidesDataSource;
                rblTransplantSide.DataBind();

                ListItem li = rblTransplantSide.Items.FindByValue(STR_UNKNOWN_SELECTION);

                if (li!=null)
                {
                    rblTransplantSide.Items.Remove(li);
                }
                
                //rblTransplantSide.SelectedValue = STR_UNKNOWN_SELECTION;

                ddArterialProblems.DataSource = XMLArterialProblemsRecipientDataSource;
                ddArterialProblems.DataBind();

                rblVenousProblems.DataSource = XMLMainOptionsDataSource;
                rblVenousProblems.DataBind();
                //rblVenousProblems.SelectedValue = STR_UNKNOWN_SELECTION;

                txtStartAnastomosisDate_CalendarExtender.EndDate = DateTime.Today;
                txtReperfusionDate_CalendarExtender.EndDate = DateTime.Today;
                                        
                rblIntraOperativeDiuresis.DataSource = XMLMainOptionsDataSource;
                rblIntraOperativeDiuresis.DataBind();
                //rblIntraOperativeDiuresis.SelectedValue = STR_UNKNOWN_SELECTION;

                    
                pnlKidneyDiscardedYes.Visible = false;
                pnlKidneyDiscardedNo.Visible = false;    

                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;


                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

                lblDescription.Text = "Please Enter RecipientID Peri-Operative Data for " + Request.QueryString["TID_R"];

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

            strSQL += "SELECT t1.*,t2.RecipientID, t2.TrialID,   ";
            strSQL += "DATE_FORMAT(t1.TransplantationDate, '%d/%m/%Y') Transplantation_Date, ";
            strSQL += "CONCAT(IF(t1.MachinePerfusionStopDate IS NULL, 'NA', DATE_FORMAT(t1.MachinePerfusionStopDate, '%d/%m/%Y')), ";
            strSQL += " ' ', IF(t1.MachinePerfusionStopTime IS NULL, 'NA', DATE_FORMAT(t1.MachinePerfusionStopTime, '%H:%i'))) MachinePerfusion, ";
            strSQL += "CONCAT(IF(t1.OperationStartDate IS NULL, 'NA', DATE_FORMAT(t1.OperationStartDate, '%d/%m/%Y')), ";
            strSQL += " ' ', IF(t1.OperationStartTime IS NULL, 'NA', DATE_FORMAT(t1.OperationStartTime, '%H:%i'))) Operation_Start, ";
            strSQL += "CONCAT(IF(t1.StartAnastomosisDate IS NULL, 'NA', DATE_FORMAT(t1.StartAnastomosisDate, '%d/%m/%Y')), ";
            strSQL += " ' ', IF(t1.StartAnastomosisTime IS NULL, 'NA', DATE_FORMAT(t1.StartAnastomosisTime, '%H:%i'))) StartAnastomosis, ";
            strSQL += "CONCAT(IF(t1.ReperfusionDate IS NULL, 'NA', DATE_FORMAT(t1.ReperfusionDate, '%d/%m/%Y')), ";
            strSQL += " ' ', IF(t1.ReperfusionTime IS NULL, 'NA', DATE_FORMAT(t1.ReperfusionTime, '%H:%i'))) Reperfusion ";
            //strSQL += "DATE_FORMAT(t1.OperationFinishTime, '%H:%i') OperationFinish_Time ";
            strSQL += "FROM r_perioperative t1 ";
            strSQL += "LEFT JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();
            //lblGV1.Text = "Click on RecipientID to Edit Recipient Peri-Operative Data.";

            //lblDescription.Text = "Add  Recipient Peri-Operative Data for " + Request.QueryString["TID_R"].ToString() + " and DonorID " + strDonorID;
            //if (strRecipientID != string.Empty)
            //{ 
            //    lblDescription.Text += " and RecipientID " + strRecipientID; 
            //}

            if (GV1.Rows.Count == 1)
            {

                AssignData();

                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                lblGV1.Text = "Summary of Recipient Peri-Operative Data";
            }
            else
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
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
        //string strDateOperation = string.Empty;
        //string strCrossClampingTime = string.Empty;

        //string strLiverPlacementIceDate = string.Empty;
        //string strLiverPlacementIceTime = string.Empty;

        //string strLiverOnMachine = string.Empty;

        //string strInitiationNMPDate = string.Empty;
        //string strInitiationNMPTime = string.Empty;

        //string strCessationNMPDate = string.Empty;
        //string strCessationNMPTime = string.Empty;

        string strDateCreated = string.Empty;
        string strKidneyRemovalLeftDate = string.Empty;
        string strKidneyRemovalLeftTime = string.Empty;

        string strKidneyRemovalRightDate = string.Empty;
        string strKidneyRemovalRightTime = string.Empty;

        string strKidneyReceived = string.Empty;

        try
        {
            string STRSQL = string.Empty;

            STRSQL = "SELECT t1.*, t2.RemovalDate, t2.RemovalTime,  t2.Removal_RDate, t2.Removal_RTime, t3.KidneyReceived ";
            STRSQL += "FROM trialdetails t1  ";
            STRSQL += "LEFT JOIN kidneyinspection t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "LEFT JOIN trialdetails_recipient t3 ON t1.TrialID=t3.TrialID  AND t3.TrialIDRecipient=?TrialIDRecipient ";
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

                            if (!DBNull.Value.Equals(myDr["RemovalDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["RemovalDate"].ToString()) == true)
                                {
                                    strKidneyRemovalLeftDate = myDr["RemovalDate"].ToString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["RemovalTime"]))
                            {
                                if (myDr["RemovalTime"].ToString().Length >= 5)
                                {
                                    strKidneyRemovalLeftTime = myDr["RemovalTime"].ToString().Substring(0, 5);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["Removal_RDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["Removal_RDate"].ToString()) == true)
                                {
                                    strKidneyRemovalRightDate = myDr["Removal_RDate"].ToString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Removal_RTime"]))
                            {
                                if (myDr["Removal_RTime"].ToString().Length >= 5)
                                {
                                    strKidneyRemovalRightTime = myDr["Removal_RTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["KidneyReceived"]))
                            {
                                strKidneyReceived = myDr["KidneyReceived"].ToString();
                                
                            }
                           

                            //if (!DBNull.Value.Equals(myDr["InitiationNMPDate"]))
                            //{
                            //    if (GeneralRoutines.IsDate(myDr["InitiationNMPDate"].ToString()) == true)
                            //    {
                            //        strInitiationNMPDate = Convert.ToDateTime(myDr["InitiationNMPDate"]).ToShortDateString();
                            //    }
                            //}
                            //if (!DBNull.Value.Equals(myDr["InitiationNMPTime"]))
                            //{
                            //    if (myDr["InitiationNMPTime"].ToString().Length >= 5)
                            //    {
                            //        strInitiationNMPTime = myDr["InitiationNMPTime"].ToString().Substring(0, 5);
                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["CessationNMPDate"]))
                            //{
                            //    if (GeneralRoutines.IsDate(myDr["InitiationNMPDate"].ToString()) == true)
                            //    {
                            //        strCessationNMPDate = Convert.ToDateTime(myDr["CessationNMPDate"]).ToShortDateString();
                            //    }
                            //}
                            //if (!DBNull.Value.Equals(myDr["CessationNMPTime"]))
                            //{
                            //    if (myDr["InitiationNMPTime"].ToString().Length >= 5)
                            //    {
                            //        strCessationNMPTime = myDr["CessationNMPTime"].ToString().Substring(0, 5);
                            //    }
                            //}

                        }
                    }
                }


                ViewState["DateCreated"] = strDateCreated;
                ViewState["KidneyRemovalLeftDate"] = strKidneyRemovalLeftDate;
                ViewState["KidneyRemovalLeftTime"] = strKidneyRemovalLeftTime;
                ViewState["KidneyRemovalRightDate"] = strKidneyRemovalRightDate;
                ViewState["KidneyRemovalRightTime"] = strKidneyRemovalRightTime;
                ViewState["KidneyReceived"] = strKidneyReceived;



                //lblTransplantationDate.Text += "DateCreated " + ViewState["DateCreated"].ToString();
                //lblTransplantationDate.Text += "KidneyRemovalLeftDate " + ViewState["KidneyRemovalLeftDate"].ToString();
                //lblTransplantationDate.Text += "KidneyRemovalLeftTime " + ViewState["KidneyRemovalLeftTime"].ToString();
                //lblTransplantationDate.Text += "KidneyRemovalRightDate " + ViewState["KidneyRemovalRightDate"].ToString();
                //lblTransplantationDate.Text += "KidneyRemovalRightTime " + ViewState["KidneyRemovalRightTime"].ToString();
                //lblTransplantationDate.Text += ", KidneyReceived " + ViewState["KidneyReceived"].ToString();
                //lblTransplantationDate.Text += "CessationNMPDate " + ViewState["CessationNMPDate"].ToString();
                //lblTransplantationDate.Text += "CessationNMPTime " + ViewState["CessationNMPTime"].ToString();

                //lblTransplantationDate.Text += "</br>";

                

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
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.*, t3.TrialID, t2.DateOfBirth FROM  r_perioperative t1 ";
            STRSQL += "INNER JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            STRSQL += "INNER JOIN trialdetails_Recipient t3 ON t1.TrialIDRecipient=t3.TrialIDRecipient ";
            STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t3.TrialID=?TrialID ";

            if (!string.IsNullOrEmpty(Request.QueryString["RPerioperativeID"]))
            {
                STRSQL += "AND RPerioperativeID=?RPerioperativeID ";
            }
            //STRSQL += "LEFT JOIN r_identification
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
            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];
            if (!string.IsNullOrEmpty(Request.QueryString["RPerioperativeID"]))
            {
                MyCMD.Parameters.Add("?RPerioperativeID", MySqlDbType.Int32).Value = Request.QueryString["RPerioperativeID"];
            }

            

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


                            if (!DBNull.Value.Equals(myDr["TransplantationDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["TransplantationDate"].ToString()))
                                {
                                    txtTransplantationDate.Text = Convert.ToDateTime(myDr["TransplantationDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (lblTransplantationDate.Font.Bold == true)
                            {
                                if (txtTransplantationDate.Text == string.Empty)
                                {
                                    lblTransplantationDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["MachinePerfusionStopDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["MachinePerfusionStopDate"].ToString()))
                                {
                                    txtMachinePerfusionStopDate.Text = Convert.ToDateTime(myDr["MachinePerfusionStopDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            
                            if (!DBNull.Value.Equals(myDr["MachinePerfusionStopTime"]))
                            {
                                txtMachinePerfusionStopTime.Text = myDr["MachinePerfusionStopTime"].ToString().Substring(0, 5);
                            }

                            if (lblMachinePerfusionStopDate.Font.Bold == true)
                            {
                                if (txtMachinePerfusionStopDate.Text == string.Empty || txtMachinePerfusionStopTime.Text == string.Empty)
                                {
                                    lblMachinePerfusionStopDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["TapeBroken"]))
                            {
                                rblTapeBroken.SelectedValue = (string)(myDr["TapeBroken"]);
                            }

                            if (lblTapeBroken.Font.Bold == true)
                            {
                                if (rblTapeBroken.SelectedIndex == -1)
                                {
                                    lblTapeBroken.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            

                            if (!DBNull.Value.Equals(myDr["KidneyRemovedFromIceDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["KidneyRemovedFromIceDate"].ToString()))
                                {
                                    txtKidneyRemovedFromIceDate.Text = Convert.ToDateTime(myDr["KidneyRemovedFromIceDate"]).ToString("dd/MM/yyyy");
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["KidneyRemovedFromIceTime"]))
                            {
                                txtKidneyRemovedFromIceTime.Text = myDr["KidneyRemovedFromIceTime"].ToString().Substring(0, 5);
                            }

                            if (lblKidneyRemovedFromIceDate.Font.Bold == true)
                            {
                                if (txtKidneyRemovedFromIceDate.Text == string.Empty || txtKidneyRemovedFromIceTime.Text == string.Empty)
                                {
                                    lblKidneyRemovedFromIceDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            
                            if (!DBNull.Value.Equals(myDr["KidneyDiscarded"]))
                            {
                                rblKidneyDiscarded.SelectedValue = (string)(myDr["KidneyDiscarded"]);
                            }

                            if (lblKidneyDiscarded.Font.Bold == true)
                            {
                                if (rblKidneyDiscarded.SelectedIndex == -1)
                                {
                                    lblKidneyDiscarded.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
                            {
                                if (!DBNull.Value.Equals(myDr["OxygenBottleFullAndTurnedOpen"]))
                                {
                                    rblOxygenBottleFullAndTurnedOpen.SelectedValue = (string)(myDr["OxygenBottleFullAndTurnedOpen"]);
                                }
                            }
                            

                            if (lblOxygenBottleFullAndTurnedOpen.Font.Bold == true)
                            {
                                if (rblOxygenBottleFullAndTurnedOpen.SelectedIndex == -1)
                                {
                                    lblOxygenBottleFullAndTurnedOpen.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            //if (rblKidneyDiscarded.SelectedValue == STR_YES_SELECTION)
                            //{
                            //    pnlKidneyDiscardedYes.Visible = true;
                            //}

                            if (!DBNull.Value.Equals(myDr["KidneyDiscardedYes"]))
                            {
                                txtKidneyDiscardedYes.Text = (string)(myDr["KidneyDiscardedYes"]);
                            }


                            if (lblKidneyDiscardedYes.Font.Bold == true)
                            {
                                if (txtKidneyDiscardedYes.Text == string.Empty)
                                {
                                    lblKidneyDiscardedYes.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TimeOnMachine"]))
                            {
                                txtTimeOnMachine.Text = myDr["TimeOnMachine"].ToString();
                            }

                            if (lblTimeOnMachine.Font.Bold == true)
                            {
                                if (txtTimeOnMachine.Text == string.Empty)
                                {
                                    lblTimeOnMachine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["TimeOnMachine"]))
                            //{
                            //    txtTimeOnMachine.Text = myDr["TimeOnMachine"].ToString().Substring(0, 5);
                            //}

                            if (!DBNull.Value.Equals(myDr["OperationStartDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["OperationStartDate"].ToString()))
                                {
                                    txtOperationStartDate.Text = Convert.ToDateTime(myDr["OperationStartDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["OperationStartTime"]))
                            {
                                txtOperationStartTime.Text = myDr["OperationStartTime"].ToString().Substring(0, 5);
                            }

                            if (lblOperationStart.Font.Bold == true)
                            {
                                if (txtOperationStartDate.Text == string.Empty || txtOperationStartTime.Text==string.Empty)
                                {
                                    lblOperationStart.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["MannitolUsed"]))
                            {
                                rblMannitolUsed.SelectedValue = (string)(myDr["MannitolUsed"]);
                            }

                            if (!DBNull.Value.Equals(myDr["DiureticsUsed"]))
                            {
                                rblDiureticsUsed.SelectedValue = (string)(myDr["DiureticsUsed"]);
                            }

                            if (!DBNull.Value.Equals(myDr["HypotensivePeriod"]))
                            {
                                rblHypotensivePeriod.SelectedValue = (string)(myDr["HypotensivePeriod"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CVPReperfusion"]))
                            {
                                txtCVPReperfusion.Text = (string)(myDr["CVPReperfusion"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Incision"]))
                            {
                                rblIncision.SelectedValue = (string)(myDr["Incision"]);
                            }

                            if (!DBNull.Value.Equals(myDr["TransplantSide"]))
                            {
                                rblTransplantSide.SelectedValue = (string)(myDr["TransplantSide"]);
                            }

                            if (lblTransplantSide.Font.Bold == true)
                            {
                                if (rblTransplantSide.SelectedIndex == -1)
                                {
                                    lblTransplantSide.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["ArterialProblems"]))
                            {
                                ddArterialProblems.SelectedValue = (string)(myDr["ArterialProblems"]);
                            }

                            if (lblArterialProblems.Font.Bold == true)
                            {
                                if (ddArterialProblems.SelectedIndex == -1 || ddArterialProblems.SelectedValue==STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblArterialProblems.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArterialProblemsOther"]))
                            {
                                txtArterialProblemsOther.Text = (string)(myDr["ArterialProblemsOther"]);
                            }

                            if (ddArterialProblems.SelectedValue== STR_OTHER_SELECTION )
                            {
                                if (txtArterialProblemsOther.Text==string.Empty)
                                {
                                    lblArterialProblemsOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["VenousProblems"]))
                            {
                                rblVenousProblems.SelectedValue = (string)(myDr["VenousProblems"]);
                            }

                            if (!DBNull.Value.Equals(myDr["StartAnastomosisDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["StartAnastomosisDate"].ToString()))
                                {
                                    txtStartAnastomosisDate.Text = Convert.ToDateTime(myDr["StartAnastomosisDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["StartAnastomosisTime"]))
                            {
                                txtStartAnastomosisTime.Text = myDr["StartAnastomosisTime"].ToString().Substring(0, 5);
                            }

                            if (!DBNull.Value.Equals(myDr["ReperfusionDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ReperfusionDate"].ToString()))
                                {
                                    txtReperfusionDate.Text = Convert.ToDateTime(myDr["ReperfusionDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ReperfusionTime"]))
                            {
                                txtReperfusionTime.Text = myDr["ReperfusionTime"].ToString().Substring(0, 5);
                            }

                            if (!DBNull.Value.Equals(myDr["ColdIschemiaPeriod"]))
                            {
                                txtColdIschemiaPeriod.Text = (string)(myDr["ColdIschemiaPeriod"]);
                            }

                            //if (!DBNull.Value.Equals(myDr["ColdIschemiaPeriodMinutes"]))
                            //{
                            //    txtColdIschemiaPeriodMinutes.Text = (string)(myDr["ColdIschemiaPeriodMinutes"]);
                            //}

                            if (!DBNull.Value.Equals(myDr["TotalAnastomosisTime"]))
                            {
                                txtTotalAnastomosisTime.Text = myDr["TotalAnastomosisTime"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["IntraOperativeDiuresis"]))
                            {
                                rblIntraOperativeDiuresis.SelectedValue = (string)(myDr["IntraOperativeDiuresis"]);
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

                rblKidneyDiscarded_SelectedIndexChanged(this, EventArgs.Empty);


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

    //add data
    protected void cmdAddData_Click(object sender, EventArgs e)
    {

        try
        {
            lblUserMessages.Text = string.Empty;


            if (rblKidneyDiscarded.SelectedValue==STR_YES_SELECTION)
            {
                Page.Validate("KidneyDiscardedYes");
            }
            else
            {
                Page.Validate("KidneyDiscardedNo");
            }

            if (ddArterialProblems.SelectedValue==STR_OTHER_SELECTION)
            {
                Page.Validate("ArterialProblemsOther");
            }
            else
            {
                if (txtArterialProblemsOther.Text!=string.Empty)
                {
                    throw new Exception("As '" + lblArterialProblems.Text + "' is not " + STR_OTHER_SELECTION + "  '" + lblArterialProblemsOther.Text + "' should be empty..");
                }
            }
            if (Page.IsValid==false)
            {
                throw new Exception("Please Check the Data you have entered.");
            }

            //DateTime? dteMachinePerfusionStop = null;
            //DateTime? dteOperationStart = null;
            //DateTime? dteStartAnastomosis = null;
            //DateTime? dteReperfusion = null;

            DateTime dteMachinePerfusionStop = DateTime.MinValue;
            DateTime dteOperationStart = DateTime.MinValue;
            DateTime dteStartAnastomosis = DateTime.MinValue;
            DateTime dteReperfusion = DateTime.MinValue;


            
            if (txtTransplantationDate.Text!=string.Empty)
            {
                if (GeneralRoutines.IsDate(txtTransplantationDate.Text) == false)
                {
                    throw new Exception("Please enter the Transplantion Date.");
                }


                if (Convert.ToDateTime(txtTransplantationDate.Text) > DateTime.Today)
                {
                    throw new Exception("Transplantion Date cannot be greater than Today's date.");
                }

            }


            if (txtMachinePerfusionStopDate.Text!=string.Empty)
            {
                if (GeneralRoutines.IsDate(txtMachinePerfusionStopDate.Text) == false)
                {
                    throw new Exception("Please enter the Machine Perfusion Stop Date.");
                }

                if (Convert.ToDateTime(txtMachinePerfusionStopDate.Text) > DateTime.Today)
                {
                    throw new Exception("Machine Perfusion Stop Date cannot be greater than Today's date.");
                }
            }




            if (txtMachinePerfusionStopTime.Text == string.Empty || txtMachinePerfusionStopTime.Text=="__:__")
            {
                //throw new Exception("Please enter the Machine Perfusion Stop Time. ");
            }
            else
            {
                if (GeneralRoutines.IsNumeric(txtMachinePerfusionStopTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("Time Hour forMachine Perfusion Stop Time should be numeric.");
                }

                if (Convert.ToInt16(txtMachinePerfusionStopTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("Time Hour for Machine Perfusion Stop Time should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtMachinePerfusionStopTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("Time Minute for Machine Perfusion Stop Time should be numeric.");
                }

                if (Convert.ToInt16(txtMachinePerfusionStopTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("Time Minute for Machine Perfusion Stop Time should not be greater than 59.");
                }
            }

            //dteMachinePerfusionStop = new DateTime();

            if (GeneralRoutines.IsDate(txtMachinePerfusionStopDate.Text) && GeneralRoutines.IsDate(txtMachinePerfusionStopTime.Text))
            {
                dteMachinePerfusionStop = Convert.ToDateTime(txtMachinePerfusionStopDate.Text + " " + txtMachinePerfusionStopTime.Text);
            }


            if (txtKidneyRemovedFromIceDate.Text!=string.Empty)
            {
                if (GeneralRoutines.IsDate(txtKidneyRemovedFromIceDate.Text) == false)
                {
                    throw new Exception("Please enter the Kidney Removed From Ice Date.");
                }

                if (Convert.ToDateTime(txtKidneyRemovedFromIceDate.Text) > DateTime.Today)
                {
                    throw new Exception("Kidney Removed From Ice Date cannot be greater than Today's date.");
                }
            }



            if (txtKidneyRemovedFromIceTime.Text == string.Empty || txtKidneyRemovedFromIceTime.Text == "__:__")
            {
                //throw new Exception("Please enter the Kidney Removed From Ice Time. ");
            }
            else
            {
                if (GeneralRoutines.IsNumeric(txtKidneyRemovedFromIceTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("Time Hour forKidney Removed From Ice Time should be numeric.");
                }

                if (Convert.ToInt16(txtKidneyRemovedFromIceTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("Time Hour for Kidney Removed From Ice Time should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtKidneyRemovedFromIceTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("Time Minute for Kidney Removed From Ice Time should be numeric.");
                }

                if (Convert.ToInt16(txtKidneyRemovedFromIceTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("Time Minute for Kidney Removed From Ice Time should not be greater than 59.");
                }
            }

            


            //if (!string.IsNullOrEmpty(txtColdIschemiaPeriodHours.Text))
            //{
            //    if (GeneralRoutines.IsNumeric(txtColdIschemiaPeriodHours.Text) == false)
            //    {
            //        throw new Exception("Cold Ischemia Period Housr (First Box) should be numeric");
            //    }
            //}

            //if (!string.IsNullOrEmpty(txtColdIschemiaPeriodMinutes.Text))
            //{
            //    if (GeneralRoutines.IsNumeric(txtColdIschemiaPeriodMinutes.Text) == false)
            //    {
            //        throw new Exception("Cold Ischemia Period Minutes (Second Box) should be numeric");
            //    }
            //}

            if (rblKidneyDiscarded.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please select if Kidney Discarded.");
            }

            if (rblKidneyDiscarded.SelectedValue == STR_YES_SELECTION)
            {
                if (!string.IsNullOrEmpty(txtKidneyDiscardedYes.Text))
                {
                    txtKidneyDiscardedYes.Text = string.Empty;
                }
            }

            else
            {

                if (txtTimeOnMachine.Text != string.Empty)
                {
                    if (GeneralRoutines.IsNumeric(txtTimeOnMachine.Text) == false)
                    {
                        throw new Exception("Time On Machine should be numeric.");
                    }

                    //if (Convert.ToInt16(txtTimeOnMachine.Text.Substring(0, 2)) > 23)
                    //{
                    //    throw new Exception("Time Hour for Time On Machine Time should not be greater than 23.");
                    //}

                    //if (GeneralRoutines.IsNumeric(txtTimeOnMachine.Text.Substring(3, 2)) == false)
                    //{
                    //    throw new Exception("Time Minute for Time On MachineTime should be numeric.");
                    //}

                    //if (Convert.ToInt16(txtTimeOnMachine.Text.Substring(3, 2)) > 59)
                    //{
                    //    throw new Exception("Time Minute for Time On MachineTime should not be greater than 59.");
                    //}
                }

                //if (txtTimeOnMachine.Text == string.Empty)
                //{
                //    throw new Exception("Please enter the Time On Machine. ");
                //}
                //else
                //{
                //    if (GeneralRoutines.IsNumeric(txtTimeOnMachine.Text) == false)
                //    {
                //        throw new Exception("Time On Machine should be numeric.");
                //    }

                //    //if (Convert.ToInt16(txtTimeOnMachine.Text.Substring(0, 2)) > 23)
                //    //{
                //    //    throw new Exception("Time Hour for Time On Machine Time should not be greater than 23.");
                //    //}

                //    //if (GeneralRoutines.IsNumeric(txtTimeOnMachine.Text.Substring(3, 2)) == false)
                //    //{
                //    //    throw new Exception("Time Minute for Time On MachineTime should be numeric.");
                //    //}

                //    //if (Convert.ToInt16(txtTimeOnMachine.Text.Substring(3, 2)) > 59)
                //    //{
                //    //    throw new Exception("Time Minute for Time On MachineTime should not be greater than 59.");
                //    //}
                //}

                if (txtOperationStartDate.Text!=string.Empty)
                {
                    if (GeneralRoutines.IsDate(txtOperationStartDate.Text) == false)
                    {
                        throw new Exception("Please enter the Operation Start Date.");
                    }

                    if (Convert.ToDateTime(txtOperationStartDate.Text) > DateTime.Today)
                    {
                        throw new Exception("Operation Start Date cannot be greater than Today's date.");
                    }
                }



                if (txtOperationStartTime.Text == string.Empty || txtOperationStartTime.Text == "__:__")
                {
                    //throw new Exception("Please enter the Operation Start Time. ");
                }
                else
                {
                    if (GeneralRoutines.IsNumeric(txtOperationStartTime.Text.Substring(0, 2)) == false)
                    {
                        throw new Exception("Time Hour for Operation Start Time should be numeric.");
                    }

                    if (Convert.ToInt16(txtOperationStartTime.Text.Substring(0, 2)) > 23)
                    {
                        throw new Exception("Time Hour for Operation Start Time should not be greater than 23.");
                    }

                    if (GeneralRoutines.IsNumeric(txtOperationStartTime.Text.Substring(3, 2)) == false)
                    {
                        throw new Exception("Time Minute for Operation Start Time should be numeric.");
                    }

                    if (Convert.ToInt16(txtOperationStartTime.Text.Substring(3, 2)) > 59)
                    {
                        throw new Exception("Time Minute for Operation Start Time should not be greater than 59.");
                    }
                }

                //dteOperationStart = new DateTime();

                if (GeneralRoutines.IsDate(txtOperationStartDate.Text) && GeneralRoutines.IsDate(txtOperationStartTime.Text))
                {
                    dteOperationStart = Convert.ToDateTime(txtOperationStartDate.Text + " " + txtOperationStartTime.Text);
                }
                

                //if (dteOperationStart < dteMachinePerfusionStop)
                //{
                //    throw new Exception("Operation Start Date/Time cannot be earlier than Machine Perfusion Stop Date/Time.");
                //}


                if (txtStartAnastomosisDate.Text!=string.Empty)
                {
                    if (GeneralRoutines.IsDate(txtStartAnastomosisDate.Text) == false)
                    {
                        throw new Exception("Please enter the Anastomosis Start Date.");
                    }

                    if (Convert.ToDateTime(txtStartAnastomosisDate.Text) > DateTime.Today)
                    {
                        throw new Exception("Anastomosis Start Date cannot be greater than Today's date.");
                    }
                }
                


                if (txtStartAnastomosisTime.Text != string.Empty && txtStartAnastomosisTime.Text != "__:__")                
                {
                    if (GeneralRoutines.IsNumeric(txtStartAnastomosisTime.Text.Substring(0, 2)) == false)
                    {
                        throw new Exception("Time Hour for Anastomosis Start Time should be numeric.");
                    }

                    if (Convert.ToInt16(txtStartAnastomosisTime.Text.Substring(0, 2)) > 23)
                    {
                        throw new Exception("Time Hour for Anastomosis Start Time should not be greater than 23.");
                    }

                    if (GeneralRoutines.IsNumeric(txtStartAnastomosisTime.Text.Substring(3, 2)) == false)
                    {
                        throw new Exception("Time Minute for Anastomosis Start Time should be numeric.");
                    }

                    if (Convert.ToInt16(txtStartAnastomosisTime.Text.Substring(3, 2)) > 59)
                    {
                        throw new Exception("Time Minute for Anastomosis Start Time should not be greater than 59.");
                    }
                }


                if (GeneralRoutines.IsDate(txtStartAnastomosisDate.Text) && GeneralRoutines.IsDate(txtStartAnastomosisTime.Text))
                {
                    dteStartAnastomosis = Convert.ToDateTime(txtStartAnastomosisDate.Text + " " + txtStartAnastomosisTime.Text);
                }
                //dteStartAnastomosis = new DateTime();


                if (dteStartAnastomosis!=DateTime.MinValue)
                {

                    if (dteOperationStart!=DateTime.MinValue)
                    {
                        if (dteStartAnastomosis < dteOperationStart)
                        {
                            throw new Exception("Anastomosis Start Date/Time cannot be earlier than Operation Start Date/Time");
                        }
                    }
                    
                }


                if (txtReperfusionDate.Text!=String.Empty)
                {
                    if (GeneralRoutines.IsDate(txtReperfusionDate.Text) == false)
                    {
                        throw new Exception("Please enter the Reperfusion Date.");
                    }

                    if (Convert.ToDateTime(txtReperfusionDate.Text) > DateTime.Today)
                    {
                        throw new Exception("Reperfusion Date cannot be greater than Today's date.");
                    }
                }



                if (txtReperfusionTime.Text != string.Empty && txtReperfusionTime.Text != "__:__")
                
                {
                    if (GeneralRoutines.IsNumeric(txtReperfusionTime.Text.Substring(0, 2)) == false)
                    {
                        throw new Exception("Time Hour for Reperfusion Time should be numeric.");
                    }

                    if (Convert.ToInt16(txtReperfusionTime.Text.Substring(0, 2)) > 23)
                    {
                        throw new Exception("Time Hour for Reperfusion Time should not be greater than 23.");
                    }

                    if (GeneralRoutines.IsNumeric(txtReperfusionTime.Text.Substring(3, 2)) == false)
                    {
                        throw new Exception("Time Minute for Reperfusion Time should be numeric.");
                    }

                    if (Convert.ToInt16(txtReperfusionTime.Text.Substring(3, 2)) > 59)
                    {
                        throw new Exception("Time Minute for Reperfusion Time should not be greater than 59.");
                    }
                }

                if (GeneralRoutines.IsDate(txtReperfusionDate.Text) && GeneralRoutines.IsDate(txtReperfusionTime.Text))
                {
                    dteReperfusion = Convert.ToDateTime(txtReperfusionDate.Text + " " + txtReperfusionTime.Text);
                }


                if (dteReperfusion!=DateTime.MinValue)
                {
                    if (dteStartAnastomosis!=DateTime.MinValue)
                    {
                        if (dteReperfusion < dteStartAnastomosis)
                        {
                            throw new Exception("Reperfusion Start Date/Time cannot be earlier than Anastomosis Start Date/Time");
                        }
                    }
                    
                }
                //else
                //{
                //    if (dteReperfusion < dteOperationStart)
                //    {
                //        throw new Exception("Reperfusion Start Date/Time cannot be earlier than Operation Start Date/Time");
                //    }
                //}

                

                if (!string.IsNullOrEmpty(txtTotalAnastomosisTime.Text))
                {
                    if (GeneralRoutines.IsNumeric(txtTotalAnastomosisTime.Text) == false)
                    {
                        throw new Exception("Total Anastomosis Time should be numeric");
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


            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_perioperative WHERE TrialIDRecipient=?TrialIDRecipient ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), STRCONN));

            if (intCountFind > 1)
            {
                throw new Exception("More than one Peri-Operative Data has already been added for " + Request.QueryString["TID_R"] + " (Recipient). Please Delete a record.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Peri-Operative Data has already been added for  " + Request.QueryString["TID_R"] + " (Recipient). ");
            }



            string STRSQL = String.Empty;
                                
                
            STRSQL += "INSERT INTO r_perioperative ";
            STRSQL += "(TrialIDRecipient, TransplantationDate, MachinePerfusionStopDate, MachinePerfusionStopTime, TapeBroken, KidneyRemovedFromIceDate, KidneyRemovedFromIceTime,  ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL += "OxygenBottleFullAndTurnedOpen, ";
            }
            STRSQL += "KidneyDiscarded, KidneyDiscardedYes,";
            STRSQL += "TimeOnMachine, OperationStartDate, OperationStartTime, MannitolUsed, DiureticsUsed, HypotensivePeriod, CVPReperfusion, Incision, ";
            STRSQL += "TransplantSide, ArterialProblems, ArterialProblemsOther, VenousProblems, StartAnastomosisDate, StartAnastomosisTime, ";
            STRSQL += "ReperfusionDate, ReperfusionTime, TotalAnastomosisTime,  IntraoperativeDiuresis,";
            STRSQL += "";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?TransplantationDate, ?MachinePerfusionStopDate, ?MachinePerfusionStopTime, ?TapeBroken, ?KidneyRemovedFromIceDate, ?KidneyRemovedFromIceTime, ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL += "?OxygenBottleFullAndTurnedOpen, ";
            }
            STRSQL += " ?KidneyDiscarded, ?KidneyDiscardedYes,";
            STRSQL += "?TimeOnMachine, ?OperationStartDate, ?OperationStartTime, ?MannitolUsed, ?DiureticsUsed, ?HypotensivePeriod, ?CVPReperfusion, ?Incision, ";
            STRSQL += "?TransplantSide, ?ArterialProblems, ?ArterialProblemsOther, ?VenousProblems, ?StartAnastomosisDate, ?StartAnastomosisTime, ";
            STRSQL += "?ReperfusionDate, ?ReperfusionTime, ?TotalAnastomosisTime,  ?IntraoperativeDiuresis,";
            STRSQL += "";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE r_perioperative SET ";
            STRSQL_UPDATE += "TransplantationDate=?TransplantationDate,  MachinePerfusionStopDate=?MachinePerfusionStopDate, MachinePerfusionStopTime=?MachinePerfusionStopTime, ";
            STRSQL_UPDATE += "TapeBroken=?TapeBroken, ";
            STRSQL_UPDATE += "KidneyRemovedFromIceDate=?KidneyRemovedFromIceDate, KidneyRemovedFromIceTime=?KidneyRemovedFromIceTime,";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL_UPDATE += "OxygenBottleFullAndTurnedOpen=?OxygenBottleFullAndTurnedOpen, ";
            }

            STRSQL_UPDATE += "KidneyDiscarded=?KidneyDiscarded, KidneyDiscardedYes=?KidneyDiscardedYes,";
            STRSQL_UPDATE += "TimeOnMachine=?TimeOnMachine, OperationStartDate=?OperationStartDate, OperationStartTime=?OperationStartTime, ";
            STRSQL_UPDATE += "MannitolUsed=?MannitolUsed, DiureticsUsed=?DiureticsUsed, HypotensivePeriod=?HypotensivePeriod, CVPReperfusion=?CVPReperfusion,";
            STRSQL_UPDATE += "Incision=?Incision, TransplantSide=?TransplantSide, ArterialProblems=?ArterialProblems, ArterialProblemsOther=?ArterialProblemsOther,  ";
            STRSQL_UPDATE += "VenousProblems=?VenousProblems, StartAnastomosisDate=?StartAnastomosisDate, StartAnastomosisTime=?StartAnastomosisTime,";
            STRSQL_UPDATE += "TotalAnastomosisTime=?TotalAnastomosisTime,";
            STRSQL_UPDATE += "ReperfusionDate=?ReperfusionDate, ReperfusionTime=?ReperfusionTime, TotalAnastomosisTime=?TotalAnastomosisTime,";
            //STRSQL_UPDATE += "ColdIschemiaPeriodHours=?ColdIschemiaPeriodHours, ColdIschemiaPeriodMinutes=?ColdIschemiaPeriodMinutes,";
            STRSQL_UPDATE += "IntraoperativeDiuresis=?IntraoperativeDiuresis, ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient ";


            string STRSQL_TimeOnMachine = string.Empty;
            STRSQL_TimeOnMachine = @"UPDATE r_perioperative t1
                                    INNER JOIN (SELECT CONCAT(TrialID, LEFT(Side,1)) TrialR, PerfusionStartDate, PerfusionStartTime FROM machineperfusion) t2
                                    ON t1.TrialIDRecipient=t2.TrialR
                                    SET t1.TimeOnMachine= TIMESTAMPDIFF(minute,
                                    CONCAT(t2.PerfusionStartDate, ' ', t2.PerfusionStartTime), CONCAT(t1.MachinePerfusionStopDate, ' ', t1.MachinePerfusionStopTime)  )
                                    WHERE t1.MachinePerfusionStopDate IS NOT NULL AND  t1.MachinePerfusionStopTime IS NOT NULL
                                    AND t2.PerfusionStartDate IS NOT NULL AND  t2.PerfusionStartTime IS NOT NULL 
                                    AND t1.TrialIDRecipient=?TrialIDRecipient";

            string STRSQL_TotalAnastomosisTime = string.Empty;
            STRSQL_TotalAnastomosisTime = @"UPDATE r_perioperative t1
                                    SET t1.TotalAnastomosisTime= TIMESTAMPDIFF(minute,
                                    CONCAT(t1.StartAnastomosisDate, ' ', t1.StartAnastomosisTime),  CONCAT(t1.ReperfusionDate, ' ', t1.ReperfusionTime) )
                                    WHERE t1.ReperfusionDate IS NOT NULL AND  t1.ReperfusionTime IS NOT NULL
                                    AND t1.StartAnastomosisDate IS NOT NULL AND  t1.StartAnastomosisTime IS NOT NULL 
                                    AND t1.TrialIDRecipient=?TrialIDRecipient";


            string STRSQL_ColdIschemiaPeriod  = string.Empty;
            STRSQL_ColdIschemiaPeriod = @"UPDATE r_perioperative t1 INNER JOIN donor_operationdata t2 ON LEFT(t1.TrialIDRecipient,7)=t2.TrialID
                                        SET t1.ColdIschemiaPeriod=
                                        TIME_FORMAT(timediff(CONCAT(t1.KidneyRemovedFromIceDate, ' ',  t1.KidneyRemovedFromIceTime),
                                        CONCAT(t2.StartInSituColdPerfusionDate,  ' ',  t2.StartInSituColdPerfusionTime)),'%H:%i')
                                        WHERE t1.KidneyRemovedFromIceDate IS NOT NULL AND  t1.KidneyRemovedFromIceTime IS NOT NULL
                                        AND t2.StartInSituColdPerfusionDate IS NOT NULL AND t2.StartInSituColdPerfusionTime 
                                        AND t1.TrialIDRecipient=?TrialIDRecipient";


            //change kidney discrd status
            string STRSQLUPDATE_DISCARDYES = String.Empty;
            STRSQLUPDATE_DISCARDYES += @"UPDATE trialdetails_recipient t1 INNER JOIN r_perioperative t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient
                                    SET t1.Active=2 
                                    WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t2.KidneyDiscarded='YES' AND (t1.Active IS NULL or t1.Active <> 0)";

            string STRSQLUPDATE_DISCARDNO = String.Empty;
            STRSQLUPDATE_DISCARDNO += @"UPDATE trialdetails_recipient t1 INNER JOIN r_perioperative t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient
                                    SET t1.Active=1 
                                    WHERE t1.TrialIDRecipient=?TrialIDRecipient AND t2.KidneyDiscarded='NO' AND (t1.Active IS NULL or t1.Active <> 0)";
            
            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE r_perioperative SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            

            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE r_perioperative SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient=?TrialIDRecipient ";

            if (intCountFind==1)
            {
                STRSQL = STRSQL_UPDATE;
            }
            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

                

            //if (intCountFind == 1)
            //{
                                                         
            //    MyCMD.CommandText = STRSQL_UPDATE;
            //}
            //else if (intCountFind == 0)
            //{
                                       
            //    MyCMD.CommandText = STRSQL;
            //}

            //else if (intCountFind > 1)
            //{
            //    throw new Exception("More than One Recipient PeriOperative Data exists for this TrialID. Please Delete an existing Recipient PeriOperative Data.");
            //}

            //else
            //{
            //    throw new Exception("An error occured while checking if Recipient PeriOperative Data  already exist in the database.");
            //}

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();

            //MyCMD.Parameters.Add("?RIdentificationID", MySqlDbType.VarChar).Value = ddRIdentificationID.SelectedValue;

            if (GeneralRoutines.IsDate(txtTransplantationDate.Text) == true)
            {
                MyCMD.Parameters.Add("?TransplantationDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtTransplantationDate.Text); ;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantationDate", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (GeneralRoutines.IsDate(txtMachinePerfusionStopDate.Text) == true)
            {
                MyCMD.Parameters.Add("?MachinePerfusionStopDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtMachinePerfusionStopDate.Text); ;
            }
            else
            {
                MyCMD.Parameters.Add("?MachinePerfusionStopDate", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (txtMachinePerfusionStopTime.Text == string.Empty || txtMachinePerfusionStopTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?MachinePerfusionStopTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MachinePerfusionStopTime", MySqlDbType.VarChar).Value = txtMachinePerfusionStopTime.Text;
            }

            if (rblTapeBroken.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?TapeBroken", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TapeBroken", MySqlDbType.VarChar).Value = rblTapeBroken.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtKidneyRemovedFromIceDate.Text) == true)
            {
                MyCMD.Parameters.Add("?KidneyRemovedFromIceDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtKidneyRemovedFromIceDate.Text); ;
            }
            else
            {
                MyCMD.Parameters.Add("?KidneyRemovedFromIceDate", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (txtKidneyRemovedFromIceTime.Text == string.Empty || txtKidneyRemovedFromIceTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?KidneyRemovedFromIceTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?KidneyRemovedFromIceTime", MySqlDbType.VarChar).Value = txtKidneyRemovedFromIceTime.Text;
            }

            if (rblOxygenBottleFullAndTurnedOpen.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?OxygenBottleFullAndTurnedOpen", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OxygenBottleFullAndTurnedOpen", MySqlDbType.VarChar).Value = rblOxygenBottleFullAndTurnedOpen.SelectedValue;
            }

            if (rblKidneyDiscarded.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?KidneyDiscarded", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else 
            {
                MyCMD.Parameters.Add("?KidneyDiscarded", MySqlDbType.VarChar).Value = rblKidneyDiscarded.SelectedValue;
            }

            if (txtKidneyDiscardedYes.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?KidneyDiscardedYes", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?KidneyDiscardedYes", MySqlDbType.VarChar).Value = txtKidneyDiscardedYes.Text;
            }

            if (txtTimeOnMachine.Text == string.Empty || txtTimeOnMachine.Text == "__:__")
            {
                MyCMD.Parameters.Add("?TimeOnMachine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TimeOnMachine", MySqlDbType.VarChar).Value = txtTimeOnMachine.Text;
            }

            if (GeneralRoutines.IsDate(txtOperationStartDate.Text) == true)
            {
                MyCMD.Parameters.Add("?OperationStartDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtOperationStartDate.Text); ;
            }
            else
            {
                MyCMD.Parameters.Add("?OperationStartDate", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (txtOperationStartTime.Text == string.Empty || txtOperationStartTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?OperationStartTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OperationStartTime", MySqlDbType.VarChar).Value = txtOperationStartTime.Text;
            }

            if ( rblMannitolUsed.SelectedIndex==-1)
            { MyCMD.Parameters.Add("?MannitolUsed", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?MannitolUsed", MySqlDbType.VarChar).Value = rblMannitolUsed.SelectedValue; }

            if ( rblDiureticsUsed.SelectedIndex == -1)
            { MyCMD.Parameters.Add("?DiureticsUsed", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?DiureticsUsed", MySqlDbType.VarChar).Value = rblDiureticsUsed.SelectedValue; }

            if (rblHypotensivePeriod.SelectedIndex == -1)
            { MyCMD.Parameters.Add("?HypotensivePeriod", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?HypotensivePeriod", MySqlDbType.VarChar).Value = rblHypotensivePeriod.SelectedValue; }

            if (txtCVPReperfusion.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?CVPReperfusion", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CVPReperfusion", MySqlDbType.VarChar).Value = txtCVPReperfusion.Text;
            }

            if ( rblIncision.SelectedIndex == -1)
            { MyCMD.Parameters.Add("?Incision", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?Incision", MySqlDbType.VarChar).Value = rblIncision.SelectedValue; }


            if (rblTransplantSide.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?TransplantSide", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantSide", MySqlDbType.VarChar).Value = rblTransplantSide.SelectedValue;
            }

            if (ddArterialProblems.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ArterialProblems", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?ArterialProblems", MySqlDbType.VarChar).Value = ddArterialProblems.SelectedValue;
            }

            if (txtArterialProblemsOther.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?ArterialProblemsOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            { 
                MyCMD.Parameters.Add("?ArterialProblemsOther", MySqlDbType.VarChar).Value = txtArterialProblemsOther.Text;
            }

            if (rblVenousProblems.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?VenousProblems", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?VenousProblems", MySqlDbType.VarChar).Value = rblVenousProblems.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtStartAnastomosisDate.Text) == true)
            {
                MyCMD.Parameters.Add("?StartAnastomosisDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtStartAnastomosisDate.Text); ;
            }
            else
            {
                MyCMD.Parameters.Add("?StartAnastomosisDate", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (txtStartAnastomosisTime.Text == string.Empty || txtStartAnastomosisTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?StartAnastomosisTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?StartAnastomosisTime", MySqlDbType.VarChar).Value = txtStartAnastomosisTime.Text;
            }

            if (GeneralRoutines.IsDate(txtReperfusionDate.Text) == true)
            {
                MyCMD.Parameters.Add("?ReperfusionDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtReperfusionDate.Text); ;
            }
            else
            {
                MyCMD.Parameters.Add("?ReperfusionDate", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (txtReperfusionTime.Text == string.Empty || txtReperfusionTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?ReperfusionTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReperfusionTime", MySqlDbType.VarChar).Value = txtReperfusionTime.Text;
            }

            if (txtTotalAnastomosisTime.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TotalAnastomosisTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TotalAnastomosisTime", MySqlDbType.VarChar).Value = txtTotalAnastomosisTime.Text;
            }

            //if (txtColdIschemiaPeriodHours.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?ColdIschemiaPeriodHours", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?ColdIschemiaPeriodHours", MySqlDbType.VarChar).Value = txtColdIschemiaPeriodHours.Text;
            //}

            //if (txtColdIschemiaPeriodMinutes.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?ColdIschemiaPeriodMinutes", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?ColdIschemiaPeriodMinutes", MySqlDbType.VarChar).Value = txtColdIschemiaPeriodMinutes.Text;
            //}

            if (rblVenousProblems.SelectedIndex == -1)
            { 
                MyCMD.Parameters.Add("?IntraOperativeDiuresis", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?IntraOperativeDiuresis", MySqlDbType.VarChar).Value = rblIntraOperativeDiuresis.SelectedValue; 
            }

            if (txtComments.Text == string.Empty)
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text; }

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

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            try
            {
                MyCMD.ExecuteNonQuery();

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQL_TimeOnMachine;
                MyCMD.ExecuteNonQuery();
                
                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQL_TotalAnastomosisTime;
                MyCMD.ExecuteNonQuery();


                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQL_ColdIschemiaPeriod;
                MyCMD.ExecuteNonQuery();

                if (rblKidneyDiscarded.SelectedValue == STR_YES_SELECTION)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQLUPDATE_DISCARDYES;
                    MyCMD.ExecuteNonQuery();
                }

                if (rblKidneyDiscarded.SelectedValue == STR_NO_SELECTION)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQLUPDATE_DISCARDNO;
                    MyCMD.ExecuteNonQuery();
                }

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

                BindData();

                lblUserMessages.Text = "Data Added.";


                //redirect to summary page if data complete is not no
                string strSQLCOMPLETE = string.Empty;

                strSQLCOMPLETE += "SELECT  ";
                strSQLCOMPLETE += "IF(t2.TransplantationDate IS NOT NULL AND t2.MachinePerfusionStopDate IS NOT NULL AND t2.MachinePerfusionStopTime IS NOT NULL  ";
                strSQLCOMPLETE += "AND t2.TapeBroken IS NOT NULL AND t2.OxygenBottleFullAndTurnedOpen IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.KidneyRemovedFromIceDate IS NOT NULL AND t2.KidneyRemovedFromIceTime IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.KidneyDiscarded IS NOT NULL ";
                strSQLCOMPLETE += "AND IF(t2.KidneyDiscarded ='" + STR_YES_SELECTION + "',  t2.KidneyDiscardedYes IS NOT NULL,  ";
                strSQLCOMPLETE += "t2.TimeOnMachine IS NOT NULL AND t2.OperationStartDate IS NOT NULL AND t2.OperationStartTime IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.ArterialProblems IS NOT NULL AND IF(t2.ArterialProblems='" + STR_OTHER_SELECTION + "', ";
                strSQLCOMPLETE += "t2.ArterialProblems IS NOT NULL, t2.ArterialProblems IS NOT NULL)) ";
                strSQLCOMPLETE += ",'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM r_perioperative t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialIDRecipient=?TrialIDRecipient ";


                //lblVenousProblems.Text = strSQLCOMPLETE;

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                //redirect to summary page
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
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = ex.Message + " An error occured while executing add/update query.";
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
            string STRSQL_FIND = @"SELECT COUNT(*) CR FROM r_perioperative t1 INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient 
                                WHERE t2.TrialID=?TrialID AND t2.TrialIDRecipient=?TrialIDRecipient";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), STRCONN));

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
                //if (String.IsNullOrEmpty(Request.QueryString["RIdentificationID"]))
                //{
                //    throw new Exception("More than one Record exists for deletion.");
                //}
                throw new Exception("More than one Record exists for deletion.");
            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM r_perioperative ";
            STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient ";

            if (!string.IsNullOrEmpty(Request.QueryString["RPerioperativeID"]))
            {
                STRSQL += "AND RPerioperativeID=?RPerioperativeID ";
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

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();
            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();

            if (!String.IsNullOrEmpty(Request.QueryString["RPerioperativeID"]))
            {
                MyCMD.Parameters.Add("?RPerioperativeID", MySqlDbType.VarChar).Value = Request.QueryString["RPerioperativeID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                BindData();

                lblUserMessages.Text = "Data Deleted.";

                Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing delete query.";
            }


            //finally //close connection
            //{
            //    if (MyCONN.State == ConnectionState.Open)
            //    { MyCONN.Close(); }
            //}



        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }

    //selection changed
    protected void rblKidneyDiscarded_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblKidneyDiscarded.SelectedValue == STR_YES_SELECTION)
        {
            pnlKidneyDiscardedYes.Visible = true;
            pnlKidneyDiscardedNo.Visible = false;

        }
        else
        {
            pnlKidneyDiscardedYes.Visible = false;
            pnlKidneyDiscardedNo.Visible = true;
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


//    protected string eGFR_Calculate(string strTrialIDRecipient, Double dblCreatinine, string strUnit)
//    {

//        string streGFR = "NA";
//        string strError = string.Empty;

//        try
//        {
//            //get DOB
//            string STRSQL_DOB = @"SELECT DateOfBirth, FORMAT(DATEDIFF(TransplantationDate,DateOfBirth) / 365.25,2) DOB FROM r_identification t1
//                                    INNER JOIN r_perioperative t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient 
//                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

//            string strDOB = GeneralRoutines.ReturnScalar(STRSQL_DOB, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);

//            if (GeneralRoutines.IsNumeric(strDOB)==false)
//            {
//                throw new Exception("An error occured while calculating Date of Birth.");
//            }


//            //get ethnicity
//            string STRSQL_BLACK = @"SELECT EthnicityBlack FROM r_identification  
//                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

//            string strEthnicityBlack = GeneralRoutines.ReturnScalar(STRSQL_BLACK, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);


//            //check if female
//            string STRSQL_FEMALE = @"SELECT Sex FROM r_identification  
//                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

//            string strFemale = GeneralRoutines.ReturnScalar(STRSQL_FEMALE, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);

//            //now calculate
//            Double dbleGFR;

//            if (strUnit == "µmol/l")
//            {
//                dbleGFR = 32788;
//            }
//            else if (strUnit == "mg/dL")
//            {
//                dbleGFR = 186;
//            }
//            else
//            {
//                throw new Exception("Unknown Unit.");
//            }

//            dbleGFR = dbleGFR * Math.Pow(dblCreatinine, -1.154) * Math.Pow(Convert.ToDouble(strDOB), -0.203);

//            if (strEthnicityBlack==STR_YES_SELECTION)
//            {
//                dbleGFR = dbleGFR * 1.212;
//            }

//            if (strFemale=="Female")
//            {
//                dbleGFR = dbleGFR * 0.742;
//            }

//            streGFR = Math.Round(dbleGFR, 2).ToString(); //round to two decimal places
//        }
//        catch (Exception ex)
//        {
//            strError = ex.Message + " An error occured while calulating eGFR.";
//            streGFR = "Error";
//        }
//        return streGFR;

//    }

    //protected void txtTransplantationDate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        txtMachinePerfusionStopDate.Text = txtTransplantationDate.Text;
    //        txtOperationStartDate.Text = txtTransplantationDate.Text;
    //        txtStartAnastomosisDate.Text = txtTransplantationDate.Text;
    //        txtReperfusionDate.Text = txtTransplantationDate.Text;
    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while executing TextBox changed event.";
    //    }
            
    //}

    //protected void MachinePerfusionStop_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;
    //        if (GeneralRoutines.IsDate(txtMachinePerfusionStopDate.Text) == true && GeneralRoutines.IsDate(txtMachinePerfusionStopTime.Text) == true)
    //        {
    //            if (txtMachinePerfusionStopTime.Text.Length == 5 && txtMachinePerfusionStopTime.Text != "__:__")
    //            {
    //                //get date time for machine perfusion start

    //                string STRSQL_MPStartDT = "";
    //                STRSQL_MPStartDT += "SELECT CONCAT(MachinePerfusionStartDate, ' ', MachinePerfusionStartTime) MPStartDT FROM kidneyinspection ";
    //                STRSQL_MPStartDT += " WHERE TrialID=?TrialID AND LEFT(Side,1)=?Side ";

    //                string strSide = string.Empty;
    //                strSide = Request.QueryString["TID_R"].ToString().Substring(Request.QueryString["TID_R"].ToString().Length - 1, 1);

    //                string strMPStartDT = GeneralRoutines.ReturnScalarTwo(STRSQL_MPStartDT, "?TrialID", Request.QueryString["TID"], "?Side", strSide, STRCONN);

    //                if (GeneralRoutines.IsDate(strMPStartDT) == true)
    //                {
    //                    if (GeneralRoutines.IsDate(txtMachinePerfusionStopDate.Text + " " + txtMachinePerfusionStopTime.Text) == true)
    //                    {
    //                        DateTime? dteMachinePerfusionStart = null;
    //                        DateTime? dteMachinePerfusionEnd = null;

    //                        //lblTimeOnMachine.Text = strMPStartDT;
    //                        //lblTimeOnMachine.Text += " " + txtMachinePerfusionStopDate.Text + " " + txtMachinePerfusionStopTime.Text;

    //                        dteMachinePerfusionStart = Convert.ToDateTime(strMPStartDT);
    //                        dteMachinePerfusionEnd = Convert.ToDateTime(txtMachinePerfusionStopDate.Text + " " + txtMachinePerfusionStopTime.Text);

    //                        if (dteMachinePerfusionStart > dteMachinePerfusionEnd)
    //                        {
    //                            throw new Exception("Machine Perfusion Stop Date/Time cannot be earlier than Machine Perfusion Start Date/Time");
    //                        }

    //                        TimeSpan tsGetSpan;

    //                        tsGetSpan = Convert.ToDateTime(dteMachinePerfusionEnd) - Convert.ToDateTime(dteMachinePerfusionStart);

    //                        txtTimeOnMachine.Text = tsGetSpan.TotalMinutes.ToString();

    //                        //lblTimeOnMachine.Text = dteMachinePerfusionStart.ToString();
    //                        //lblTimeOnMachine.Text += " " + dteMachinePerfusionEnd.ToString();

    //                    }

    //                }
    //            }

    //        }
    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while clicking on Machine Perfusion Stop Date/Time Text Box. Check the Date/Time you have entered.";
    //    }

    //}


    //protected void StartAnastomosisDate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;
    //        if (GeneralRoutines.IsDate(txtStartAnastomosisDate.Text) == true && GeneralRoutines.IsDate(txtStartAnastomosisTime.Text) == true)
    //        {
    //            if (txtStartAnastomosisTime.Text.Length == 5 && txtStartAnastomosisTime.Text != "__:__")
    //            {
    //                //get date time for machine perfusion start

    //                string STRSQL_ColdPerfusionDT = "";
    //                STRSQL_ColdPerfusionDT += "SELECT CONCAT(StartInSituColdPerfusionDate, ' ', StartInSituColdPerfusionTime) ColdPerfusionDT FROM donor_operationdata ";
    //                STRSQL_ColdPerfusionDT += " WHERE TrialID=?TrialID ";


    //                string strColdPerfusionDT = GeneralRoutines.ReturnScalar(STRSQL_ColdPerfusionDT, "?TrialID", Request.QueryString["TID"], STRCONN);

    //                if (GeneralRoutines.IsDate(strColdPerfusionDT) == true)
    //                {
    //                    if (GeneralRoutines.IsDate(txtStartAnastomosisDate.Text + " " + txtStartAnastomosisTime.Text) == true)
    //                    {
    //                        DateTime? dteColdPerfusion = null;
    //                        DateTime? dteStartAnastomosis = null;

    //                        //lblTimeOnMachine.Text = strMPStartDT;
    //                        //lblTimeOnMachine.Text += " " + txtMachinePerfusionStopDate.Text + " " + txtMachinePerfusionStopTime.Text;

    //                        dteColdPerfusion = Convert.ToDateTime(strColdPerfusionDT);
    //                        dteStartAnastomosis = Convert.ToDateTime(txtStartAnastomosisDate.Text + " " + txtStartAnastomosisTime.Text);

    //                        if (dteColdPerfusion > dteStartAnastomosis)
    //                        {
    //                            throw new Exception("Anastomosis Start Date Time cannot be earlier than Cold Perfusion Date/Time");
    //                        }

    //                        TimeSpan tsGetSpan;

    //                        tsGetSpan = Convert.ToDateTime(dteStartAnastomosis) - Convert.ToDateTime(dteColdPerfusion);

    //                        //txtTimeOnMachine.Text = tsGetSpan.TotalMinutes.ToString();

    //                        txtColdIschemiaPeriodHours.Text = Math.Truncate(tsGetSpan.TotalHours).ToString();
    //                        txtColdIschemiaPeriodMinutes.Text = tsGetSpan.Minutes.ToString();
    //                        //lblTimeOnMachine.Text = dteMachinePerfusionStart.ToString();
    //                        //lblTimeOnMachine.Text += " " + dteMachinePerfusionEnd.ToString();

    //                    }

    //                }
    //            }

    //        }
    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while clicking on Machine Perfusion Stop Date/Time Text Box. Check the Date/Time you have entered.";
    //    }

    //}
    
}