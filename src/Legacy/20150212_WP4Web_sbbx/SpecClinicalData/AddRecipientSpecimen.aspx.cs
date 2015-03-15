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
public partial class SpecClinicalData_AddRecipientSpecimen : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

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

                if (Request.QueryString["TID_R"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID (Recipient).");
                }

                lblDescription.Text = "Enter Specimen Data.";
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";

                rblKidneyReceived.DataSource = XMLKidneySidesDataSource;
                rblKidneyReceived.DataBind();
                rblKidneyReceived.SelectedValue = STR_UNKNOWN_SELECTION;

                lblP1_1.Text = "P1 (Kidney: 15 min after HMP Start)";
                ddP1_1.DataSource = XMLSpecimenTypesDataSource;
                ddP1_1.DataBind();
                ddP1_1.SelectedValue = "Perfusate";
                ddP1_1.Enabled = false;

                lblP2_1.Text = "P2 (Kidney: Leaving Hospital)";
                ddP2_1.DataSource = XMLSpecimenTypesDataSource;
                ddP2_1.DataBind();
                ddP2_1.SelectedValue = "Perfusate";
                ddP2_1.Enabled = false;

                lblP3_1.Text = "P3 (Kidney: End of HMP)";
                ddP3_1.DataSource = XMLSpecimenTypesDataSource;
                ddP3_1.DataBind();
                ddP3_1.SelectedValue = "Perfusate";
                ddP3_1.Enabled = false;

                lblRB1_1.Text = "RB1 (After Anaesthesia)";
                ddRB1_1.DataSource = XMLSpecimenTypesDataSource;
                ddRB1_1.DataBind();
                ddRB1_1.SelectedValue = "EDTA";
                ddRB1_1.Enabled = false;

                ddRB1_2.DataSource = XMLSpecimenTypesDataSource;
                ddRB1_2.DataBind();
                ddRB1_2.SelectedValue = "SST";
                ddRB1_2.Enabled = false;

                lblRB2_1.Text = "RB2 (After Reperfusion)";
                ddRB2_1.DataSource = XMLSpecimenTypesDataSource;
                ddRB2_1.DataBind();
                ddRB2_1.SelectedValue = "EDTA";
                ddRB2_1.Enabled = false;

                ddRB2_2.DataSource = XMLSpecimenTypesDataSource;
                ddRB2_2.DataBind();
                ddRB2_2.SelectedValue = "SST";
                ddRB2_2.Enabled = false;

                lblReK1R.Text = "ReK1 (Reperfusion Kidney Biopsy  )";
                ddPreservationReK1R.DataSource = XMLPreservationTypesDataSource;
                ddPreservationReK1R.DataBind();
                ddPreservationReK1R.SelectedValue = "RNAlater";
                ddPreservationReK1R.Enabled = false;


                ddPreservationReK1F.DataSource = XMLPreservationTypesDataSource;
                ddPreservationReK1F.DataBind();
                ddPreservationReK1F.SelectedValue = "Formalin";
                ddPreservationReK1F.Enabled = false;

                txtDateCollectedP1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedP2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedP3_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedRB1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedRB2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCollectedReK1R_CalendarExtender.EndDate = DateTime.Today;

                txtDateCentrifugationP1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationP2_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationP3_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationRB1_1_CalendarExtender.EndDate = DateTime.Today;
                txtDateCentrifugationRB2_1_CalendarExtender.EndDate = DateTime.Today;

                ViewState["SortField"] = "SpecimenID";
                ViewState["SortDirection"] = "ASC";

                BindData();

                AssignConsentData();

                AssignData();

                string STRSQL_Side = "SELECT KidneyReceived FROM trialdetails_recipient WHERE TrialIDRecipient=?TrialIDRecipient ";

                string STR_Side = GeneralRoutines.ReturnScalar(STRSQL_Side, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), STRCONN);

                ListItem liSide = rblKidneyReceived.Items.FindByValue(STR_Side);

                if (liSide != null)
                {
                    rblKidneyReceived.SelectedValue = STR_Side;
                    lblSideValue.Text = STR_Side;
                }
                else
                {
                    lblSideValue.Text = "Not Known";
                }


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

            strSQL += "SELECT t1.*, t2.DonorID MainDonorID,  ";
            strSQL += "DATE_FORMAT(t1.DateCollected, '%d/%m/%Y') Date_Collected,  ";
            strSQL += "DATE_FORMAT(t1.TimeCollected, '%H:%i') Time_Collected,  ";
            strSQL += "DATE_FORMAT(t2.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            strSQL += "FROM specimen t1 ";
            strSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = sqldsGV1;
            sqldsGV1.SelectCommand = strSQL;
            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on TrialID Edit/Delete Specimen Data. Number of Records " + GV1.Rows.Count.ToString();

            lblDescription.Text = "Add  Recipient Specimen Data for " + Request.QueryString["TID_R"].ToString() + " (Recipient) and DonorID " + strDonorID;
            if (strRecipientID != string.Empty)
            {
                lblDescription.Text += " and RecipientID " + strRecipientID;
            }

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

            STRSQL += "DELETE FROM specimen WHERE SpecimenID=?SpecimenID AND TrialIDRecipient=?TrialIDRecipient  ";

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
            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();

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
            STRSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";

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

            STRSQL += "SELECT * FROM specimen WHERE TrialIDRecipient=?TrialIDRecipient ";

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
                        DropDownList ddPreservation;
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

                                //LT1 allow dry ice
                                if (!DBNull.Value.Equals(myDr["State"]))
                                {

                                    if (myDr["Occasion"].ToString() == "" || myDr["Occasion"].ToString()=="")
                                    {
                                        ddPreservation = (DropDownList)(mpCPHNested.FindControl("ddPreservation" + myDr["Occasion"].ToString()));
                                    }
                                    //ddSampleType = (DropDownList)(mpCPHNested.FindControl("dd" + myDr["Occasion"].ToString()));
                                    //if (ddSampleType != null)
                                    //{
                                    //    ddSampleType.SelectedValue = myDr["State"].ToString();
                                    //}

                                }

                                if (!DBNull.Value.Equals(myDr["DateCollected"]))
                                {
                                    if (GeneralRoutines.IsDate(myDr["DateCollected"].ToString()) == true)
                                    {
                                        switch (myDr["Occasion"].ToString())
                                        {

                                            case "RB1_1":
                                            case "RB1_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtDateCollectedRB1_1.Text = Convert.ToDateTime(myDr["DateCollected"]).ToShortDateString();
                                                break;

                                            case "RB2_1":
                                            case "RB2_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtDateCollectedRB2_1.Text = myDr["DateCollected"].ToString().Substring(0, 5);
                                                break;
                                            case "ReK1R":
                                            case "ReK1F":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtDateCollectedReK1R.Text = myDr["DateCollected"].ToString().Substring(0, 5);
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

                                            case "RB1_1":
                                            case "RB1_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtTimeCollectedRB1_1.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
                                                break;

                                            case "RB2_1":
                                            case "RB2_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtTimeCollectedRB2_1.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
                                                break;
                                            case "ReK1R":
                                            case "ReK1F":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtTimeCollectedReK1R.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
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

                                            case "RB1_1":
                                            case "RB1_2":
                                                //txtDateCentrifugation = (TextBox)mpCPHNested.FindControl("txtDateCentrifugation" + myDr["Occasion"].ToString());
                                                txtDateCentrifugationRB1_1.Text = Convert.ToDateTime(myDr["DateCentrifugation"]).ToShortDateString();
                                                break;
                                            case "RB2_1":
                                            case "RB2_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtDateCentrifugationRB2_1.Text = myDr["DateCentrifugation"].ToString().Substring(0, 5);
                                                break;
                                            case "ReK1R":
                                            case "ReK1F":
                                                //do nothing
                                                //txtTimeCollectedReK1R.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
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

                                            case "RB1_1":
                                            case "RB1_2":
                                                //txtDateCentrifugation = (TextBox)mpCPHNested.FindControl("txtDateCentrifugation" + myDr["Occasion"].ToString());
                                                txtTimeCentrifugationRB1_1.Text = myDr["TimeCentrifugation"].ToString().Substring(0, 5);
                                                break;
                                            case "RB2_1":
                                            case "RB2_2":
                                                //txtDateCollected = (TextBox)mpCPHNested.FindControl("txtDateCollected" + myDr["Occasion"].ToString());
                                                txtTimeCentrifugationRB2_1.Text = myDr["TimeCentrifugation"].ToString().Substring(0, 5);
                                                break;
                                            case "ReK1R":
                                            case "ReK1F":
                                                //do nothing
                                                //txtTimeCollectedReK1R.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
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

            if (rblKidneyReceived.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select 'Side' of Kidney.");
            }

            //if (txtWorksheetBarcode.Text == string.Empty)
            //{
            //    throw new Exception("Please Enter Worksheet Barcode.");
            //}
            //else
            //{
            //    if (txtWorksheetBarcode.Text.Length != intWorksheetBarcodeLength)
            //    {
            //        throw new Exception("The length of Worksheet Barcode should be " + intWorksheetBarcodeLength.ToString() + ".");
            //    }
            //    if (txtWorksheetBarcode.Text.Substring(0, 4) != intBarcodeFirstFourDigits.ToString())
            //    {
            //        throw new Exception("The first four characters of Worksheet Barcode should be " + intBarcodeFirstFourDigits.ToString() + ". Please check the Worksheet barcode you have entered.");
            //    }
            //    if (GeneralRoutines.IsNumeric(txtWorksheetBarcode.Text) == false)
            //    {
            //        throw new Exception("The 'Worksheet Barcode' should be numeric.");
            //    }
            //}

            SpecimenStructure specstrucSpecimen;


            ArrayList arlOccasions = new ArrayList();
            ArrayList arlBarcodes = new ArrayList();
            ArrayList arlSpecimens = new ArrayList();

            arlOccasions.Add("P1_1");
            arlOccasions.Add("P2_1");
            arlOccasions.Add("P3_1");
            arlOccasions.Add("RB1_1");
            arlOccasions.Add("RB1_2");
            arlOccasions.Add("RB2_1");
            arlOccasions.Add("RB2_2");
            arlOccasions.Add("ReK1R");
            arlOccasions.Add("ReK1F");

            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
            ContentPlaceHolder mpNestedCPH = (ContentPlaceHolder)(mpCPH.FindControl(strNestedCPH));


            Label lblSpecimenID;
            CheckBox chkCollected;
            DropDownList ddSampleType;
            DropDownList ddPreservation;
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

                //ddSampleType = (DropDownList)(mpNestedCPH.FindControl("dd" + strControlName));

                ////Check if specimen type is selected
                //if (ddSampleType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                //{
                //    throw new Exception("Please Select Specimen Type " + strControlName + ".");
                //}


                if (strControlName != "ReK1R" && strControlName != "ReK1F")
                {
                    ddSampleType = (DropDownList)(mpNestedCPH.FindControl("dd" + strControlName));
                    specstrucSpecimen.Specimentype = ddSampleType.SelectedValue;
                    specstrucSpecimen.Preservation = string.Empty;

                    if (ddSampleType.SelectedValue == "Perfusate")
                    {
                        specstrucSpecimen.Tissuelocation = rblKidneyReceived.SelectedValue + " Kidney";
                    }

                    else
                    {
                        specstrucSpecimen.Tissuelocation = string.Empty;
                    }


                }
                else
                {
                    specstrucSpecimen.Specimentype = rblKidneyReceived.SelectedValue + " Kidney";
                    ddPreservation = (DropDownList)(mpNestedCPH.FindControl("ddPreservation" + strControlName));
                    specstrucSpecimen.Preservation = ddPreservation.SelectedValue;
                    specstrucSpecimen.Tissuelocation = rblKidneyReceived.SelectedValue + " Kidney";
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
                        STRSQLFindBarcode = "SELECT COUNT(*) CR FROM specimen WHERE Barcode=?Barcode AND TrialIDRecipient <> ?TrialIDRecipient AND TrialIDRecipient IS NOT NULL ";

                        int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQLFindBarcode, "?Barcode", txtBarcode.Text, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));

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

                    DateTime dteDateTimeCollected = DateTime.MinValue;
                    DateTime dteDateTimeCentrifugation = DateTime.MinValue;


                    //if (strControlName != "DB1_2" && strControlName != "DU1_1")
                    if (strControlName != "RB1_2" && strControlName != "RB2_2" && strControlName != "ReK1R" && strControlName != "ReK1F")
                    {
                        txtDateCollected = (TextBox)(mpNestedCPH.FindControl("txtDateCollected" + strControlName));
                        txtTimeCollected = (TextBox)(mpNestedCPH.FindControl("txtTimeCollected" + strControlName));


                        txtDateCentrifugation = (TextBox)(mpNestedCPH.FindControl("txtDateCentrifugation" + strControlName));
                        txtTimeCentrifugation = (TextBox)(mpNestedCPH.FindControl("txtTimeCentrifugation" + strControlName));

                    }
                    else
                    {
                        if (strControlName == "RB1_2")
                        {
                            txtDateCollected = txtDateCollectedRB1_1;
                            txtTimeCollected = txtTimeCollectedRB1_1;

                            txtDateCentrifugation = txtDateCentrifugationRB1_1;
                            txtTimeCentrifugation = txtTimeCentrifugationRB1_1;
                        }
                        else if (strControlName == "RB2_2")
                        {
                            txtDateCollected = txtDateCollectedRB2_1;
                            txtTimeCollected = txtTimeCollectedRB2_1;

                            txtDateCentrifugation = txtDateCentrifugationRB2_1;
                            txtTimeCentrifugation = txtTimeCentrifugationRB2_1;
                        }
                        else if (strControlName == "ReK1R" || strControlName == "ReK1F")
                        {
                            txtDateCollected = txtDateCollectedReK1R;
                            txtTimeCollected = txtTimeCollectedReK1R;

                            //assign default values
                            txtDateCentrifugation = txtDateCollectedReK1R;
                            txtTimeCentrifugation = txtTimeCollectedReK1R;
                        }
                        else
                        {
                            //default txtDateCollected, txtTimeCollected, txtDateCentrifugation, txtTimeCentrifugation
                            txtDateCollected = txtDateCollectedRB1_1;
                            txtTimeCollected = txtTimeCollectedRB1_1;

                            txtDateCentrifugation = txtDateCentrifugationRB1_1;
                            txtTimeCentrifugation = txtTimeCentrifugationRB1_1;
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

                        if (GeneralRoutines.IsDate(txtDateCollected.Text + " " + txtTimeCollected.Text) == false)
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

                          
                    if (txtDateCentrifugation.Text != string.Empty)
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
                    if (txtTimeCentrifugation.Text != string.Empty && txtTimeCentrifugation.Text != "__:__")
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
                            if (dteDateTimeCentrifugation < dteDateTimeCollected)
                            {
                                throw new Exception("Time of Centrifugation cannot be earlier than Time Collected for " + strControlName + ".");
                            }
                        }
                        if (dteDateTimeCentrifugation > DateTime.Now.AddHours(1))
                        {
                            throw new Exception("Time Centrifugation cannot be greater than Current Time for " + strControlName + ".");
                        }

                    }



                    if (strControlName == "ReK1R" || strControlName == "ReK1F")
                    {
                        specstrucSpecimen.Datecentrifugation = string.Empty;
                        specstrucSpecimen.Timecentrifugation = string.Empty;
                    }
                    else
                    {
                        specstrucSpecimen.Datecentrifugation = txtDateCentrifugation.Text;
                        specstrucSpecimen.Timecentrifugation = txtTimeCentrifugation.Text;
                    }

                        

                    arlSpecimens.Add(specstrucSpecimen);

                }
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
                //lblUserMessages.Text += chkCollected.ID + ". ";



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
                    string strSQL_WB = "SELECT COUNT(*) CR FROM specimenmaindetails WHERE WorksheetBarcode=?WorksheetBarcode AND (TrialIDRecipient <> ?TrialIDRecipient OR TrialIDRecipient IS NULL)";

                    int intWBCount = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(strSQL_WB, "?WorksheetBarcode", txtWorksheetBarcode.Text, "TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));

                    if (intWBCount > 0)
                    {
                        strSQL_WB = "SELECT TrialID FROM specimenmaindetails WHERE WorksheetBarcode=?WorksheetBarcode AND (TrialIDRecipient <> ?TrialIDRecipient OR TrialIDRecipient IS NULL) ";

                        string strWBTrialID = GeneralRoutines.ReturnScalarTwo(strSQL_WB, "?WorksheetBarcode", txtWorksheetBarcode.Text, "TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);
                        throw new Exception("Worksheet barcode " + txtWorksheetBarcode.Text + " has already been added for a different recipient/donor. ");
                    }
                    if (intWBCount < 0)
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
            STRSQL += "(TrialID,  TrialIDRecipient, Collected, Barcode, SpecimenType, DateCollected, TimeCollected, DateCentrifugation, TimeCentrifugation,  ";
            STRSQL += "Occasion, TissueSource, State, PageVersion,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID,  ?TrialIDRecipient, ?Collected, ?Barcode, ?SpecimenType, ?DateCollected, ?TimeCollected, ?DateCentrifugation, ?TimeCentrifugation,  ";
            STRSQL += "?Occasion, ?TissueSource, ?State,?PageVersion,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";


            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE specimen SET ";
            STRSQL_UPDATE += "Collected=?Collected, Barcode=?Barcode, SpecimenType=?SpecimenType, DateCollected=?DateCollected, TimeCollected=?TimeCollected,   ";
            STRSQL_UPDATE += "DateCentrifugation=?DateCentrifugation, TimeCentrifugation=?TimeCentrifugation,";
            STRSQL_UPDATE += "Occasion=?Occasion, TissueSource=?TissueSource, PageVersion=?PageVersion,";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "WHERE SpecimenID=?SpecimenID AND TrialIDRecipient=?TrialIDRecipient ";


            string STRSQL_CONSENTS = String.Empty;
            STRSQL_CONSENTS += "INSERT INTO specimenmaindetails ";
            STRSQL_CONSENTS += "(TrialID, TrialIDRecipient, ConsentAdditionalSamples, WorksheetBarcode,";
            STRSQL_CONSENTS += "ResearcherName, ResearcherTelephoneNumber, SamplesStoredOxfordDate, SamplesStoredOxfordTime,";
            STRSQL_CONSENTS += "Comments, DateCreated, CreatedBy) ";
            STRSQL_CONSENTS += "VALUES ";
            STRSQL_CONSENTS += "(?TrialID, ?TrialIDRecipient,?ConsentAdditionalSamples, ?WorksheetBarcode,";
            STRSQL_CONSENTS += "?ResearcherName, ?ResearcherTelephoneNumber, ?SamplesStoredOxfordDate, ?SamplesStoredOxfordTime,";
            STRSQL_CONSENTS += "?ConsentComments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATECONSENTS = String.Empty;
            STRSQL_UPDATECONSENTS += "UPDATE specimenmaindetails SET ";
            STRSQL_UPDATECONSENTS += "ConsentAdditionalSamples=?ConsentAdditionalSamples, WorksheetBarcode=?WorksheetBarcode, ";
            STRSQL_UPDATECONSENTS += "ResearcherName=?ResearcherName, ResearcherTelephoneNumber=?ResearcherTelephoneNumber, ";
            STRSQL_UPDATECONSENTS += "SamplesStoredOxfordDate=?SamplesStoredOxfordDate, SamplesStoredOxfordTime=?SamplesStoredOxfordTime,";
            STRSQL_UPDATECONSENTS += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATECONSENTS += "Comments=?ConsentComments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATECONSENTS += "WHERE TrialID=?TrialID AND TrialIDRecipient =?TrialIDRecipient ";


            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE specimenmaindetails SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy  ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE specimenmaindetails SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID AND TrialIDRecipient =?TrialIDRecipient ";


            string STRSQL_COUNTCONSENTS = "SELECT COUNT(*) CR FROM specimenmaindetails WHERE TrialID=?TrialID AND TrialIDRecipient =?TrialIDRecipient ";
            int intCountConsents = 0;
            intCountConsents = Convert.ToInt32(GeneralRoutines.ReturnScalarTwo(STRSQL_COUNTCONSENTS, "?TrialID", Request.QueryString["TID"], "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));

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
            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            if (chkConsentQuod.Checked == false)
            {
                if (SessionVariablesAll.CentreCode.Substring(0, 1) == "1")
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

            MyCMD.Parameters.Add("?ConsentComments", MySqlDbType.VarChar).Value = txtConsentComments.Text;
            MyCMD.Parameters.Add("?WorksheetBarcode", MySqlDbType.VarChar).Value = txtWorksheetBarcode.Text;

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
            MyCMD.Parameters.Add("?State", MySqlDbType.VarChar);
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

                    if (GeneralRoutines.IsDate(spStruct.Datecollected) == false)
                    {
                        MyCMD.Parameters["?DateCollected"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?DateCollected"].Value = Convert.ToDateTime(spStruct.Datecollected);
                    }

                    if (spStruct.Timecollected == string.Empty || spStruct.Timecollected =="__:__")
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

                    if (spStruct.Preservation == string.Empty)
                    {
                        MyCMD.Parameters["?State"].Value = DBNull.Value;
                    }
                    else
                    {
                        MyCMD.Parameters["?State"].Value = spStruct.Preservation;
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

                    case "chkP1_1":
                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeP1_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 0).ToString();

                        }
                        else
                        {
                            txtBarcodeP1_1.Text = string.Empty;
                        }
                        break;

                    case "chkP2_1":
                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeP2_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 1).ToString();

                        }
                        else
                        {
                            txtBarcodeP2_1.Text = string.Empty;
                        }
                        break;

                    case "chkP3_1":
                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeP3_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 2).ToString();

                        }
                        else
                        {
                            txtBarcodeP3_1.Text = string.Empty;
                        }
                        break;

                    case "chkRB1_1":
                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeRB1_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 3).ToString();

                        }
                        else
                        {
                            txtBarcodeRB1_1.Text = string.Empty;
                        }
                        break;

                    case "chkRB1_2":

                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeRB1_2.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 4).ToString();
                        }
                        else
                        {
                            txtBarcodeRB1_2.Text = string.Empty;
                        }
                        break;
                    case "chkRB2_1":
                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeRB2_1.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 5).ToString();

                        }
                        else
                        {
                            txtBarcodeRB2_1.Text = string.Empty;
                        }
                        break;

                    case "chkRB2_2":

                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeRB2_2.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 6).ToString();
                        }
                        else
                        {
                            txtBarcodeRB2_2.Text = string.Empty;
                        }
                        break;
                    case "chkReK1R":
                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeReK1R.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 7).ToString();

                        }
                        else
                        {
                            txtBarcodeReK1R.Text = string.Empty;
                        }
                        break;

                    case "chkReK1F":

                        if (chkTemp.Checked == true)
                        {
                            txtBarcodeReK1F.Text = (Convert.ToInt64(txtWorksheetBarcode.Text) + 8).ToString();
                        }
                        else
                        {
                            txtBarcodeReK1F.Text = string.Empty;
                        }
                        break;
                }
            }
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
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
                //mark all samples inside pnlWP7Samples as not ticked
                ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));
                if (mpCPH != null)
                {

                    ContentPlaceHolder mpCPH_SCMP = (ContentPlaceHolder)(mpCPH.FindControl(strNestedCPH));

                    //get the panel

                    Panel pnl = (Panel)(mpCPH_SCMP.FindControl("pnlWP7Samples"));
                    if (pnl != null)
                    {
                        CheckBox chb;

                        foreach (Control c in pnl.Controls)
                        {
                            if (c.GetType() == typeof(CheckBox))
                            {
                                chb = (CheckBox)c;

                                chb.Checked = false;
                            }
                        }

                    }


                }
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