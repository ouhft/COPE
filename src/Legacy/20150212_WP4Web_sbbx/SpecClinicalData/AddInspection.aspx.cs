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

public partial class SpecClinicalData_AddInspection : System.Web.UI.Page
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

    //static Random _random = new Random();
    
    #endregion

    protected void Page_LoadComplete(object sender, EventArgs e)
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

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                //cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                ddSide.DataSource = XMLKidneySidesDataSource;
                ddSide.DataBind();

                //remove Unknown if exists
                ListItem liDDSide = ddSide.Items.FindByValue( STR_UNKNOWN_SELECTION);
                if (liDDSide != null)
                {
                    ddSide.Items.Remove(liDDSide);
                }

                rblPreservationModality.DataSource = XMLPreservationModalitiesDataSource;
                rblPreservationModality.DataBind();
                rblPreservationModality.SelectedValue = STR_UNKNOWN_SELECTION;

                rblRandomisationComplete.DataSource = XMLMainOptionsDataSource;
                rblRandomisationComplete.DataBind();
                rblRandomisationComplete.SelectedValue = STR_UNKNOWN_SELECTION;

                rblWashoutPerfusion.DataSource = XMLWashoutPerfusionDataSource;
                rblWashoutPerfusion.DataBind();
                rblWashoutPerfusion.SelectedValue = STR_UNKNOWN_SELECTION;

                rblVisiblePerfusionDefects.DataSource = XMLMainOptionsDataSource;
                rblVisiblePerfusionDefects.DataBind();
                rblVisiblePerfusionDefects.SelectedValue = STR_UNKNOWN_SELECTION;

                cblArterialProblems.DataSource = XMLArterialProblemsDataSource;
                cblArterialProblems.DataBind();

                
                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

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

            strSQL += "SELECT t1.*  ";
            //strSQL += "DATE_FORMAT(t1.DateBaggedColdStorage, '%d/%m/%Y') Date_BaggedColdStorage, ";
            //strSQL += "TIME_FORMAT(t1.TimeBaggedColdStorage, '%H:%i') Time_BaggedColdStorage, ";
            //strSQL += "DATE_FORMAT(t1.DateArrivalTransplantCentre, '%d/%m/%Y') Date_ArrivalTransplantCentre, ";
            //strSQL += "TIME_FORMAT(t1.TimeArrivalTransplantCentre, '%H:%i') Time_ArrivalTransplantCentre ";
            strSQL += "FROM kidneyinspection t1 ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on TrialID to Edit Kidney Inspection Data.";

            lblDescription.Text = "Add  Kidney Inspection Data for " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;

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

    // Reset Page
    //protected void AssignData()
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;

    //        string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  kidneyinspection t1 ";
    //        STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
    //        STRSQL += "WHERE t1.TrialID=?TrialID ";
    //        string CS = string.Empty;
    //        CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

    //        MySqlConnection MyCONN = new MySqlConnection();
    //        MyCONN.ConnectionString = CS;

    //        MySqlCommand MyCMD = new MySqlCommand();
    //        MyCMD.Connection = MyCONN;

    //        MyCMD.CommandType = CommandType.Text;
    //        MyCMD.CommandText = STRSQL;

    //        MyCMD.Parameters.Clear();

    //        MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];

    //        MyCONN.Open();

    //        try
    //        {
    //            using (MySqlDataReader myDr = MyCMD.ExecuteReader())
    //            {
    //                if (myDr.HasRows)
    //                {
    //                    while (myDr.Read())
    //                    {

    //                        if (!DBNull.Value.Equals(myDr["Side"]))
    //                        {
    //                            ddSide.SelectedValue = myDr["Side"].ToString();
    //                        }

    //                        //if (!DBNull.Value.Equals(myDr["AnatomyOther"]))
    //                        //{
    //                        //    txtAnatomyOther.Text = myDr["AnatomyOther"].ToString();
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["DateBaggedColdStorage"]))
    //                        //{
    //                        //    if (GeneralRoutines.IsDate(myDr["DateBaggedColdStorage"].ToString()))
    //                        //    {
    //                        //        txtDateBaggedColdStorage.Text = Convert.ToDateTime(myDr["DateBaggedColdStorage"]).ToString("dd/MM/yyyy");
    //                        //    }
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["TimeBaggedColdStorage"]))
    //                        //{
    //                        //    if (myDr["TimeBaggedColdStorage"].ToString().Length >= 5)
    //                        //    {
    //                        //        txtTimeBaggedColdStorage.Text = myDr["TimeBaggedColdStorage"].ToString().Substring(0, 5);
    //                        //    }
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["DateArrivalTransplantCentre"]))
    //                        //{

    //                        //    if (GeneralRoutines.IsDate(myDr["DateArrivalTransplantCentre"].ToString()))
    //                        //    {
    //                        //        txtDateArrivalTransplantCentre.Text = Convert.ToDateTime(myDr["DateArrivalTransplantCentre"]).ToString("dd/MM/yyyy");
    //                        //    }
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["TimeArrivalTransplantCentre"]))
    //                        //{
    //                        //    if (myDr["TimeArrivalTransplantCentre"].ToString().Length >= 5)
    //                        //    {
    //                        //        txtTimeArrivalTransplantCentre.Text = myDr["TimeArrivalTransplantCentre"].ToString().Substring(0, 5);
    //                        //    }
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["ColdStorageSolution"]))
    //                        //{
    //                        //    ddColdStorageSolution.SelectedValue = myDr["ColdStorageSolution"].ToString();
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["KidneySuitableTransplant"]))
    //                        //{
    //                        //    rblKidneySuitableTransplant.SelectedValue = myDr["KidneySuitableTransplant"].ToString();
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["KidneySuitableTransplantNo"]))
    //                        //{
    //                        //    txtKidneySuitableTransplantNo.Text = myDr["KidneySuitableTransplantNo"].ToString();
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["ArterialProblems"]))
    //                        //{
    //                        //    string[] items = myDr["ArterialProblems"].ToString().Split(',');

    //                        //    for (int i = 0; i <= items.GetUpperBound(0); i++)
    //                        //    {
    //                        //        ListItem currentCheckBox = cblArterialProblems.Items.FindByValue(items[i].ToString());
    //                        //        if (currentCheckBox != null)
    //                        //        {
    //                        //            currentCheckBox.Selected = true;
    //                        //        }
    //                        //    }
    //                        //}

    //                        //if (!DBNull.Value.Equals(myDr["ArterialProblemsRemarks"]))
    //                        //{
    //                        //    txtArterialProblemsRemarks.Text = myDr["ArterialProblemsRemarks"].ToString();
    //                        //}

    //                        if (!DBNull.Value.Equals(myDr["Comments"]))
    //                        {
    //                            txtComments.Text = myDr["Comments"].ToString();
    //                        }


    //                    }
    //                }
    //            }
    //        }
    //        catch (System.Exception ex)
    //        {
    //            lblUserMessages.Text = ex.Message + " An error occured while assigning data.";
    //        }

    //    }

    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
    //    }
    //}
    // Reset Page
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

            if (ddSide.SelectedValue == "0")
            { 
                throw new Exception("Please Select an option for Anatomy."); 
            }

            if (rblRandomisationComplete.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select if 'Randomisation' has been completed.");
            }

            if (GeneralRoutines.IsNumeric(txtNumberRenalArteries.Text) == false)
            {
                throw new Exception("Please Enter 'Number of Renal Arteries' in numeric format.");
            }

            //if (rblDamage.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select an option for Damage."); 
            //}

            if (GeneralRoutines.IsDate(txtRemovalDate.Text) == false)
            {
                throw new Exception("Please enter 'Kidney Removal' Date.");
            }
            else
            {
                if (Convert.ToDateTime(txtRemovalDate.Text) > DateTime.Now)
                {
                    throw new Exception("Date when 'Kidney Removed' cannot be greater than Today's date.");
                }
            }

            if (txtRemovalTime.Text == string.Empty)
            {
                throw new Exception("Please enter the Time when 'Kidney was Removed'. ");
            }
            else
            {
                if (GeneralRoutines.IsNumeric(txtRemovalTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("Time Hour when 'Kidney was Removed' should be numeric.");
                }

                if (Convert.ToInt16(txtRemovalTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("Time Hour when 'Kidney was Removed' should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtRemovalTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("Time Minute when 'Kidney was Removed' should be numeric.");
                }

                if (Convert.ToInt16(txtRemovalTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("Time Minute when 'Kidney was Removed' should not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtMachinePerfusionStartDate.Text) == false)
            {
                throw new Exception("Please enter 'Machine Perfusion Start' Date.");
            }
            else
            {
                if (Convert.ToDateTime(txtMachinePerfusionStartDate.Text) > DateTime.Now)
                {
                    throw new Exception("'Machine Perfusion Start' Date cannot be greater than Today's date.");
                }
            }

            if (txtMachinePerfusionStartTime.Text == string.Empty)
            {
                throw new Exception("Please enter the 'Machine Perfusion Start' Time. ");
            }
            else
            {
                if (GeneralRoutines.IsNumeric(txtMachinePerfusionStartTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'Machine Perfusion Start' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtMachinePerfusionStartTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'Machine Perfusion Start' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtMachinePerfusionStartTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'Machine Perfusion Start' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtMachinePerfusionStartTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'Machine Perfusion Start' Time Minute should not be greater than 59.");
                }
            }

            if (rblVisiblePerfusionDefects.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select if there are any 'Visible Perfusion Defects'.");
            }

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID AND Side=?Side";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?Side", ddSide.SelectedValue, STRCONN));

            if (intCountFind >= 1)
            {
                throw new Exception("Data already added for '" + ddSide.SelectedValue + " Kidney'. Please Select existing data to Edit by clicking on the link below.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occcured when checking if data already added for '" + ddSide.SelectedValue + " Kidney'. ");
            }

            //now add data

            string STRSQL = string.Empty;
            STRSQL += "INSERT INTO kidneyinspection ";
            STRSQL += "(TrialID, Side, PreservationModality, RandomisationComplete, NumberRenalArteries, ArterialProblems, ";
            STRSQL += "WashoutPerfusion, RemovalDate, RemovalTime, VisiblePerfusionDefects, ";
            STRSQL += "MachinePerfusionStartDate,MachinePerfusionStartTime, RecipientTransplantationCentre,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID,  ?Side, ?PreservationModality, ?RandomisationComplete, ?NumberRenalArteries, ?ArterialProblems, ";
            STRSQL += "?WashoutPerfusion, ?RemovalDate, ?RemovalTime, ?VisiblePerfusionDefects, ";
            STRSQL += "?MachinePerfusionStartDate, ?MachinePerfusionStartTime, ?RecipientTransplantationCentre,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";
            STRSQL += " ";


            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE kidneyinspection SET ";
            STRSQL_UPDATE += "Side=?Side, PreservationModality=?PreservationModality, RandomisationComplete=?RandomisationComplete, NumberRenalArteries=?NumberRenalArteries, ";
            STRSQL_UPDATE += "ArterialProblems=?ArterialProblems, WashoutPerfusion=?WashoutPerfusion,RemovalDate=?RemovalDate, RemovalTime=?RemovalTime, ";
            STRSQL_UPDATE += "VisiblePerfusionDefects=?VisiblePerfusionDefects,";
            STRSQL_UPDATE += "MachinePerfusionStartDate=?MachinePerfusionStartDate, ";
            STRSQL_UPDATE += "MachinePerfusionStartTime=?MachinePerfusionStartTime,RecipientTransplantationCentre=?RecipientTransplantationCentre,";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";

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

            if (ddSide.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;
            }

            if (rblPreservationModality.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?PreservationModality", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PreservationModality", MySqlDbType.VarChar).Value = rblPreservationModality.SelectedValue;
            }

            if (rblRandomisationComplete.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RandomisationComplete", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RandomisationComplete", MySqlDbType.VarChar).Value = rblRandomisationComplete.SelectedValue;
            }

            if (txtNumberRenalArteries.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NumberRenalArteries", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NumberRenalArteries", MySqlDbType.VarChar).Value = txtNumberRenalArteries.Text;
            }

            //append selection
            string strArterialProblems = string.Empty;
            //Set up connection and command objects
            //Open connection
            for (int i = 0; i < cblArterialProblems.Items.Count; i++)
            {
                strArterialProblems += cblArterialProblems.Items[i].Value + ":";
                if (cblArterialProblems.Items[i].Selected)
                {
                    strArterialProblems += STR_YES_SELECTION;
                }
                else
                {
                    strArterialProblems += STR_NO_SELECTION;
                }

                if (i < cblArterialProblems.Items.Count - 1)
                {
                    strArterialProblems += ",";
                }

            }

            if (strArterialProblems == string.Empty)
            {
                MyCMD.Parameters.Add("?ArterialProblems", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArterialProblems", MySqlDbType.VarChar).Value = strArterialProblems;
            }

            if (rblWashoutPerfusion.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?WashoutPerfusion", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?WashoutPerfusion", MySqlDbType.VarChar).Value = rblWashoutPerfusion.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtRemovalDate.Text) == false)
            {
                MyCMD.Parameters.Add("?RemovalDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RemovalDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtRemovalDate.Text);
            }

            if (txtRemovalTime.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RemovalTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RemovalTime", MySqlDbType.VarChar).Value = txtRemovalTime.Text;
            }

            if (rblVisiblePerfusionDefects.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?VisiblePerfusionDefects", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?VisiblePerfusionDefects", MySqlDbType.VarChar).Value = rblVisiblePerfusionDefects.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtMachinePerfusionStartDate.Text) == false)
            {
                MyCMD.Parameters.Add("?MachinePerfusionStartDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MachinePerfusionStartDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtMachinePerfusionStartDate.Text);
            }

            if (txtMachinePerfusionStartTime.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?MachinePerfusionStartTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MachinePerfusionStartTime", MySqlDbType.VarChar).Value = txtMachinePerfusionStartTime.Text;
            }



            if (txtRecipientTransplantationCentre.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RecipientTransplantationCentre", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientTransplantationCentre", MySqlDbType.VarChar).Value = txtRecipientTransplantationCentre.Text;
            }

                     

            if (txtComments.Text==string.Empty)
            { 
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text; 
            }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
            }

            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing add/update query.";
            }

            finally //close connection
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }

            BindData();


        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text=ex.Message + " An error occured while adding data.";
        }
    }
    
    protected void ddSide_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {
            lblUserMessages.Text = string.Empty;

            if (ddSide.SelectedValue == "Left")
            {
                string STRSQL_FindLeftSide = string.Empty;
                STRSQL_FindLeftSide = "SELECT KidneyLeftDonated FROM donor_identification WHERE TrialID=?TrialID ";

                string strLeftSide = GeneralRoutines.ReturnScalar(STRSQL_FindLeftSide, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN);
                                

                if (strLeftSide == "-1")
                {
                    throw new Exception("Could not check if Left Kidney was donated.");
                }
                else
                {
                    if (strLeftSide == STR_YES_SELECTION) //assign preservation modality
                    {
                        //find preservation modality from randomised data
                        string STRSQL_FindPreservationModality = string.Empty;
                        STRSQL_FindPreservationModality = "SELECT LeftRancategory FROM kidneyr WHERE TrialID=?TrialID ";

                        string strPreservationModality = GeneralRoutines.ReturnScalar(STRSQL_FindPreservationModality, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN);
                                                
                        if (strPreservationModality != "-1")
                        {
                            rblPreservationModality.SelectedValue = strPreservationModality;
                            rblRandomisationComplete.SelectedValue = STR_YES_SELECTION;
                        }
                        else
                        {
                            throw new Exception("Could not check Randomised Category for selected Kidney '" + ddSide.SelectedValue + "'");
                        }
                    }
                }
 
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting an item from the Drop Down box.";
        }
    }
}