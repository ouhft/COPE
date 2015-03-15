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

public partial class OtherArea_ROther : System.Web.UI.Page
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

                if (SessionVariablesAll.SuperUser != "YES")
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
                    lblPageDescription.Text = "Reset Password for " + txtUserName.Text;
                }
                else 
                {
                    lblPageDescription.Text = "Select User Name from the table below to reset the password.";
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

           
            //strSQL += "SELECT t1.ListUsersID, t1.UserName, t1.Active, t1.Firstname, t1.LastName,t1.Email,t1.JobTitle,t1.LockedUser, ";
            //strSQL += "t2.ID, t2.SuperUser,t2.Randomise,t2.PersonalData,";
            //strSQL += "t3.LastLogin ";
            //strSQL += "FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  ";
            //strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName=?DbName GROUP BY UserID) t3 ";
            //strSQL += "ON t1.UserName=t3.UserID ";
            //strSQL += "WHERE t2.dataname=?DbName ";
            ////strSQL += "AND t1.centre='" + STRCentre + "' ";
            //strSQL += "AND t2.centrecode=?centrecode ";
            //strSQL += "AND (t1.Active <> 0 OR t1.Active IS NULL) ";
            //strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            //lblUserMessages.Text = strSQL;

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
                strSQL += " ";
            }
            else
            {

                strSQL += "SELECT t1.*,";
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

            strSQL += "WHERE  (t1.Active <> 0 OR t1.Active IS NULL) ";
            if (SessionVariablesAll.AdminSuperUser != STR_YES_SELECTION)
            {
                strSQL += "AND (t2.AdminSuperUser <> 'YES' OR  t2.AdminSuperUser IS NULL) ";
            }
            strSQL += "GROUP BY t1.ListusersID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?DbName", STRDbName);
            SqlDataSource1.SelectParameters.Add("?centrecode", SessionVariablesAll.CentreCode);
            SqlDataSource1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            GV1.DataBind();

            lblGV1.Text = "Click on UserName to Reset Password. Number of records  " + GV1.Rows.Count.ToString() + ".";
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
                            e.Row.BackColor = Color.LightBlue;
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
            
            string strSQL=string.Empty;
            strSQL += "SELECT t1.UserName FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  ";
            strSQL += "";
            strSQL += "WHERE t1.ListUsersID=?ListUsersID  AND t2.DataName=?DataName ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.Parameters.Add("?ListUsersID", MySqlDbType.Int32).Value = Request.QueryString["OtherID"];
            //MyCMD.Parameters.Add("?ID", MySqlDbType.Int32).Value = Request.QueryString["ModifyID"];
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

            string PASSLENGTH = System.Configuration.ConfigurationManager.AppSettings.Get(KeyPassLength);
            string PASSDISTINCT = System.Configuration.ConfigurationManager.AppSettings.Get(KeyPassUniqueCharacter);



            if (GeneralRoutines.IsNumeric(PASSLENGTH) == false)
            {
                throw new Exception("Could not obtain minimum length of password.");
            }

            int INT_PASSLENGTH = 0;

            INT_PASSLENGTH = Convert.ToInt32(PASSLENGTH);

            //Int32.TryParse(PASSLENGTH, out INT_PASSLENGTH);

            if (INT_PASSLENGTH == 0)
            {
                throw new Exception("Password length is not numeric");
            }

            int INT_PASSDISTINCT = 0;
            INT_PASSDISTINCT = Convert.ToInt32(PASSDISTINCT);

            if (INT_PASSDISTINCT == 0)
            {
                throw new Exception("Count of Distinct characters is not numeric");
            }

            if (txtUserPassNew.Text.Length < INT_PASSLENGTH)
            { throw new Exception("The Length of password should be " + INT_PASSLENGTH.ToString()); }

            if (txtUserPassNew.Text.Contains(" "))
            { throw new Exception("Password should not contain a space."); }

            if (txtUserPassNew.Text == txtUserName.Text)
            { throw new Exception("The User Name and the New Password cannot be the same."); }

            if (txtUserPassNew.Text != txtReEnterUserPassNew.Text)
            { throw new Exception("Re-entered/Second password should match the new password."); }

            int INT_PASSCHECK = 0; //set it back to 0 to check Lower Case, Upper Case
            INT_PASSCHECK = GeneralRoutines.PCheck(txtUserPassNew.Text, INT_PASSLENGTH, INT_PASSDISTINCT);

            switch (INT_PASSCHECK)
            {
                case 1:
                    throw new Exception("The password length should be " + INT_PASSLENGTH.ToString());

                case 2:
                    throw new Exception("The password should contain at least one Lower Case alphabet.");

                case 3:
                    throw new Exception("The password should contain at least one Upper Case alphabet.");

                case 4:
                    throw new Exception("The password should contain at least one Number.");

                case 5:
                    throw new Exception("The maximum number of repetitions allowed is 2.");

                case 6:
                    throw new Exception("The password should contain at least " + INT_PASSDISTINCT.ToString() + " distinct characters.");

            }

            string STR_SQLDATECREATED = "SELECT DateCreated FROM listusers WHERE UserName=?UserName  ";

            string STR_DATECREATED = GeneralRoutines.ReturnScalar(STR_SQLDATECREATED, "?UserName", txtUserName.Text, STRCONN);

            string STR_SQLCREATEDBY = "SELECT CreatedBy FROM listusers WHERE UserName=?UserName  ";

            string STR_CREATEDBY = GeneralRoutines.ReturnScalar(STR_SQLCREATEDBY, "?UserName", txtUserName.Text, STRCONN);

            string STRPWORD_NEW = string.Empty;

            if (GeneralRoutines.IsDate(STR_DATECREATED) == true)
            {
                if (!String.IsNullOrEmpty(STR_CREATEDBY))
                {
                    
                    STRPWORD_NEW = txtUserPassNew.Text + ":" + Convert.ToDateTime(STR_DATECREATED).ToString("yyyy-MM-dd HH:mm:ss") + ";" + STR_CREATEDBY.Substring(0, 2);

                }
                else
                {
                    throw new Exception("Required data can not be retrieved.");
                }
            }
            else
            {
                throw new Exception("Could not retrieve required data.");
            }

            string strUPDATE = string.Empty;
            strUPDATE += "UPDATE listusers SET ";
            strUPDATE += "UserPword=SHA2(?UserPwordNew,512), ";
            strUPDATE += "DatePassUpdatedSuperUser=?DatePassUpdatedSuperUser, PassUpdatedSuperUser=?PassUpdatedSuperUser ";
            strUPDATE += "WHERE UserName=?UserName ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strUPDATE;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?UserName", MySqlDbType.VarChar).Value = txtUserName.Text;
            MyCMD.Parameters.Add("?UserPwordNew", MySqlDbType.VarChar).Value = STRPWORD_NEW;
            MyCMD.Parameters.Add("?DatePassUpdatedSuperUser", MySqlDbType.DateTime).Value = DateTime.Now;
            MyCMD.Parameters.Add("?PassUpdatedSuperUser", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = "Password has been reset.";
            }

            catch (Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = excep.Message + "An error occured while resetting the password.";
            }

        }
        catch  (System.Exception excep)
        { 
            lblUserMessages.Text = excep.Message + " An error occured while resetting password";
        }
    }
}