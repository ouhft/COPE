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

public partial class OtherArea_AUList : System.Web.UI.Page
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
                if (SessionVariablesAll.AdminSuperUser == "YES" || SessionVariablesAll.SuperUser=="YES")
                {
                    ViewState["SortField"] = "UserName";
                    ViewState["SortDirection"] = "ASC";

                    BindData();


                    ViewState["SortField2"] = "UserName";
                    ViewState["SortDirection2"] = "ASC";

                   

                    //BindData();

                    if (SessionVariablesAll.AdminSuperUser == "YES")
                    {
                        BindDataGV2();
                    }
                }
                else
                {
                    Response.Redirect(strAccessDenied, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
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

            //string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

            string strSQL = String.Empty;

            strSQL += "SELECT t1.ListUsersID, t1.UserName, t1.Active, t1.Firstname, t1.LastName,t1.Email,t1.JobTitle,t1.LockedUser, ";
            //strSQL += "t2.ID, t2.CentreCode, t2.AdminSuperUser, t2.SuperUser, t2.AddEdit, t2.Randomise,t2.PersonalData,";
            strSQL += "t2.*,";
            strSQL += "t3.LastLogin ";
            strSQL += "FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  AND t2.dataname='" + STRDbName + "' ";
            strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
            strSQL += "ON t1.UserName=t3.UserID ";
            //strSQL += "AND t2.dataname='" + STRDbName + "' ";
            //strSQL += "AND t1.centre='" + STRCentre + "' ";
            //strSQL += "AND t2.centrecode='" + STRCentre + "' ";
            //strSQL += "AND t2.centrecode='" + SessionVariablesAll.CentreCode + "' ";
            if (SessionVariablesAll.AdminSuperUser=="YES")
            {

            }
            else
            {
                strSQL += "INNER JOIN ";
                strSQL += @"(SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                            FROM cope_wp_four.lstcentres  t1  
                            INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                            INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                            WHERE t3.UserName=?UserName  
                            AND (t2.SuperUser='YES') ORDER BY t1.CountryCode, t1.CentreCode) t4 ";

                strSQL += "ON t4.Centre=t2.centrecode ";
            }

            strSQL += "WHERE (t1.Active <> 0 OR t1.Active IS NULL) AND (t2.AdminSuperUser <> 'YES' OR  t2.AdminSuperUser IS NULL) ";

            if ((string)ViewState["SortField"]=="CentreCode")
            {
                strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
                strSQL += ", UserName ";
            }
            else if ((string)ViewState["SortField"] == "UserName")
            {
                strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
                strSQL += ", CentreCode ";
            }
            else
            {
                strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            }
            

            //lblUserMessages.Text = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);
            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;

            GV1.DataBind();
            lblGV1.Text = "Number of records returned " + GV1.Rows.Count.ToString() + ". ";
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
}