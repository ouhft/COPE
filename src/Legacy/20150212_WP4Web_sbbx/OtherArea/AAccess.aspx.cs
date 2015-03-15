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

using System.Drawing;

public partial class OtherArea_AAccess : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4usrconn";

        private const string KeyDbName = "dbname";
        private const string KeyCentreCode = "CentreCode"; //Main Centre Code associated with the user
        private const string KeyPassLength = "passlength";
        private const string KeyPassUniqueCharacter = "passuniquecharacter";
        private const int INT_ACTIVE = 1; //by default user is active

        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx";

        string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);
        string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

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

                    if (String.IsNullOrEmpty(Request.QueryString["OtherID"]) == true)
                    {
                        throw new Exception("Could not obtain Unique Identifier");
                    }

                                        
                    AssignAccess();

                    cmdDelete_ConfirmButtonExtender.ConfirmText = "Would you like to Remove Access for the user " + txtUserName.Text + " ? Click 'OK' if true. ";
                    if (String.IsNullOrEmpty(Request.QueryString["ModifyID"]) == true)
                    {
                        cmdAddRecord.Text="Assign Access";
                        lblPageDescription.Text = "Assign Access to User " + txtUserName.Text;

                        cmdDelete.Visible = false;
                        cmdDelete.Enabled = false;

                    }
                    else
                    {
                        cmdAddRecord.Text = "Update Access";
                        lblPageDescription.Text = "Modify Access for User " + txtUserName.Text;

                        cmdDelete.Visible = true;
                        cmdDelete.Enabled = true;
                    }

                    ViewState["SortField"] = "UserName";
                    ViewState["SortDirection"] = "ASC";

                    ViewState["SortField2"] = "UserName";
                    ViewState["SortDirection2"] = "ASC";

                    BindData();
                    BindData2();

                    string STRSQL = "SELECT t1.CountryCode,  CONCAT(t2.CountryCode, t2.CentreCode) CentreCodeMerged, ";
                    STRSQL += "CONCAT(t2.CountryCode, t2.CentreCode, ' - ', t2.CentreName,', ', t1.Country) CentreDetails ";
                    STRSQL += "FROM lstcountries t1 INNER JOIN lstcentres t2 ON t1.CountryCode=t2.CountryCode ";
                    STRSQL += "ORDER BY t2.CountryCode, t2.CentreCode ";

                    sqldsCentreLists.SelectParameters.Clear();


                    sqldsCentreLists.SelectCommand = STRSQL;
                    ddCentreList.DataSource = sqldsCentreLists;
                    ddCentreList.DataBind();

                    ddCentreList.SelectedValue = SessionVariablesAll.CentreCode;

                    lblCentreDetails.Text = ddCentreList.SelectedItem.Text;


                }
            }

            catch (System.Exception excep)
            {
                lblUserMessages.Text = excep.Message + " An error occured while loading the page.";
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddMainCountry = (DropDownList)(Master.FindControl("ddCentreCode"));

                if (ddMainCountry != null)
                {

                    lblPageDescription.Text += "(" + ddMainCountry.SelectedItem.Text + ") ";
                }
            }
            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while page complete event was executed.";
            }

        }
        protected void BindData()
        {
            try
            {

                //string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

                //string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

                string strSQL = String.Empty;

                strSQL += "SELECT t1.ListUsersID, t1.UserName, t1.Active, t1.Firstname, t1.LastName,t1.Email,t1.JobTitle,t1.LockedUser, ";
                strSQL += "t2.ID,t2.CentreCode, t2.SuperUser, t2.AddEdit, t2.Randomise,t2.PersonalData,";
                strSQL += "t3.LastLogin ";
                strSQL += "FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  ";
                strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
                strSQL += "ON t1.UserName=t3.UserID ";
                strSQL += "WHERE t2.dataname='" + STRDbName + "' ";
                strSQL += "AND t1.centre='" + STRCentre + "' ";
                //strSQL += "AND t2.centrecode='" + STRCentre + "' ";
                strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
                strSQL += "AND (t1.Active <> 0 OR t1.Active IS NULL) ";
                strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];

                //lblUserMessages.Text = strSQL;

                GV1.DataSource = SqlDataSource1;
                SqlDataSource1.SelectCommand = strSQL;

                GV1.DataBind();
                lblGV1.Text = "Number of Users assigned Access Rights " + GV1.Rows.Count.ToString() + ". Click on UserID to edit access rights.";
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
                    if (String.IsNullOrEmpty(Request.QueryString["ModifyID"]) == false)
                    {
                        {
                            if (drv["ID"].ToString() == Request.QueryString["ModifyID"].ToString())
                            {
                                e.Row.BackColor = Color.LightBlue;
                            }
                        }
                    }

                    

                    
                }
            }
        }

        protected void BindData2()
        {
            try
            {

                //string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

                ////string STRCentre = SessionVariablesAll.CentreCode;

                //string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

                string strSQL = String.Empty;

                //strSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Centre  ";
                //strSQL += " ";
                //strSQL += "FROM listusers t1 LEFT JOIN  listdbuser t2 on t1.ListUsersID=t2. ListUsersID  ";
                //strSQL += "AND t2.DataName='" + STRDbName + "' ";
                ////strSQL += "AND t2.centrecode='" + STRCentre + "' ";
                ////strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
                //strSQL += "WHERE t2.ID IS NULL AND t1.Centre='" + STRCentre + "' ";
                //strSQL += "AND (t1.Active <> 0 or t1.Active IS NULL) ";
                //strSQL += "AND (t2.centrecode='' OR t2.centrecode IS NULL) ";
                //strSQL += "ORDER BY " + ViewState["SortField2"] + " " + ViewState["SortDirection2"];

                //lblUserMessages.Text = strSQL;

                strSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Centre  ";
                strSQL += " ";
                strSQL += "FROM listusers t1 LEFT JOIN (SELECT t1.* FROM listusers t1 INNER JOIN listdbuser t2  ";
                strSQL += "ON t1.ListUsersID=t2.ListUsersID WHERE t2.centrecode='" + SessionVariablesAll.CentreCode + "') t2 ";
                strSQL += "ON t1.ListUsersID=t2.ListUsersID ";
                //strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
                strSQL += "WHERE (t1.Active <> 0 or t1.Active IS NULL) AND  t2.ListUsersID IS NULL  ";
                strSQL += "ORDER BY " + ViewState["SortField2"] + " " + ViewState["SortDirection2"];

                GV2.DataSource = SqlDataSource2;
                SqlDataSource2.SelectCommand = strSQL;

                GV2.DataBind();
                lblGV2.Text = "Number of Users Not assigned Access Rights " + GV2.Rows.Count.ToString() + ". Click on UserID to Assign access rights.";
            }
            catch (Exception excep)
            {
                lblGV2.Text = excep.Message + " An error occured while binding the page.";
            }

            //lblUserMessages.Text = strSQL;
        }

        //sorting main datagrid
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
            BindData2();
        }

        protected void GV2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = (DataRowView)(e.Row.DataItem);
                    if (drv["ListUsersID"].ToString() == Request.QueryString["OtherID"].ToString())
                    {
                        e.Row.BackColor = Color.LightBlue;
                    }
                }
            }
        }
        // reset button
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

        protected void AssignAccess()
        {

            string STRSQL = string.Empty;

            string strID = string.Empty;

            if (String.IsNullOrEmpty(Request.QueryString["ModifyID"]))
            {
                //find unique identifier of the user
                string STRSQL_FINDID = "SELECT COUNT(*) CR FROM listdbuser WHERE ListusersID=?ListusersID AND centrecode='" + SessionVariablesAll.CentreCode + "' ";

                //string STRSQL_FINDID = "SELECT COUNT(*) CR FROM listdbuser WHERE ListusersID='" + Request.QueryString["OtherID"] + "' AND centrecode='" + SessionVariablesAll.CentreCode + "' ";

                //lblAddEdit.Text = STRSQL_FINDID;

                int intCountID = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQL_FINDID, "?ListusersID", Request.QueryString["OtherID"], STRCONN));


                if (intCountID > 1)
                {
                    throw new Exception("User has been assigned access more than once to this centre. Please check your data.");
                }

                if (intCountID < 0)
                {
                    throw new Exception("An error occured while checking if User has been assigned access more than once to this centre. ");
                }

                if (intCountID == 1)
                {
                    STRSQL_FINDID = "SELECT ID FROM listdbuser WHERE ListusersID=?ListusersID AND centrecode='" + SessionVariablesAll.CentreCode + "' ";
                    strID = GeneralRoutines.ReturnScalar(STRSQL_FINDID, "?ListusersID", Request.QueryString["OtherID"], STRCONN);

                    //lblAddEdit.Text += " " + strID;
                }

            }

            if (String.IsNullOrEmpty(Request.QueryString["ModifyID"]) == false)
            {
                STRSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Comments,  ";
                STRSQL += "t2.SuperUser, t2.AddEdit, t2.Randomise, t2.Comments DataComments ";
                STRSQL += "FROM listusers t1 INNER JOIN listdbuser t2 ON t1.ListUsersID=t2.ListUsersID ";
                STRSQL += "WHERE t1.ListUsersID=?ListUsersID  AND t2.ID=?ID ";
                STRSQL += "AND (t1.Active <> 0 or t1.Active IS NULL) ";


            }

            else
            {
                if (strID == string.Empty)
                {
                    STRSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Comments  ";
                    STRSQL += "FROM listusers t1 ";
                    STRSQL += "WHERE t1.ListUsersID=?ListUsersID  ";
                    STRSQL += "AND (t1.Active <> 0 or t1.Active IS NULL) ";
                }
                else
                {
                    STRSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Comments,  ";
                    STRSQL += "t2.SuperUser, t2.AddEdit, t2.Randomise, t2.Comments DataComments ";
                    STRSQL += "FROM listusers t1 INNER JOIN listdbuser t2 ON t1.ListUsersID=t2.ListUsersID ";
                    STRSQL += "WHERE t1.ListUsersID=?ListUsersID  AND t2.ID='" + strID + "' ";
                    STRSQL += "AND (t1.Active <> 0 or t1.Active IS NULL) ";
                }

            }

            //lblAddEdit.Text += STRSQL;

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.Parameters.Add("?ListUsersID", MySqlDbType.Int32).Value = Request.QueryString["OtherID"];
            if (String.IsNullOrEmpty(Request.QueryString["ModifyID"]) == false)
            {
                MyCMD.Parameters.Add("?ID", MySqlDbType.Int32).Value = Request.QueryString["ModifyID"];
            }
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

                            if (!DBNull.Value.Equals(myDr["SuperUser"]))
                            {
                                if ((string)myDr["SuperUser"] == "YES")
                                {
                                    rdoSuperUser.SelectedValue = (string)myDr["SuperUser"];
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["AddEdit"]))
                            {
                                if ((string)myDr["AddEdit"] == "YES")
                                {
                                    rdoAddEdit.SelectedValue = (string)myDr["AddEdit"];
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Randomise"]))
                            {
                                if ((string)myDr["Randomise"] == "YES")
                                {
                                    rdoRandomise.SelectedValue = (string)myDr["Randomise"];
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["DataComments"]))
                            {
                                txtDataComments.Text = (string)(myDr["DataComments"]);
                            }

                        }

                    }
                    else
                    {
                        lblUserMessages.Text = "No records available";
                    }

                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }

                }




            }
            catch (Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = excep.Message + " An error occured while assigning data.";
            }

        }
        protected void cmdAddRecord_Click(object sender, EventArgs e)
        {
            try
            {

                string strID = string.Empty;

                if (String.IsNullOrEmpty(Request.QueryString["ModifyID"]))
                {
                    //find unique identifier of the user
                    string STRSQL_FINDID = "SELECT COUNT(*) CR FROM listdbuser WHERE ListusersID=?ListusersID AND centrecode='" + SessionVariablesAll.CentreCode + "' ";

                    int intCountID = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQL_FINDID, "?ListusersID", Request.QueryString["OtherID"], STRCONN));

                    if (intCountID > 1)
                    {
                        throw new Exception("User has been assigned access more than once to this centre. Please check your data.");
                    }

                    if (intCountID < 0)
                    {
                        throw new Exception("An error occured while checking if User has been assigned access more than once to this centre. ");
                    }

                    if (intCountID == 1)
                    {
                        STRSQL_FINDID = "SELECT ID FROM listdbuser WHERE ListusersID=?ListusersID AND centrecode='" + SessionVariablesAll.CentreCode + "' ";
                        strID = GeneralRoutines.ReturnScalar(STRSQL_FINDID, "?ListusersID", Request.QueryString["OtherID"], STRCONN);
                    }

                }


                string STRSQL_USER = string.Empty;

                STRSQL_USER += "UPDATE listusers SET ";
                STRSQL_USER += "FirstName=?FirstName, LastName=?LastName, Email=?Email, JobTitle=?JobTitle, ";
                STRSQL_USER += "Comments=?Comments, DateUpdated=?DateUpdated, UpdatedBy=?UpdatedBy ";
                STRSQL_USER += "WHERE ListusersID=?ListusersID AND Centre=?Centre ";

                string SQL_INSERTRIGHTS = string.Empty;
                SQL_INSERTRIGHTS += "INSERT INTO listdbuser ";
                SQL_INSERTRIGHTS += "(dataname, ListusersID, centrecode, SuperUser, AddEdit, Randomise, Comments, DateCreated, CreatedBy) ";
                SQL_INSERTRIGHTS += "VALUES ";
                SQL_INSERTRIGHTS += "(?dataname, ?ListusersID, ?centrecode, ?SuperUser, ?AddEdit, ?Randomise, ?DataComments, ?DateCreated, ?CreatedBy) ";


                string STRSQL_UPDATERIGHTS = string.Empty;

                STRSQL_UPDATERIGHTS += "UPDATE listdbuser SET ";
                STRSQL_UPDATERIGHTS += "SuperUser=?SuperUser, AddEdit=?AddEdit, Randomise=?Randomise,";
                STRSQL_UPDATERIGHTS += "Comments=?DataComments, DateUpdated=?DateUpdated, UpdatedBy=?UpdatedBy ";
                STRSQL_UPDATERIGHTS += "WHERE ID=?ID AND ListusersID=?ListusersID AND DataName=?DataName "; //AND centrecode=?centrecode ";




                string CS = string.Empty;
                CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

                MySqlConnection MyCONN = new MySqlConnection();
                MyCONN.ConnectionString = CS;

                MySqlCommand MyCMD = new MySqlCommand();
                MyCMD.Connection = MyCONN;

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQL_USER;

                MyCMD.Parameters.Clear();

                MyCMD.Parameters.Add("?ListusersID", MySqlDbType.VarChar).Value = Request.QueryString["OtherID"];

                MyCMD.Parameters.Add("?FirstName", MySqlDbType.VarChar).Value = txtFirstName.Text;
                MyCMD.Parameters.Add("?LastName", MySqlDbType.VarChar).Value = txtLastName.Text;
                MyCMD.Parameters.Add("?EMail", MySqlDbType.VarChar).Value = txtEmail.Text;
                MyCMD.Parameters.Add("?JobTitle", MySqlDbType.VarChar).Value = txtJobTitle.Text;

                MyCMD.Parameters.Add("?Centre", MySqlDbType.VarChar).Value = STRCentre;
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
                MyCMD.Parameters.Add("?DateUpdated", MySqlDbType.DateTime).Value = DateTime.Now;
                MyCMD.Parameters.Add("?UpdatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

                //remaining parameters for data access
                MyCMD.Parameters.Add("?dataname", MySqlDbType.VarChar).Value = STRDbName;
                //MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar).Value = STRCentre;
                MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar).Value = SessionVariablesAll.CentreCode;
                MyCMD.Parameters.Add("?SuperUser", MySqlDbType.VarChar).Value = rdoSuperUser.SelectedValue;
                MyCMD.Parameters.Add("?AddEdit", MySqlDbType.VarChar).Value = rdoAddEdit.SelectedValue;
                MyCMD.Parameters.Add("?Randomise", MySqlDbType.VarChar).Value = rdoRandomise.SelectedValue;
                MyCMD.Parameters.Add("?DataComments", MySqlDbType.VarChar).Value = txtDataComments.Text;
                MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;
                MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;


                MyCONN.Open();

                MySqlTransaction myTrans = MyCONN.BeginTransaction();

                MyCMD.Transaction = myTrans;

                try
                {


                    MyCMD.ExecuteNonQuery();

                    if (!String.IsNullOrEmpty(Request.QueryString["ModifyID"]))
                    {

                        MyCMD.CommandText = STRSQL_UPDATERIGHTS;
                        MyCMD.Parameters.Add("?ID", MySqlDbType.VarChar).Value = Request.QueryString["ModifyID"];
                    }
                    else
                    {
                        if (strID != string.Empty)
                        {
                            MyCMD.CommandText = STRSQL_UPDATERIGHTS;
                            MyCMD.Parameters.Add("?ID", MySqlDbType.VarChar).Value = strID;
                        }
                        else
                        {
                            MyCMD.CommandText = SQL_INSERTRIGHTS;
                        }

                    }

                    MyCMD.ExecuteNonQuery();

                    myTrans.Commit();

                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }

                    BindData();
                    BindData2();




                }
                catch (System.Exception excep)
                {

                    myTrans.Rollback();
                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }
                    lblUserMessages.Text = excep.Message + " An error occured while executing the query.";

                }
            }
            catch (System.Exception excep)
            {
                lblUserMessages.Text = excep.Message + " An error occured while updating data.";
            }
        }
        
        
    
    
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                lblUserMessages.Text = string.Empty;

                if (String.IsNullOrEmpty(Request.QueryString["ModifyID"])==true)
                {
                    throw new Exception("Could not obtain the Unique Identifier.");
                }

                string STRSQL = string.Empty;

                STRSQL += "DELETE FROM listdbuser ";
                STRSQL += "WHERE ID=?ID AND ListusersID=?ListusersID AND DataName=?DataName AND centrecode=?centrecode  ";

                string CS = string.Empty;
                CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

                MySqlConnection MyCONN = new MySqlConnection();
                MyCONN.ConnectionString = CS;

                MySqlCommand MyCMD = new MySqlCommand();
                MyCMD.Connection = MyCONN;

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STRSQL;

                MyCMD.Parameters.Clear();

                MyCMD.Parameters.Add("?ID", MySqlDbType.VarChar).Value = Request.QueryString["ModifyID"];
                MyCMD.Parameters.Add("?ListusersID", MySqlDbType.VarChar).Value = Request.QueryString["OtherID"];

                MyCMD.Parameters.Add("?dataname", MySqlDbType.VarChar).Value = STRDbName;
                //MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar).Value = STRCentre;
                MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar).Value = SessionVariablesAll.CentreCode;
                MyCONN.Open();
                try
                {
                    MyCMD.ExecuteNonQuery();

                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }

                    BindData();
                    BindData2();
                }
                catch (System.Exception excep)
                {
                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }

                    lblUserMessages.Text = excep.Message + " An error occured while executing Delete Access query.";
                }
            }
            catch (System.Exception excep)
            {
                lblUserMessages.Text = excep.Message + " An error occured while deleting access.";
            }
        }
}