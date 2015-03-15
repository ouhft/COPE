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


public partial class OtherArea_AENUser : System.Web.UI.Page
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

                lblPageDescription.Text = "Add a New User.";

                lblPageDescription.Text += "<br/>(Click on User in the table at the bottom of the page to modify access rights for an existing user.)";
                ViewState["SortField"] = "UserName";
                ViewState["SortDirection"] = "ASC";

                ViewState["SortField2"] = "UserName";
                ViewState["SortDirection2"] = "ASC";

                ViewState["SortField3"] = "Centre";
                ViewState["SortDirection3"] = "ASC";

                BindData();

                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    BindDataGV2();
                }
                BindDataGV3();
                

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
    protected void BindDataGV3()
    {
        try
        {

            lblGV3.Text = "Assign Access ";

            string strSQL = String.Empty;

            if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
            {
                strSQL = @"SELECT CONCAT(CountryCode, CentreCode) Centre, CentreName 
                        FROM cope_wp_four.lstcentres 
                        ";
            }
            else
            {
                strSQL = @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, t1.CentreName 
                        FROM cope_wp_four.lstcentres  t1 
                        INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                        INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                        WHERE t3.UserName=?UserName  
                        AND t2.SuperUser='" + STR_YES_SELECTION + "' ";
            }

            strSQL += "ORDER BY " + ViewState["SortField3"] + " " + ViewState["SortDirection3"] ;


            GV3.DataSource = sqldsGV3;
            
            sqldsGV3.SelectCommand = strSQL;
            sqldsGV3.SelectParameters.Clear();
            sqldsGV3.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            GV3.DataBind();

            lblGV3.Text = "Tick Checkboxes for Access";

        }
        catch (Exception excep)
        {
            lblGV3.Text = excep.Message + " An error occured while binding the page.";
        }
    }

    //sorting main datagrid
    protected void GV3_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression.ToString() == ViewState["SortField3"].ToString())
        {
            switch (ViewState["SortDirection3"].ToString())
            {
                case "ASC":
                    ViewState["SortDirection3"] = "DESC";
                    break;
                case "DESC":
                    ViewState["SortDirection3"] = "ASC";
                    break;
            }

        }
        else
        {
            ViewState["SortField3"] = e.SortExpression;
            ViewState["SortDirection3"] = "DESC";
        }
        BindDataGV3();
    }

    //protected void BindData2()
    //{
    //    try
    //    {

    //        string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

    //        //string STRCentre = SessionVariablesAll.CentreCode;

    //        string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

    //        string strSQL = String.Empty;

    //        //strSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Centre  ";
    //        //strSQL += " ";
    //        //strSQL += "FROM listusers t1 LEFT JOIN  listdbuser t2 on t1.ListUsersID=t2. ListUsersID  ";
    //        //strSQL += "AND t2.DataName='" + STRDbName + "' ";
    //        ////strSQL += "AND t2.centrecode='" + STRCentre + "' ";
    //        ////strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
    //        ////strSQL += "WHERE t2.ID IS NULL AND t1.Centre='" + STRCentre + "' ";
    //        //strSQL += "WHERE t2.ID IS NULL  ";
    //        //strSQL += "AND (t1.Active <> 0 or t1.Active IS NULL) ";
    //        //strSQL += "AND (t2.centrecode='' OR t2.centrecode IS NULL) ";
    //        //strSQL += "ORDER BY " + ViewState["SortField2"] + " " + ViewState["SortDirection2"];

    //        //lblUserMessages.Text = strSQL;
    //        strSQL += "SELECT t1.ListUsersID, t1.Active, t1.username,t1.FirstName, t1.LastName, t1.Email, t1.JobTitle, t1.Centre  ";
    //        strSQL += " ";
    //        strSQL += "FROM listusers t1 LEFT JOIN (SELECT t1.* FROM listusers t1 INNER JOIN listdbuser t2  ";
    //        strSQL += "ON t1.ListUsersID=t2.ListUsersID WHERE t2.centrecode='" + SessionVariablesAll.CentreCode + "') t2 ";
    //        strSQL += "ON t1.ListUsersID=t2.ListUsersID ";
    //        //strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
    //        strSQL += "WHERE (t1.Active <> 0 or t1.Active IS NULL) AND  t2.ListUsersID IS NULL  ";
    //        strSQL += "ORDER BY " + ViewState["SortField2"] + " " + ViewState["SortDirection2"];

    //        GV2.DataSource = SqlDataSource2;
    //        SqlDataSource2.SelectCommand = strSQL;

    //        GV2.DataBind();
    //        lblGV2.Text = "Number of Users Not assigned Access Rights " + GV2.Rows.Count.ToString() + ". Click on UserID to Assign access rights.";
    //    }
    //    catch (Exception excep)
    //    {
    //        lblGV2.Text = excep.Message + " An error occured while binding the page.";
    //    }

    //    //lblUserMessages.Text = strSQL;
    //}

    ////sorting main datagrid
    //protected void GV2_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    if (e.SortExpression.ToString() == ViewState["SortField2"].ToString())
    //    {
    //        switch (ViewState["SortDirection2"].ToString())
    //        {
    //            case "ASC":
    //                ViewState["SortDirection2"] = "DESC";
    //                break;
    //            case "DESC":
    //                ViewState["SortDirection2"] = "ASC";
    //                break;
    //        }

    //    }
    //    else
    //    {
    //        ViewState["SortField2"] = e.SortExpression;
    //        ViewState["SortDirection2"] = "DESC";
    //    }
    //    BindData2();
    //}
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
            SQLDATADETAILS += "(dataname, ListusersID, centrecode, SuperUser, AddEdit,  AddEditRecipient, AddEditFollowUp,  Randomise, ViewRandomise, ";
            SQLDATADETAILS += "SAECentre, TrialIDCentre, ";
            SQLDATADETAILS += "Comments, DateCreated, CreatedBy) ";
            SQLDATADETAILS += "VALUES ";
            SQLDATADETAILS += "(?dataname, ?ListusersID, ?centrecode, ?SuperUser, ?AddEdit, ?AddEditRecipient, ?AddEditFollowUp,  ?Randomise, ?ViewRandomise, ";
            SQLDATADETAILS += "?SAECentre, ?TrialIDCentre,";
            SQLDATADETAILS += "?DataComments, ?DateCreated, ?CreatedBy) ";
            
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
            //MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar).Value = SessionVariablesAll.CentreCode;
            //MyCMD.Parameters.Add("?SuperUser", MySqlDbType.VarChar).Value = rdoSuperUser.SelectedValue;
            //MyCMD.Parameters.Add("?AddEdit", MySqlDbType.VarChar).Value = rdoAddEdit.SelectedValue;
            //MyCMD.Parameters.Add("?Randomise", MySqlDbType.VarChar).Value = rdoRandomise.SelectedValue;
            //MyCMD.Parameters.Add("?DataComments", MySqlDbType.VarChar).Value = txtDataComments.Text;

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

                MyCMD.Parameters.Add("?ListusersID", MySqlDbType.VarChar).Value = UserID;

                ////loop through selection
                //Label lblCentreGV3;
                //CheckBox chkGV3;

                MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?SuperUser", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?AddEdit", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?AddEditRecipient", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?AddEditFollowUp", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?Randomise", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?ViewRandomise", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?SAECentre", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?TrialIDCentre", MySqlDbType.VarChar);
                MyCMD.Parameters.Add("?DataComments", MySqlDbType.VarChar);

                string strCentreCode = string.Empty;
                string strSuperUser = STR_NO_SELECTION;
                string strAddEdit = STR_NO_SELECTION;
                string strAddEditRecipient = STR_NO_SELECTION;
                string strAddEditFollowUp = STR_NO_SELECTION;
                string strRandomise = STR_NO_SELECTION;
                string strViewRandomise = STR_NO_SELECTION;
                string strSAECentre = STR_NO_SELECTION;
                string strTrialIDCentre = STR_NO_SELECTION;
                string strDataComments = STR_NO_SELECTION;

                //loop through selection
                Label lblCentreGV3;
                CheckBox chkGV3;
                TextBox txtCommentsGV3;


                foreach (GridViewRow row in GV3.Rows)
                {

                    lblCentreGV3 = (Label)row.FindControl("lblCentre");

                    //lblEmail.Text += lblCentreGV3.Text + ">";
                    strCentreCode = lblCentreGV3.Text;

                    chkGV3 = (CheckBox)row.FindControl("chkSuperUser");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strSuperUser = STR_YES_SELECTION;
                        }
                        else
                        {
                            strSuperUser = STR_NO_SELECTION;
                        }

                    }
                    //Randomise
                    chkGV3 = (CheckBox)row.FindControl("chkRandomise");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strRandomise = STR_YES_SELECTION;
                        }
                        else
                        {
                            strRandomise = STR_NO_SELECTION;
                        }
                    }

                    //ViewRandomise
                    chkGV3 = (CheckBox)row.FindControl("chkViewRandomise");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strViewRandomise = STR_YES_SELECTION;
                        }
                        else
                        {
                            strViewRandomise = STR_NO_SELECTION;
                        }
                    }

                    //AddEdit
                    chkGV3 = (CheckBox)row.FindControl("chkAddEdit");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strAddEdit = STR_YES_SELECTION;
                        }
                        else
                        {
                            strAddEdit = STR_NO_SELECTION;
                        }
                    }

                    //AddEdit
                    chkGV3 = (CheckBox)row.FindControl("chkAddEditRecipient");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strAddEditRecipient = STR_YES_SELECTION;
                        }
                        else
                        {
                            strAddEditRecipient = STR_NO_SELECTION;
                        }
                    }

                    //AddEditFollowUp
                    chkGV3 = (CheckBox)row.FindControl("chkAddEditFollowUp");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strAddEditFollowUp = STR_YES_SELECTION;
                        }
                        else
                        {
                            strAddEditFollowUp = STR_NO_SELECTION;
                        }
                    }

                    //TrialIDCentre
                    chkGV3 = (CheckBox)row.FindControl("chkTrialIDCentre");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strTrialIDCentre = STR_YES_SELECTION;
                        }
                        else
                        {
                            strTrialIDCentre = STR_NO_SELECTION;
                        }
                    }

                    //SAECentre
                    chkGV3 = (CheckBox)row.FindControl("chkSAECentre");
                    if (chkGV3 != null)
                    {
                        if (chkGV3.Checked == true)
                        {
                            strSAECentre = STR_YES_SELECTION;
                        }
                        else
                        {
                            strSAECentre = STR_NO_SELECTION;
                        }
                    }

                    //comments
                    txtCommentsGV3 = (TextBox)row.FindControl("txtComments");
                    strDataComments = txtCommentsGV3.Text;


                    MyCMD.Parameters["?centrecode"].Value = strCentreCode;
                    MyCMD.Parameters["?SuperUser"].Value = strSuperUser;
                    MyCMD.Parameters["?Randomise"].Value = strRandomise;
                    MyCMD.Parameters["?ViewRandomise"].Value = strViewRandomise;
                    MyCMD.Parameters["?AddEdit"].Value = strAddEdit;
                    MyCMD.Parameters["?AddEditRecipient"].Value = strAddEditRecipient;
                    MyCMD.Parameters["?AddEditFollowUp"].Value = strAddEditFollowUp;
                    MyCMD.Parameters["?SAECentre"].Value = strSAECentre;
                    MyCMD.Parameters["?TrialIDCentre"].Value = strTrialIDCentre;
                    MyCMD.Parameters["?DataComments"].Value = strDataComments;

                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = SQLDATADETAILS;
                    MyCMD.ExecuteNonQuery();

                }

                

                myTrans.Commit();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();

                }

                BindData();
                //BindData2();
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