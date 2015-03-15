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

public partial class OtherArea_AddSaeList : System.Web.UI.Page
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

                lblPageDescription.Text = "Add/Update List of Users to whom all Serious Adverse Events will be Emailed (irresepective of centres)";
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

            lblGV1.Text = "Tick the CheckBox to include in the list to whom All Serious Adverse Events are emailed.";
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
                Label lblSAEAllCentre = (Label)e.Row.FindControl("lblSAEAllCentre");

                if (lblSAEAllCentre != null)
                {
                    CheckBox chkIncluded = (CheckBox)e.Row.FindControl("chkIncluded");

                    if (chkIncluded != null)
                    {
                        if (lblSAEAllCentre.Text == "YES")
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
            ArrayList arlEmailList=new ArrayList();

            foreach (GridViewRow row in GV1.Rows)
            {
                //get uniqueid
                string strUniqueID = string.Empty;

                string chkSAEAllCentre = string.Empty;
                string strSAEAllCentre = string.Empty;

                string strSAEAllCentreComments = string.Empty;
                string strLabelSAEAllCentreComments = string.Empty;

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
                        chkSAEAllCentre = "YES";
                    }

                }

                Label lblSAEAllCentre = (Label)row.FindControl("lblSAEAllCentre");

                if (lblSAEAllCentre != null)
                {
                    strSAEAllCentre = lblSAEAllCentre.Text;
                }

                //get comments and old data
                TextBox txtSAEAllCentreComments = (TextBox)row.FindControl("txtSAEAllCentreComments");
                if (txtSAEAllCentreComments != null)
                {
                    strSAEAllCentreComments = txtSAEAllCentreComments.Text;
                }

                Label lblSAEAllCentreComments = (Label)row.FindControl("lblSAEAllCentreComments");
                if (lblSAEAllCentreComments != null)
                {
                    strLabelSAEAllCentreComments = lblSAEAllCentreComments.Text;
                }


                //lblUserMessages.Text += "<br/>" + strUniqueID + ",  chkSAEAllCentre" + chkSAEAllCentre + ", strSAEAllCentre " + strSAEAllCentre ;
                //lblUserMessages.Text += ",  strSAEAllCentreComments " + strSAEAllCentreComments + ", strLabelSAEAllCentreComments " + strLabelSAEAllCentreComments + ". ";
                 
                //if (chkSAEAllCentre != strSAEAllCentre)
                //{
                //    lblUserMessages.Text += "<br/>" + strUniqueID + " chkSAEAllCentre" + chkSAEAllCentre + ", strSAEAllCentre " + strSAEAllCentre;
                //}

                //if (strSAEAllCentreComments != strLabelSAEAllCentreComments)
                //{
                //    lblUserMessages.Text += "<br/>" + strUniqueID + " strSAEAllCentreComments" + strSAEAllCentreComments + " strLabelSAEAllCentreComments " + strLabelSAEAllCentreComments;

                //}


                if (chkSAEAllCentre != strSAEAllCentre || strSAEAllCentreComments != strLabelSAEAllCentreComments)
                {
                    strucEmailList = new EmailListSAE();
                    strucEmailList.UniqueID = strUniqueID;
                    strucEmailList.SAEAllCentre = chkSAEAllCentre;
                    strucEmailList.SAEAllCentreComments = strSAEAllCentreComments;

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
            STRSQL += "SAEAllCentre=?SAEAllCentre, SAEAllCentreComments=?SAEAllCentreComments ";
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
                            MyCMD.Parameters.Add("SAEAllCentre", MySqlDbType.VarChar).Value = spStruct.SAEAllCentre;
                            MyCMD.Parameters.Add("SAEAllCentreComments", MySqlDbType.VarChar).Value = spStruct.SAEAllCentreComments;
                            MyCMD.Parameters.Add("ListusersID", MySqlDbType.VarChar).Value = spStruct.UniqueID;
                        }
                        else
                        {
                            MyCMD.Parameters["SAEAllCentre"].Value = spStruct.SAEAllCentre;
                            MyCMD.Parameters["SAEAllCentreComments"].Value = spStruct.SAEAllCentreComments;
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