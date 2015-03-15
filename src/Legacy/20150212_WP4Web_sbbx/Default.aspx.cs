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





public partial class _Default : System.Web.UI.Page
{
    #region " Private Constants & Variables "

       // private const string STRCONN = "cope4dbconn";
        private const string STRCONN = "cope4usrconn";
        private const string strKeyDataName = "dbname";
        private const string strKeyCentreCode = "CentreCode";
        private const string strKeyNumberTries = "NumberTries";

        //private const int CENTRECODE = 12;
        private const string REDIRECTPAGE = "~/WP4HomePage.aspx";

        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";


        //intUserErrorCode values 0-successful, 5 access to database denied, 6 more than one access rights to the database, 7 could not check access rights, 
        //11 user does not exist, 12 more than one user, 
        //20 - last failure user locked  21 password does not match, 99 unknown
        //51 username was not entered
        //52 pass not entered


    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            

            if (!IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (SessionVariablesAll.Web_cope41 == "True")
                {
                    Response.Redirect(REDIRECTPAGE, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

            }
        }
        catch (System.Exception excep)
        {

            lblUserMessages.Text = excep.Message + " An error occured while loading the page.";
        }
    }


    protected void cmdLogin_Click(object sender, EventArgs e)
    {

        int intUserErrorCode = 0;

        string strDataName = System.Configuration.ConfigurationManager.AppSettings.Get(strKeyDataName);

        string strCentreCode = System.Configuration.ConfigurationManager.AppSettings.Get(strKeyCentreCode);

        int intFailedLogins; //for failed logins

        SessionVariablesAll.Web_cope41 = "False";
        try
        {
            lblUserMessages.Text = string.Empty;


            intUserErrorCode = 0;

            if (txtUsername.Text == string.Empty)
            {
                intUserErrorCode = 51;
                throw new Exception("Please enter Username.");
            }

            if (txtPassword.Text == string.Empty)
            {
                intUserErrorCode = 52;
                throw new Exception("Please enter Password.");
            }

            SessionVariablesAll.Web_cope41 = "False";

            string strSQLFindUser = "SELECT COUNT(*) CR FROM listusers WHERE UserName = ?UserName ";

            string strCountUsers = GeneralRoutines.ReturnScalar(strSQLFindUser, "?UserName", txtUsername.Text, STRCONN);

            if (GeneralRoutines.IsNumeric(strCountUsers) == false)
            { throw new Exception("Could not convert user exists to numeric."); }

            
            int intCountUsers = 0;

            bool canConvert = int.TryParse(strCountUsers, out intCountUsers);

            if (canConvert == false)
            {
              throw new Exception("Could not convert String to Integer.");             
            }

            if (intCountUsers == 0)
            {
                intUserErrorCode  = 11; //user does not exist
                SessionVariablesAll.Web_cope41 = "False";
                throw new Exception("The username/ password you have entered is incorrect.");
            }

            if (intCountUsers >= 2)
            {
                intUserErrorCode = 12; //morethan one user with the same username exists
                SessionVariablesAll.Web_cope41 = "False";
                throw new Exception("More than one username exist.");
            }

            //check if user is not blocked
            string strSQLLOCKEDUSER ="SELECT COUNT(*) CR FROM listusers WHERE UserName = ?UserName AND LockedUser=1 ";

            string strLOCKEDUSER = GeneralRoutines.ReturnScalar(strSQLLOCKEDUSER, "?UserName", txtUsername.Text, STRCONN);

            
            if (GeneralRoutines.IsNumeric(strLOCKEDUSER) == false)
            { 
                throw new Exception("Could not check if the user is locked."); 
            }

            if (Convert.ToInt16(strLOCKEDUSER)>0)
            {
                intUserErrorCode = 20;
                throw new Exception("The user is locked.");
            }

            //check username/ password is incorrect

            string strSQLTS = "SELECT DateCreated FROM listusers WHERE UserName = ?UserName ";

            string strTS = GeneralRoutines.ReturnScalar(strSQLTS, "?UserName", txtUsername.Text, STRCONN);

            string strSQLCREATOR = "SELECT CreatedBy FROM listusers WHERE UserName = ?UserName ";
            string strCREATOR = GeneralRoutines.ReturnScalar(strSQLCREATOR, "?UserName", txtUsername.Text, STRCONN);

            if (GeneralRoutines.IsDate(strTS) == false)
            { 
                throw new Exception("Could not check for user details."); 
            }

            
            //check if password is correct

            string strSQLCHECKPW = "SELECT COUNT(*) CR FROM listusers WHERE UserName = ?UserName ";
            strSQLCHECKPW += "AND userpword=SHA2(CONCAT(?UserP, ':','" + Convert.ToDateTime(strTS).ToString("yyyy-MM-dd HH:mm:ss") + "' , ';',";
            strSQLCHECKPW += "LEFT(CreatedBy,2)), 512) ";
            strSQLCHECKPW += "";

            //lblLogin.Text = strSQLCHECKPW;

            
            int intCHECKPW=Convert.ToInt32(GeneralRoutines.ReturnScalarTwo(strSQLCHECKPW, "?UserName", txtUsername.Text,"?UserP", txtPassword.Text, STRCONN));
             
            if (intCHECKPW==0 )
            {
                intUserErrorCode = 21; // the password entered is incorrect

                string strSQLLockReset = "SELECT UnlockUserTimestamp FROM listusers WHERE username=?UserID ";

                string strLockReset = GeneralRoutines.ReturnScalar(strSQLLockReset, "?UserID", txtUsername.Text, STRCONN);

                DateTime? dteLockReset = null;

                if (GeneralRoutines.IsDate(strLockReset))
                { 
                    dteLockReset = Convert.ToDateTime(strLockReset); 
                }

                
                string strSQLLastLogin = String.Empty;

                if (dteLockReset == null)
                {
                    strSQLLastLogin = "SELECT MAX(LoggedIn) MaxLoggedIn FROM usererrors WHERE UserID=?UserID AND ErrorCode='0'";
                }
                else
                { 
                    strSQLLastLogin = "SELECT MAX(LoggedIn) MaxLoggedIn FROM usererrors WHERE UserID=?UserID AND ErrorCode='0' ";
                    strSQLLastLogin += "AND LoggedIn>='" + Convert.ToDateTime(dteLockReset).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

               
                string strLastLogin = GeneralRoutines.ReturnScalar(strSQLLastLogin, "?UserID", txtUsername.Text, STRCONN);

                string strSQLFailedLogins = "SELECT COUNT(*) CR FROM usererrors WHERE UserID=?UserID AND ErrorCode='21' ";

                if (dteLockReset != null)
                {
                    strSQLFailedLogins += "AND LoggedIn>='" + Convert.ToDateTime(dteLockReset).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

                
                DateTime? dteLastLogin = null;

                if (!String.IsNullOrEmpty(strLastLogin))
                {
                    dteLastLogin = Convert.ToDateTime(strLastLogin);
                    strSQLFailedLogins += "AND LoggedIn>='" + Convert.ToDateTime(dteLastLogin).ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                }

                
                intFailedLogins = Convert.ToInt32(GeneralRoutines.ReturnScalar(strSQLFailedLogins, "?UserID", txtUsername.Text, STRCONN));
                //increment by 1 the failed login
                intFailedLogins += 1;
                if (intFailedLogins>=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get(strKeyNumberTries)))
                {
                    Boolean blnLockUser = LockUserDB(txtUsername.Text);
                    intUserErrorCode = 20; //locked user

                    throw new Exception("The user is locked.");
                }


                throw new Exception("The password you have entered is incorrect.");
            }
            

            // now get the session variables            
            string strSQL_DATAACCESS = string.Empty;

            //strSQL_DATAACCESS = "SELECT COUNT(*) CR FROM listdbuser t1 INNER JOIN listusers t2 ON t1.ListUsersID=t2.ListUsersID ";
            //strSQL_DATAACCESS += "WHERE t2.username=?username ";
            //strSQL_DATAACCESS += "AND t1.dataname=?DataName ";
            ////changed rajeev on 20140926
            //strSQL_DATAACCESS += "AND t1.centrecode=?Centre";
            
            //changed rajeev on 20141001
            

            strSQL_DATAACCESS = "SELECT COUNT(*) CR FROM (SELECT t2.username FROM listdbuser t1 INNER JOIN listusers t2 ON t1.ListUsersID=t2.ListUsersID ";
            strSQL_DATAACCESS += "WHERE t2.username=?username ";
            strSQL_DATAACCESS += "AND t1.dataname=?DataName ";
            //added rajeev 20140605 19:49 to allow access from more than once centre
            strSQL_DATAACCESS += "GROUP BY t2.username) t1 ";

            int intDATAACCESS = Convert.ToInt32(GeneralRoutines.ReturnScalarThree(strSQL_DATAACCESS, "?username", txtUsername.Text, "?DataName", strDataName, "?Centre", strCentreCode, STRCONN));

            if (intDATAACCESS < 0)
            {
                
                throw new Exception("Error while checking if user has rights to access the database.");
            }

            if (intDATAACCESS == 0)
            {
                intUserErrorCode = 5;
                throw new Exception("You do not have access to this database.");
            }

            if (intDATAACCESS > 1)
            {
                intUserErrorCode = 6;
                throw new Exception("More than one access right.");
            }

            //Assign sessionvariables

            string strSQL= "SELECT t1.* FROM listdbuser t1 INNER JOIN listusers t2 ON t1.ListUsersID=t2.ListUsersID ";
            strSQL += "WHERE t2.username=?username ";
            strSQL += "AND t1.dataname=?DataName";

            

            string CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;
            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?username", MySqlDbType.VarChar).Value = txtUsername.Text;
            MyCMD.Parameters.Add("?dataname", MySqlDbType.VarChar).Value = strDataName;


            MyCONN.Open();
            try
            {
                MySqlDataReader DR = MyCMD.ExecuteReader();

                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        SessionVariablesAll.Web_cope41 = "True";
                        SessionVariablesAll.UserName = txtUsername.Text;

                        if (DR["AdminSuperUser"] != DBNull.Value)
                        {
                            SessionVariablesAll.AdminSuperUser = DR["AdminSuperUser"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.AdminSuperUser = "NO";
                        }


                        if (SessionVariablesAll.AdminSuperUser==STR_YES_SELECTION)
                        {
                            SessionVariablesAll.SuperUser = STR_YES_SELECTION;
                            SessionVariablesAll.CreateStudyID = STR_YES_SELECTION;
                            SessionVariablesAll.AddEdit = STR_YES_SELECTION;
                            SessionVariablesAll.AddEditRecipient = STR_YES_SELECTION;
                            SessionVariablesAll.AddEditFollowUp = STR_YES_SELECTION;
                            SessionVariablesAll.Randomise = STR_YES_SELECTION;
                            SessionVariablesAll.ViewRandomise = STR_YES_SELECTION;
                        }
                        else
                        {
                            if (DR["SuperUser"] != DBNull.Value)
                            {
                                SessionVariablesAll.SuperUser = DR["SuperUser"].ToString();
                            }
                            else
                            {
                                SessionVariablesAll.SuperUser = "NO";
                            }

                            if (DR["CreateStudyID"] != DBNull.Value)
                            {
                                SessionVariablesAll.CreateStudyID = DR["CreateStudyID"].ToString();
                            }
                            else
                            {
                                SessionVariablesAll.CreateStudyID = "NO";
                            }

                            if (DR["AddEdit"] != DBNull.Value)
                            {
                                SessionVariablesAll.AddEdit = DR["AddEdit"].ToString();
                            }
                            else
                            {
                                SessionVariablesAll.AddEdit = "NO";
                            }

                            if (DR["AddEditRecipient"] != DBNull.Value)
                            {
                                SessionVariablesAll.AddEditRecipient = DR["AddEditRecipient"].ToString();
                            }
                            else
                            {
                                SessionVariablesAll.AddEditRecipient = "NO";
                            }

                            if (DR["AddEditFollowUp"] != DBNull.Value)
                            {
                                SessionVariablesAll.AddEditFollowUp = DR["AddEditFollowUp"].ToString();
                            }
                            else
                            {
                                SessionVariablesAll.AddEditFollowUp = "NO";
                            }

                            if (DR["Randomise"] != DBNull.Value)
                            {
                                SessionVariablesAll.Randomise = DR["Randomise"].ToString();
                            }
                            else
                            {
                                SessionVariablesAll.Randomise = "NO";
                            }

                            if (DR["ViewRandomise"] != DBNull.Value)
                            {
                                SessionVariablesAll.ViewRandomise = DR["ViewRandomise"].ToString();
                            }
                            else
                            {
                                SessionVariablesAll.ViewRandomise = "NO";
                            }


                            
                        }

                        if (DR["centrecode"] != DBNull.Value)
                        {
                            SessionVariablesAll.CentreCode = DR["centrecode"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.CentreCode = "";
                        }


                    }
                }

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

            }

            catch (System.Exception excep)
                                
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                SessionVariablesAll.Web_cope41 = "False";
                txtPassword.Text = string.Empty;

                intUserErrorCode = 7;

                lblUserMessages.Text = excep.Message + " An error occured while querying access rights.";
            }


            //update error code


            Boolean insertData;


            if (intUserErrorCode == 0)
            {

                insertData = InsertErrorLog(txtUsername.Text, strDataName, intUserErrorCode);
                insertData = InsertLogInDetails(txtUsername.Text, strDataName, intUserErrorCode);
            }
            else
            {
                insertData = InsertErrorLog(txtUsername.Text, strDataName, intUserErrorCode);
            }

            if (insertData == false)
            {
                lblUserMessages.Text += "An error occured while updating logs";
            }

            //get the last login date time

            string STRSQL_LASTLOGIN = string.Empty;

            STRSQL_LASTLOGIN = "SELECT LoggedIn FROM copewpfourother.userlogs WHERE UserID=?UserID ORDER BY LoggedIn DESC LIMIT 1 OFFSET 1";

            string STRLASTLOGIN = GeneralRoutines.ReturnScalar(STRSQL_LASTLOGIN, "?UserID", txtUsername.Text, STRCONN);

            if (STRLASTLOGIN != string.Empty && STRLASTLOGIN != "-1")
            {
                SessionVariablesAll.LastLogin = STRLASTLOGIN;
            }

            if (SessionVariablesAll.Web_cope41 == "True")
            {
                Response.Redirect(REDIRECTPAGE, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }


        }
        
        
        catch (System.Exception excep)
        {
            SessionVariablesAll.Web_cope41 = "False";

            txtPassword.Text = string.Empty;
            if (intUserErrorCode == 0)
            {
                intUserErrorCode = 99; // unknown error code 
            }

            
            lblUserMessages.Text += excep.Message + " An error occured while logging into the database.";
        }

        ////update error code

       
        //Boolean insertData;

        
        //if (intUserErrorCode == 0)
        //{

        //    insertData = InsertErrorLog(txtUsername.Text, strDataName, intUserErrorCode);
        //    insertData = InsertLogInDetails(txtUsername.Text, strDataName, intUserErrorCode);
        //}
        //else
        //{
        //    insertData = InsertErrorLog(txtUsername.Text, strDataName, intUserErrorCode);
        //}

        //if (insertData == false)
        //{
        //    lblUserMessages.Text += "An error occured while updating logs";
        //}

        ////get the last login date time

        //string STRSQL_LASTLOGIN = string.Empty;

        //STRSQL_LASTLOGIN = "SELECT LoggedIn FROM copewpfourother.userlogs WHERE UserID=?UserID ORDER BY LoggedIn DESC LIMIT 1 OFFSET 1";

        //string STRLASTLOGIN = GeneralRoutines.ReturnScalar(STRSQL_LASTLOGIN, "?UserID", txtUsername.Text, STRCONN);

        //if (STRLASTLOGIN != string.Empty && STRLASTLOGIN != "-1")
        //{
        //    SessionVariablesAll.LastLogin = STRLASTLOGIN;
        //}

        //if (SessionVariablesAll.Web_cope41 == "True")
        //{
        //    Response.Redirect(REDIRECTPAGE, false);
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        
    }

    protected Boolean InsertErrorLog(string strUserID, string strDatabase, int intError)
    {
        Boolean blnError = false;

        try
        {
            string strSQL = string.Empty;

            strSQL += "INSERT INTO usererrors";
            //strSQL += "(UserID, ErrorCode, DbName, LoggedIn, IPAddress) "; //, SessionID) ";
            strSQL += "(UserID, ErrorCode, DbName, LoggedIn, IPAddress, SessionID) ";
            strSQL += "VALUES ";
           strSQL += "(?UserID, ?ErrorCode, ?DbName, ?LoggedIn, ?IPAddress, ?SessionID)";
            
            string CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;
            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?UserID", MySqlDbType.VarChar).Value = strUserID;
            MyCMD.Parameters.Add("?ErrorCode", MySqlDbType.Int64).Value = intError;
            MyCMD.Parameters.Add("?DbName", MySqlDbType.VarChar).Value = strDatabase;
            MyCMD.Parameters.Add("?LoggedIn", MySqlDbType.DateTime).Value = DateTime.Now;
            MyCMD.Parameters.Add("?IPAddress", MySqlDbType.VarChar).Value = Request.UserHostAddress;

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            { MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = Request.Cookies["ASP.NET_SessionId"].Value; }
            else
            { MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = DBNull.Value; }
            //if (!String.IsNullOrEmpty(Request.Cookies["ASP.NET_SessionId"].Value))
            //{ }
            //else
            //{ MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = DBNull.Value; }

                        

            MyCONN.Open();
            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                { 
                    MyCONN.Close(); 
                }
                blnError = true;
            }
            catch (System.Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open)
                
                {
                    MyCONN.Close();
                }

                blnError = false;
                lblUserMessages.Text = excep.Message + " An error occured while executing error log query.";
            }

        }
        catch (System.Exception excep)
            {
            blnError=false;
            lblUserMessages.Text=excep + " An error occured while updating error log";
            }


