using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SpecClinicalData_ViewSummary : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";

        private const string STR_DD_UNKNOWN_SELECTION = "0";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (Request.QueryString["TID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID.");
                }

                lblDescription.Text = "View Summary Data (Donor) for " + Request.QueryString["TID"].ToString();

                ViewState["SortField"] = "EventCode";
                ViewState["SortDirection"] = "ASC";

                BindData();

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
            string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver"); //if server


            strSQL += "SELECT t1.* FROM (";

            //Donor General Procedure data
            strEventCode = "10";
            strSQL += "SELECT t1.TrialID, t2.DonorGeneralProcedureDataID EventID, IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(t2.TrialID IS NOT NULL, 'Complete', 'Incomplete') ";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.DateCreated EventDate,  ";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN donor_generalproceduredata t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID ";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";


            //Donor Details Event Code=11
            strEventCode = "11";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.DonorIdentificationID EventID,  ";
            strSQL += "IF(t2.TrialID IS NOT NULL, IF(t2.DateDonorAdmission IS NOT NULL AND t2.DateDonorOperation IS NOT NULL AND t2.Sex IS NOT NULL ";
            strSQL += "AND t2.Weight IS NOT NULL AND t2.Height IS NOT NULL ";
            strSQL += ",'Complete', 'Incomplete') ";
            strSQL += ",'NO') Completed, ";
            strSQL += "t2.DateCreated EventDate,";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN donor_identification t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID ";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            
            ////Donor Preop DA Event Code=12
            strEventCode = "12";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.DonorPreOpClinicalDataID EventID,   ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(IF(t2.Diagnosis ='Other', t2.DiagnosisOtherDetails IS NOT NULL, t2.Diagnosis IS NOT NULL)  ";
            strSQL += "AND t2.DiabetesMellitus IS NOT NULL AND t2.AlcoholAbuse IS NOT NULL AND t2.CardiacArrest IS NOT NULL ";
            strSQL += "AND t2.SystolicBloodPressure IS NOT NULL AND t2.DiastolicBloodPressure IS NOT NULL ";
            strSQL += "AND t2.Diuresis IS NOT NULL AND t2.Dopamine IS NOT NULL AND t2.Dobutamine IS NOT NULL  ";
            strSQL += "AND t2.NorAdrenaline IS NOT NULL ";
            strSQL += ",'Complete', 'Incomplete') ";
            strSQL += "";
            strSQL += ",'NO') Completed, ";
            strSQL += "t2.DateCreated EventDate,"; 
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN donor_preop_clinicaldata t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID ";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";

            ////Donor Lab results Data Event Code=13
            strEventCode = "13";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.DonorLabResultsID EventID, ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(t2.Hb IS NOT NULL AND t2.HbUnit IS NOT NULL  ";
            strSQL += "AND t2.Ht IS NOT NULL AND t2.pH IS NOT NULL AND t2.pCO2 IS NOT NULL ";
            strSQL += "AND t2.pO2 IS NOT NULL AND t2.Urea IS NOT NULL AND t2.UreaUnit IS NOT NULL ";
            strSQL += "AND t2.Creatinine IS NOT NULL AND t2.CreatinineUnit IS NOT NULL ";
            strSQL += ",'Complete', 'Incomplete') ";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.DateCreated EventDate,  ";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN donor_labresults t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID ";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";

            ////Donor Operation Data Event Code=14
            strEventCode = "14";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.DonorOperatationDataID EventID, ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(t2.WithdrawlLifeSupportTreatmentDate IS NOT NULL AND t2.WithdrawlLifeSupportTreatmentTime IS NOT NULL  ";
            strSQL += "AND t2.StartNoTouchPeriodDate IS NOT NULL AND t2.StartNoTouchPeriodTime IS NOT NULL ";
            strSQL += "AND t2.LengthNoTouchPeriod IS NOT NULL  AND t2.ConfirmationDeathDate IS NOT NULL AND t2.ConfirmationDeathTime IS NOT NULL ";
            strSQL += "AND t2.StartInSituColdPerfusionDate IS NOT NULL  AND t2.StartInSituColdPerfusionTime IS NOT NULL   ";
            strSQL += "AND IF(t2.SystemicFlushSolutionUsed ='Other', t2.SystemicFlushSolutionUsedOther IS NOT NULL,t2.SystemicFlushSolutionUsed IS NOT NULL) ";
            strSQL += "AND IF(t2.PreservationSolutionColdPerfusion ='Other', t2.PreservationSolutionColdPerfusionOther IS NOT NULL,t2.PreservationSolutionColdPerfusion IS NOT NULL) ";
            strSQL += "AND t2.Heparin IS NOT NULL  ";
            strSQL += ",'Complete', 'Incomplete') ";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.DateCreated EventDate,  ";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN donor_operationdata t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID ";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";

            if (strIsServer == "1")
            {
                //Randomisation Event Code=25
                strEventCode = "25";

                strSQL += "UNION ";
                strSQL += "SELECT t1.TrialID, t2.KidneyRID EventID, ";
                strSQL += "IF(t2.TrialID IS NOT NULL, ";
                strSQL += "IF(t2.LeftRandomisationArm IS NOT NULL AND t2.RightRandomisationArm IS NOT NULL ";
                strSQL += ",'Complete', 'Incomplete') ";
                strSQL += ", 'NO') Completed,";
                strSQL += "t2.DateCreated EventDate,  ";
                strSQL += "t2.DataLocked, t2.DataFinal,";
                strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
                strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
                strSQL += "FROM trialdetails t1 LEFT JOIN kidneyr t2 ";
                strSQL += "ON t1.TrialID=t2.TrialID ";
                strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
                strSQL += "WHERE t1.TrialID=?TrialID ";


            }

            


            
            //Kidney Inspection Event Code=27
            strEventCode = "27";

                       
            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.KidneyInspectionID EventID,   ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(t2.ArterialProblems LIKE '%YES%'";
            strSQL += "AND t2.ArterialProblems_R LIKE '%YES%' ";
            strSQL += "AND t2.KidneyTransplantable IS NOT NULL AND IF(t2.KidneyTransplantable='NO', t2.ReasonNotTransplantable IS NOT NULL, t2.KidneyTransplantable IS NOT NULL)   ";
            strSQL += "AND t2.KidneyTransplantable_R IS NOT NULL AND IF(t2.KidneyTransplantable_R='NO', t2.ReasonNotTransplantable_R IS NOT NULL, t2.KidneyTransplantable_R IS NOT NULL) ";
                
            strSQL += ",'Complete', 'Incomplete') ";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.RemovalDate EventDate,";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.RemovalDate, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN kidneyinspection t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID ";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";


            string strSide = string.Empty;
            //machine perfusion data
            strEventCode = "29";

            //first left kidney
            strSide = "Left";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.MachinePerfusionID EventID,   ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(IF(t2.KidneyOnMachine='" + STR_NO_SELECTION + "', t2.KidneyOnMachineNo  IS NOT NULL, ";
            strSQL += "t2.PerfusionStartDate IS NOT NULL AND t2.PerfusionStartTime IS NOT NULL ";
            strSQL += "AND t2.MachineSerialNumber IS NOT NULL AND t2.MachineReferenceModelNumber  IS NOT NULL ";
            strSQL += "AND t2.LotNumberPerfusionSolution IS NOT NULL AND t2.LotNumberDisposables IS NOT NULL ";
            strSQL += "AND t2.UsedPatchHolder IS NOT NULL AND t2.ArtificialPatchUsed  IS NOT NULL ";
            strSQL += "AND t2.OxygenBottleFull IS NOT NULL AND t2.OxygenBottleOpened IS NOT NULL) ";
            strSQL += ", 'Complete', 'Incomplete') ";
            strSQL += "";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.PerfusionStartDate EventDate,";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.PerfusionStartDate, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN machineperfusion t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID AND t2.Side='" + strSide + "'";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' AND t3.PageIdentifier LIKE '%" + strSide + "%' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";


            //strSQL += "UNION ";
            //strSQL += "SELECT t1.TrialID, t2.MachinePerfusionID EventID, IF(t2.TrialID IS NOT NULL, 'YES', 'NO') Completed,t2.PerfusionStartDate EventDate,  ";
            //strSQL += "";
            //strSQL += "";
            //strSQL += "";
            //strSQL += "t2.DataLocked, t2.DataFinal,";
            //strSQL += "DATE_FORMAT(t2.PerfusionStartDate, '%d/%m%/%Y') Event_Date, ";
            //strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink ";
            //strSQL += "FROM trialdetails t1 LEFT JOIN machineperfusion t2 ";
            //strSQL += "ON t1.TrialID=t2.TrialID AND t2.Side='" + strSide + "'";
            //strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' AND t3.PageIdentifier LIKE '%" + strSide + "%' ";
            //strSQL += "WHERE t1.TrialID=?TrialID ";

            //first left kidney
            strSide = "Right";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.MachinePerfusionID EventID,   ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(IF(t2.KidneyOnMachine='" + STR_NO_SELECTION + "', t2.KidneyOnMachineNo  IS NOT NULL, ";
            strSQL += "t2.PerfusionStartDate IS NOT NULL AND t2.PerfusionStartTime IS NOT NULL ";
            strSQL += "AND t2.MachineSerialNumber IS NOT NULL AND t2.MachineReferenceModelNumber  IS NOT NULL ";
            strSQL += "AND t2.LotNumberPerfusionSolution IS NOT NULL AND t2.LotNumberDisposables IS NOT NULL ";
            strSQL += "AND t2.UsedPatchHolder IS NOT NULL AND t2.ArtificialPatchUsed  IS NOT NULL ";
            strSQL += "AND t2.OxygenBottleFull IS NOT NULL AND t2.OxygenBottleOpened IS NOT NULL) ";
            strSQL += ", 'Complete', 'Incomplete') ";
            strSQL += "";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.PerfusionStartDate EventDate,";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.PerfusionStartDate, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN machineperfusion t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID AND t2.Side='" + strSide + "'";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' AND t3.PageIdentifier LIKE '%" + strSide + "%' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";

            
            //Kidney Procedure data
            strEventCode = "30";

            //first left kidney
            strSide = "Left";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.KidneyProcedureDataID EventID,  ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(t2.Reallocated  IS NOT NULL ";
            strSQL += ", 'Complete', 'Incomplete') ";
            strSQL += "";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.DateCreated EventDate, ";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN kidneyproceduredata t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID AND t2.Side='" + strSide + "'";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' AND t3.PageIdentifier LIKE '%" + strSide + "%' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";

            //first left kidney
            strSide = "Right";

            strSQL += "UNION ";
            strSQL += "SELECT t1.TrialID, t2.KidneyProcedureDataID EventID,  ";
            strSQL += "IF(t2.TrialID IS NOT NULL, ";
            strSQL += "IF(t2.Reallocated  IS NOT NULL ";
            strSQL += ", 'Complete', 'Incomplete') ";
            strSQL += "";
            strSQL += ", 'NO') Completed,";
            strSQL += "t2.DateCreated EventDate, ";
            strSQL += "t2.DataLocked, t2.DataFinal,";
            strSQL += "DATE_FORMAT(t2.DateCreated, '%d/%m%/%Y') Event_Date, ";
            strSQL += "t3.EventCode, t3.EventName, t3.PageLink, t3.PageIdentifier, t3.DateLink, t3.OrderSequence ";
            strSQL += "FROM trialdetails t1 LEFT JOIN kidneyproceduredata t2 ";
            strSQL += "ON t1.TrialID=t2.TrialID AND t2.Side='" + strSide + "'";
            strSQL += "LEFT JOIN eventcodes t3 ON t3.EventCode='" + strEventCode + "' AND t3.PageIdentifier LIKE '%" + strSide + "%' ";
            strSQL += "WHERE t1.TrialID=?TrialID ";



            //close the query
            strSQL += ") t1 ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            strSQL += ", OrderSequence ";

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

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
}