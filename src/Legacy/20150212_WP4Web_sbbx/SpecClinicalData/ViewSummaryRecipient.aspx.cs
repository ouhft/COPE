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

public partial class SpecClinicalData_ViewSummaryRecipient : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        //access denied cannot randomise
        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx?EID=51";

        private const string STR_OTHER_SELECTION = "Other";
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
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (Request.QueryString["TID_R"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID (Recipient).");
                }

                lblDescription.Text = "View Summary Data for " + Request.QueryString["TID_R"].ToString() + " (Recipient)";

                ViewState["SortField"] = "EventCode";
                ViewState["SortDirection"] = "ASC";

                BindData();


                AssignMainData();

            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading page.";
        }
    }

    protected void BindData()
    {
        try
        {
            lblGV1.Text = string.Empty;

            string strSQL = string.Empty;
            string strEventCode = string.Empty;
            string strOccasion = string.Empty; //for Follow Up data
            string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

            strSQL += "SELECT t1.* FROM (";

           
            if (SessionVariablesAll.AddEditRecipient==STR_YES_SELECTION)
            {
                //Recipient Identification Event Code 31
                strEventCode = "31";

                //strSQL += "UNION ";
                strSQL += "SELECT t1.TrialID, CAST(t2.RIdentificationID AS CHAR) EventID, t2.TrialIDRecipient,   ";
                strSQL += "IF(t2.TrialIDRecipient IS NOT NULL, ";
                strSQL += "IF(t2.Weight IS NOT NULL AND t2.Height IS NOT NULL AND t2.Sex IS NOT NULL AND t2.EthnicityBlack IS NOT NULL ";
                strSQL += "AND IF(t2.RenalDisease ='" + STR_OTHER_SELECTION + "',  t2.RenalDiseaseOther IS NOT NULL, t2.RenalDisease IS NOT NULL) ";
                strSQL += "AND t2.PreTransplantDiuresis IS NOT NULL AND t2.BloodGroup IS NOT NULL AND t2.HLA_A_Mismatch IS NOT NULL ";
                strSQL += "AND t2.HLA_B_Mismatch IS NOT NULL AND t2.HLA_DR_Mismatch IS NOT NULL ";
                strSQL += "";
                strSQL += "AND t2.DateQOLFilled IS NOT NULL AND t2.QOLFilledAt IS NOT NULL AND ";
                strSQL += "t2.Mobility IS NOT NULL AND t2.SelfCare IS NOT NULL AND t2.UsualActivities IS NOT NULL AND t2.PainDiscomfort IS NOT NULL AND t2.AnxietyDepression IS NOT NULL ";
                strSQL += "AND t2.VASScore IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQL += "";
                strSQL += ", 'NO') Completed,";
                strSQL += "t2.DateCreated EventDate,";
                strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t2.DataLocked, t2.DataFinal,";
                strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink ";
                strSQL += "FROM trialdetails_recipient t1 LEFT JOIN r_identification t2 ";
                strSQL += "ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
                strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";



                //Recipient Peri-Operative Data Event Code 32
                strEventCode = "32";

                strSQL += "UNION ";
                strSQL += "SELECT t1.TrialID, CAST(t3.RPerioperativeID AS CHAR)  EventID, t2.TrialIDRecipient,  ";
                strSQL += "IF(t3.TrialIDRecipient IS NOT NULL, ";
                strSQL += "IF(t3.TransplantationDate IS NOT NULL AND t3.MachinePerfusionStopDate IS NOT NULL AND t3.MachinePerfusionStopTime IS NOT NULL  ";
                strSQL += "AND t3.TapeBroken IS NOT NULL AND t3.OxygenBottleFullAndTurnedOpen IS NOT NULL ";
                strSQL += "AND t3.KidneyRemovedFromIceDate IS NOT NULL AND t3.KidneyRemovedFromIceTime IS NOT NULL ";
                strSQL += "AND t3.KidneyDiscarded IS NOT NULL ";
                strSQL += "AND IF(t3.KidneyDiscarded ='" + STR_YES_SELECTION + "',  t3.KidneyDiscardedYes IS NOT NULL,  ";
                strSQL += "t3.TimeOnMachine IS NOT NULL AND t3.OperationStartDate IS NOT NULL AND t3.OperationStartTime IS NOT NULL ";
                strSQL += "AND t3.ArterialProblems IS NOT NULL AND IF(t3.ArterialProblems='" + STR_OTHER_SELECTION + "', ";
                strSQL += "t3.ArterialProblems IS NOT NULL, t3.ArterialProblems IS NOT NULL)) ";
                strSQL += ",'Complete', 'Incomplete') ";
                strSQL += "";
                strSQL += ", 'NO') Completed,";
                strSQL += "t3.TransplantationDate EventDate,";
                strSQL += "DATE_FORMAT(t3.TransplantationDate, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t3.DataLocked, t3.DataFinal,";
                strSQL += "t4.EventCode, t4.EventName, t4.PageLink, t4.PageIdentifier, t4.DateLink ";
                strSQL += "FROM trialdetails_recipient t1 LEFT JOIN r_identification t2 ";
                strSQL += "ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
                strSQL += "LEFT JOIN r_perioperative t3 ON t2.TrialIDRecipient=t3.TrialIDRecipient  ";
                strSQL += "";

                strSQL += "LEFT JOIN eventcodes t4 ON t4.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            }




            if (strIsServer == "1" && SessionVariablesAll.AddEditFollowUp == STR_YES_SELECTION)
            {

                ////Recipient Follow Up (Days 1 to 14) Event Code 41
                strEventCode = "41";

                if (SessionVariablesAll.AddEditRecipient==STR_YES_SELECTION)
                {
                    strSQL += "UNION ";
                }
                
                

                strSQL += "SELECT t1.TrialID, CAST(t3.RFUPostTreatmentID AS CHAR)  EventID, t2.TrialIDRecipient,   ";
                strSQL += "IF(t3.TrialIDRecipient IS NOT NULL, ";
                strSQL += "IF(t3.Occasion IS NOT NULL AND t3.FollowUpDate IS NOT NULL AND t3.GraftFailure IS NOT NULL AND t3.GraftRemoval IS NOT NULL   ";
                strSQL += "AND t3.CreatinineUnit IS NOT NULL ";
                strSQL += "AND IF(t3.NeedDialysis LIKE '%YES%', t3.DialysisType IS NOT NULL, t3.NeedDialysis IS NOT NULL) ";
                strSQL += "  ";
                strSQL += "AND IF(t3.Rejection ='YES', t3.PostTxPrednisolon IS NOT NULL AND t3.PostTxOther IS NOT NULL ";
                strSQL += "AND t3.RejectionBiopsyProven IS NOT NULL,  t3.Rejection IS NOT NULL) ";
                strSQL += " ";
                strSQL += " ";
                strSQL += ", 'Complete', 'Incomplete') ";
                strSQL += "";
                strSQL += ", 'NO') Completed,";
                strSQL += "t3.FollowUpDate EventDate,";
                strSQL += "DATE_FORMAT(t3.FollowUpDate, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t3.DataLocked, t3.DataFinal,";
                strSQL += "t4.EventCode, t4.EventName, t4.PageLink, t4.PageIdentifier, t4.DateLink ";
                strSQL += "FROM trialdetails_recipient t1 LEFT JOIN r_identification t2 ";
                strSQL += "ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
                //Follow up data days 1-14
                strOccasion = "1-7 Days";
                strSQL += "LEFT JOIN (SELECT * FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion='" + strOccasion + "') t3 ";
                strSQL += "ON t2.TrialIDRecipient=t3.TrialIDRecipient  ";

                strSQL += "LEFT JOIN eventcodes t4 ON t4.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
                

                ////Recipient Follow Up (3 Month) Event Code 43
                strEventCode = "43";

                //Follow up data 3 Months
                strOccasion = "3 Months";

                strSQL += "UNION ";
                strSQL += "SELECT t1.TrialID, CAST(t3.RFUPostTreatmentID AS CHAR) EventID, t2.TrialIDRecipient,   ";
                strSQL += "IF(t3.TrialIDRecipient IS NOT NULL, ";
                strSQL += "";
                strSQL += "IF(t3.Occasion IS NOT NULL AND t3.FollowUpDate IS NOT NULL AND t3.GraftFailure IS NOT NULL AND t3.GraftRemoval IS NOT NULL   ";
                strSQL += "AND t3.SerumCreatinine IS NOT NULL AND t3.CreatinineUnit IS NOT NULL ";
                strSQL += "AND t3.UrineCreatinine IS NOT NULL AND t3.UrineUnit IS NOT NULL ";
                strSQL += "AND t3.CreatinineClearance IS NOT NULL AND t3.CreatinineClearanceUnit IS NOT NULL ";
                strSQL += "AND IF(t3.CurrentlyDialysis = 'YES', t3.DialysisType IS NOT NULL, t3.CurrentlyDialysis IS NOT NULL) ";
                strSQL += "  ";
                strSQL += "AND IF(t3.Rejection ='YES', t3.PostTxPrednisolon IS NOT NULL AND t3.PostTxOther IS NOT NULL ";
                strSQL += "AND t3.RejectionBiopsyProven IS NOT NULL,  t3.Rejection IS NOT NULL) ";
                //QOL Data
                if (strOccasion == "3 Months" || strOccasion == "1 Year")
                {
                    strSQL += "AND t3.QOLFilledAt IS NOT NULL ";
                    strSQL += "AND t3.Mobility IS NOT NULL AND t3.SelfCare IS NOT NULL AND t3.UsualActivities IS NOT NULL  ";
                    strSQL += "AND t3.PainDiscomfort IS NOT NULL AND t3.AnxietyDepression IS NOT NULL AND t3.VASScore IS NOT NULL ";
                }

                //strSQLCOMPLETE += "AND t3.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQL += ", 'Complete', 'Incomplete') ";
                strSQL += ", 'NO') Completed,";
                strSQL += "t3.FollowUpDate EventDate,";
                strSQL += "DATE_FORMAT(t3.FollowUpDate, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t3.DataLocked, t3.DataFinal,";
                strSQL += "t4.EventCode, t4.EventName, t4.PageLink, t4.PageIdentifier, t4.DateLink ";
                strSQL += "FROM trialdetails_recipient t1 LEFT JOIN r_identification t2 ";
                strSQL += "ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
                
                strSQL += "LEFT JOIN (SELECT * FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion='" + strOccasion + "') t3 ";
                strSQL += "ON t2.TrialIDRecipient=t3.TrialIDRecipient  ";

                strSQL += "LEFT JOIN eventcodes t4 ON t4.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";


                ////Recipient Follow Up (6 Months) Event Code 44
                strEventCode = "44";

                //Follow up data 6 Months
                strOccasion = "6 Months";

                strSQL += "UNION ";
                strSQL += "SELECT t1.TrialID, CAST(t3.RFUPostTreatmentID AS CHAR) EventID, t2.TrialIDRecipient,   ";
                strSQL += "IF(t3.TrialIDRecipient IS NOT NULL, ";
                strSQL += "";
                strSQL += "IF(t3.Occasion IS NOT NULL AND t3.FollowUpDate IS NOT NULL AND t3.GraftFailure IS NOT NULL AND t3.GraftRemoval IS NOT NULL   ";
                strSQL += "AND t3.SerumCreatinine IS NOT NULL AND t3.CreatinineUnit IS NOT NULL ";
                strSQL += "AND t3.UrineCreatinine IS NOT NULL AND t3.UrineUnit IS NOT NULL ";
                strSQL += "AND t3.CreatinineClearance IS NOT NULL AND t3.CreatinineClearanceUnit IS NOT NULL ";
                strSQL += "AND IF(t3.CurrentlyDialysis = 'YES', t3.DialysisType IS NOT NULL, t3.CurrentlyDialysis IS NOT NULL) ";
                strSQL += "  ";
                strSQL += "AND IF(t3.Rejection ='YES', t3.PostTxPrednisolon IS NOT NULL AND t3.PostTxOther IS NOT NULL ";
                strSQL += "AND t3.RejectionBiopsyProven IS NOT NULL,  t3.Rejection IS NOT NULL) ";
                //QOL Data
                if (strOccasion == "3 Months" || strOccasion == "1 Year")
                {
                    strSQL += "AND t3.QOLFilledAt IS NOT NULL ";
                    strSQL += "AND t3.Mobility IS NOT NULL AND t3.SelfCare IS NOT NULL AND t3.UsualActivities IS NOT NULL  ";
                    strSQL += "AND t3.PainDiscomfort IS NOT NULL AND t3.AnxietyDepression IS NOT NULL AND t3.VASScore IS NOT NULL ";
                }

                strSQL += ", 'Complete', 'Incomplete') ";
                strSQL += ", 'NO') Completed,";
                strSQL += "t3.FollowUpDate EventDate,";
                strSQL += "DATE_FORMAT(t3.FollowUpDate, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t3.DataLocked, t3.DataFinal,";
                strSQL += "t4.EventCode, t4.EventName, t4.PageLink, t4.PageIdentifier, t4.DateLink ";
                strSQL += "FROM trialdetails_recipient t1 LEFT JOIN r_identification t2 ";
                strSQL += "ON t1.TrialIDRecipient=t2.TrialIDRecipient ";

                strSQL += "LEFT JOIN (SELECT * FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion='" + strOccasion + "') t3 ";
                strSQL += "ON t2.TrialIDRecipient=t3.TrialIDRecipient  ";

                strSQL += "LEFT JOIN eventcodes t4 ON t4.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";

                               


                ////Recipient Follow Up (1 Year) Event Code 45
                //strEventCode = "45";
                strEventCode = "45";

                //Follow up data 1 Year
                strOccasion = "1 Year";

                strSQL += "UNION ";
                strSQL += "SELECT t1.TrialID, CAST(t3.RFUPostTreatmentID AS CHAR) EventID, t2.TrialIDRecipient,   ";
                strSQL += "IF(t3.TrialIDRecipient IS NOT NULL, ";
                strSQL += "";
                strSQL += "IF(t3.Occasion IS NOT NULL AND t3.FollowUpDate IS NOT NULL AND t3.GraftFailure IS NOT NULL AND t3.GraftRemoval IS NOT NULL   ";
                strSQL += "AND t3.SerumCreatinine IS NOT NULL AND t3.CreatinineUnit IS NOT NULL ";
                strSQL += "AND t3.UrineCreatinine IS NOT NULL AND t3.UrineUnit IS NOT NULL ";
                strSQL += "AND t3.CreatinineClearance IS NOT NULL AND t3.CreatinineClearanceUnit IS NOT NULL ";
                strSQL += "AND IF(t3.CurrentlyDialysis = 'YES', t3.DialysisType IS NOT NULL, t3.CurrentlyDialysis IS NOT NULL) ";
                strSQL += "  ";
                strSQL += "AND IF(t3.Rejection ='YES', t3.PostTxPrednisolon IS NOT NULL AND t3.PostTxOther IS NOT NULL ";
                strSQL += "AND t3.RejectionBiopsyProven IS NOT NULL,  t3.Rejection IS NOT NULL) ";
                //QOL Data
                if (strOccasion == "3 Months" || strOccasion == "1 Year")
                {
                    strSQL += "AND t3.QOLFilledAt IS NOT NULL ";
                    strSQL += "AND t3.Mobility IS NOT NULL AND t3.SelfCare IS NOT NULL AND t3.UsualActivities IS NOT NULL  ";
                    strSQL += "AND t3.PainDiscomfort IS NOT NULL AND t3.AnxietyDepression IS NOT NULL AND t3.VASScore IS NOT NULL ";
                }

                strSQL += ", 'Complete', 'Incomplete') ";
                strSQL += ", 'NO') Completed,";
                strSQL += "t3.FollowUpDate EventDate,";
                strSQL += "DATE_FORMAT(t3.FollowUpDate, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t3.DataLocked, t3.DataFinal,";
                strSQL += "t4.EventCode, t4.EventName, t4.PageLink, t4.PageIdentifier, t4.DateLink ";
                strSQL += "FROM trialdetails_recipient t1 LEFT JOIN r_identification t2 ";
                strSQL += "ON t1.TrialIDRecipient=t2.TrialIDRecipient ";

                strSQL += "LEFT JOIN (SELECT * FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion='" + strOccasion + "') t3 ";
                strSQL += "ON t2.TrialIDRecipient=t3.TrialIDRecipient  ";

                strSQL += "LEFT JOIN eventcodes t4 ON t4.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";




                //Resource Use Log Day 1 Year
                strEventCode = "66";
                strOccasion = "1 Year";


                strSQL += "UNION ";
                strSQL += "SELECT t1.TrialID, CAST(t3.ResUseID AS CHAR) EventID, t2.TrialIDRecipient, IF(t3.TrialIDRecipient IS NOT NULL,";
                strSQL += "IF(t3.Occasion IS NOT NULL AND t3.GPAppointment IS NOT NULL AND t3.GPHomeVisit IS NOT NULL  ";
                strSQL += "AND t3.SpecConsultantAppointment IS NOT NULL AND t3.AETreatment IS NOT NULL ";
                strSQL += ", 'Complete', 'Incomplete'), ";
                strSQL += "'NO') Available, ";
                strSQL += "t3.DateCreated EventDate,   ";
                strSQL += "DATE_FORMAT(t3.DateCreated, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t3.DataLocked, t3.DataFinal,";
                //strSQL += "t3.OrderSequence,";
                strSQL += "t4.EventCode, t4.EventName, t4.PageLink, t4.PageIdentifier, t4.DateLink ";
                strSQL += "FROM trialdetails_recipient t1 LEFT JOIN r_identification t2 ";
                strSQL += "ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
                strSQL += "LEFT JOIN ";
                strSQL += "(SELECT * FROM ResUse WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion='" + strOccasion + "') t3 ";
                strSQL += "ON t1.TrialIDRecipient=t3.TrialIDRecipient ";
                strSQL += "LEFT JOIN eventcodes t4 ON t4.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";

            }
            



            //close the query
            strSQL += ") t1 ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on Event Name to View Data.";

            //lblUserMessages.Text = strSQL;


        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while loading page.";
        }

    }

    protected void GV1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int i = 0;
            for (i = 0; i <= 3; i++)
            {

                if (i == 1)
                {
                    if (e.Row.Cells[i].Text == "NO")
                    {
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[i].Font.Bold = true;

                    }
                    //if (e.Row.Cells[i].Text == "InComplete")
                    //{
                    //    e.Row.Cells[i].ForeColor = System.Drawing.Color.PaleGreen;
                    //    e.Row.Cells[i].Font.Bold = true;
                    //}

                    if (e.Row.Cells[i].Text == "Complete")
                    {
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Green;
                        e.Row.Cells[i].Font.Bold = true;
                    }
                }
                //else
                //{
                //    e.Row.Cells[i].ForeColor = System.Drawing.Color.LightGray;
                //    e.Row.Cells[i].BackColor = System.Drawing.Color.LightGray;
                //}

            }

        }
    }


    protected void AssignMainData()
    {
        try
        {
            string STRSQL = string.Empty;
            STRSQL += "SELECT t1.TrialID, t1.DonorID, t2.TrialIDRecipient, t3.RecipientID, t3.DateOfBirth, t2.Active,  ";
            STRSQL += "CAST(IF(t3.DateOfBirth IS NULL, 'NA', (YEAR(CURDATE())-YEAR(t3.DateOfBirth)) - (RIGHT(CURDATE(),5)<RIGHT(t3.DateOfBirth,5))) AS CHAR) Age,";
            STRSQL += "t5.TransplantationDate, ";
            STRSQL += "t6.DateWithdrawn, t6.ReasonWithdrawn, ";
            STRSQL += "t7.DeathDate ";
            STRSQL += "FROM trialdetails  t1  ";
            STRSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "LEFT JOIN r_identification t3 ON t2.TrialIDRecipient=t3.TrialIDRecipient ";
            STRSQL += "LEFT JOIN kidneyr t4 ON t2.TrialID=t4.TrialID ";
            STRSQL += "LEFT JOIN r_perioperative t5 ON t2.TrialIDRecipient=t5.TrialIDRecipient ";
            STRSQL += "LEFT JOIN trialidwithdrawn t6 ON t2.TrialIDRecipient=t6.TrialIDRecipient ";
            STRSQL += "LEFT JOIN r_deceased t7 ON t2.TrialIDRecipient=t7.TrialIDRecipient ";
            STRSQL += "WHERE t1.TrialID=?TrialID AND t2.TrialIDRecipient=?TrialIDRecipient ";
            STRSQL += "GROUP BY t1.TrialID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];
            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    while (myDr.Read())
                    {
                        if (myDr.HasRows)
                        {

                            //lblActive.Text="Active/Withdrawn - ";
                            if (!DBNull.Value.Equals(myDr["ReasonWithdrawn"]) || !DBNull.Value.Equals(myDr["DateWithdrawn"]))
                            {
                                if (!DBNull.Value.Equals(myDr["ReasonWithdrawn"]))
                                {
                                    lblActive.Text = "Withdrawn  " + myDr["ReasonWithdrawn"].ToString() + " ";
                                }
                                if (GeneralRoutines.IsDate(myDr["DateWithdrawn"].ToString()))
                                {

                                    if (lblActive.Text == string.Empty)
                                    {
                                        lblActive.Text = "Withdrawn ";
                                    }
                                    lblActive.Text += Convert.ToDateTime(myDr["DateWithdrawn"]).ToShortDateString();
                                    lblActive.ForeColor = System.Drawing.Color.Red;
                                    lblActive.Font.Bold = true;
                                    lblActive.Font.Size = 14;
                                }
                            }
                            else
                            {
                                if (!DBNull.Value.Equals(myDr["Active"]))
                                {

                                    if (!DBNull.Value.Equals(myDr["DeathDate"]))
                                    {
                                        if (GeneralRoutines.IsDate(myDr["DeathDate"].ToString()))
                                        {
                                            lblActive.ForeColor = System.Drawing.Color.Red;
                                            lblActive.Font.Bold = true;
                                            lblActive.Font.Size = 14;
                                            if (lblActive.Text != string.Empty)
                                            {
                                                lblActive.Text += ";";
                                            }
                                            lblActive.Text += " Deceased " + Convert.ToDateTime(myDr["DeathDate"]).ToShortDateString();
                                        }
                                    }
                                    else
                                    {
                                        if (myDr["Active"].ToString() == "1")
                                        {
                                            lblActive.Text = "Active";
                                            lblActive.ForeColor = System.Drawing.Color.Green;
                                            lblActive.Font.Bold = true;

                                            if (!DBNull.Value.Equals(myDr["Age"]))
                                            {
                                                if (lblActive.Text != string.Empty)
                                                {
                                                    lblActive.Text += ";";
                                                }
                                                lblActive.Text += " Age Now " + myDr["Age"].ToString() + " years";
                                            }
                                        }
                                        if (myDr["Active"].ToString() == "0")
                                        {
                                            lblActive.Text = "Inactive";
                                            lblActive.ForeColor = System.Drawing.Color.Red;
                                            lblActive.Font.Bold = true;
                                        }

                                        if (myDr["Active"].ToString() == "2")
                                        {
                                            lblActive.Text = "Kidney Discarded";
                                            lblActive.ForeColor = System.Drawing.Color.Blue;
                                            lblActive.Font.Bold = true;
                                        }

                                    }


                                    //if (myDr["Active"].ToString() == "1")
                                    //{
                                    //    lblActive.Text="Active";
                                    //} 
                                }
                                else
                                {
                                    if (!DBNull.Value.Equals(myDr["DeathDate"]))
                                    {
                                        if (GeneralRoutines.IsDate(myDr["DeathDate"].ToString()))
                                        {
                                            lblActive.ForeColor = System.Drawing.Color.Red;
                                            lblActive.Font.Bold = true;
                                            lblActive.Font.Size = 14;
                                            if (lblActive.Text != string.Empty)
                                            {
                                                lblActive.Text += ";";
                                            }
                                            lblActive.Text += " Deceased " + Convert.ToDateTime(myDr["DeathDate"]).ToShortDateString();
                                        }
                                    }

                                }


                            }






                        }
                    }
                }
                // close connection
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

            }

            catch (System.Exception ex)
            {
                // close connection
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = ex.Message + " An error occured while executing Assign query.";
            }

        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Assigning Main Details.";
        }
    }
}