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

public partial class OtherArea_RUser : System.Web.UI.Page
{

    #region " Private Constants & Variables "

        private const string STRCONN = "cope4usrconn";

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

                if (String.IsNullOrEmpty(SessionVariablesAll.UserName))
                {
                    throw new Exception("Could not obtain User Name.");
                }

                //string STRSQL = string.Empty;

                //STRSQL += "SELECT COUNT(*) CR FROM listusers WHERE UserName=?UserName ";

                //string STRCOUNT = GeneralRoutines.ReturnScalar(STRSQL, "?UserName", SessionVariablesAll.UserName, STRCONN);
                //if (!GeneralRoutines.IsNumeric(STRCOUNT))
                //{
                //    throw new Exception("Could not check if User exists in the database.");
                //}

                //if (Convert.ToInt32(STRCONN) < 0)
                //{
                //    throw new Exception("Error when checking if User exists in the database.");
                //}

                //if (Convert.ToInt32(STRCONN) > 1)
                //{
                //    throw new Exception("More than one user with the same User Name exists in the database.");
                //}

                txtUserName.Text = SessionVariablesAll.UserName;

            }
        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while loading the page.";
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

            if (txtUserPassNew.Text.Length < INT_PASSLENGTH)
            { throw new Exception("The Length of password should be " + INT_PASSLENGTH.ToString()); }

            if (txtUserPassNew.Text.Contains(" "))
            { throw new Exception("Password should not contain a space."); }

            if (txtUserPassNew.Text == txtUserName.Text)
            { throw new Exception("The User Name and the New Password cannot be the same."); }

            if (txtUserPass.Text == txtUserPassNew.Text)
            { throw new Exception("The New Password should be different from the current password."); }

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

            string STR_DATECREATED = GeneralRoutines.ReturnScalar(STR_SQLDATECREATED, "?UserName", SessionVariablesAll.UserName, STRCONN);

            string STR_SQLCREATEDBY = "SELECT CreatedBy FROM listusers WHERE UserName=?UserName  ";

            string STR_CREATEDBY = GeneralRoutines.ReturnScalar(STR_SQLCREATEDBY, "?UserName", SessionVariablesAll.UserName, STRCONN);

            string STRPWORD = string.Empty;
            string STRPWORD_NEW = string.Empty;

            if (GeneralRoutines.IsDate(STR_DATECREATED) == true)
            {
                if (!String.IsNullOrEmpty(STR_CREATEDBY))
                {
                    //STRPWORD_NEW = txtUserPassNew.Text + ":" + Convert.ToDateTime(STR_DATECREATED).ToString("yyyy-MM-dd HH:mm:ss") + ";" + STR_CREATEDBY.Substring(0, 2);
                    STRPWORD = txtUserPass.Text + ":" + Convert.ToDateTime(STR_DATECREATED).ToString("yyyy-MM-dd HH:mm:ss") + ";" + STR_CREATEDBY.Substring(0, 2);
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

            
            string STR_SQLCHECKCURRENT = string.Empty;

            STR_SQLCHECKCURRENT += "SELECT COUNT(*) CR FROM listusers WHERE UserName=?UserName AND UserPword=SHA2(?UserPword,512) ";

            int INT_CHECKCURRENT = 0;

            if (GeneralRoutines.IsNumeric(GeneralRoutines.ReturnScalarTwo(STR_SQLCHECKCURRENT, "?UserName", SessionVariablesAll.UserName, "?UserPword", STRPWORD, STRCONN)) == true)
            { 
                INT_CHECKCURRENT=Convert.ToInt32(GeneralRoutines.ReturnScalarTwo(STR_SQLCHECKCURRENT, "?UserName", SessionVariablesAll.UserName, "?UserPword", STRPWORD, STRCONN));

            }

            if (INT_CHECKCURRENT < 0)
            { throw new Exception("Could not check if the password you have entered is correct."); }

            if (INT_CHECKCURRENT == 0)
            { throw new Exception("The password you have entered does not match."); }


            string strUPDATE = string.Empty;
            strUPDATE += "UPDATE listusers SET ";
            strUPDATE += "UserPword=SHA2(?UserPwordNew,512), ";
            strUPDATE += "PassUpdated=?PassUpdated, PassUpdatedBy=?PassUpdatedBy ";
            strUPDATE += "WHERE UserName=?UserName ";
            //strUPDATE += "AND Centre=?Centre ";

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
            MyCMD.Parameters.Add("?PassUpdated", MySqlDbType.DateTime).Value = DateTime.Now;
            MyCMD.Parameters.Add("?PassUpdatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

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

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while resetting the password.";
        }
    }
}