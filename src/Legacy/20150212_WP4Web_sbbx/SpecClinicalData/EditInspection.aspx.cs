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

public partial class SpecClinicalData_EditInspection : System.Web.UI.Page
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

                if (Request.QueryString["KidneyInspectionID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain the Unique Identifier.");
                }

                //cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                ddSide.DataSource = XMLKidneySidesDataSource;
                ddSide.DataBind();

                //remove Unknown if exists
                ListItem liDDSide = ddSide.Items.FindByValue(STR_UNKNOWN_SELECTION);
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

                ViewState["SortField"] = "Side";
                ViewState["SortDirection"] = "ASC";

                BindData();

                AssignData();

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

            lblDescription.Text = "Update  Kidney Inspection Data for " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;

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
                if (String.IsNullOrEmpty(Request.QueryString["KidneyInspectionID"]) == false)
                {
                    {
                        if (drv["KidneyInspectionID"].ToString() == Request.QueryString["KidneyInspectionID"].ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }

                
            }
        }
    }
     
    //Assign Data
    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor, t3.LeftRanCategory, t3.RightRanCategory FROM  kidneyinspection t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "LEFT JOIN kidneyr t3 ON t1.TrialID=t3.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID ";
            STRSQL += "AND KidneyInspectionID=?KidneyInspectionID ";
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
            MyCMD.Parameters.Add("?KidneyInspectionID", MySqlDbType.Int32).Value = Request.QueryString["KidneyInspectionID"];

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {

                            if (!DBNull.Value.Equals(myDr["Side"]))
                            {
                                ddSide.SelectedValue = myDr["Side"].ToString();

                                if (!DBNull.Value.Equals(myDr["LeftRanCategory"]))
                                {
                                    if (ddSide.SelectedValue == "Left")
                                    {
                                        rblPreservationModality.SelectedValue = myDr["LeftRanCategory"].ToString();
                                    }
                                }

                                if (!DBNull.Value.Equals(myDr["RightRanCategory"]))
                                {
                                    if (ddSide.SelectedValue == "Right")
                                    {
                                        rblPreservationModality.SelectedValue = myDr["RightRanCategory"].ToString();
                                    }
                                }
                                

                            }

                            if (!DBNull.Value.Equals(myDr["RandomisationComplete"]))
                            {
                                rblRandomisationComplete.SelectedValue = myDr["RandomisationComplete"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["NumberRenalArteries"]))
                            {
                                txtNumberRenalArteries.Text = myDr["NumberRenalArteries"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["ArterialProblems"]))
                            {
                                //lblCentre.Text += myDr["OtherOrgansDonated"].ToString();
                                string[] strSC_Sets = myDr["ArterialProblems"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    ListItem currentCheckBox = cblArterialProblems.Items.FindByValue(strSC_Contents[0].ToString());

                                    if (currentCheckBox != null)
                                    {
                                        if (strSC_Contents[1].ToString() == STR_YES_SELECTION)
                                        {
                                            currentCheckBox.Selected = true;
                                        }

                                    }

                                }
                            }

                            if (!DBNull.Value.Equals(myDr["WashoutPerfusion"]))
                            {
                                rblWashoutPerfusion.SelectedValue = myDr["WashoutPerfusion"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["RemovalDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["RemovalDate"].ToString()))
                                {
                                    txtRemovalDate.Text = Convert.ToDateTime(myDr["RemovalDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["RemovalTime"]))
                            {
                                if (myDr["RemovalTime"].ToString().Length >= 5)
                                {
                                    txtRemovalTime.Text = myDr["RemovalTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["VisiblePerfusionDefects"]))
                            {
                                rblVisiblePerfusionDefects.SelectedValue = myDr["VisiblePerfusionDefects"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["MachinePerfusionStartDate"]))
                            {

                                if (GeneralRoutines.IsDate(myDr["MachinePerfusionStartDate"].ToString()))
                                {
                                    txtMachinePerfusionStartDate.Text = Convert.ToDateTime(myDr["MachinePerfusionStartDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["MachinePerfusionStartTime"]))
                            {
                                if (myDr["MachinePerfusionStartTime"].ToString().Length >= 5)
                                {
                                    txtMachinePerfusionStartTime.Text = myDr["MachinePerfusionStartTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["RecipientTransplantationCentre"]))
                            {
                                txtRecipientTransplantationCentre.Text = myDr["RecipientTransplantationCentre"].ToString();
                            }

                            

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = myDr["Comments"].ToString();
                            }


                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while assigning data.";
            }

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
        }
    }
    // Reset Page
    //protected void cmdReset_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Response.Redirect(Request.Url.AbsoluteUri, false);
    //        //lblUserMessages.Text = "yoooo";
    //    }

    //    catch (System.Exception excep)
    //    {
    //        lblUserMessages.Text = excep.Message + " An error occured while resetting the page.";
    //    }
    //}
    
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID AND KidneyInspectionID=?KidneyInspectionID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?KidneyInspectionID", Request.QueryString["KidneyInspectionID"].ToString(), STRCONN));

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
                
                {
                    throw new Exception("More than one Record exists for deletion. Click on DonorID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM kidneyinspection ";
            STRSQL += "WHERE TrialID=?TrialID AND KidneyInspectionID=?KidneyInspectionID ";
            

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

            MyCMD.Parameters.Add("?KidneyInspectionID", MySqlDbType.VarChar).Value = Request.QueryString["KidneyInspectionID"].ToString();

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
    //protected void ddSide_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;

    //        if (ddSide.SelectedValue == "Left")
    //        {
    //            string STRSQL_FindLeftSide = string.Empty;
    //            STRSQL_FindLeftSide = "SELECT KidneyLeftDonated FROM donor_identification WHERE TrialID=?TrialID ";

    //            string strLeftSide = GeneralRoutines.ReturnScalar(STRSQL_FindLeftSide, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN);


    //            if (strLeftSide == "-1")
    //            {
    //                throw new Exception("Could not check if Left Kidney was donated.");
    //            }
    //            else
    //            {
    //                if (strLeftSide == STR_YES_SELECTION) //assign preservation modality
    //                {
    //                    //find preservation modality from randomised data
    //                    string STRSQL_FindPreservationModality = string.Empty;
    //                    STRSQL_FindPreservationModality = "SELECT LeftRancategory FROM kidneyr WHERE TrialID=?TrialID ";

    //                    string strPreservationModality = GeneralRoutines.ReturnScalar(STRSQL_FindPreservationModality, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN);

    //                    if (strPreservationModality != "-1")
    //                    {
    //                        rblPreservationModality.SelectedValue = strPreservationModality;
    //                        rblRandomisationComplete.SelectedValue = STR_YES_SELECTION;
    //                    }
    //                    else
    //                    {
    //                        throw new Exception("Could not check Randomised Category for selected Kidney '" + ddSide.SelectedValue + "'");
    //                    }
    //                }
    //            }

    //        }

    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while selecting an item from the Drop Down box.";
    //    }
    //}
    protected void cmdUpdate_Click(object sender, EventArgs e)
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

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID AND Side=?Side AND KidneyInspectionID=?KidneyInspectionID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarThree(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?Side", ddSide.SelectedValue, "?KidneyInspectionID", Request.QueryString["KidneyInspectionID"].ToString(), STRCONN));

            if (intCountFind != 1)
            {
                throw new Exception("No data available for '" + ddSide.SelectedValue + " Kidney' to Update. Please Check the data you are entering.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occcured when checking if data for '" + ddSide.SelectedValue + " Kidney' is available. ");
            }

            //check if same data exists with another identifier
            string STRSQL_FINDOTHER = "SELECT COUNT(*) CR FROM kidneyinspection WHERE TrialID=?TrialID AND Side=?Side AND KidneyInspectionID<>?KidneyInspectionID ";
            int intCountFindOther = Convert.ToInt16(GeneralRoutines.ReturnScalarThree(STRSQL_FINDOTHER, "?TrialID", Request.QueryString["TID"].ToString(), "?Side", ddSide.SelectedValue, "?KidneyInspectionID", Request.QueryString["KidneyInspectionID"].ToString(), STRCONN));

            if (intCountFindOther == 1)
            {
                throw new Exception("Another data exists for '" + ddSide.SelectedValue + " Kidney' to Update. Please Delete Other data before updating existing data.");
            }

            if (intCountFindOther <0)
            {
                throw new Exception("An error occcured when checking if another data for '" + ddSide.SelectedValue + " Kidney' is available. .");
            }

            //now add data

           
            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE kidneyinspection SET ";
            STRSQL_UPDATE += "Side=?Side, PreservationModality=?PreservationModality, RandomisationComplete=?RandomisationComplete, NumberRenalArteries=?NumberRenalArteries, ";
            STRSQL_UPDATE += "ArterialProblems=?ArterialProblems, WashoutPerfusion=?WashoutPerfusion,RemovalDate=?RemovalDate, RemovalTime=?RemovalTime, ";
            STRSQL_UPDATE += "VisiblePerfusionDefects=?VisiblePerfusionDefects,";
            STRSQL_UPDATE += "MachinePerfusionStartDate=?MachinePerfusionStartDate, ";
            STRSQL_UPDATE += "MachinePerfusionStartTime=?MachinePerfusionStartTime,RecipientTransplantationCentre=?RecipientTransplantationCentre,";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID AND KidneyInspectionID=?KidneyInspectionID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL_UPDATE;


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();
            MyCMD.Parameters.Add("?KidneyInspectionID", MySqlDbType.VarChar).Value = Request.QueryString["KidneyInspectionID"].ToString();

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



            if (txtComments.Text == string.Empty)
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
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }
}