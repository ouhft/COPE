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

public partial class SpecClinicalData_AddFUReadmissions : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";
        
        private const string strExcludeOccasion = "1-7 Days";
        
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

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (Request.QueryString["TID_R"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID (Recipient)");
                }


                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                //cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                //ddOccasion.DataSource = XMLFollowupsDataSource;
                //ddOccasion.DataBind();
                //rblKidneyDiscarded.SelectedValue = STR_UNKNOWN_SELECTION;

                string STRSQL = "";
                STRSQL += "SELECT RFUPostTreatmentID UniqueID, CONCAT(DATE_FORMAT(FollowUpDate, '%d/%m/%Y'), ' (', Occasion, ')') FollowUp FROM  r_fuposttreatment ";
                STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion <> ?Occasion ";
                STRSQL += "ORDER BY FollowUpDate";
                    
                SQLDS_Occasion.SelectCommand = STRSQL;
                SQLDS_Occasion.SelectParameters.Clear();
                SQLDS_Occasion.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());
                SQLDS_Occasion.SelectParameters.Add("?Occasion", strExcludeOccasion);

                ddOccasion.DataSource = SQLDS_Occasion;
                ddOccasion.DataBind();

                
                txtDateAdmission_CalendarExtender.EndDate = DateTime.Today;
                txtDateDischarge_CalendarExtender.EndDate = DateTime.Today;

                ddICU.DataSource = XMLMainOptionsDataYNSource;
                ddICU.DataBind();

                ddNeedDialysis.DataSource = XMLMainOptionsDataYNSource;
                ddNeedDialysis.DataBind();

                ddBiopsyTaken.DataSource = XMLMainOptionsDataYNSource;
                ddBiopsyTaken.DataBind();

                ddSurgery.DataSource = XMLMainOptionsDataYNSource;
                ddSurgery.DataBind();

                ViewState["SortField"] = "DateAdmission";
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
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
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
            if (GV1.Rows.Count>0)
            {
                lblGV1.Text = "Click on Date Admission to Edit Recipient Readmissions Data";
            }
            else
            {
                lblGV1.Text = "Readmission Data not added";
            }
            

            lblDescription.Text = "Add  Recipient Readmissions Data for " + Request.QueryString["TID"].ToString() + " ";
            //if (strRecipientID != string.Empty)
            //{ lblDescription.Text += " and RecipientID " + strRecipientID; }



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

            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select an Occasion.");
            }


            

            if (txtDateAdmission.Text == string.Empty)
            {
                throw new Exception("Please Enter Date of Admission.");
            }

            if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
            {
                throw new Exception("Please Enter Date of Admission in the correct format.");
            }

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_readmissions WHERE TrialIDRecipient=?TrialIDRecipient AND DateAdmission=?DateAdmission ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?DateAdmission", Convert.ToDateTime(txtDateAdmission.Text).ToString("yyyy-MM-dd"), STRCONN));

            if (intCountFind > 0)
            {
                throw new Exception("There already exists an Admission data for the date you have entered. Please Edit the existing data.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("Could not check if there already exists na Admission Up data for the date you have entered.");
            }


            if (Convert.ToDateTime(txtDateAdmission.Text) > DateTime.Today)
            {
                throw new Exception("Date of Admission cannot be greater than Today's Date.");
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

            if (txtComments.Text == string.Empty)
            {
                throw new Exception("Please Enter Reason for Readmission.");
            }


            //add the data
            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO r_readmissions ";
            STRSQL += "(TrialIDRecipient,  RFUPostTreatmentID, Occasion, DateAdmission, DateDischarge, ICU, NeedDialysis, BiopsyTaken, Surgery, ";
            STRSQL += "ReasonAdmission, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?RFUPostTreatmentID, ?Occasion, ?DateAdmission, ?DateDischarge, ?ICU, ?NeedDialysis, ?BiopsyTaken, ?Surgery,";
            STRSQL += "?ReasonAdmission, ?DateCreated, ?CreatedBy) ";

            string STRSQL_LOCK = string.Empty ;
            STRSQL_LOCK += "UPDATE r_readmissions SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "AND DateAdmission=?DateAdmission ";
            //STRSQL_LOCK += "AND Occasion IS NOT NULL AND DateAdmission IS NOT NULL AND DateDischarge IS NOT NULL ";
            //STRSQL_LOCK += "AND ICU IS NOT NULL AND NeedDialysis IS NOT NULL AND  BiopsyTaken IS NOT NULL AND Surgery IS NOT NULL ";
            //STRSQL_LOCK += "AND ReasonAdmission IS NOT NULL";
            //STRSQL_LOCK += "";
            //STRSQL_LOCK += "";

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
            MyCMD.Parameters.Add("?RFUPostTreatmentID", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedItem.Text;


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

            if (ddICU.SelectedIndex==-1 || ddICU.SelectedValue== STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ICU", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ICU", MySqlDbType.VarChar).Value = ddICU.SelectedValue;
            }

            if (ddNeedDialysis.SelectedIndex == -1 || ddNeedDialysis.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?NeedDialysis", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NeedDialysis", MySqlDbType.VarChar).Value = ddNeedDialysis.SelectedValue;
            }

            if (ddBiopsyTaken.SelectedIndex == -1 || ddBiopsyTaken.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?BiopsyTaken", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?BiopsyTaken", MySqlDbType.VarChar).Value = ddBiopsyTaken.SelectedValue;
            }

            if (ddSurgery.SelectedIndex == -1 || ddSurgery.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Surgery", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Surgery", MySqlDbType.VarChar).Value = ddSurgery.SelectedValue;
            }


            if (string.IsNullOrEmpty(txtComments.Text))
            {
                MyCMD.Parameters.Add("?ReasonAdmission", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonAdmission", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = 1;

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                //lock data
                if (chkAllDataAdded.Checked == true)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQL_LOCK;
                    MyCMD.ExecuteNonQuery();

                }

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }


                //redirect
                Response.Redirect(Request.Url.AbsoluteUri, false);

                BindData();
                lblUserMessages.Text = "Data Added.";


            }

            catch (System.Exception ex)
            {

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing insert query.";
            }




           

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Adding Data.";
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