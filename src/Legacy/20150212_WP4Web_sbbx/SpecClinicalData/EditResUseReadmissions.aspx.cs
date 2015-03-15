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

public partial class SpecClinicalData_EditResUseReadmissions : System.Web.UI.Page
{
    #region " Private Constants & Variables "
    private const string STRCONN = "cope4dbconn";

    private const string strOccasionLike = "Month";

    private const string strMainCPH = "cplMainContents";
    private const string strMainLabel = "lblDonorID";
    private const string strRecipientLabel = "lblRecipientID";

    private const string STR_OTHER_SELECTION = "Other";
    private const string STR_UNKNOWN_SELECTION = "Unknown";
    private const string STR_NO_SELECTION = "NO";
    private const string STR_YES_SELECTION = "YES";

    private const string STR_DD_UNKNOWN_SELECTION = "0";
    //static Random _random = new Random();

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

                if (Request.QueryString["RReadmissionsID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain Unique Identifier.");
                }

                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK selected data will be deleted.";

                //cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                //ddOccasion.DataSource = XMLFollowupsDataSource;
                //ddOccasion.DataBind();
                //rblKidneyDiscarded.SelectedValue = STR_UNKNOWN_SELECTION;

                //string STRSQL = "";
                //STRSQL += "SELECT RFUPostTreatmentID UniqueID, CONCAT(DATE_FORMAT(FollowUpDate, '%d/%m/%Y'), ' (', Occasion, ')') FollowUp FROM  r_fuposttreatment ";
                //STRSQL += "WHERE TrialID=?TrialID AND Occasion LIKE ?Occasion ";
                //STRSQL += "ORDER BY FollowUpDate";

                //SQLDS_Occasion.SelectCommand = STRSQL;
                //SQLDS_Occasion.SelectParameters.Clear();
                //SQLDS_Occasion.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());
                //SQLDS_Occasion.SelectParameters.Add("?Occasion", strOccasionLike + "%");

                //ddOccasion.DataSource = SQLDS_Occasion;
                //ddOccasion.DataBind();

                ddOccasion.Items.Clear();
                ListItem li = new ListItem();

                li.Text = "Select Occasion";
                li.Value = "0";
                li.Selected = true;
                ddOccasion.Items.Add(li);


                string strOccasionDay30 = "1 Month";
                li = new ListItem();
                li.Text = strOccasionDay30;
                li.Value = strOccasionDay30;
                li.Selected = false;

                ddOccasion.Items.Add(li);

                string strOccasionMonth6 = "6 Months";
                li = new ListItem();
                li.Text = strOccasionMonth6;
                li.Value = strOccasionMonth6;
                ddOccasion.Items.Add(li);

                ddReAdmissionType.DataSource = XMLReAdmissionTypesDataSource;
                ddReAdmissionType.DataBind();


                txtDateAdmission_CalendarExtender.EndDate = DateTime.Today;
                txtDateDischarge_CalendarExtender.EndDate = DateTime.Today;


                rblRequiredSurgery.DataSource = XMLMainOptionsDataSource;
                rblRequiredSurgery.DataBind();
                rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;

                rblITU_HDU.DataSource = XMLMainOptionsDataSource;
                rblITU_HDU.DataBind();
                rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;

                if (!string.IsNullOrEmpty(Request.QueryString["Occasion"]))
                {
                }

                ViewState["SortField"] = "DateAdmission";
                ViewState["SortDirection"] = "ASC";

                BindData();

                AssignData();

                ViewState["SortFieldGV2"] = "Occasion";
                ViewState["SortDirectionGV2"] = "ASC";

                BindDataGV2();


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

            strSQL += "SELECT t1.*, t2.TrialID,  ";
            strSQL += "DATE_FORMAT(t1.DateAdmission, '%d/%m/%Y') Date_Admission, ";
            strSQL += "DATE_FORMAT(t1.DateDischarge, '%d/%m/%Y') Date_Discharge ";
            strSQL += "FROM r_readmissions t1 ";
            strSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on Date Admission to Edit Recipient Readmissions Data.";

