using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _WP4HomePage : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        // private const string STRCONN = "cope4dbconn";

        private const string REDIRECT_ADD = "~/AddEditData/AddWP4Donor.aspx";
        private const string KeyDbName = "dbname";

        private const string REDIRECT_ADD_RECIPIENT = "~/AddEditData/AddWP4Recipient.aspx";

        private const string REDIRECT_VIEW = "~/AddEditData/WP4TList.aspx";
        private const string REDIRECT_TRIALID = "~/SpecClinicalData/ViewSummary.aspx?TID=";
        private const int intLengthTrialID = 5;

        private const string REDIRECT_VIEWRECIPIENT = "~/AddEditData/WP4ListRecipient.aspx";
        private const string REDIRECT_RECIPIENT = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //lblUserMessages.Text = "User " + SessionVariablesAll.UserName;
                //lblUserMessages.Text += ", AdminSuperUser " + SessionVariablesAll.AdminSuperUser;
                //lblUserMessages.Text += ", SuperUser " + SessionVariablesAll.SuperUser;
                //lblUserMessages.Text += ", CentreCode " + SessionVariablesAll.CentreCode;
                //lblUserMessages.Text += ", Randomise " + SessionVariablesAll.Randomise;
                string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");

                txtTrialID.Text = strTrialIDLeadingCharacters + SessionVariablesAll.CentreCode.Substring(0, 1);
                txtRecipient.Text = strTrialIDLeadingCharacters + SessionVariablesAll.CentreCode.Substring(0, 1);

                ViewState["SortField"] = "CentreCode";
                ViewState["SortDirection"] = "ASC";

                if (string.IsNullOrEmpty(SessionVariablesAll.AdminSuperUser) == false)
                {
                    if (SessionVariablesAll.AdminSuperUser == "YES")
                    {
                        lblGV1.Text = "As a Super Admin User you have access to all the centres.";
                    }
                    else
                    {
                        BindData();
                    }
                }
                else
                {
                    BindData();
                }
            }
        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while loading the page.";
        }
    }

    protected void cmdAddKidney_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            Response.Redirect(REDIRECT_ADD, false);
            // HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while redirecting to Add Kidney page.";
        }
    }
    protected void cmdEditExisting_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            Response.Redirect(REDIRECT_VIEW, false);
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while redirecting to Add Kidney page.";
        }
    }
    protected void cmdGoTo_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (txtTrialID.Text == string.Empty)
            {
                throw new Exception("Please Enter TrialID");
            }

            if (txtTrialID.Text.Length != ConstantsGeneral.intTrialIDMaximumLength)
            {
                throw new Exception("The length of TrialID should be " + ConstantsGeneral.intTrialIDMaximumLength.ToString() + ".");
            }
            if (txtTrialID.Text.Substring(0, 3) != ConstantsGeneral.LeadingChars)
            {
                throw new Exception("The first three characters of TrialID should be " + ConstantsGeneral.LeadingChars);
            }

            //if (txtTrialID.Text.Substring(3, 3) != SessionVariablesAll.CentreCode)
            //{
            //    throw new Exception("The first two characters of TrialID should be " + SessionVariablesAll.CentreCode);
            //}

            Response.Redirect(REDIRECT_TRIALID + txtTrialID.Text, false);

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while redirecting to Add Kidney page.";
        }
    }
    protected void cmdAddRecipient_Click(object sender, EventArgs e)
    {
        try 
        {
            lblUserMessages.Text = string.Empty;
            Response.Redirect(REDIRECT_ADD, false);
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while redirecting to Add Recipient Page.";
        }
    }
    protected void cmdEditingExisting_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            Response.Redirect(REDIRECT_VIEWRECIPIENT, false);
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while redirecting to View Recipient  Datapage.";
        }
    }
    protected void cmdGoToRecipient_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (txtRecipient.Text.Length!=ConstantsGeneral.intTrialIDRecipientMaximumLength)
            {
                throw new Exception("The length of TrialID (Recipient) should be" + ConstantsGeneral.intTrialIDRecipientMaximumLength.ToString() + " .");
            }

            if (txtRecipient.Text.Substring(0, 3) != ConstantsGeneral.LeadingChars)
            {
                throw new Exception("The first three characters of TrialID should be " + ConstantsGeneral.LeadingChars + ".");
            }

            Response.Redirect(REDIRECT_RECIPIENT + txtRecipient.Text.Substring(0, 8) + "&TID_R=" + txtRecipient.Text, false);
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while redirecting to View Recipient  Datapage.";
        }
    }

    protected void BindData()
    {
        try
        {

            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

            //string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

            string strSQL = String.Empty;

            strSQL += "SELECT t1.ListUsersID, t1.UserName, t1.Active, t1.Firstname, t1.LastName,t1.Email,t1.JobTitle,t1.LockedUser, ";
            //strSQL += "t2.ID, t2.CentreCode, t2.AdminSuperUser, t2.SuperUser, t2.AddEdit, t2.Randomise,t2.PersonalData,";
            strSQL += "t2.*,";
            strSQL += "t3.LastLogin, ";
            strSQL += "CONCAT(t4.CountryCode, t4.CentreCode, ' - ' , t4.CentreName) CentreDetails ";
            strSQL += "FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  AND t2.dataname='" + STRDbName + "' ";
            strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
            strSQL += "ON t1.UserName=t3.UserID ";
            strSQL += "INNER JOIN cope_wp_four.lstcentres t4 ON CAST(t2.CentreCode AS CHAR)=CONCAT(t4.CountryCode, t4.CentreCode) ";
            //strSQL += "AND t2.dataname='" + STRDbName + "' ";
            //strSQL += "AND t1.centre='" + STRCentre + "' ";
            //strSQL += "AND t2.centrecode='" + STRCentre + "' ";
            //strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
            strSQL += "WHERE (t1.Active <> 0 OR t1.Active IS NULL) ";
            strSQL += "AND t1.UserName=?UserName ";
            if ((string)ViewState["SortField"] == "CentreCode")
            {
                strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
                strSQL += ", UserName ";
            }
            else if ((string)ViewState["SortField"] == "UserName")
            {
                strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
                strSQL += ", CentreCode ";
            }
            else
            {
                strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            }


            //lblUserMessages.Text = strSQL;

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            GV1.DataBind();
            lblGV1.Text = "The table below displays your Current Access Rights.";
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
}