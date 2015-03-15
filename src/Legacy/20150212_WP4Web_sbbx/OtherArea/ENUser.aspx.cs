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

public partial class OtherArea_ENUser : System.Web.UI.Page
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
                lblUserMessages.Text=string.Empty;


                if (SessionVariablesAll.SuperUser != "YES")
                {
                    Response.Redirect(strAccessDenied, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }

                if (string.IsNullOrEmpty(Request.QueryString["OtherID"]))
                {
                    throw new Exception("Could not obtain the Unique identifier for the User whose access needs to be modified.");
                }

                ViewState["SortField"] = "UserName";
                ViewState["SortDirection"] = "ASC";

                
                BindData();
                
                

                //get username
                string STRSQL = "SELECT username FROM listusers WHERE ListusersID=?ListusersID ";
                string strUserID = GeneralRoutines.ReturnScalar(STRSQL, "?ListusersID", Request.QueryString["OtherID"], STRCONN);


                ViewState["username"] = string.Empty;

                if (strUserID=="-1")
                {
                    throw new Exception("Selected user does not exist.");
                }

                ViewState["username"] = strUserID;

                STRSQL = "SELECT Active FROM listusers WHERE ListusersID=?ListusersID ";
                int intActive = Convert.ToInt16( GeneralRoutines.ReturnScalar(STRSQL, "?ListusersID", Request.QueryString["OtherID"], STRCONN));
                if (intActive <=0)
                {
                    throw new Exception("User " + strUserID + " is not Active");
                }

                ViewState["SortField3"] = "Centre";
                ViewState["SortDirection3"] = "ASC";
                               

                BindDataGV3();
                //lblGV3.Text += " " + strUserID;

            }
        }
        catch(Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }

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

            strSQL += "WHERE  (t1.Active <> 0 OR t1.Active IS NULL) AND (t2.AdminSuperUser <> 'YES' OR  t2.AdminSuperUser IS NULL) ";
            strSQL += "GROUP BY t1.ListusersID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

            GV1.DataBind();
            lblGV1.Text = "Number of Users assigned Access Rights " + GV1.Rows.Count.ToString() + ". Click on UserID to edit access rights.";
            //lblUserMessages.Text = strSQL;

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

    protected void BindDataGV3()
    {
        try
        {

            lblGV3.Text = "Assign Access ";

            string strSQL = String.Empty;

            if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
            {
                strSQL = @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, t1.CentreName, t3.*  
                        FROM cope_wp_four.lstcentres  t1                        
                        LEFT JOIN (SELECT t1.*, t2.Active FROM listdbuser t1 INNER JOIN listusers t2
                        ON t1.ListusersID=t2.ListusersID WHERE t1.ListusersID=?ListusersID ) t3
                        ON CONCAT(t1.CountryCode, t1.CentreCode)=t3.centrecode 
                         
                        
                          ";
            }
            else
            {
                strSQL = @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, t1.CentreName, t2.*  
                        FROM cope_wp_four.lstcentres  t1 
                        INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                        INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                        WHERE t3.UserName=?UserName  
                        AND t2.SuperUser='" + STR_YES_SELECTION + "' AND (t3.Active<>0) ";
            }

            strSQL += "AND (t3.Active <> 0 OR t3.Active IS NULL) ";
            strSQL += "ORDER BY " + ViewState["SortField3"] + " " + ViewState["SortDirection3"] ;

            //lblUserMessages.Text = strSQL;
            GV3.DataSource = sqldsGV3;
            
            sqldsGV3.SelectCommand = strSQL;
            sqldsGV3.SelectParameters.Clear();
            sqldsGV3.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);
            sqldsGV3.SelectParameters.Add("?ListusersID", Request.QueryString["OtherID"]);
            GV3.DataBind();

            lblGV3.Text = "Add/Update Access Rights for <font color=Red>" + ViewState["username"].ToString() + "</font>";
            
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

    protected void GV3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // CHECK IF ITS A ROW. ELSE THE HEADER WILL BE TREATED AS A ROW.
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            //e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
            e.Row.Attributes["onmouseover"] = "highlight(this, true);";
            e.Row.Attributes["onmouseout"] = "highlight(this, false);";
            HttpResponse myHttpResponse = Response;
            HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(myHttpResponse.Output);
            e.Row.Attributes.AddAttributes(myHtmlTextWriter);
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
            if (string.IsNullOrEmpty(Request.QueryString["OtherID"]))
            {
                throw new Exception("Could not obtain the Unique identifier for the User whose access needs to be modified.");
            }

            //check if the user is a superadmin user
            string STRSQLFIND ="";

            STRSQLFIND += "SELECT COUNT(*) CR FROM listdbuser t1 WHERE ListUsersID=?ListUsersID AND AdminSuperUser='YES' GROUP BY ListUsersID ";


            Int16 intCountAdminSuperUser = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQLFIND, "?ListUsersID", Request.QueryString["OtherID"], STRCONN));

            if (intCountAdminSuperUser>0)
            {
                throw new Exception("You do not have the rights to modify useraccess for an 'Admin Super User'");
            }


            //if (intCountAdminSuperUser < 0)
            //{
            //    throw new Exception("An error occured while checking if you have the rights to modify useraccess for an 'Admin Super User'");
            //}

            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);
            string strOtherID = Request.QueryString["OtherID"];
            string STRSQL = string.Empty;
            STRSQL += "INSERT INTO listdbuser ";
            STRSQL += "(dataname, ListusersID, centrecode, SuperUser, AddEdit,  AddEditRecipient, AddEditFollowUp,  Randomise, ViewRandomise, ";
            STRSQL += "SAECentre, TrialIDCentre, ";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?dataname, ?ListusersID, ?centrecode, ?SuperUser, ?AddEdit, ?AddEditRecipient, ?AddEditFollowUp,  ?Randomise, ?ViewRandomise, ";
            STRSQL += "?SAECentre, ?TrialIDCentre,";
            STRSQL += "?DataComments, ?DateCreated, ?CreatedBy) ";

            string STRSQLUPDATE = string.Empty;

            STRSQLUPDATE += "UPDATE listdbuser SET ";
            STRSQLUPDATE += "dataname=?dataname, SuperUser=?SuperUser, AddEdit=?AddEdit,  AddEditRecipient=?AddEditRecipient, AddEditFollowUp=?AddEditFollowUp, ";  
            STRSQLUPDATE += "Randomise=?Randomise, ViewRandomise=?ViewRandomise, ";
            STRSQLUPDATE += "SAECentre=?SAECentre, TrialIDCentre=?TrialIDCentre, ";
            STRSQLUPDATE += "Comments=?DataComments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQLUPDATE += "WHERE ListUsersID=?ListUsersID AND centrecode=?centrecode ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            //MyCMD.CommandType = CommandType.Text;

            MyCMD.Parameters.Clear();


            MyCMD.Parameters.Add("?ListUsersID", MySqlDbType.VarChar).Value = strOtherID;
            MyCMD.Parameters.Add("?centrecode", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;
            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCMD.Parameters.Add("?dataname", MySqlDbType.VarChar).Value = STRDbName;
            MyCMD.Parameters.Add("?SuperUser", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?AddEdit", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?AddEditRecipient", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?AddEditFollowUp", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?Randomise", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?ViewRandomise", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?SAECentre", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?TrialIDCentre", MySqlDbType.VarChar);
            MyCMD.Parameters.Add("?DataComments", MySqlDbType.VarChar);

            string strListUsersID = string.Empty;
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



            MyCONN.Open();

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            //loop through selection
            Label lblListUsersID;
            Label lblCentreGV3;
            CheckBox chkGV3;
            TextBox txtCommentsGV3;

            try
            {
                foreach (GridViewRow row in GV3.Rows)
                {

                    lblListUsersID = (Label)row.FindControl("lblListusersID");

                    //lblEmail.Text += lblCentreGV3.Text + ">";
                    strListUsersID = lblListUsersID.Text;

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

                    
                    if (strListUsersID == string.Empty)
                    {
                        MyCMD.CommandText = STRSQL;

                    }
                    else
                    {
                        MyCMD.CommandText = STRSQLUPDATE;
                    }
                    MyCMD.ExecuteNonQuery();

                    
                }

                myTrans.Commit();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();

                }

                BindData();

                BindDataGV3();
            }
            catch (Exception ex)
            {

                myTrans.Rollback();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();

                }

                lblUserMessages.Text=ex.Message + " An error occured while executing query.";
            }
            

            

            


        }
        catch(Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while updating access rights.";
        }
    }
}