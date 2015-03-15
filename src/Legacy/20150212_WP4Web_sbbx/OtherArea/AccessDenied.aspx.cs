using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OtherArea_AccessDenied : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4usrconn";
        private const string KeyDbName = "dbname";
    //private const string strRedirectOwn = "~/OtherArea/AccessDenied.aspx?EID=";



    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //Query String EID
            //Error code 1 - TrialID belongs to a different Centre
            if (!string.IsNullOrEmpty(Request.QueryString["EID"]))
            {
                if (Request.QueryString["EID"] == "1")
                {
                    lblErrorMessage.Text = "The TrialID ";
                    if (!string.IsNullOrEmpty(Request.QueryString["TID"]))
                    {
                        lblErrorMessage.Text += Request.QueryString["TID"];
                    }
                    lblErrorMessage.Text += " you are trying to access belongs to a different centre. Please Check your TrialID.";
                }

                
                if (Request.QueryString["EID"] == "51")
                {
                    lblErrorMessage.Text = "You do not have access rights to Randomising a Kidney. You cannot view the page you tried to access. ";
                    //if (!string.IsNullOrEmpty(Request.QueryString["TID"]))
                    //{
                    //    lblErrorMessage.Text += "You cannnot therefore access this page for TrialID  ";
                    //    lblErrorMessage.Text += Request.QueryString["TID"] + ".";
                    //}
                    lblErrorMessage.Text += "Please Check your access rights.";
                }

                if (Request.QueryString["EID"] == "61")
                {
                    lblErrorMessage.Text = "You do not have access rights to Add/Edit/View Main TrialID Data. ";
                    if (!string.IsNullOrEmpty(Request.QueryString["TID"]))
                    {
                        lblErrorMessage.Text += "You cannnot therefore access this page for TrialID  ";
                        lblErrorMessage.Text += Request.QueryString["TID"] + ".";
                    }
                    else
                    {
                        lblErrorMessage.Text += "You cannot view the page you tried to access. ";
                    }
                    lblErrorMessage.Text += "Please Check your access rights.";
                }

                if (Request.QueryString["EID"] == "71")
                {
                    //lblErrorMessage.Text = "Trial ID ";
                    if (!string.IsNullOrEmpty(Request.QueryString["TID"]))
                    {
                        lblErrorMessage.Text += "  ";
                        lblErrorMessage.Text += " TrialID " + Request.QueryString["TID"] + " does not exist.";
                    }
                    else
                    {
                        lblErrorMessage.Text += "TrialID you are accessing does not exist. ";
                    }
                    lblErrorMessage.Text += "Please add TrialID.";
                }

                if (Request.QueryString["EID"] == "72")
                {
                    //lblErrorMessage.Text = "Trial ID ";
                    if (!string.IsNullOrEmpty(Request.QueryString["TID"]))
                    {
                        lblErrorMessage.Text += "  ";
                        lblErrorMessage.Text += " More than one TrialID with the identifier " + Request.QueryString["TID"] + " exists in the database.";
                    }
                    else
                    {
                        lblErrorMessage.Text += "More than one TrialID you are trying to access exist in the database. ";
                    }
                    lblErrorMessage.Text += "<br/>Please add this TrialID.";
                }


                if (Request.QueryString["EID"] == "81")
                {
                    //lblErrorMessage.Text = "Trial ID ";
                    if (!string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                    {
                        lblErrorMessage.Text += "  ";
                        lblErrorMessage.Text += " TrialID (Recipient) " + Request.QueryString["TID_R"] + " does not exist.";
                    }
                    else
                    {
                        lblErrorMessage.Text += "TrialID (Recipient) you are accessing does not exist. ";
                    }
                    lblErrorMessage.Text += "Please add TrialID (Recipient).";
                }

                if (Request.QueryString["EID"] == "82")
                {
                    //lblErrorMessage.Text = "Trial ID ";
                    if (!string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                    {
                        lblErrorMessage.Text += "  ";
                        lblErrorMessage.Text += " More than one TrialID (Recipient) with the identifier " + Request.QueryString["TID_R"] + " exists in the database.";
                    }
                    else
                    {
                        lblErrorMessage.Text += "More than one TrialID (Recipient) you are trying to access exist in the database. ";
                    }
                    lblErrorMessage.Text += "<br/>Please add this TrialID.";
                }

                if (Request.QueryString["EID"] == "91")
                {
                    //lblErrorMessage.Text = "Trial ID ";
                    if (!string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                    {
                        lblErrorMessage.Text += "  ";
                        lblErrorMessage.Text += "Not all the inclusion criteria for TrialID (Recipient) " + Request.QueryString["TID_R"] + " have been entered correctly. <br/> No data can be entered for this recipient.";
                    }
                    else
                    {
                        lblErrorMessage.Text += "Not all the inclusion criteria for TrialID (Recipient) have been entered as YES.<br/> No data can be entered for this recipient.";
                    }
                    //lblErrorMessage.Text += "Please add TrialID (Recipient).";
                }
            }
            else
            {
                lblErrorMessage.Text += "Error Code has not been provided.";
            }

            ViewState["SortField"] = "UserName";
            ViewState["SortDirection"] = "ASC";
            BindData();

            //lblGV1.Text += "<br/> AddEditFollow Up " + SessionVariablesAll.AddEditFollowUp.ToString();
            //lblGV1.Text += "<br/> AddEditFollow Up " + SessionVariablesAll.AddEditFollowUp.ToString();
            //lblGV1.Text += "<br/> AddEditFollow Up " + SessionVariablesAll.AddEditFollowUp.ToString();
            //lblGV1.Text += "<br/> AddEditFollow Up " + SessionVariablesAll.AddEditFollowUp.ToString();
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured with the page.";
        }
    }

    protected void BindData()
    {

        string strSQL = String.Empty;

        try
        {
            string STRDbName = System.Configuration.ConfigurationManager.AppSettings.Get(KeyDbName);

            //string STRCentre = System.Configuration.ConfigurationManager.AppSettings.Get(KeyCentreCode);

            

            strSQL += "SELECT t1.ListUsersID, t1.UserName, t1.Active, t1.Firstname, t1.LastName,t1.Email,t1.JobTitle,t1.LockedUser, ";
            //strSQL += "t2.ID, t2.CentreCode, t2.AdminSuperUser, t2.SuperUser, t2.AddEdit, t2.Randomise,t2.PersonalData,";
            strSQL += "t2.*,";
            strSQL += "t3.LastLogin ";
            strSQL += "FROM listusers t1 INNER JOIN listdbuser t2 on t1.ListUsersID=t2.ListUsersID  AND t2.dataname='" + STRDbName + "' ";
            strSQL += "LEFT JOIN (SELECT UserID, MAX(LoggedIn) LastLogin FROM userlogs WHERE DbName='" + STRDbName + "' GROUP BY UserID) t3 ";
            strSQL += "ON t1.UserName=t3.UserID ";
            
            strSQL += "WHERE (t1.Active <> 0 OR t1.Active IS NULL) AND t1.UserName=?UserName ";

            if ((string)ViewState["SortField"] == "CentreCode")
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

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);


            GV1.DataBind();
            lblGV1.Text = "Your Current Access Rights";
        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured with the page." ;
        }
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
}