        return blnError;
    }

    //insert log in details
    protected Boolean InsertLogInDetails(string strUserID, string strDatabase, int intError)
    {
        Boolean blnError = false;

        try
        {
            string strSQL = string.Empty;

            strSQL += "INSERT INTO userlogs";
            strSQL += "(UserID, DbName, LoggedIn, IPAddress, SessionID) ";
            strSQL += "VALUES ";
            strSQL += "(?UserID, ?DbName, ?LoggedIn, ?IPAddress, ?SessionID)";

            string CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;
            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?UserID", MySqlDbType.VarChar).Value = strUserID;
            MyCMD.Parameters.Add("?DbName", MySqlDbType.VarChar).Value = strDatabase;
            MyCMD.Parameters.Add("?LoggedIn", MySqlDbType.DateTime).Value = DateTime.Now;
            MyCMD.Parameters.Add("?IPAddress", MySqlDbType.VarChar).Value = Request.UserHostAddress;
            //MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = Request.Cookies["ASP.NET_SessionId"].Value;

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            { MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = Request.Cookies["ASP.NET_SessionId"].Value; }
            else
            { MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = DBNull.Value; }

            MyCONN.Open();
            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                blnError = true;
            }
            catch (System.Exception excep)

            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                blnError = false;
                lblUserMessages.Text = excep.Message + " An error occured while executing error log query.";
            }

        }
        catch (System.Exception excep)
        {
            blnError = false;
            lblUserMessages.Text = excep + " An error occured while updating error log";
        }


        return blnError;



    }

    protected Boolean LockUserDB(string strUserID)
    {
        Boolean blnLockedUser=false;

        try
        {

            string strSQL = string.Empty;

            strSQL += "UPDATE listusers SET ";
            strSQL += "LockedUser=?LockedUser, LockedUserTimestamp=?LockedUserTimestamp WHERE UserName=?UserName ";


            string CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;
            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?LockedUser", MySqlDbType.VarChar).Value = 1; //user locked
            MyCMD.Parameters.Add("?LockedUserTimestamp", MySqlDbType.DateTime).Value = DateTime.Now;
            MyCMD.Parameters.Add("?UserName", MySqlDbType.VarChar).Value = strUserID;

            MyCONN.Open();
            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                blnLockedUser = true;
            }
            catch (System.Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                blnLockedUser = false;
                lblUserMessages.Text = excep.Message + " An error occured while executing error log query.";
            }

        }
        catch (System.Exception excep)
        {
            blnLockedUser = false;
            lblUserMessages.Text = excep + " An error occured while locking user";
        }

        return blnLockedUser;
    }

}