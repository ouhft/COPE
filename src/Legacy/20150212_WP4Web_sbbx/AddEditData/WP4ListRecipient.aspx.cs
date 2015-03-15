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

public partial class AddEditData_WP4ListRecipient : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STR_DEFAULT_SELECTION = "Unknown";
        private const string STR_YES_SELECTION = "YES";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                lblUserMessages.Text = string.Empty;

                lblDescription.Text = "Select Country Codes and/or Active and then click on display to display Recipients.";

                string STRSQL = string.Empty;
                if (!string.IsNullOrEmpty(SessionVariablesAll.AdminSuperUser))
                {
                    if (SessionVariablesAll.AdminSuperUser == "YES")
                    {
                        STRSQL = @"SELECT t2.CountryCode, CONCAT(t2.CountryCode, ' - ',Country) CountryDetails  FROM lstcentres t1 
                                    INNER JOIN lstcountries t2 ON t1.CountryCode=t2.CountryCode
                                    GROUP BY t1.CountryCode";

                        sqldsCentreLists.SelectParameters.Clear();


                        sqldsCentreLists.SelectCommand = STRSQL;
                        cblCentreList.DataSource = sqldsCentreLists;
                        cblCentreList.DataBind();

                    }
                    else
                    {
                        //get all the centres an individual can  access
                        STRSQL = "SELECT t2.CountryCode, CONCAT(t2.CountryCode, ' - ',Country) CountryDetails  FROM lstcentres t1 ";
                        STRSQL += "INNER JOIN lstcountries t2 ON t1.CountryCode=t2.CountryCode ";
                        STRSQL += "INNER JOIN copewpfourother.listdbuser t3 ON t3.centrecode=CONCAT(t1.CountryCode, t1.CentreCode)";
                        STRSQL += "INNER JOIN copewpfourother.listusers t4 ON t3.ListUsersID=t4.ListusersID ";
                        STRSQL += "WHERE t4.username=?UserName AND  (t3.AddEditRecipient='YES' OR t3.AddEditFollowUp='YES') ";
                        //STRSQL += "WHERE t4.username='" + SessionVariablesAll.UserName + "' ";
                        STRSQL += "";
                        STRSQL += "GROUP BY t1.CountryCode ";

                        sqldsCentreLists.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);
                        sqldsCentreLists.SelectCommand = STRSQL;

                        cblCentreList.DataSource = sqldsCentreLists;
                        cblCentreList.DataBind();

                    }

                    //mark current centre as selected

                    ListItem li;
                    li = cblCentreList.Items.FindByValue(SessionVariablesAll.CentreCode.Substring(0, 1));
                    if (li != null)
                    {
                        li.Selected = true;
                        //cblCentreList.SelectedItem.Value = li.Value;
                    }
                }

                cblActive.DataSource = XMLActiveDataSource;
                cblActive.DataBind();

                cmdToggle.Text = "Select All";
                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";
                BindData();


            }
            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
            }
        }

    }

    protected void BindData()
    {
        try
        {
            if (cblCentreList.SelectedIndex == -1)
            {
                throw new Exception("Please Select at least one Country.");


            }

            string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");

            ArrayList arlCentres = new ArrayList();
            ArrayList arlActive = new ArrayList();

            for (int i = 0; i < cblCentreList.Items.Count; i++)
            {
                if (cblCentreList.Items[i].Selected)
                {
                    arlCentres.Add(cblCentreList.Items[i].Value);
                }
            }


            for (int i = 0; i < cblActive.Items.Count; i++)
            {
                if (cblActive.Items[i].Selected)
                {
                    arlActive.Add(cblActive.Items[i].Value);
                }
            }

            lblGV1.Text = string.Empty;

            string STRSQL = string.Empty;
            STRSQL += "SELECT t1.*, t2.DonorID DonorIdentifier, ";
            STRSQL += "CONCAT(t4.CountryCode, t4.CentreCode, ' - ' , t4.CentreName) t4CentreNameMerged, ";
            STRSQL += "IF(t5.KidneyDiscarded='YES', t5.KidneyDiscarded, NULL) KidneyDiscarded, t6.ReasonWithdrawn, t8.ConsentAdditionalSamples, ";
            STRSQL += "DATE_FORMAT( t2.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirth, ";
            STRSQL += "DATE_FORMAT( t7.DeathDate, '%d/%m/%Y') Death_Date ";
            STRSQL += "FROM trialdetails_recipient  t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";

            string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");
            if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION || strIsServer == "0")
            {
                //do nothing

            }
            else
            {
                //filter only those where a user has access
                STRSQL += @"INNER JOIN (SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    AND (t2.AddEditRecipient='" + STR_YES_SELECTION + "' OR  t2.AddEditFollowUp='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode) t3 ON CAST(t1.TransplantCentre AS UNSIGNED)=CAST(t3.Centre AS UNSIGNED) ";
            }

            STRSQL += "LEFT JOIN lstcentres t4 ON  CAST(CONCAT(t4.CountryCode, t4.CentreCode) AS UNSIGNED)=CAST(t1.TransplantCentre AS UNSIGNED) ";
            STRSQL += "LEFT JOIN r_perioperative t5 ON t1.TrialIDRecipient=t5.TrialIDRecipient ";
            STRSQL += "LEFT JOIN trialidwithdrawn t6 ON t1.TrialIDRecipient=t6.TrialIDRecipient ";
            STRSQL += "LEFT JOIN r_deceased t7 ON t1.TrialIDRecipient=t7.TrialIDRecipient ";
            STRSQL += "LEFT JOIN specimenmaindetails t8 ON t1.TrialIDRecipient=t8.TrialIDRecipient ";
            STRSQL += "WHERE  t1.TrialIDRecipient LIKE ?TrialIDRecipientMain ";


            STRSQL += "AND (";
            for (int i = 0; i <= arlCentres.Count - 1; i++)
            {
                if (i == 0)
                {
                    STRSQL += "t1.TransplantCentre LIKE '" + arlCentres[i].ToString() + "%' ";
                }
                else
                {
                    STRSQL += "OR t1.TransplantCentre LIKE '" +  arlCentres[i].ToString() + "%' ";
                }
            }
            STRSQL += ") ";


            if (arlActive.Count > 0)
            {


                STRSQL += "AND (";

                for (int i = 0; i <= arlActive.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        STRSQL += "OR ";

                        //strActiveTypeSelection += ",";
                    }

                    STRSQL += "t1.Active='" + arlActive[i].ToString() + "' ";
                    //strActiveTypeSelection += arlActive[i].ToString();
                }


                STRSQL += ") ";
            }

            if (Request.QueryString["QT"] == "TID_R")
            {
                if (Request.QueryString["ES"] == "1")
                {
                    STRSQL += "AND t1.TrialIDRecipient=?TrialIDRecipient ";
                }
                else
                {
                    STRSQL += "AND t1.TrialIDRecipient LIKE ?TrialIDRecipient ";
                }
            }
            else if (Request.QueryString["QT"] == "RID")
            {
                if (Request.QueryString["ES"] == "1")
                {
                    STRSQL += "AND t1.RecipientID=?RecipientID ";
                }
                else
                {
                    STRSQL += "AND t1.RecipientID LIKE ?RecipientID ";
                }
            }
            else
            {
                //do nothing
            }

            if (ViewState["SortField"].ToString() == "TrialIDRecipient")
            {
                STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
            }
            else
            {
                STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                STRSQL += ", TrialIDRecipient ";
            }

            //string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");

            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialIDRecipientMain", strTrialIDLeadingCharacters  + "%");
            sqldsGV1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            if (Request.QueryString["QT"] == "TID_R")
            {
                if (Request.QueryString["ES"] == "1")
                {
                    sqldsGV1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["QV"]);
                }
                else
                {
                    sqldsGV1.SelectParameters.Add("?TrialIDRecipient", "%" + Request.QueryString["QV"] + "%");
                }
            }
            else if (Request.QueryString["QT"] == "RID")
            {
                if (Request.QueryString["ES"] == "1")
                {

                    sqldsGV1.SelectParameters.Add("?RecipientID", Request.QueryString["QV"]);
                }
                else
                {
                    sqldsGV1.SelectParameters.Add("?RecipientID", "%" + Request.QueryString["QV"] + "%");
                }
            }
            else
            {
                //do nothing
            }

            sqldsGV1.SelectCommand = STRSQL;
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();

            lblGV1.Text = "Total Number of of Recipients " + GV1.Rows.Count.ToString() + ". Click on TrialID (Recipient) to Edit Recipient Data.";
        }

        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding data.";
        }



    }

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

    protected void cblCentreList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (cblCentreList.SelectedIndex == -1)
            {
                throw new Exception("Please Select at least one Country.");


            }

            BindData();
        }

        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting a Country.";
        }
    }
    protected void cmdToggle_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmdToggle.Text == "Select All")
            {
                foreach (ListItem li in cblCentreList.Items)
                {
                    li.Selected = true;
                }
                cmdToggle.Text = "Deselect All";
            }

            else
            {
                foreach (ListItem li in cblCentreList.Items)
                {
                    //if (li.Value != SessionVariablesAll.CentreCode)
                    //{
                    //    li.Selected = false;
                    //}
                    //else
                    //{
                    //    li.Selected = true;
                    //}
                    li.Selected = false;
                }

                cmdToggle.Text = "Select All";
            }


            //BindData();

        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while clicking on Select/ Deselect All button.";
        }
    }
    protected void cmdReset_Click(object sender, EventArgs e)
    {
        try
        {
            cblCentreList.SelectedIndex = -1;
            cblActive.SelectedIndex = -1;
            

            cmdToggle.Text = "Select All";
            lblGV1.Text = string.Empty;
            GV1.DataSource = null;
            GV1.DataBind();


        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + "An error occured while resetting values.";
        }
    }
    protected void cmdDisplay_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (cblCentreList.SelectedIndex == -1)
            {
                throw new Exception("Please Select at least one Country.");
            }

            BindData();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + "An error occured while displaying data.";
        }
    }
}