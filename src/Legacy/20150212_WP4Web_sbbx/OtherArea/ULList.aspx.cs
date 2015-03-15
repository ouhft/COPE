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
public partial class OtherArea_ULList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                lblUserMessages.Text = string.Empty;

                ViewState["SortField"] = "LoggedIn";
                ViewState["SortDirection"] = "DESC";
                BindData();


            }
            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
            }
        }

    }

    protected void BindData()
    {
        try
        {
            lblGV1.Text = string.Empty;

            string STRSQL = string.Empty;
            STRSQL += "SELECT t1.*,  ";
            STRSQL += "DATE_FORMAT(LoggedIn, '%d/%m/%Y %H:%i') Logged_In ";
            STRSQL += "FROM wp2_cope_other.userlogs t1 ";
            STRSQL += "WHERE t1.UserID=?UserID ";

            STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?UserID", SessionVariablesAll.UserName);


            sqldsGV1.SelectCommand = STRSQL;
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();


            lblGV1.Text = "Number of Database Login Records " + GV1.Rows.Count.ToString();

        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding data.";
        }
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
}