using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;

using System.Collections;


using MySql.Data;
using MySql.Data.MySqlClient;

public partial class AddEditData_WP4TList : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        //private const string REDIRECTPAGE_DONOR = "~/AddEditData/WP4TList.aspx?";
        //private const string REDIRECTPAGE_RECIPIENT = "~/AddEditData/WP4TListRecipient.aspx?";
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
                lblDescription.Text = "Select Country Codes to display Donors.";

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
                        STRSQL += "WHERE t4.username=?UserName AND  (t3.AddEdit='YES' OR t3.Randomise='YES' OR t3.ViewRandomise='YES') ";
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
                    li = cblCentreList.Items.FindByValue(SessionVariablesAll.CentreCode.Substring(0,1));
                    if (li != null)
                    {
                        li.Selected = true;
                        //cblCentreList.SelectedItem.Value = li.Value;
                    }
                }

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
            lblGV1.Text = string.Empty;

            if (cblCentreList.SelectedIndex == -1)
            {
                throw new Exception("Please Select at least one Country.");


            }

            string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");
            
            ArrayList arlCentres = new ArrayList();


            for (int i = 0; i < cblCentreList.Items.Count; i++)
            {
                if (cblCentreList.Items[i].Selected)
                {
                    arlCentres.Add(cblCentreList.Items[i].Value);
                }
            }

            string STRSQL = string.Empty;

            string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

            STRSQL += "SELECT t1.*, ";
            STRSQL += "CONCAT(t3.CountryCode, t3.CentreCode, ' - ' , t3.CentreName) t3CentreNameMerged, ";
            //STRSQL += "IF(t4.TrialID IS NOT NULL, 'YES', NULL) Randomised, ";
            STRSQL += "t4.DateCreated Randomised, ";
            STRSQL += "DATE_FORMAT(t1.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            STRSQL += "FROM trialdetails t1 ";
            if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION || strIsServer=="0")
            {
                //do nothing

            }
            else
            {
                //filter only those where a user has access
                STRSQL += @"INNER JOIN (SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CAST(CONCAT(t1.CountryCode, t1.CentreCode) AS UNSIGNED)=CAST(t2.centrecode AS UNSIGNED)
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    AND (t2.AddEdit='YES' OR t2.Randomise='YES' OR t2.ViewRandomise='YES') ORDER BY t1.CountryCode, t1.CentreCode) t2 ON CAST(t1.CentreCode AS UNSIGNED)=CAST(t2.Centre AS UNSIGNED) ";
            }

            STRSQL += "LEFT JOIN lstcentres t3 ON  CAST(CONCAT(t3.CountryCode, t3.CentreCode) AS UNSIGNED)=CAST(t1.centrecode AS UNSIGNED) ";
            STRSQL += "LEFT JOIN kidneyr t4 ON t1.TrialID=t4.TrialID ";
            STRSQL += "WHERE t1.TrialID  LIKE ?TrialIDMain ";

            STRSQL += "AND (";
            for (int i = 0; i <= arlCentres.Count - 1; i++)
            {
                if (i == 0)
                {
                    STRSQL += "t1.TrialID LIKE '" + strTrialIDLeadingCharacters + arlCentres[i].ToString() + "%' ";
                }
                else
                {
                    STRSQL += "OR t1.TrialID LIKE '" + strTrialIDLeadingCharacters+ arlCentres[i].ToString() + "%' ";
                }
            }
            STRSQL += ") ";


            if (Request.QueryString["QT"] == "DID")
            {
                if (Request.QueryString["ES"] == "1")
                {
                    STRSQL += "AND t1.DonorID=?DonorID ";
                }
                else
                {
                    STRSQL += "AND t1.DonorID LIKE ?DonorID ";
                }
            }
            else if (Request.QueryString["QT"] == "TID")
            {
                if (Request.QueryString["ES"] == "1")
                {
                    STRSQL += "AND t1.TrialID=?TrialID ";
                }
                else
                {
                    STRSQL += "AND t1.TrialID LIKE ?TrialID ";
                }
            }
            else
            {
                //do nothing
            }

            if (ViewState["SortField"].ToString() == "TrialID")
            {
                STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
            }
            else
            {
                STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();
                STRSQL += ", TrialID ";
            }


            lblGV1.Text = STRSQL;


            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialIDMain", strTrialIDLeadingCharacters  + "%");
            sqldsGV1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            if (Request.QueryString["QT"] == "DID")
            {
                if (Request.QueryString["ES"] == "1")
                {
                    // STRSQL += "AND t2.DonorID=?DonorID ";
                    sqldsGV1.SelectParameters.Add("?DonorID", Request.QueryString["QV"]);
                }
                else
                {
                    //STRSQL += "AND t2.DonorID LIKE ?DonorID ";
                    sqldsGV1.SelectParameters.Add("?DonorID", "%" + Request.QueryString["QV"] + "%");
                }
            }
            else if (Request.QueryString["QT"] == "TID")
            {
                if (Request.QueryString["ES"] == "1")
                {
                    sqldsGV1.SelectParameters.Add("?TrialID", Request.QueryString["QV"]);
                }
                else
                {
                    sqldsGV1.SelectParameters.Add("?TrialID", "%" + Request.QueryString["QV"] + "%");
                }
            }

            sqldsGV1.SelectCommand = STRSQL;
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();

            lblGV1.Text = "Total Number of of Donors. Click on TrialID to Edit Donor Data.";
        }

        catch (System.Exception ex)
        {
            lblGV1.Text += ex.Message + " An error occured while binding data.";
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


            BindData();

        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while clicking on Select/ Deselect All button.";
        }
    }
}