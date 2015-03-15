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

using System.Collections;
public partial class OtherArea_PENUser : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4usrconn";
    private const string strMainCPH = "cplMainContents";

    private const string KeyDbName = "dbname";
    private const string KeyCentreCode = "CentreCode";
    private const string KeyPassLength = "passlength";
    private const string KeyPassUniqueCharacter = "passuniquecharacter";
    private const int INT_ACTIVE = 1; //by default user is active

    private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx";

    private const string STR_YES_SELECTION = "YES";
    private const string STR_NO_SELECTION = "NO";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblUserMessages.Text = String.Empty;

                if (SessionVariablesAll.SuperUser != "YES")
                {
                    Response.Redirect(strAccessDenied, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }


                if (string.IsNullOrEmpty(Request.QueryString["OtherID"]))
                {
                    throw new Exception("Could not obtain the Unique identifier for the User whose access needs to be modified.");
                }

                //super admin user can change data on other users

                if (SessionVariablesAll.AdminSuperUser!=STR_YES_SELECTION)
                {
                    //check if the user is a superadmin user
                    string STRSQLFIND = "";

                    STRSQLFIND += "SELECT COUNT(*) CR FROM listdbuser t1 WHERE ListUsersID=?ListUsersID AND AdminSuperUser='YES' GROUP BY ListUsersID ";


                    Int16 intCountAdminSuperUser = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQLFIND, "?ListUsersID", Request.QueryString["OtherID"], STRCONN));

                    if (intCountAdminSuperUser > 0)
                    {
                        throw new Exception("You do not have the rights to modify useraccess for an 'Admin Super User'");
                    }
                }
                

                

                lblPageDescription.Text += "<br/>(Click on User in the table at the bottom of the page to modify access rights for an existing user.)";
                ViewState["SortField"] = "UserName";
                ViewState["SortDirection"] = "ASC";

                ViewState["SortField2"] = "UserName";
                ViewState["SortDirection2"] = "ASC";

                //ViewState["SortField3"] = "Centre";
                //ViewState["SortDirection3"] = "ASC";

                BindData();

                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    BindDataGV2();
                }

                AssignData();

                lblPageDescription.Text = "Update Main details for '" + txtUserName.Text + "' ";
            }
        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while loading the page.";
        }

    }

    //protected void Page_LoadComplete(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DropDownList ddMainCountry = (DropDownList)(Master.FindControl("ddCentreCode"));

    //        if (ddMainCountry != null)
    //        {

    //            lblPageDescription.Text += " (" + ddMainCountry.SelectedItem.Text + ") ";
    //        }
    //    }
    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while page complete event was executed.";
    //    }

    //}

    protected void BindData()
    {
        try
        {

            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

            string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

            string strSQL = String.Empty;

            if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
            {
                strSQL += "SELECT t1.*,";
                strSQL += "t3.LastLogin ";
                strSQL += "FROM listusers t1  LEFT JOIN listdbuser t2 On t1.ListusersID=t2.ListusersID  ";
                strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
                strSQL += "ON t1.UserName=t3.UserID ";
                //strSQL += "WHERE t2.dataname='" + STRDbName + "' ";
                //strSQL += "AND t1.centre='" + STRCentre + "' ";
                //strSQL += "AND t2.centrecode='" + STRCentre + "' ";
                //strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
                strSQL += "AND (t1.Active <> 0 OR t1.Active IS NULL) ";
            }
            else
            {

                strSQL += "SELECT t1.*,";
                strSQL += "t3.LastLogin ";
                strSQL += "FROM listusers t1  INNER JOIN listdbuser t2 On t1.ListusersID=t2.ListusersID  ";
                strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
                strSQL += "ON t1.UserName=t3.UserID ";
                strSQL += "AND (t1.Active <> 0 OR t1.Active IS NULL) ";

                strSQL += "INNER JOIN ";
                strSQL += @"(SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                            FROM cope_wp_four.lstcentres  t1  
                            INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                            INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                            WHERE t3.UserName=?UserName  
                            AND (t2.SuperUser='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode) t4 ";

                strSQL += "ON t4.Centre=t2.centrecode ";
            }

            strSQL += "WHERE  (t1.Active <> 0 OR t1.Active IS NULL) AND (t2.AdminSuperUser <> 'YES' OR  t2.AdminSuperUser IS NULL) ";
            strSQL += "GROUP BY t1.ListusersID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            GV1.DataBind();
            lblGV1.Text = "Number of Users " + GV1.Rows.Count.ToString() + ". Click on UserID to edit access rights.";
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
                if (String.IsNullOrEmpty(Request.QueryString["OtherID"]) == false)
                {
                    {
                        if (drv["listusersid"].ToString() == Request.QueryString["OtherID"].ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }
            }
        }
    }

    protected void BindDataGV2()
    {
        try
        {

            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

            string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

            string strSQL = String.Empty;

            strSQL += "SELECT t1.*,";
            strSQL += "t3.LastLogin ";
            strSQL += "FROM listusers t1  LEFT JOIN listdbuser t2 On t1.ListusersID=t2.ListusersID  ";
            strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
            strSQL += "ON t1.UserName=t3.UserID ";
            //strSQL += "WHERE t2.dataname='" + STRDbName + "' ";
            //strSQL += "AND t1.centre='" + STRCentre + "' ";
            //strSQL += "AND t2.centrecode='" + STRCentre + "' ";
            //strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
            strSQL += "AND (t1.Active <> 0 OR t1.Active IS NULL) ";

            strSQL += "WHERE  (t1.Active <> 0 OR t1.Active IS NULL) AND (t2.AdminSuperUser ='YES') ";
            strSQL += "GROUP BY t1.ListusersID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField2"] + " " + (string)ViewState["SortDirection2"];

            GV2.DataSource = SqlDataSource2;
            SqlDataSource2.SelectCommand = strSQL;
            SqlDataSource2.SelectParameters.Clear();
            SqlDataSource2.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            GV2.DataBind();
            lblGV2.Text = "Number of Admin Super Users " + GV2.Rows.Count.ToString() + ". ";
        }
        catch (Exception excep)
        {
            lblGV1.Text = excep.Message + " An error occured while binding the Admin Super Users page.";
        }

        //lblUserMessages.Text = strSQL;
    }


    protected void GV2_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression.ToString() == ViewState["SortField2"].ToString())
        {
            switch (ViewState["SortDirection2"].ToString())
            {
                case "ASC":
                    ViewState["SortDirection2"] = "DESC";
                    break;
                case "DESC":
                    ViewState["SortDirection2"] = "ASC";
                    break;
            }

        }
        else
        {
            ViewState["SortField2"] = e.SortExpression;
            ViewState["SortDirection2"] = "DESC";
        }
        BindDataGV2();
    }

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

    protected void AssignData()
    {
        try
        {
            string STRSQL = string.Empty;

            STRSQL = @"SELECT username, Active, FirstName, LastName,
                    Email, JobTitle, Comments FROM listusers WHERE ListusersID=?ListusersID ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.Parameters.Add("?ListUsersID", MySqlDbType.Int32).Value = Request.QueryString["OtherID"];


            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["UserName"]))
                            {
                                txtUserName.Text = (string)(myDr["UserName"]);
                            }


                            if (!DBNull.Value.Equals(myDr["FirstName"]))
                            {
                                txtFirstName.Text = (string)(myDr["FirstName"]);
                            }


                            if (!DBNull.Value.Equals(myDr["LastName"]))
                            {
                                txtLastName.Text = (string)(myDr["LastName"]);
                            }


                            if (!DBNull.Value.Equals(myDr["Email"]))
                            {
                                txtEmail.Text = (string)(myDr["Email"]);
                            }

                            if (!DBNull.Value.Equals(myDr["JobTitle"]))
                            {
                                txtJobTitle.Text = (string)(myDr["JobTitle"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
                            }
                        }

                    }

                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }

                }
            }
            catch (System.Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = excep.Message + " An error occured while executing assign query.";
            }

        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while assigning data.";
        }
    }
    protected void cmdAddRecord_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Request.QueryString["OtherID"]))
            {
                throw new Exception("Could not obtain the Unique identifier for the User whose access needs to be modified.");
            }

            string STRSQLFIND = "";

            if (SessionVariablesAll.AdminSuperUser!=STR_YES_SELECTION )
            {
                //check if the user is a superadmin user


                STRSQLFIND += "SELECT COUNT(*) CR FROM listdbuser t1 WHERE ListUsersID=?ListUsersID AND AdminSuperUser='YES' GROUP BY ListUsersID ";


                Int16 intCountAdminSuperUser = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQLFIND, "?ListUsersID", Request.QueryString["OtherID"], STRCONN));

                if (intCountAdminSuperUser > 0)
                {
                    throw new Exception("You do not have the rights to modify useraccess for an 'Admin Super User'");
                }


                //check if the user is a super user
                STRSQLFIND = @"SELECT COUNT(*) CR FROM (SELECT t2.* FROM listusers t1 INNER JOIN listdbuser t2 ON t1.ListUsersID=t2.ListUsersID
                                WHERE t1.UserName=?UserNameAssign AND SuperUser='YES') t1
                                INNER JOIN (SELECT t1.* FROM listdbuser t1
                                WHERE t1.ListUsersID=?ListUsersID 
                                AND (t1.SuperUser='YES' OR t1.AddEdit='YES' OR  t1.AddEditRecipient='YES' OR  t1.AddEditFollowUp='YES' 
                                OR t1.Randomise='YES' OR t1.ViewRandomise='YES'
                                OR t1.SAECentre='YES' OR t1.TrialIDCEntre='YES')
                                GROUP BY t1.CentreCode) t2 
                                ON t1.CentreCode=t2.CentreCode ";


                Int16 intCountSuperUser = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQLFIND, "?UserNameAssign", SessionVariablesAll.UserName, "?ListUsersID", Request.QueryString["OtherID"], STRCONN));

                if (intCountSuperUser < 0)
                {
                    throw new Exception("You do not have the rights to modify useraccess for an the selected user");
                }


            }
            

            //update data

            string STRSQL = @"UPDATE listusers SET 
                                FirstName=?FirstName, LastName=?LastName, Email=?Email, JobTitle=?JobTitle, 
                                Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy 
                                WHERE ListUsersID=?ListUsersID";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?ListusersID", MySqlDbType.VarChar).Value = Request.QueryString["OtherID"];

            MyCMD.Parameters.Add("?FirstName", MySqlDbType.VarChar).Value = txtFirstName.Text;
            MyCMD.Parameters.Add("?LastName", MySqlDbType.VarChar).Value = txtLastName.Text;
            MyCMD.Parameters.Add("?EMail", MySqlDbType.VarChar).Value = txtEmail.Text;

            if (txtJobTitle.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?JobTitle", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?JobTitle", MySqlDbType.VarChar).Value = txtJobTitle.Text;
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

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = "Data Updated.";
            }
            catch (Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = ex.Message + " An error occured while executing update query.";

            }

            AssignData();
        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while adding data.";
        }
    }
}