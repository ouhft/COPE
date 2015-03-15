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

using AjaxControlToolkit;



public partial class SpecClinicalData_AddDonorSpecimen : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

        private const string strExcludeOccasion = "1-7 Days";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

        private const string strNestedCPH = "SpecimenContents";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const Int16 intWorksheetBarcodeLength = 10;

        private const Int16 intBarcodeFirstFourDigits = 1721;

        private const string strPageVersion = "0"; //default page version


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

                lblDescription.Text = "Enter Specimen Data.";
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";

                if (SessionVariablesAll.CentreCode.Substring(0,1)=="1")
                {
                    pnlQuodConsent.Visible = true;
                }
                else
                {
                    pnlQuodConsent.Visible = false;
                }

                txtSamplesStoredOxfordDate_CalendarExtender.EndDate = DateTime.Today;

                lblDB1_1.Text = "DB1 (Before withdrawal of support)";
                ddDB1_1.DataSource = XMLSpecimenTypesDataSource;
                ddDB1_1.DataBind();
                ddDB1_1.SelectedValue = "EDTA";
                ddDB1_1.Enabled = false;

                txtDateCollectedDB1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedDB1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                txtDateCentrifugationDB1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationDB1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                ddDB1_2.DataSource = XMLSpecimenTypesDataSource;
                ddDB1_2.DataBind();
                ddDB1_2.SelectedValue = "SST";
                ddDB1_2.Enabled = false;

                lblDU1_1.Text = "DU1 (Before withdrawal of support)";
                ddDU1_1.DataSource = XMLSpecimenTypesDataSource;
                ddDU1_1.DataBind();
                ddDU1_1.SelectedValue = "Urine";
                ddDU1_1.Enabled = false;

                txtDateCollectedDU1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedDU1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                txtDateCentrifugationDU1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationDU1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                lblDU2_1.Text = "DU2 (Last Urine Collected)";
                ddDU2_1.DataSource = XMLSpecimenTypesDataSource;
                ddDU2_1.DataBind();
                ddDU2_1.SelectedValue = "Urine";
                ddDU2_1.Enabled = false;

                txtDateCollectedDU2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedDU2_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                txtDateCentrifugationDU2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationDU2_1_CalendarExtender.PopupPosition = CalendarPosition.Right;


                lblRKP1_1.Text = "RK P1 (Right Kidney: 15 min after HMP Start)";
                ddRKP1_1.DataSource = XMLSpecimenTypesDataSource;
                ddRKP1_1.DataBind();
                ddRKP1_1.SelectedValue = "Perfusate";
                ddRKP1_1.Enabled = false;

                txtDateCollectedRKP1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedRKP1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                txtDateCentrifugationRKP1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationRKP1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                lblLKP1_1.Text = "LK P1 (Left Kidney: 15 min after HMP Start)";
                ddLKP1_1.DataSource = XMLSpecimenTypesDataSource;
                ddLKP1_1.DataBind();
                ddLKP1_1.SelectedValue = "Perfusate";
                ddLKP1_1.Enabled = false;

                txtDateCollectedLKP1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedLKP1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                txtDateCentrifugationLKP1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationLKP1_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                lblRKP2_1.Text = "RK P2 (Right Kidney: Leaving Hospital)";
                ddRKP2_1.DataSource = XMLSpecimenTypesDataSource;
                ddRKP2_1.DataBind();
                ddRKP2_1.SelectedValue = "Perfusate";
                ddRKP2_1.Enabled = false;

                txtDateCollectedRKP2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedRKP2_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                txtDateCentrifugationRKP2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationRKP2_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                lblLKP2_1.Text = "LK P2 (Left Kidney: Leaving Hospital)";
                ddLKP2_1.DataSource = XMLSpecimenTypesDataSource;
                ddLKP2_1.DataBind();
                ddLKP2_1.SelectedValue = "Perfusate";
                ddLKP2_1.Enabled = false;

                txtDateCollectedLKP2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedLKP2_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                txtDateCentrifugationLKP2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationLKP2_1_CalendarExtender.PopupPosition = CalendarPosition.Right;

                ViewState["SortField"] = "SpecimenID";
                ViewState["SortDirection"] = "ASC";

                BindData();

                AssignData();

                AssignConsentData();


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
                        //cmdDelete.Enabled = true;
                        cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        cmdAddData.Enabled = false;
                        //cmdDelete.Enabled = false;
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

            strSQL += "SELECT t1.*, t2.DonorID MainDonorID,  ";
            strSQL += "DATE_FORMAT(t1.DateCollected, '%d/%m/%Y') Date_Collected,  ";
            strSQL += "DATE_FORMAT(t1.TimeCollected, '%H:%i') Time_Collected,  ";
            strSQL += "DATE_FORMAT(t2.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            strSQL += "FROM specimen t1 ";
            strSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID AND t1.TrialIDRecipient IS NULL ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = sqldsGV1;
            sqldsGV1.SelectCommand = strSQL;
            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on TrialID Edit/Delete Specimen Data. Number of Records " + GV1.Rows.Count.ToString();

            lblDescription.Text = "Add  Donor Specimen Data for " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;


        }
        catch (Exception excep)
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
                if (String.IsNullOrEmpty(Request.QueryString["SpecimenID"]) == false)
                {
                    if (drv["SpecimenID"].ToString() == Request.QueryString["SpecimenID"].ToString())
                    {
                        e.Row.BackColor = System.Drawing.Color.LightBlue;
                    }                    
                }
                ConfirmButtonExtender cmdDeleteRow_ConfirmButtonExtenderGV1 = e.Row.FindControl("cmdDeleteRow_ConfirmButtonExtender") as ConfirmButtonExtender;

                if (cmdDeleteRow_ConfirmButtonExtenderGV1 != null)
                {
                    cmdDeleteRow_ConfirmButtonExtenderGV1.ConfirmText = "If you click 'OK' the selected data will be deleted. Click 'CANCEL' if you do not wish to delete selected data.";
                }
            }
        }
    }
    protected void cmdDeleteRow_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            Button cmdDelRow = sender as Button;

            if (cmdDelRow == null)
            {
                throw new Exception("Could not obtain the Delete button.");
            }

            GridViewRow rowGRV;
            rowGRV = cmdDelRow.Parent.Parent as GridViewRow;

            Label lblSpecimenID_GV1 = rowGRV.FindControl("lblSpecimenID") as Label;

            if (lblSpecimenID_GV1.Text == null)
            {
                throw new Exception("Could not obtain the Label.");
            }

            if (string.IsNullOrEmpty(lblSpecimenID_GV1.Text))
            {
                throw new Exception("Could not obtain the Unique Identifier.");
            }
            //lblUserMessages.Text = lblSpecimenID_GV1.Text;

            string STRSQL = String.Empty;

            STRSQL += "DELETE FROM specimen WHERE SpecimenID=?SpecimenID AND TrialID=?TrialID  ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?SpecimenID", MySqlDbType.VarChar).Value = lblSpecimenID_GV1.Text;
            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();
            
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
            lblUserMessages.Text = ex.Message + " An error occured while clicking on Delete button.";
        }

    }

    protected void AssignConsentData()
    {
        try
        {
            string STRSQL = string.Empty;
            STRSQL += "SELECT t1.* ";
            STRSQL += "FROM specimenmaindetails t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID AND t1.TrialIDRecipient IS NULL ";

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

            MyCONN.Open();
            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {

                            if (!DBNull.Value.Equals(myDr["WorksheetBarcode"]))
                            {
                                txtWorksheetBarcode.Text = myDr["WorksheetBarcode"].ToString();


                            }


                            if (!DBNull.Value.Equals(myDr["ConsentQuod"]))
                            {
                                if (myDr["ConsentQuod"].ToString() == STR_YES_SELECTION)
                                {
                                    chkConsentQuod.Checked = true;
                                }


                            }

                            if (!DBNull.Value.Equals(myDr["ConsentAdditionalSamples"]))
                            {
                                if (myDr["ConsentAdditionalSamples"].ToString() == STR_YES_SELECTION)
                                {
                                    chkConsentAdditionalSamples.Checked = true;
                                }


                            }

                            if (!DBNull.Value.Equals(myDr["ResearcherName"]))
                            {
                                txtResearcherName.Text = myDr["ResearcherName"].ToString();


                            }

                            if (!DBNull.Value.Equals(myDr["ResearcherTelephoneNumber"]))
                            {
                                txtResearcherTelephoneNumber.Text = myDr["ResearcherTelephoneNumber"].ToString();

                            }


                            if (!DBNull.Value.Equals(myDr["SamplesStoredOxfordDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["SamplesStoredOxfordDate"].ToString()) == true)
                                {
                                    txtSamplesStoredOxfordDate.Text = Convert.ToDateTime(myDr["SamplesStoredOxfordDate"]).ToShortDateString();
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["SamplesStoredOxfordTime"]))
                            {
                                if (myDr["SamplesStoredOxfordTime"].ToString().Length >= 5)
                                {
                                    txtSamplesStoredOxfordTime.Text = myDr["SamplesStoredOxfordTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtConsentComments.Text = myDr["Comments"].ToString();


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

                            //chkConsentQuod_CheckedChanged(this, EventArgs.Empty);
                            chkConsentAdditionalSamples_CheckedChanged(this, EventArgs.Empty);


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
                lblUserMessages.Text = ex.Message + " An error occured while assigning executing query.";
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Assigning Main Data.";
        }
    }

    protected void AssignData()
    {
        try
        {

            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
            ContentPlaceHolder mpCPHNested = (ContentPlaceHolder)(mpCPH.FindControl(strNestedCPH));

            string STRSQL = string.Empty;

            STRSQL += "SELECT * FROM specimen WHERE TrialID=?TrialID AND TrialIDRecipient IS NULL ";

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

            MyCONN.Open();
            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        Label lblSpecimenID;
                        CheckBox chkCollected;
                        //LT1 allow dry ice
                        //DropDownList ddSampleType;
                        TextBox txtBarcode;
                        TextBox txtDateCollected;
                        TextBox txtTimeCollected;
                        TextBox txtDateCentrifugation;
                        TextBox txtTimeCentrifugation;
                        TextBox txtComments;
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["Occasion"]))
                            {
                                //txtComments.Text = (string)(myDr["Occasion"]);
                                chkCollected = (CheckBox)mpCPHNested.FindControl("chk" + myDr["Occasion"].ToString());
                                if (chkCollected != null)
                                {
                                    if (!DBNull.Value.Equals(myDr["Collected"]))
                                    {
                                        if (myDr["Collected"].ToString() == "1")
                                        {
                                            chkCollected.Checked = true;
                                        }
                                        else
                                        {
                                            chkCollected.Checked = false;
                                        }
                                    }

                                }


                                if (!DBNull.Value.Equals(myDr["SpecimenID"]))
                                {
                                    lblSpecimenID = (Label)mpCPHNested.FindControl("lblSpecimenID" + myDr["Occasion"].ToString());
                                    if (lblSpecimenID != null)
                                    {
                                        lblSpecimenID.Visible = false;
                                        lblSpecimenID.Text = myDr["SpecimenID"].ToString();
                                    }

                                    //lblUserMessages.Text += lblSpecimenID.ID + ". ";
                                }
                                if (!DBNull.Value.Equals(myDr["Barcode"]))
                                {
                                    txtBarcode = (TextBox)mpCPHNested.FindControl("txtBarcode" + myDr["Occasion"].ToString());
                                    if (txtBarcode != null)
                                    {
                                        txtBarcode.Text = myDr["Barcode"].ToString();
                                    }
                                }

                                ////LT1 allow dry ice
                                //if (!DBNull.Value.Equals(myDr["State"]))
                                //{
                                //    ddSampleType = (DropDownList)(mpCPHNested.FindControl("dd" + myDr["Occasion"].ToString()));
                                //    if (ddSampleType != null)
                                //    {
                                //        ddSampleType.SelectedValue = myDr["State"].ToString();
                                //    }

                                //}

                                if (!DBNull.Value.Equals(myDr["DateCollected"]))
                                {
                                    if (GeneralRoutines.IsDate(myDr["DateCollected"].ToString()) == true)
                                    {
                                        switch (myDr["Occasion"].ToString())
                                        {

                                            case "DB1_1":
                                            case "DB1_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtDateCollectedDB1_1.Text = Convert.ToDateTime(myDr["DateCollected"]).ToShortDateString();
                                                break;

                                            
                                            default:
                                                txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtDateCollected.Text = Convert.ToDateTime(myDr["DateCollected"]).ToShortDateString();
                                                break;

                                        }

                                    }
                                }
                                if (!DBNull.Value.Equals(myDr["TimeCollected"]))
                                {
                                    if (myDr["TimeCollected"].ToString().Length >= 5)
                                    {
                                        switch (myDr["Occasion"].ToString())
                                        {

                                            case "DB1_1":
                                            case "DB1_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtTimeCollectedDB1_1.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
                                                break;

                                            
                                            default:
                                                txtTimeCollected = (TextBox)mpCPHNested.FindControl("txtTimeCollected" + myDr["Occasion"].ToString());
                                                txtTimeCollected.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
                                                break;

                                        }

                                    }
                                }
                                if (!DBNull.Value.Equals(myDr["DateCentrifugation"]))
                                {
                                    if (GeneralRoutines.IsDate(myDr["DateCentrifugation"].ToString()) == true)
                                    {
                                        switch (myDr["Occasion"].ToString())
                                        {

                                            case "DB1_1":
                                            case "DB1_2":
                                                //txtDateCentrifugation = (TextBox)mpCPHNested.FindControl("txtDateCentrifugation" + myDr["Occasion"].ToString());
                                                txtDateCentrifugationDB1_1.Text = Convert.ToDateTime(myDr["DateCentrifugation"]).ToShortDateString();
                                                break;

                                            

                                            default:
                                                txtDateCentrifugation = (TextBox)mpCPHNested.FindControl("txtDateCentrifugation" + myDr["Occasion"].ToString());
                                                txtDateCentrifugation.Text = Convert.ToDateTime(myDr["DateCentrifugation"]).ToShortDateString();
                                                break;

                                        }

                                    }
                                }
                                if (!DBNull.Value.Equals(myDr["TimeCentrifugation"]))
                                {
                                    if (myDr["TimeCentrifugation"].ToString().Length >= 5)
                                    {
                                        switch (myDr["Occasion"].ToString())
                                        {

                                            case "DB1_1":
                                            case "DB1_2":
                                                //txtDateCentrifugation = (TextBox)mpCPHNested.FindControl("txtDateCentrifugation" + myDr["Occasion"].ToString());
                                                txtTimeCentrifugationDB1_1.Text = myDr["TimeCentrifugation"].ToString().Substring(0, 5);
                                                break;
                                            

                                            default:
                                                txtTimeCentrifugation = (TextBox)mpCPHNested.FindControl("txtTimeCentrifugation" + myDr["Occasion"].ToString());
                                                txtTimeCentrifugation.Text = myDr["TimeCentrifugation"].ToString().Substring(0, 5);
                                                break;

                                        }

                                    }
                                }
                                if (!DBNull.Value.Equals(myDr["Comments"]))
                                {
                                    txtComments = (TextBox)mpCPHNested.FindControl("txtComments" + myDr["Occasion"].ToString());
                                    if (txtComments != null)
                                    {
                                        txtComments.Text = myDr["Comments"].ToString();
                                    }
                                }


                            }
                            cmdAddData.Text = "Add/Update Specimen Data";
                            lblDescription.Text = "Add/Update Specimen Data";
                            //if (!DBNull.Value.Equals(myDr["SpecimenID"]))
                            //{
                            //    lblSpecimenID = (Label)mpCPHNested.FindControl("lblSpecimenID" + myDr["Occasion"].ToString());
                            //    if (lblSpecimenID != null)
                            //    {
                            //        lblSpecimenID.Text = myDr["SpecimenID"].ToString();
                            //    }

                            //    //lblUserMessages.Text += lblSpecimenID.ID + ". ";
                            //}
                            //if (!DBNull.Value.Equals(myDr["Barcode"]))
                            //{
                            //    txtBarcode = (TextBox)mpCPHNested.FindControl("txtBarcode" + myDr["Occasion"].ToString());
                            //    if (txtBarcode != null)
                            //    {
                            //        txtBarcode.Text = myDr["Barcode"].ToString();
                            //    }
                            //}
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
            lblUserMessages.Text = ex.Message + " An error occured while Assigning Data.";
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

    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //if (txtWorksheetBarcode.Text == string.Empty)
            //{
            //    throw new Exception("Please Enter Worksheet Barcode.");
            //}
            //else
            //{
            //    if (txtWorksheetBarcode.Text.Length != intWorksheetBarcodeLength )
            //    {
            //        throw new Exception("The length of Worksheet Barcode should be " + intWorksheetBarcodeLength.ToString() + ".");
            //    }
            //    if (txtWorksheetBarcode.Text.Substring(0,4) != intBarcodeFirstFourDigits.ToString())
            //    {
            //        throw new Exception("The first four characters of Worksheet Barcode should be " + intBarcodeFirstFourDigits.ToString() + ". Please check the Worksheet barcode you have entered.");
            //    }
            //    if (GeneralRoutines.IsNumeric(txtWorksheetBarcode.Text) == false)
            //    {
            //        throw new Exception("The 'Worksheet Barcode' should be numeric.");
            //    }
            //}

            if (txtSamplesStoredOxfordDate.Text!=string.Empty)
            {
                if (GeneralRoutines.IsDate(txtSamplesStoredOxfordDate.Text)==false)
                {
                    throw new Exception("Please Enter Date for " + lblSamplesStoredOxford.Text + ". ");
                }

                if (Convert.ToDateTime(txtSamplesStoredOxfordDate.Text).Date > DateTime.Today.Date)
                {
                    throw new Exception("Date for " + lblSamplesStoredOxford.Text  + " cannot be later than Today's date");
                }
            }

            if (txtSamplesStoredOxfordTime.Text!=string.Empty && txtSamplesStoredOxfordTime.Text!="__:__")
            {
                if (GeneralRoutines.IsDate(txtSamplesStoredOxfordDate.Text) == false)
                {
                    throw new Exception("Please Enter Date for " + lblSamplesStoredOxford.Text + " in correct format. ");
                }

                if (GeneralRoutines.IsDate(txtSamplesStoredOxfordDate.Text + " " + txtSamplesStoredOxfordTime.Text) == false)
                {
                    throw new Exception("Please Enter Time for " + lblSamplesStoredOxford.Text + " in correct format. ");
                }

                if (Convert.ToDateTime(txtSamplesStoredOxfordDate.Text + " " + txtSamplesStoredOxfordTime.Text).AddHours(1) > DateTime.Today)
                {
                    throw new Exception("Time for " + lblSamplesStoredOxford.Text + " cannot be later than Today's date");
                }


            }
            
            SpecimenStructure specstrucSpecimen ;


            ArrayList arlOccasions = new ArrayList();
            ArrayList arlBarcodes= new ArrayList();
            ArrayList arlSpecimens = new ArrayList();

            arlOccasions.Add("DU1_1");
            arlOccasions.Add("DB1_1");
            arlOccasions.Add("DB1_2");
            arlOccasions.Add("DU2_1");
            arlOccasions.Add("RKP1_1");
            arlOccasions.Add("LKP1_1");
            arlOccasions.Add("RKP2_1");
            arlOccasions.Add("LKP2_1");

            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
            ContentPlaceHolder mpNestedCPH = (ContentPlaceHolder)(mpCPH.FindControl(strNestedCPH));


            Label lblSpecimenID;
            CheckBox chkCollected;
            DropDownList ddSampleType;
            TextBox txtBarcode;
            TextBox txtDateCollected;
            TextBox txtTimeCollected;
            TextBox txtDateCentrifugation;
            TextBox txtTimeCentrifugation;
            TextBox txtComments;

            foreach (string strControlName in arlOccasions)
            {
                //lblUserMessages.Text += value + ". ";
                chkCollected = (CheckBox)(mpNestedCPH.FindControl("chk" + strControlName));

                specstrucSpecimen = new SpecimenStructure();

                specstrucSpecimen.Occasion = strControlName;

                //get the unique identifier
                lblSpecimenID = (Label)(mpNestedCPH.FindControl("lblSpecimenID" + strControlName));

                specstrucSpecimen.SqlCode = 4;//0 insert, 1 Update, 2 Delete, 4 do nothing

                if (lblSpecimenID.Text != string.Empty)
                {
                    if (GeneralRoutines.IsNumeric(lblSpecimenID.Text) == true)
                    {
                        specstrucSpecimen.UniqueID = Convert.ToInt64(lblSpecimenID.Text);                        

                        specstrucSpecimen.SqlCode = 1;//update
                    }
                    else
                    {
                        specstrucSpecimen.SqlCode = 4;

                    }
                }
                else
                {
                   
                    specstrucSpecimen.SqlCode = 0;
                }

                ddSampleType = (DropDownList)(mpNestedCPH.FindControl("dd" + strControlName));
                
                //Check if specimen type is selected
                if (ddSampleType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                {
                    throw new Exception("Please Select Specimen Type " + strControlName + ".");
                }
                specstrucSpecimen.Specimentype = ddSampleType.SelectedValue;

                if (strControlName.Contains("RK"))
                {
                    specstrucSpecimen.Tissuelocation = "Right Kidney";
                }
                else if (strControlName.Contains("LK"))
                {
                    specstrucSpecimen.Tissuelocation = "Left Kidney";
                }
                else
                {
                    specstrucSpecimen.Tissuelocation = string.Empty;
                }

                txtComments = (TextBox)(mpNestedCPH.FindControl("txtComments" + strControlName));
                specstrucSpecimen.Comments = txtComments.Text;

                if (chkCollected.Checked == true)
                {

                    specstrucSpecimen.Collected = "1";

                    txtBarcode = (TextBox)(mpNestedCPH.FindControl("txtBarcode" + strControlName));
                    
                    if (txtBarcode.Text != string.Empty)
                    {

                        string STRSQLFindBarcode = string.Empty;
                        STRSQLFindBarcode = "SELECT COUNT(*) CR FROM specimen WHERE Barcode=?Barcode AND TrialID <> ?TrialID AND TrialIDRecipient IS NULL ";

                        int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQLFindBarcode, "?Barcode", txtBarcode.Text, "?TrialID", Request.QueryString["TID"], STRCONN));

                        if (intCountFind > 0)
                        {
                            throw new Exception("Barcode " + txtBarcode.Text + " already exists in the database.");
                        }

                        if (arlBarcodes.Contains(txtBarcode.Text))
                        {
                            throw new Exception("Barcode " + txtBarcode.Text + " has been entered more than once.");
                        }
                        arlBarcodes.Add(txtBarcode.Text);
                        specstrucSpecimen.Barcode = txtBarcode.Text;
                    }

                    DateTime dteDateTimeCollected=DateTime.MinValue;
                    DateTime dteDateTimeCentrifugation = DateTime.MinValue;

                    //if (strControlName != "DB1_2" && strControlName != "DU1_1")
                    if (strControlName != "DB1_1" && strControlName != "DB1_2" )
                    {
                        txtDateCollected = (TextBox)(mpNestedCPH.FindControl("txtDateCollected" + strControlName));
                        txtTimeCollected = (TextBox)(mpNestedCPH.FindControl("txtTimeCollected" + strControlName));
                        

                        txtDateCentrifugation = (TextBox)(mpNestedCPH.FindControl("txtDateCentrifugation" + strControlName));
                        txtTimeCentrifugation = (TextBox)(mpNestedCPH.FindControl("txtTimeCentrifugation" + strControlName));

                    }
                    else
                    {
                        if (strControlName == "DB1_1" || strControlName == "DB1_2")
                        {
                            txtDateCollected = txtDateCollectedDB1_1;
                            txtTimeCollected = txtTimeCollectedDB1_1;
                            
                            txtDateCentrifugation = txtDateCentrifugationDB1_1;
                            txtTimeCentrifugation = txtTimeCentrifugationDB1_1;
                        }
                        else
                        {
                            //default txtDateCollected, txtTimeCollected, txtDateCentrifugation, txtTimeCentrifugation
                            txtDateCollected = txtDateCollectedDB1_1;
                            txtTimeCollected = txtTimeCollectedDB1_1;

                            txtDateCentrifugation = txtDateCentrifugationDB1_1;
                            txtTimeCentrifugation = txtTimeCentrifugationDB1_1;
                        }

                        
                    }

                    if (GeneralRoutines.IsDate(txtDateCollected.Text) == false)
                    {
                        throw new Exception("Please Enter Date Collected as DD/MM/YYYY for " + strControlName + ".");
                    }

                    if (Convert.ToDateTime(txtDateCollected.Text).Date > DateTime.Today.Date)
                    {
                        throw new Exception("Date Collected cannot be greater than Today's date.");
                    }

                    //assign date
                    dteDateTimeCollected = Convert.ToDateTime(txtDateCollected.Text);


                    if (txtTimeCollected.Text != string.Empty && txtTimeCollected.Text != "__:__")
                    {
                            
                        if (GeneralRoutines.IsDate(txtDateCollected.Text + " " + txtTimeCollected.Text)==false)
                        {
                            throw new Exception("Please Enter Time Collected in correct format for " + strControlName + " in the correct format.");
                        }

                        dteDateTimeCollected = Convert.ToDateTime(txtDateCollected.Text + " " + txtTimeCollected.Text);

                        if (dteDateTimeCollected > DateTime.Now.AddHours(1))
                        {
                            throw new Exception("Time Collected cannot be greater than Current Time.");
                        }

                    }

                    specstrucSpecimen.Datecollected = txtDateCollected.Text;
                    specstrucSpecimen.Timecollected = txtTimeCollected.Text;


                        

                    if (txtDateCentrifugation.Text!=string.Empty)
                    {
                        
                            
                        if (GeneralRoutines.IsDate(txtDateCentrifugation.Text) == false)
                        {
                            throw new Exception("Please Enter Date Centrifugation as DD/MM/YYYY for " + strControlName + ".");
                        }

                        if (Convert.ToDateTime(txtDateCentrifugation.Text).Date > DateTime.Today.Date)
                        {
                            throw new Exception("Date Centrifugation cannot be greater than Today's date for " + strControlName + ".");
                        }

                        if (Convert.ToDateTime(txtDateCentrifugation.Text) < Convert.ToDateTime(txtDateCollected.Text))
                        {
                            throw new Exception("Date Centrifugation cannot be earlier than Date Collected for " + strControlName + ".");
                        }

                        //assign date
                        dteDateTimeCentrifugation = Convert.ToDateTime(txtDateCentrifugation.Text);
                    }
                        

                        

                    //txtTimeCentrifugation = (TextBox)(mpNestedCPH.FindControl("txtTimeCentrifugation" + strControlName));
                    if (txtTimeCentrifugation.Text != string.Empty && txtTimeCentrifugation.Text !="__:__")
                    {
                        if (txtDateCentrifugation.Text == string.Empty)
                        {
                            throw new Exception("Please enter Date of Centrifugation for " + strControlName + ".");
                        }
                        if (GeneralRoutines.IsDate(txtDateCentrifugation.Text + " " + txtTimeCentrifugation.Text) == false)
                        {
                            throw new Exception("Please Enter Time Centrifugation in correct format for " + strControlName + ".");
                        }

                        dteDateTimeCentrifugation = Convert.ToDateTime(txtDateCentrifugation.Text + " " + txtTimeCentrifugation.Text);


                        if (txtTimeCollected.Text != string.Empty)
                        {
                            if (dteDateTimeCentrifugation<dteDateTimeCollected)
                            {
                                throw new Exception("Time of Centrifugation cannot be earlier than Time Collected for " + strControlName + ".");
                            }
                        }
                        if (dteDateTimeCentrifugation > DateTime.Now.AddHours(1))
                        {
                            throw new Exception("Time Centrifugation cannot be greater than Current Time for " + strControlName + ".");
                        }

                    }

                    specstrucSpecimen.Datecentrifugation = txtDateCentrifugation.Text;
                    specstrucSpecimen.Timecentrifugation = txtTimeCentrifugation.Text;

                    
                    
                    

                    arlSpecimens.Add(specstrucSpecimen);

                }
                //lblUserMessages.Text += chkCollected.ID + ". ";
                else
                {
                    if (chkCollected.Visible == true)
                    {

                        specstrucSpecimen.Collected = "0"; //Not collected

                        txtBarcode = (TextBox)(mpNestedCPH.FindControl("txtBarcode" + strControlName));

                        //lblBarcode.Text += txtBarcode.Text + "";
                        if (txtBarcode.Text != string.Empty)
                        {
                            throw new Exception("As Collected CheckBox has not been ticked, Barcode for " + strControlName + " should be empty.");
                        }


                        arlSpecimens.Add(specstrucSpecimen);
                    }
                    
                }

            }


            if (arlSpecimens.Count == 0)
            {
                //throw new Exception("No Specimen Data Entered.");
            }
            else
            {
                //check barcode has been added
                if (txtWorksheetBarcode.Text == string.Empty)
                {
                    throw new Exception("Please Enter Worksheet Barcode.");
                }
                else
                {
                    if (txtWorksheetBarcode.Text.Length != intWorksheetBarcodeLength)
                    {
                        throw new Exception("The length of Worksheet Barcode should be " + intWorksheetBarcodeLength.ToString() + ".");
                    }
                    if (txtWorksheetBarcode.Text.Substring(0, 4) != intBarcodeFirstFourDigits.ToString())
                    {
                        throw new Exception("The first four characters of Worksheet Barcode should be " + intBarcodeFirstFourDigits.ToString() + ". Please check the Worksheet barcode you have entered.");
                    }
                    if (GeneralRoutines.IsNumeric(txtWorksheetBarcode.Text) == false)
                    {
                        throw new Exception("The 'Worksheet Barcode' should be numeric.");
                    }

                    //check if worksheet barcode has already been added
                    string strSQL_WB = "SELECT COUNT(*) CR FROM specimenmaindetails WHERE WorksheetBarcode=?WorksheetBarcode AND TrialID <> ?TrialID ";

                    int intWBCount = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(strSQL_WB, "?WorksheetBarcode", txtWorksheetBarcode.Text, "TrialID", Request.QueryString["TID"], STRCONN));

                    if (intWBCount>0)
                    {
                        strSQL_WB = "SELECT TrialID FROM specimenmaindetails WHERE WorksheetBarcode=?WorksheetBarcode AND TrialID <> ?TrialID ";

                        string strWBTrialID = GeneralRoutines.ReturnScalarTwo(strSQL_WB, "?WorksheetBarcode", txtWorksheetBarcode.Text, "TrialID", Request.QueryString["TID"], STRCONN);
                        throw new Exception("Worksheet barcode " + txtWorksheetBarcode.Text + " has already been added for " +  strWBTrialID + "");
                    }
                    if (intWBCount<0)
                    {
                        throw new Exception("An error occured while checking if Worksheet barcode " + txtWorksheetBarcode.Text + " has already been added for another TrialID");
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

            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO specimen ";
            STRSQL += "(TrialID,  Collected, Barcode, SpecimenType, DateCollected, TimeCollected, DateCentrifugation, TimeCentrifugation, Occasion, TissueSource,  ";
            STRSQL += "PageVersion,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID,  ?Collected, ?Barcode, ?SpecimenType, ?DateCollected, ?TimeCollected, ?DateCentrifugation, ?TimeCentrifugation, ?Occasion, ?TissueSource,  ";
            STRSQL += "?PageVersion,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE specimen SET ";
            STRSQL_UPDATE += "Collected=?Collected, Barcode=?Barcode, SpecimenType=?SpecimenType, DateCollected=?DateCollected, TimeCollected=?TimeCollected,   ";
            STRSQL_UPDATE += "DateCentrifugation=?DateCentrifugation, TimeCentrifugation=?TimeCentrifugation,";
            STRSQL_UPDATE += "Occasion=?Occasion, TissueSource=?TissueSource, PageVersion=?PageVersion,";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "WHERE SpecimenID=?SpecimenID AND TrialID=?TrialID ";


            string STRSQL_CONSENTS = String.Empty;
            STRSQL_CONSENTS += "INSERT INTO specimenmaindetails ";
            STRSQL_CONSENTS += "(TrialID, ConsentQuod, ConsentAdditionalSamples, WorksheetBarcode,";
            STRSQL_CONSENTS += "ResearcherName, ResearcherTelephoneNumber, SamplesStoredOxfordDate, SamplesStoredOxfordTime,";
            STRSQL_CONSENTS += "Comments, DateCreated, CreatedBy) ";
            STRSQL_CONSENTS += "VALUES ";
            STRSQL_CONSENTS += "(?TrialID, ?ConsentQuod, ?ConsentAdditionalSamples, ?WorksheetBarcode,";
            STRSQL_CONSENTS += "?ResearcherName, ?ResearcherTelephoneNumber, ?SamplesStoredOxfordDate, ?SamplesStoredOxfordTime,";
            STRSQL_CONSENTS += "?ConsentComments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATECONSENTS = String.Empty;
            STRSQL_UPDATECONSENTS += "UPDATE specimenmaindetails SET ";
            STRSQL_UPDATECONSENTS += "ConsentQuod=?ConsentQuod, ConsentAdditionalSamples=?ConsentAdditionalSamples, WorksheetBarcode=?WorksheetBarcode, ";
            STRSQL_UPDATECONSENTS += "ResearcherName=?ResearcherName, ResearcherTelephoneNumber=?ResearcherTelephoneNumber, ";
            STRSQL_UPDATECONSENTS += "SamplesStoredOxfordDate=?SamplesStoredOxfordDate, SamplesStoredOxfordTime=?SamplesStoredOxfordTime,";
            STRSQL_UPDATECONSENTS += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATECONSENTS += "Comments=?ConsentComments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATECONSENTS += "WHERE TrialID=?TrialID AND TrialIDRecipient IS NULL ";


            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE specimenmaindetails SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy  ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND TrialIDRecipient IS NULL AND (DataLocked IS NULL OR DataLocked=0) ";
            

            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE specimenmaindetails SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID AND TrialIDRecipient IS NULL ";


            string STRSQL_COUNTCONSENTS = "SELECT COUNT(*) CR FROM specimenmaindetails WHERE TrialID=?TrialID AND TrialIDRecipient IS NULL ";
            int intCountConsents = 0;
            intCountConsents = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQL_COUNTCONSENTS, "?TrialID", Request.QueryString["TID"], STRCONN));

            string strSQLCONSENT = string.Empty;

            if (intCountConsents < 0)
            {
                throw new Exception("An error occured while checking if Consent Details have been added.");
            }
            if (intCountConsents > 1)
            {

                throw new Exception("More than one record for Consent Details have been added.");
            }

            if (intCountConsents == 0)
            {
                strSQLCONSENT = STRSQL_CONSENTS;
            }

            if (intCountConsents == 1)
            {
                strSQLCONSENT = STRSQL_UPDATECONSENTS;
            }


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];

            if (chkConsentQuod.Checked == false)
            {
                if (SessionVariablesAll.CentreCode.Substring(0,1)=="1")
                {
                    MyCMD.Parameters.Add("?ConsentQuod", MySqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    MyCMD.Parameters.Add("?ConsentQuod", MySqlDbType.VarChar).Value = STR_NO_SELECTION;
                }
                
            }
            else
            {
                MyCMD.Parameters.Add("?ConsentQuod", MySqlDbType.VarChar).Value = STR_YES_SELECTION;
            }

            if (chkConsentAdditionalSamples.Checked == false)
            {
                MyCMD.Parameters.Add("?ConsentAdditionalSamples", MySqlDbType.VarChar).Value = STR_NO_SELECTION;
            }
            else
            {
                MyCMD.Parameters.Add("?ConsentAdditionalSamples", MySqlDbType.VarChar).Value = STR_YES_SELECTION;
            }

            MyCMD.Parameters.Add("?ConsentComments", MySqlDbType.VarChar).Value = txtConsentComments.Text;
            MyCMD.Parameters.Add("?WorksheetBarcode", MySqlDbType.VarChar).Value = txtWorksheetBarcode.Text;

            if (txtResearcherName.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ResearcherName", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ResearcherName", MySqlDbType.VarChar).Value = txtResearcherName.Text;
            }
            if (txtResearcherTelephoneNumber.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ResearcherTelephoneNumber", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ResearcherTelephoneNumber", MySqlDbType.VarChar).Value = txtResearcherTelephoneNumber.Text;
            }

            if (GeneralRoutines.IsDate(txtSamplesStoredOxfordDate.Text) == false)
            {
                MyCMD.Parameters.Add("?SamplesStoredOxfordDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SamplesStoredOxfordDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtSamplesStoredOxfordDate.Text);
            }

            if (txtSamplesStoredOxfordTime.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?SamplesStoredOxfordTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SamplesStoredOxfordTime", MySqlDbType.VarChar).Value = txtSamplesStoredOxfordTime.Text;
            }

            MyCMD.Parameters.Add("?SpecimenID", MySqlDbType.Int64);
            MyCMD.Parameters.Add("?Collected", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?Barcode", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?SpecimenType", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?DateCollected", MySqlDbType.Date);
            MyCMD.Parameters.Add("?TimeCollected", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?DateCentrifugation", MySqlDbType.Date);
            MyCMD.Parameters.Add("?TimeCentrifugation", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?TissueSource", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar);

            MyCMD.Parameters.Add("?PageVersion", MySqlDbType.VarChar).Value = strPageVersion;
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
                int i = 0;
                for (i = 0; i < arlSpecimens.Count; i++)
                {
                    SpecimenStructure spStruct = new SpecimenStructure();
                    spStruct = (SpecimenStructure)arlSpecimens[i];

                    MyCMD.Parameters["?SpecimenID"].Value = spStruct.UniqueID;

                    if (spStruct.SqlCode == 0)
                    {
                        MyCMD.CommandText = STRSQL;
                    }
                    if (spStruct.SqlCode == 1)
                    {
                        MyCMD.CommandText = STRSQL_UPDATE;
                    }

                    //MyCMD.CommandText = STRSQL;
                    if (spStruct.Collected == string.Empty)
                    {
                        MyCMD.Parameters["?Collected"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?Collected"].Value = spStruct.Collected;
                    }

                    if (spStruct.Barcode == string.Empty)
                    {
                        MyCMD.Parameters["?Barcode"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?Barcode"].Value = spStruct.Barcode;
                    }

                    if (spStruct.Specimentype == string.Empty)
                    {
                        MyCMD.Parameters["?SpecimenType"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?SpecimenType"].Value = spStruct.Specimentype;
                    }

                    if (GeneralRoutines.IsDate(spStruct.Datecollected)==false)
                    {
                        MyCMD.Parameters["?DateCollected"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?DateCollected"].Value = Convert.ToDateTime(spStruct.Datecollected);
                    }

                    if (spStruct.Timecollected == string.Empty || spStruct.Timecollected == "__:__")
                    {
                        MyCMD.Parameters["?TimeCollected"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?TimeCollected"].Value = spStruct.Timecollected;
                    }

                    if (GeneralRoutines.IsDate(spStruct.Datecentrifugation) == false)
                    {
                        MyCMD.Parameters["?DateCentrifugation"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?DateCentrifugation"].Value = Convert.ToDateTime(spStruct.Datecentrifugation);
                    }

                    if (spStruct.Timecentrifugation == string.Empty || spStruct.Timecentrifugation == "__:__")
                    {
                        MyCMD.Parameters["?TimeCentrifugation"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?TimeCentrifugation"].Value = spStruct.Timecentrifugation;
                    }

                    if (spStruct.Occasion == string.Empty)
                    {
                        MyCMD.Parameters["?Occasion"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?Occasion"].Value = spStruct.Occasion;
                    }

                    if (spStruct.Tissuelocation == string.Empty)
                    {
                        MyCMD.Parameters["?TissueSource"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?TissueSource"].Value = spStruct.Tissuelocation;
                    }

                    if (spStruct.Comments == string.Empty)
                    {
                        MyCMD.Parameters["?Comments"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?Comments"].Value = spStruct.Comments;
                    }

                    //execute query
                    MyCMD.ExecuteNonQuery();                                       

                }

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = strSQLCONSENT;
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

                BindData();
                AssignData();

                AssignConsentData();

                lblUserMessages.Text = "Data Added.";
            }

            catch (System.Exception ex)
            {
                myTrans.Rollback();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = ex.Message + " An error occured while executing transaction.";
            }
            

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }

    protected void CheckedBoxSelection_Checked(object sender, EventArgs e)
    {
        try 
        {
            lblUserMessages.Text = string.Empty;

            if (txtWorksheetBarcode.Text == string.Empty)
            {
                throw new Exception("Please Enter Worksheet Barcode.");
            }
            else
            {
                if (txtWorksheetBarcode.Text.Length != intWorksheetBarcodeLength)
                {
                    throw new Exception("The length of Worksheet Barcode should be " + intWorksheetBarcodeLength.ToString() + ".");
                }
                if (txtWorksheetBarcode.Text.Substring(0, 4) != intBarcodeFirstFourDigits.ToString())
                {
                    throw new Exception("The first four characters of Worksheet Barcode should be " + intBarcodeFirstFourDigits.ToString() + ". Please check the Worksheet barcode you have entered.");
                }
                if (GeneralRoutines.IsNumeric(txtWorksheetBarcode.Text) == false)
                {
                    throw new Exception("The 'Worksheet Barcode' should be numeric.");
                }
            }

            CheckBox chkTemp = sender as CheckBox;

            
                if (GeneralRoutines.IsNumeric(txtWorksheetBarcode.Text))
                {
                    switch (chkTemp.ID)
                    {

                        case "chkDU1_1":

                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeDU1_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 0).ToString();
                            }
                            else
                            {
                                txtBarcodeDU1_1.Text = string.Empty;
                            }
                            break;

                        case "chkDB1_1":
                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeDB1_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 1).ToString();
                                
                            }
                            else
                            {
                                txtBarcodeDB1_1.Text = string.Empty;
                            }
                            break;
                            
                        case "chkDB1_2":

                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeDB1_2.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 2).ToString();                                
                            }
                            else
                            {
                                txtBarcodeDB1_2.Text = string.Empty;
                            }
                            break;
                        

                        case "chkDU2_1":

                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeDU2_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 3).ToString();
                            }
                            else
                            {
                                txtBarcodeDU2_1.Text = string.Empty;
                            }
                            break; 
   
                        case "chkRKP1_1":
                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeRKP1_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 4).ToString();
                            }
                            else
                            {
                                txtBarcodeRKP1_1.Text = string.Empty;
                            }
                            
                            break;
                        case "chkLKP1_1":
                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeLKP1_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 5).ToString();
                            }
                            else
                            {
                                txtBarcodeLKP1_1.Text = string.Empty;
                            }
                                                        
                            break;
                        case "chkRKP2_1":
                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeRKP2_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 6).ToString();
                            }
                            else
                            {
                                txtBarcodeRKP2_1.Text = string.Empty;
                            }
                            
                            break;
                        case "chkLKP2_1":
                            if (chkTemp.Checked == true)
                            {
                                txtBarcodeLKP2_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 7).ToString();
                            }
                            else
                            {
                                txtBarcodeLKP2_1.Text = string.Empty;
                            }
                                                        
                            break;
                    }
                }

            
           

            

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assigning Barcode on checkbox selection.";
        }
    }

    protected void chkConsentAdditionalSamples_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkConsentAdditionalSamples.Checked == true)
            {
                pnlWP7Samples.Visible = true;
            }
            else
            {
                pnlWP7Samples.Visible = false;
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while clicking on Consent for Additional Samples.";
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