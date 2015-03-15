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




public partial class OtherArea_AEUser : System.Web.UI.Page
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

                    
                ViewState["SortField"] = "UserName";
                ViewState["SortDirection"] = "ASC";

                ViewState["SortField2"] = "UserName";
                ViewState["SortDirection2"] = "ASC";

                BindData();
                BindData2();

                //ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.FindControl(strMainCPH));
                //DropDownList ddMainCountry = (DropDownList)(mpCPH.FindControl("ddCentreCode"));

                //DropDownList ddMainCountry = (DropDownList)(Master.FindControl("ddCentreCode"));

                //if (ddMainCountry != null)
                //{

                //    lblUserMessages.Text = ddMainCountry.SelectedValue;
                //}

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

                lblPageDescription.Text += " (" + ddMainCountry.SelectedItem.Text + ") ";
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
                
            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

            string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

            string strSQL = String.Empty;

            strSQL += "SELECT t1.ListUsersID, t1.UserName, t1.Active, t1.Firstname, t1.LastName,t1.Email,t1.JobTitle,t1.LockedUser, ";
            strSQL += "t2.ID, t2.CentreCode, t2.SuperUser, t2.AddEdit, t2.Randomise,t2.PersonalData,";
            strSQL += "t3.LastLogin ";
            strSQL += "FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  ";
            strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
            strSQL += "ON t1.UserName=t3.UserID ";
            strSQL += "WHERE t2.dataname='" + STRDbName + "' ";
            //strSQL += "AND t1.centre='" + STRCentre + "' ";
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


    protected void BindData2()
    {
        try
        {
                
            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

            //string STRCentre = SessionVariablesAll.CentreCode;

            string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

            string strSQL = String.Empty;

            //strSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Centre  ";
            //strSQL += " ";
            //strSQL += "FROM listusers t1 LEFT JOIN  listdbuser t2 on t1.ListUsersID=t2. ListUsersID  ";
            //strSQL += "AND t2.DataName='" + STRDbName + "' ";
            ////strSQL += "AND t2.centrecode='" + STRCentre + "' ";
            ////strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
            ////strSQL += "WHERE t2.ID IS NULL AND t1.Centre='" + STRCentre + "' ";
            //strSQL += "WHERE t2.ID IS NULL  ";
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
    protected void cmdAddRecord_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = String.Empty;

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


            if (txtFirstName.Text == String.Empty)
            { throw new Exception("Please enter First Name."); }

            if (txtLastName.Text == String.Empty)
            { throw new Exception("Please enter Last Name/Surname."); }

            if (txtEmail.Text == String.Empty)
            { throw new Exception("Please enter Email address."); }

            if (!txtEmail.Text.Contains("@"))
            { throw new Exception("Please enter the email in the correct format"); }

            if (txtUserName.Text.Length < INT_PASSLENGTH)
            { throw new Exception("The Length of username should be " + INT_PASSLENGTH.ToString()); }

            if (txtUserName.Text.Contains(" "))
            { throw new Exception("UserName should not contain a space."); }

            if (txtUserPass.Text.Length < INT_PASSLENGTH)
            { throw new Exception("The Length of password should be " + INT_PASSLENGTH.ToString()); }

            if (txtUserPass.Text.Contains(" "))
            { throw new Exception("Password should not contain a space."); }

            if (txtUserName.Text == txtUserPass.Text)
            { throw new Exception("The User Name and password cannot be the same"); }

            if (txtUserPass.Text != txtReEnterUserPass.Text)
            { throw new Exception("Re-entered/Second password should match the original/first password."); }

                               

            int INT_PASSCHECK = 0; //set it back to 0 to check Lower Case, Upper Case
            INT_PASSCHECK = GeneralRoutines.PCheck(txtUserPass.Text, INT_PASSLENGTH, INT_PASSDISTINCT);

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

            string STRSQLCOUNT = string.Empty;
            STRSQLCOUNT += "SELECT COUNT(*) CR FROM listusers WHERE UserName=?UserName";

            string COUNTUSERS = "0";
            COUNTUSERS = GeneralRoutines.ReturnScalar(STRSQLCOUNT, "?UserName", txtUserName.Text, STRCONN);


            int COUNTUSERSRESULT = 0;
            bool canConvert = int.TryParse(COUNTUSERS, out COUNTUSERSRESULT);

            if (canConvert == false)
            {
                throw new Exception("Could not check if the User exists.");
            }


            if (COUNTUSERSRESULT > 0)
            {
                throw new Exception("The User Name you are trying to add already exists in the system.");
            }

            //STRSQLCOUNT = "SELECT COUNT(*) CR FROM listusers WHERE Email=?Email";
            //COUNTUSERS = GeneralRoutines.ReturnScalar(STRSQLCOUNT, "?Email", txtEmail.Text, STRCONN);

            //COUNTUSERSRESULT = 0;

            //canConvert = int.TryParse(COUNTUSERS, out COUNTUSERSRESULT);

            //if (canConvert == false)
            //{
            //    throw new Exception("Could not check if the User exists.");
            //}


            //if (COUNTUSERSRESULT > 0)
            //{
            //    throw new Exception("The Email you are trying to add already exists in the system.");
            //}

              

            string SQLINSERT = string.Empty;

            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);
            string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);
            DateTime dteNOW = DateTime.Now;

            SQLINSERT += "INSERT INTO listusers ";
            SQLINSERT += "(FirstName, LastName, EMail, JobTitle, Active, UserName, UserPWord, Centre, Comments,DateCreated, CreatedBy) ";
            SQLINSERT += "VALUES ";
            SQLINSERT += "(?FirstName, ?LastName, ?EMail, ?JobTitle, ?Active, ?UserName, SHA2(?UserPWord, 512), ";
            //SQLINSERT += "SHA2(CONCAT(?UserPWord, ':','" + dteNOW.ToString("yyyy-MM-dd HH:mm:ss") + "',';',";
            //SQLINSERT += "LEFT('" + SessionVariablesAll.UserName + "',2)), 512), ";
            SQLINSERT += "?Centre, ?Comments,?DateCreated, ?CreatedBy) ";

            //lblComments.Text = SQLINSERT;


            string SQLDATADETAILS = string.Empty;
            SQLDATADETAILS += "INSERT INTO listdbuser ";
            SQLDATADETAILS += "(dataname, ListusersID, centrecode, SuperUser, AddEdit,  Randomise, Comments, DateCreated, CreatedBy) ";
            SQLDATADETAILS += "VALUES ";
            SQLDATADETAILS += "(?dataname, ?ListusersID, ?centrecode, ?SuperUser, ?AddEdit, ?Randomise, ?DataComments, ?DateCreated, ?CreatedBy) ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = SQLINSERT;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?FirstName", MySqlDbType.VarChar).Value = txtFirstName.Text;
            MyCMD.Parameters.Add("?LastName", MySqlDbType.VarChar).Value = txtLastName.Text;
            MyCMD.Parameters.Add("?EMail", MySqlDbType.VarChar).Value = txtEmail.Text;
            MyCMD.Parameters.Add("?JobTitle", MySqlDbType.VarChar).Value = txtJobTitle.Text;
            MyCMD.Parameters.Add("?Active", MySqlDbType.VarChar).Value = INT_ACTIVE;
            MyCMD.Parameters.Add("?UserName", MySqlDbType.VarChar).Value = txtUserName.Text;

            string STRPWORD = txtUserPass.Text + ":" + dteNOW.ToString("yyyy-MM-dd HH:mm:ss") + ";" + SessionVariablesAll.UserName.Substring(0, 2);

            MyCMD.Parameters.Add("?UserPWord", MySqlDbType.VarChar).Value = STRPWORD;

            MyCMD.Parameters.Add("?Centre", MySqlDbType.VarChar).Value = STRCentre;
            MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.VarChar).Value = dteNOW.ToString("yyyy-MM-dd HH:mm:ss");
            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;
                
            //remaining parameters for data access
            MyCMD.Parameters.Add("?dataname", MySqlDbType.VarChar).Value = STRDbName;
            //MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar).Value = STRCentre;
            MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar).Value = SessionVariablesAll.CentreCode;
            MyCMD.Parameters.Add("?SuperUser", MySqlDbType.VarChar).Value = rdoSuperUser.SelectedValue;
            MyCMD.Parameters.Add("?AddEdit", MySqlDbType.VarChar).Value = rdoAddEdit.SelectedValue;
            MyCMD.Parameters.Add("?Randomise", MySqlDbType.VarChar).Value = rdoRandomise.SelectedValue;
            MyCMD.Parameters.Add("?DataComments", MySqlDbType.VarChar).Value = txtDataComments.Text;

            MyCONN.Open();

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            try
            {

                MyCMD.ExecuteNonQuery();


                string SQLID = "SELECT ListusersID FROM listusers WHERE UserName=?UserName ";

                MyCMD.CommandText = SQLID;

                int UserID = 0;

                UserID = Convert.ToInt32(MyCMD.ExecuteScalar());

                    
                if (UserID == 0)
                {
                    throw new Exception("Could not obtain the Unique Identifier");
                }

                MyCMD.CommandText = SQLDATADETAILS;

                MyCMD.Parameters.Add("?ListusersID", MySqlDbType.VarChar).Value = UserID;

                MyCMD.ExecuteNonQuery();
                    
                myTrans.Commit();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();

                }

                BindData();
                BindData2();
            }
                
            catch (Exception excep)
            {

                myTrans.Rollback();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();

                }

                lblUserMessages.Text = excep.Message + " An error occured while executing query.";
            }


        }
        catch (Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while adding page."; 
        }
    }

       
}