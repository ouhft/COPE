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

public partial class OtherArea_UUse : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4usrconn";


        private const string KeyDbName = "dbname";
        private const string KeyCentreCode = "CentreCode";
        private const string KeyPassLength = "passlength";
        private const string KeyPassUniqueCharacter = "passuniquecharacter";
        private const int INT_ACTIVE = 1; //by default user is active

        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";


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
                lblUserMessages.Text = string.Empty;

                if (SessionVariablesAll.SuperUser != "YES" )
                {
                    Response.Redirect(strAccessDenied, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }

                ViewState["SortField"] = "UserName";
                ViewState["SortDirection"] = "ASC";
                BindData();

                if (String.IsNullOrEmpty(Request.QueryString["OtherID"]) == false)
                {
                    AssignAccess();
                    lblPageDescription.Text = "Unlock User " + txtUserName.Text;
                }
                else
                {
                    lblPageDescription.Text = "Select User Name from the table below to Unlock a user.";
                }




            }
        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while loading the page.";
        }
    }

    protected void BindData()
    {
        try
        {
            //string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);
            string strSQL = String.Empty;


            if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
            {
                strSQL += "SELECT t1.*,t2.ID, ";
                strSQL += "t3.LastLogin ";
                strSQL += "FROM listusers t1  LEFT JOIN listdbuser t2 On t1.ListusersID=t2.ListusersID  ";
                strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
                strSQL += "ON t1.UserName=t3.UserID ";
                //strSQL += "WHERE t2.dataname='" + STRDbName + "' ";
                //strSQL += "AND t1.centre='" + STRCentre + "' ";
                //strSQL += "AND t2.centrecode='" + STRCentre + "' ";
                //strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
                strSQL += " ";
            }
            else
            {

                strSQL += "SELECT t1.*,t2.ID,";
                strSQL += "t3.LastLogin ";
                strSQL += "FROM listusers t1  INNER JOIN listdbuser t2 On t1.ListusersID=t2.ListusersID  ";
                strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
                strSQL += "ON t1.UserName=t3.UserID ";
                strSQL += " ";

                strSQL += "INNER JOIN ";
                strSQL += @"(SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                            FROM cope_wp_four.lstcentres  t1  
                            INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                            INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                            WHERE t3.UserName=?UserName  
                            AND (t2.SuperUser='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode) t4 ";

                strSQL += "ON t4.Centre=t2.centrecode ";
            }

            strSQL += "WHERE t2.dataname=?DbName ";

            strSQL += "AND (t1.Active <> 0 OR t1.Active IS NULL) ";
            strSQL += "AND LockedUser=1 AND IF(UnlockUserTimestamp IS NOT NULL AND LockedUserTimeStamp IS NOT NULL, LockedUserTimeStamp>UnlockUserTimestamp, LockedUser IS NOT NULL) ";
            strSQL += "GROUP BY t1.UserName ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            //lblUserMessages.Text = strSQL;

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?DbName", STRDbName);
            SqlDataSource1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);
            SqlDataSource1.SelectParameters.Add("?centrecode", SessionVariablesAll.CentreCode);

            GV1.DataBind();

            lblGV1.Text = "Click on UserName to Unlock a User. Number of records  " + GV1.Rows.Count.ToString() + ".";
        }
        catch (Exception excep)
        {
            lblGV1.Text = excep.Message + " An error occured while binding the page.";
        }

        //lblUserMessages.Text = strSQL;
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
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }




            }
        }
    }


    protected void AssignAccess()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string strSQL = string.Empty;
            strSQL += "SELECT t1.UserName FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  ";
            strSQL += "";
            strSQL += "WHERE t1.ListUsersID=?ListUsersID AND t2.ID=?ID AND t2.DataName=?DataName ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.Parameters.Add("?ListUsersID", MySqlDbType.Int32).Value = Request.QueryString["OtherID"];
            MyCMD.Parameters.Add("?ID", MySqlDbType.Int32).Value = Request.QueryString["ModifyID"];
            MyCMD.Parameters.Add("?DataName", MySqlDbType.VarChar).Value = STRDbName;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

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
            catch (System.Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = excep.Message + " An error occured while executing assign statement.";
            }

        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while Assigning Data.";
        }

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
    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (txtUserName.Text==string.Empty)
            {
                throw new Exception("Please Select a User from the listat the bottom of the page to Unlock.");
            }
            if (chkRP.Checked==false)
            {
                throw new Exception("Please Tick the CheckBox to Unlock a user.");
            }
            
            //check if user exists

            string strSQLFind = string.Empty;

            if (SessionVariablesAll.AdminSuperUser=="YES")
            {
                strSQLFind = @"SELECT COUNT(*) CR FROM 
                            (SELECT t1.UserName FROM listusers t1 
                            INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID   
                            WHERE t1.UserName=?LockedUserName 
                            AND t1.LockedUser=1 AND IF(t1.UnlockUserTimestamp IS NOT NULL 
                            AND t1.LockedUserTimeStamp IS NOT NULL, t1.LockedUserTimeStamp>t1.UnlockUserTimestamp, t1.LockedUser IS NOT NULL)
                            GROUP BY t1.ListUsersID) t1 ";
            }
            else
            {
                strSQLFind = @"SELECT COUNT(*) CR FROM 
                                (SELECT t1.UserName FROM listusers t1 
                                INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID   
                                 ";

                strSQLFind += "INNER JOIN ";
                strSQLFind += @"(SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                            FROM cope_wp_four.lstcentres  t1  
                            INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                            INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                            WHERE t3.UserName=?UserName  
                            AND (t2.SuperUser='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode) t4 ";

                strSQLFind += "ON t4.Centre=t2.centrecode ";

                strSQLFind+=@" WHERE t1.UserName=?LockedUserName 
                                AND t1.LockedUser=1 AND IF(t1.UnlockUserTimestamp IS NOT NULL 
                                AND t1.LockedUserTimeStamp IS NOT NULL, t1.LockedUserTimeStamp>t1.UnlockUserTimestamp, t1.LockedUser IS NOT NULL)
                                GROUP BY t1.ListUsersID) t1 ";
            }
            



            Int16 intCountUsers = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(strSQLFind, "?LockedUserName", txtUserName.Text, "?UserName",  SessionVariablesAll.UserName, STRCONN));
            if (intCountUsers>1)
            {
                throw new Exception("More than one Users with the username " + txtUserName.Text + " exist in the database. Please check the UserName.");
            }
            if (intCountUsers==0)
            {
                throw new Exception("No Locked Users with the username " + txtUserName.Text + " exist in the database. Please check the UserName.");
            }
            if (intCountUsers < 0)
            {
                throw new Exception("An error occured while checking if username " + txtUserName.Text + " is locked.");
            }

            string strUPDATE = string.Empty;
            strUPDATE += "UPDATE listusers SET ";
            strUPDATE += "LockedUser=NULL, LockedUserTimeStamp=NULL, UnlockUserBy=?UnlockUserBy, UnlockUserTimestamp=?UnlockUserTimestamp ";
            strUPDATE += "WHERE UserName=?LockedUserName ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strUPDATE;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?LockedUserName", MySqlDbType.VarChar).Value = txtUserName.Text;
            //MyCMD.Parameters.Add("?UserPwordNew", MySqlDbType.VarChar).Value = STRPWORD_NEW;
            MyCMD.Parameters.Add("?UnlockUserBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;
            MyCMD.Parameters.Add("?UnlockUserTimestamp", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?UserName", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                BindData();

                lblUserMessages.Text = "Selected User has been Unlocked.";
            }

            catch (Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = excep.Message + " An error occured while executing Unlock user query.";
            }

        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while Unlocking user.";
        }
    }
}