            lblDescription.Text = "Update  Recipient Readmissions Data for " + Request.QueryString["TID_R"].ToString();




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
                if (String.IsNullOrEmpty(Request.QueryString["RReadmissionsID"]) == false)
                {
                    {
                        if (drv["RReadmissionsID"].ToString() == Request.QueryString["RReadmissionsID"].ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }
            }
        }
    }

    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = string.Empty;

            STRSQL += "SELECT * FROM r_readmissions WHERE TrialIDRecipient=?TrialIDRecipient AND RReadmissionsID=?RReadmissionsID ";

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

            MyCMD.Parameters.Add("?RReadmissionsID", MySqlDbType.VarChar).Value = Request.QueryString["RReadmissionsID"];

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["Occasion"]))
                            {
                                ddOccasion.SelectedValue = myDr["Occasion"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["ReAdmissionType"]))
                            {
                                ddReAdmissionType.SelectedValue = myDr["ReAdmissionType"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["DateAdmission"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateAdmission"].ToString()) == true)
                                    txtDateAdmission.Text = Convert.ToDateTime(myDr["DateAdmission"]).ToShortDateString();
                            }

                            if (!DBNull.Value.Equals(myDr["DateDischarge"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateDischarge"].ToString()) == true)
                                    txtDateDischarge.Text = Convert.ToDateTime(myDr["DateDischarge"]).ToShortDateString();
                            }

                            if (!DBNull.Value.Equals(myDr["RequiredSurgery"]))
                            {
                                rblRequiredSurgery.SelectedValue = myDr["RequiredSurgery"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["ITU_HDU"]))
                            {
                                rblITU_HDU.SelectedValue = myDr["ITU_HDU"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["DaysHospital"]))
                            {
                                txtDaysHospital.Text = myDr["DaysHospital"].ToString();
                            }


                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = myDr["Comments"].ToString();
                            }

                            ddReAdmissionType_SelectedIndexChanged(this, EventArgs.Empty);
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

                lblUserMessages.Text = ex.Message + " An error occured while executing assign query. ";
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["TID"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }

            string STRSQL = String.Empty;

            STRSQL += "DELETE FROM r_readmissions WHERE TrialID=?TrialID AND RReadmissionsID=?RReadmissionsID ";

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

            MyCMD.Parameters.Add("?RReadmissionsID", MySqlDbType.VarChar).Value = Request.QueryString["RReadmissionsID"];


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

                lblUserMessages.Text = ex.Message + " An error occured while executing 'Readmissions Data' query.";
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting 'Readmissions Data'.";
        }
    }

    // reset page
    //protected void cmdReset_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Response.Redirect(Request.Url.AbsoluteUri, false);
    //        //lblUserMessages.Text = "";
    //    }

    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
    //    }
    //}
    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select an Occasion.");
            }

            if (ddReAdmissionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select Re-Admission Type.");
            }

            if (txtDateAdmission.Text == string.Empty)
            {
                throw new Exception("Please Enter Date of Admission.");
            }

            if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
            {
                throw new Exception("Please Enter Date of Admission in the correct format.");
            }

            if (Convert.ToDateTime(txtDateAdmission.Text) > DateTime.Today)
            {
                throw new Exception("Date of Admission cannot be greater than Today's Date.");
            }

            //get the follow up date
            string STRSQL_FOLLOWUPDATE = "SELECT FollowUpDate FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";

            string strFollowUpDate = GeneralRoutines.ReturnScalarTwo(STRSQL_FOLLOWUPDATE, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?Occasion", ddOccasion.SelectedValue, STRCONN);

            //if (GeneralRoutines.IsDate(strFollowUpDate) == false)
            //{
            //    throw new Exception("Could not obtain the follow up date for selected Occasion.");
            //}
            //else
            //{
            //    if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(strFollowUpDate))
            //    {
            //        throw new Exception("'Date of Admission' can not be later than 'Follow Up Date' " + Convert.ToDateTime(strFollowUpDate).ToShortDateString() + ".");
            //    }
            //}

            if (GeneralRoutines.IsDate(strFollowUpDate) == true)
            {
                if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(strFollowUpDate))
                {
                    throw new Exception("'Date of Admission' can not be later than 'Follow Up Date' " + Convert.ToDateTime(strFollowUpDate).ToShortDateString() + ".");
                }
            }

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_readmissions WHERE TrialIDRecipient=?TrialIDRecipient AND DateAdmission=?DateAdmission AND RReadmissionsID <> ?RReadmissionsID";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarThree(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?DateAdmission", Convert.ToDateTime(txtDateAdmission.Text).ToString("yyyy-MM-dd"), "?RReadmissionsID", Request.QueryString["RReadmissionsID"].ToString(), STRCONN));

            if (intCountFind > 0)
            {
                throw new Exception("There already exists an Admission data for the date you have entered. Please Edit the existing data.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("Could not check if there already exists an Admission Up data for the date you have entered.");
            }




            if (txtDateDischarge.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
                {
                    throw new Exception("Please Enter Date of Discharge in the correct format.");
                }


                if (Convert.ToDateTime(txtDateDischarge.Text) > DateTime.Today)
                {
                    throw new Exception("Date of Discharge cannot be greater than Today's Date.");
                }

                if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(txtDateDischarge.Text))
                {
                    throw new Exception("Date of Admission cannot be greater than Date of Discharge.");
                }


            }

            //if (rblITU_HDU.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select if 'ITU/HDU Admission' was also required.");
            //}

            if (txtDaysHospital.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtDaysHospital.Text) == false)
                {
                    throw new Exception("The 'Number of Days' should be numeric.");
                }
            }

            //if (txtComments.Text == string.Empty)
            //{
            //    throw new Exception("Please Enter Reason for Readmission.");
            //}


            //add the data
            string STRSQL = String.Empty;
            STRSQL += "UPDATE r_readmissions SET ";
            STRSQL += "Occasion=?Occasion, ReAdmissionType=?ReAdmissionType, DateAdmission=?DateAdmission, DateDischarge=?DateDischarge, RequiredSurgery=?RequiredSurgery, ";
            STRSQL += "ITU_HDU=?ITU_HDU, DaysHospital=?DaysHospital, ";
            STRSQL += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND RReadmissionsID=?RReadmissionsID ";

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
            MyCMD.Parameters.Add("?RReadmissionsID", MySqlDbType.VarChar).Value = Request.QueryString["RReadmissionsID"].ToString();

            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

            if (ddReAdmissionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ReAdmissionType", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReAdmissionType", MySqlDbType.VarChar).Value = ddReAdmissionType.SelectedValue;
            }



            if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
            {
                MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateAdmission.Text);
            }

            if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
            {
                MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateDischarge.Text);
            }

            if (rblRequiredSurgery.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RequiredSurgery", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RequiredSurgery", MySqlDbType.VarChar).Value = rblRequiredSurgery.SelectedValue;
            }

            if (rblITU_HDU.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ITU_HDU", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ITU_HDU", MySqlDbType.VarChar).Value = rblITU_HDU.SelectedValue;
            }

            if (string.IsNullOrEmpty(txtDaysHospital.Text))
            {
                MyCMD.Parameters.Add("?DaysHospital", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DaysHospital", MySqlDbType.VarChar).Value = txtDaysHospital.Text;
            }

            if (string.IsNullOrEmpty(txtComments.Text))
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
                BindData();
                lblUserMessages.Text = "'Readmissions Data' Updated.";
            }

            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing 'Readmissions Data' Update query.";
            }


                finally //close connection
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Adding Data.";
        }
    }
    protected void ddReAdmissionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddReAdmissionType.SelectedValue == "Hospital Admission")
        {
            pnlHospitalAdmission.Visible = true;

        }
        else
        {
            pnlHospitalAdmission.Visible = false;
            rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;
            rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;
            txtDaysHospital.Text = string.Empty;

        }

    }

    protected void BindDataGV2()
    {
        try
        {
            if (Request.QueryString["TID"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.RecipientID, t2.TrialID, ";
            strSQL += "DATE_FORMAT(t1.DateEntered, '%d/%m/%Y') Date_AE, ";
            strSQL += "DATE_FORMAT(t1.DateCreated, '%d/%m/%Y %H:%i') Date_Created, ";
            strSQL += "DATE_FORMAT(t2.DateOfBirth, '%d/%m/%Y') Recipient_DateOfBirth ";
            strSQL += "FROM resuse t1 ";
            strSQL += "INNER JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortFieldGV2"] + " " + (string)ViewState["SortDirectionGV2"];


            GV2.DataSource = SqlDataSource2;
            SqlDataSource2.SelectCommand = strSQL;
            SqlDataSource2.SelectParameters.Clear();
            SqlDataSource2.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV2.DataBind();

            if (GV2.Rows.Count > 0)
            {
                lblGV2.Text = "Click on TrialID to Edit 'Resource Use Log' Data.";
            }
            else
            {
                lblGV2.Text = "'Resource Use Log' Data has not been added.";
            }



        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding the page.";
        }

        //lblUserMessages.Text = strSQL;
    }

    //sorting main datagrid
    protected void GV2_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression.ToString() == ViewState["SortFieldGV2"].ToString())
        {
            switch (ViewState["SortDirectionGV2"].ToString())
            {
                case "ASC":
                    ViewState["SortDirectionGV2"] = "DESC";
                    break;
                case "DESC":
                    ViewState["SortDirectionGV2"] = "ASC";
                    break;
            }

        }
        else
        {
            ViewState["SortFieldGV2"] = e.SortExpression;
            ViewState["SortDirectionGV2"] = "DESC";
        }
        BindDataGV2();
    }

    protected void GV2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)(e.Row.DataItem);
                //if (String.IsNullOrEmpty(Request.QueryString["ResUseID"]) == false || String.IsNullOrEmpty(Request.QueryString["Occasion"]) == false)
                //{
                //    if (drv["ResUseID"].ToString() == Request.QueryString["ResUseID"].ToString())
                //    {
                //        e.Row.BackColor = System.Drawing.Color.LightBlue;
                //    }
                //    if (drv["Occasion"].ToString() == Request.QueryString["Occasion"].ToString())
                //    {
                //        e.Row.BackColor = System.Drawing.Color.LightBlue;
                //    }
                //}

                if (String.IsNullOrEmpty(Request.QueryString["Occasion"]) == false)
                {
                    
                    if (drv["Occasion"].ToString() == Request.QueryString["Occasion"].ToString())
                    {
                        e.Row.BackColor = System.Drawing.Color.LightBlue;
                    }
                }

                
            }
        }
    }

}