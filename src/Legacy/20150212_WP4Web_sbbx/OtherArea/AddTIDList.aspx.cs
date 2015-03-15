using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;

using System.Data;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;

public partial class OtherArea_AddTIDList : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4usrconn";

    private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (SessionVariablesAll.AdminSuperUser != "YES")
                {
                    Response.Redirect(strAccessDenied, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }

                lblPageDescription.Text = "Add/Update List of Users to whom an Email will be sent when a TrialID is created/randomised (irresepective of centres)";
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";

                ViewState["SortField"] = "LastName";
                ViewState["SortDirection"] = "ASC";


                BindData();
            }


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }

    protected void BindData()
    {
        try
        {
            string STRSQL = "";

            STRSQL += "SELECT  t1.*  ";
            STRSQL += "FROM listusers  t1 ";

            //strSQL += "WHERE t1.DonorID=?DonorID ";
            STRSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = STRSQL;
            SqlDataSource1.SelectParameters.Clear();
            //SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();

            lblGV1.Text = "Tick the CheckBox to include in the list to whom All TrialIDs Created/Randomised are emailed.";
            //if (GV1.Rows.Count >= 1)
            //{
            //    lblGV1.Text = "Click on TrialID to Edit Follow Up Data.";
            //}


        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding the page.";
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
                Label lblTrialIDCreated = (Label)e.Row.FindControl("lblTrialIDCreated");

                if (lblTrialIDCreated != null)
                {
                    CheckBox chkIncluded = (CheckBox)e.Row.FindControl("chkIncluded");

                    if (chkIncluded != null)
                    {
                        if (lblTrialIDCreated.Text == "YES")
                        {
                            chkIncluded.Checked = true;
                        }
                        else
                        {
                            chkIncluded.Checked = false;
                        }
                    }
                }

                //DataRowView drv = (DataRowView)(e.Row.DataItem);
                //if (String.IsNullOrEmpty(Request.QueryString["RFUPostTreatmentID"]) == false)
                //{
                //    {
                //        if (drv["RFUPostTreatmentID"].ToString() == Request.QueryString["RFUPostTreatmentID"].ToString())
                //        {
                //            e.Row.BackColor = System.Drawing.Color.LightBlue;
                //        }
                //    }
                //}
            }
        }
    }

    protected void cmdReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.Url.AbsoluteUri, false);
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
        }
    }
    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            EmailListSAE strucEmailList;
            ArrayList arlEmailList = new ArrayList();

            foreach (GridViewRow row in GV1.Rows)
            {
                //get uniqueid
                string strUniqueID = string.Empty;

                string chkTrialIDCreated = string.Empty;
                string strTrialIDCreated = string.Empty;

                string strTrialIDCreatedComments = string.Empty;
                string strLabelTrialIDCreatedComments = string.Empty;

                //get uniqueid of the table
                Label lblListusersID = (Label)row.FindControl("lblListusersID");
                if (lblListusersID != null)
                {
                    strUniqueID = lblListusersID.Text;
                }
                //get checkbox and old data
                CheckBox chkIncluded = (CheckBox)row.FindControl("chkIncluded");

                if (chkIncluded != null)
                {
                    if (chkIncluded.Checked == true)
                    {
                        chkTrialIDCreated = "YES";
                    }

                }

                Label lblTrialIDCreated = (Label)row.FindControl("lblTrialIDCreated");

                if (lblTrialIDCreated != null)
                {
                    strTrialIDCreated = lblTrialIDCreated.Text;
                }

                //get comments and old data
                TextBox txtTrialIDCreatedComments = (TextBox)row.FindControl("txtTrialIDCreatedComments");
                if (txtTrialIDCreatedComments != null)
                {
                    strTrialIDCreatedComments = txtTrialIDCreatedComments.Text;
                }

                Label lblTrialIDCreatedComments = (Label)row.FindControl("lblTrialIDCreatedComments");
                if (lblTrialIDCreatedComments != null)
                {
                    strLabelTrialIDCreatedComments = lblTrialIDCreatedComments.Text;
                }


                //lblUserMessages.Text += "<br/>" + strUniqueID + ",  chkTrialIDCreated" + chkTrialIDCreated + ", strTrialIDCreated " + strTrialIDCreated ;
                //lblUserMessages.Text += ",  strTrialIDCreatedComments " + strTrialIDCreatedComments + ", strLabelTrialIDCreatedComments " + strLabelTrialIDCreatedComments + ". ";

                //if (chkTrialIDCreated != strTrialIDCreated)
                //{
                //    lblUserMessages.Text += "<br/>" + strUniqueID + " chkTrialIDCreated" + chkTrialIDCreated + ", strTrialIDCreated " + strTrialIDCreated;
                //}

                //if (strTrialIDCreatedComments != strLabelTrialIDCreatedComments)
                //{
                //    lblUserMessages.Text += "<br/>" + strUniqueID + " strTrialIDCreatedComments" + strTrialIDCreatedComments + " strLabelTrialIDCreatedComments " + strLabelTrialIDCreatedComments;

                //}


                if (chkTrialIDCreated != strTrialIDCreated || strTrialIDCreatedComments != strLabelTrialIDCreatedComments)
                {
                    strucEmailList = new EmailListSAE();
                    strucEmailList.UniqueID = strUniqueID;
                    strucEmailList.TrialIDCreated = chkTrialIDCreated;
                    strucEmailList.TrialIDCreatedComments = strTrialIDCreatedComments;

                    arlEmailList.Add(strucEmailList);
                }

            }

            if (arlEmailList.Count == 0)
            {

                throw new Exception("No Data was changed.");
            }

            //add data to the database
            string STRSQL = String.Empty;
            STRSQL += "UPDATE listusers SET ";
            STRSQL += "TrialIDCreated=?TrialIDCreated, TrialIDCreatedComments=?TrialIDCreatedComments ";
            STRSQL += "WHERE ListusersID=?ListusersID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCONN.Open();

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            try
            {
                int i = 0;
                for (i = 0; i < arlEmailList.Count; i++)
                {
                    EmailListSAE spStruct = new EmailListSAE();
                    spStruct = (EmailListSAE)arlEmailList[i];

                    if (i == 0)
                    {
                        MyCMD.Parameters.Add("TrialIDCreated", MySqlDbType.VarChar).Value = spStruct.TrialIDCreated;
                        MyCMD.Parameters.Add("TrialIDCreatedComments", MySqlDbType.VarChar).Value = spStruct.TrialIDCreatedComments;
                        MyCMD.Parameters.Add("ListusersID", MySqlDbType.VarChar).Value = spStruct.UniqueID;
                    }
                    else
                    {
                        MyCMD.Parameters["TrialIDCreated"].Value = spStruct.TrialIDCreated;
                        MyCMD.Parameters["TrialIDCreatedComments"].Value = spStruct.TrialIDCreatedComments;
                        MyCMD.Parameters["ListusersID"].Value = spStruct.UniqueID;
                    }

                    MyCMD.ExecuteNonQuery();

                }

                myTrans.Commit();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                BindData();
                lblUserMessages.Text = "Data Added/Updated.";


            }
            catch (System.Exception ex)
            {
                myTrans.Rollback();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = ex.Message + " An error occured while executing update query.";
            }

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while updating database.";
        }
    }
}