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

public partial class SpecClinicalData_AddDonorLabProcurementData : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        //static Random _random = new Random();

        

    #endregion

    //at load complete
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (Request.QueryString["TID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID.");
                }

                lblDescription.Text = "Add Lab Results/Procurement Data for " + Request.QueryString["TID"].ToString();

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                ddUrineGlucose.DataSource = XMLUrineOptionsDataSource;
                ddUrineGlucose.DataBind();

                ddUrineProtein.DataSource = XMLUrineOptionsDataSource;
                ddUrineProtein.DataBind();

                ddInVivoFlushSolution.DataSource = XmlFlushSolutionsDataSource;
                ddInVivoFlushSolution.DataBind();


                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;

                ViewState["SortField"] = "TrialID";
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
            if (Request.QueryString["TID"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }

            // get the DonorID
            string strDonorID = string.Empty;

            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

            if (mpCPH != null)
            {
                Label lblMainLabel = (Label)(mpCPH.FindControl(strMainLabel));

                if (lblMainLabel != null)
                {
                    strDonorID = lblMainLabel.Text.Replace("(", "");
                    strDonorID = strDonorID.Replace(")", "");
                }
            }

            string strSQL = String.Empty;

            strSQL += "SELECT t1.*,  ";
            strSQL += "DATE_FORMAT(t1.DateOperation, '%d/%m/%Y') Date_Operation, ";
            strSQL += "TIME_FORMAT(t1.CrossClampingTime, '%H:%i') CrossClamping_Time ";
            strSQL += "FROM donor_labresults t1 ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on TrialID to Edit a Donor Lab Results/Procurement Details.";

            if (GV1.Rows.Count == 1)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                cmdAddData.Text = "Update Data";
                lblDescription.Text = "Update  Donor Lab Results/Procurement Data for " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;

                AssignData();
                
            }
            else if (GV1.Rows.Count == 0)
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblDescription.Text = "Add  Donor Lab Results/Procurement Data for " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;
            }
            else
            {
                throw new Exception("More than one Records exist.");
            }
           

            
            

        }
        catch (System.Exception excep)
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

    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            string STRSQL = "";

            STRSQL += "SELECT t1.*, t2.DonorID Donor FROM  donor_labresults t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID ";
            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {



                            if (!DBNull.Value.Equals(myDr["CreatinineAtAdmission"]))
                            {
                                txtCreatinineAtAdmission.Text = (string)(myDr["CreatinineAtAdmission"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CreatinineLast"]))
                            {
                                txtCreatinineLast.Text = (string)(myDr["CreatinineLast"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CreatinineMax"]))
                            {
                                txtCreatinineMax.Text = (string)(myDr["CreatinineMax"]);
                            }

                            if (!DBNull.Value.Equals(myDr["UrineGlucose"]))
                            {
                                ddUrineGlucose.SelectedValue = (string)(myDr["UrineGlucose"]);
                            }

                            if (!DBNull.Value.Equals(myDr["UrineProtein"]))
                            {
                                ddUrineProtein.SelectedValue = (string)(myDr["UrineProtein"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
                            }

                            if (!DBNull.Value.Equals(myDr["DateOperation"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateOperation"].ToString()))
                                {
                                    txtDateOperation.Text = Convert.ToDateTime(myDr["DateOperation"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CrossClampingTime"]))
                            {
                                if (myDr["CrossClampingTime"].ToString().Length >= 5)
                                {
                                    txtCrossClampingTime.Text = myDr["CrossClampingTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["InVivoFlushSolution"]))
                            {
                                ddInVivoFlushSolution.SelectedValue = (string)(myDr["InVivoFlushSolution"]);
                            }

                            if (!DBNull.Value.Equals(myDr["InVivoFlushSolutionOther"]))
                            {
                                txtInVivoFlushSolutionOther.Text = (string)(myDr["InVivoFlushSolutionOther"]);
                            }



                            if (!DBNull.Value.Equals(myDr["WashoutVolume"]))
                            {
                                txtWashoutVolume.Text = (string)(myDr["WashoutVolume"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CommentsProcurement"]))
                            {
                                txtCommentsProcurement.Text = (string)(myDr["CommentsProcurement"]);
                            }




                        }
                    }
                }
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

            }
            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing Assign query.";
            }

        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while resetting the page.";
        }
    }

    // reset page
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

    //add data
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (txtCreatinineAtAdmission.Text == string.Empty)
            {
                throw new Exception("Please enter Creatinine at Admission");
            }

            if (txtCreatinineLast.Text == string.Empty)
            {
                throw new Exception("Please enter the Last Creatinine");
            }

            if (txtCreatinineMax.Text == string.Empty)
            {
                throw new Exception("Please enter the Max Creatinine");
            }

            if (ddUrineGlucose.SelectedValue == "0")
            {
                throw new Exception("Please Select a value for Urine Glucose.");
            }

            if (ddUrineProtein.SelectedValue == "0")
            {
                throw new Exception("Please Select a value for Urine Protein.");
            }

            if (GeneralRoutines.IsDate(txtDateOperation.Text) == false)
            {
                throw new Exception("Please enter Date of Operation.");
            }
            else
            {
                if (Convert.ToDateTime(txtDateOperation.Text) > DateTime.Now)
                {
                    throw new Exception("Date of Operation cannot be greater than Today's date.");
                }
            }

            if (txtCrossClampingTime.Text == string.Empty)
            {
                throw new Exception("Please enter 'CrossClampingTime' Time. ");

                
            }
            else
            {
                if (GeneralRoutines.IsNumeric(txtCrossClampingTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'Cross Clamping Time' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtCrossClampingTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'CrossClampingTime' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtCrossClampingTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'CrossClampingTime' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtCrossClampingTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'CrossClampingTime' Time Hour Minute not be greater than 59.");
                } 
            }

            if (ddInVivoFlushSolution.SelectedValue == "0")
            {
                throw new Exception("Please Select a value for 'In Vivo Flush Solution'.");
            }
            else 
            {
                if (ddInVivoFlushSolution.SelectedValue == STR_OTHER_SELECTION)
                {
                    if (txtInVivoFlushSolutionOther.Text == string.Empty)
                    {
                        throw new Exception("Please enter details of In Vivo Flush Solution Other as you have selected 'Other'");
                    }
                }
            }

            if (GeneralRoutines.IsNumeric(txtWashoutVolume.Text) == false)
            {
                throw new Exception("Please Enter Washout Volume in numeric format.");
            }
            
            string STRSQL = "";

            STRSQL += "INSERT INTO donor_labresults ";
            STRSQL += "(TrialID, CreatinineAtAdmission, CreatinineLast, CreatinineMax, UrineGlucose, UrineProtein, Comments, DateOperation, CrossClampingTime,  ";
            STRSQL += "InVivoFlushSolution, InVivoFlushSolutionOther, WashoutVolume, CommentsProcurement, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?CreatinineAtAdmission, ?CreatinineLast, ?CreatinineMax, ?UrineGlucose, ?UrineProtein, ?Comments, ?DateOperation, ?CrossClampingTime,  ";
            STRSQL += "?InVivoFlushSolution, ?InVivoFlushSolutionOther, ?WashoutVolume, ?CommentsProcurement, ?DateCreated, ?CreatedBy) ";


            string STRSQL_UPDATE = "";
            STRSQL_UPDATE += "UPDATE donor_labresults SET ";
            STRSQL_UPDATE += "CreatinineAtAdmission=?CreatinineAtAdmission, CreatinineLast=?CreatinineLast, CreatinineMax=?CreatinineMax, UrineGlucose=?UrineGlucose, UrineProtein=?UrineProtein, ";
            STRSQL_UPDATE += "Comments=?Comments, DateOperation=?DateOperation, CrossClampingTime=?CrossClampingTime,";
            STRSQL_UPDATE += "InVivoFlushSolution=?InVivoFlushSolution, InVivoFlushSolutionOther=?InVivoFlushSolutionOther, WashoutVolume=?WashoutVolume, CommentsProcurement=?CommentsProcurement, ";
            STRSQL_UPDATE += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_labresults WHERE TrialID=?TrialID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

            if (intCountFind == 1)
            { 
                MyCMD.CommandText = STRSQL_UPDATE; 
            }
            else if (intCountFind == 0)
            { 
                MyCMD.CommandText = STRSQL; 
            }
            else if (intCountFind > 1)
            {
                throw new Exception("More than One Donor Lab Results/Procurement Data exists for this TrialID. Click on TrialID in the table below to select data to delete. ");
            }
            else
            { 
                throw new Exception("An error occured while check if Donor Lab Results/ Procurement Data already exist in the database.");
            }


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            MyCMD.Parameters.Add("?CreatinineAtAdmission", MySqlDbType.VarChar).Value = txtCreatinineAtAdmission.Text;

            MyCMD.Parameters.Add("?CreatinineLast", MySqlDbType.VarChar).Value = txtCreatinineLast.Text;

            MyCMD.Parameters.Add("?CreatinineMax", MySqlDbType.VarChar).Value = txtCreatinineMax.Text;

            if (ddUrineGlucose.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?UrineGlucose", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else 
            {
                MyCMD.Parameters.Add("?UrineGlucose", MySqlDbType.VarChar).Value = ddUrineGlucose.SelectedValue;
            }

            if (ddUrineProtein.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?UrineProtein", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UrineProtein", MySqlDbType.VarChar).Value = ddUrineProtein.SelectedValue;
            }

            if (txtComments.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }


            if (GeneralRoutines.IsDate(txtDateOperation.Text))
            { MyCMD.Parameters.Add("?DateOperation", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateOperation.Text); }
            else
            { MyCMD.Parameters.Add("?DateOperation", MySqlDbType.Date).Value = DBNull.Value; }

            if (txtCrossClampingTime.Text == string.Empty)
            { MyCMD.Parameters.Add("?CrossClampingTime", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?CrossClampingTime", MySqlDbType.VarChar).Value = txtCrossClampingTime.Text; }

            if (ddInVivoFlushSolution.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?InVivoFlushSolution", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?InVivoFlushSolution", MySqlDbType.VarChar).Value = ddInVivoFlushSolution.SelectedValue;
            }

            if (txtInVivoFlushSolutionOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?InVivoFlushSolutionOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?InVivoFlushSolutionOther", MySqlDbType.VarChar).Value = txtInVivoFlushSolutionOther.Text;
            }

            if (txtWashoutVolume.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?WashoutVolume", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?WashoutVolume", MySqlDbType.VarChar).Value = txtWashoutVolume.Text;
            }


            if (txtCommentsProcurement.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?CommentsProcurement", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CommentsProcurement", MySqlDbType.VarChar).Value = txtCommentsProcurement.Text;
            }


            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }

            catch (System.Exception ex)
            {

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                lblUserMessages.Text = ex.Message + " An error occured while executing add/update query.";
            }

            BindData();

            

        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while adding data.";
        }
    }

    // delete data
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_labresults WHERE TrialID=?TrialID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Record exists for deletion.");
            }

            if (intCountFind == 0)
            {
                throw new Exception("No Record exists for deletion.");
            }

            if (intCountFind > 1)
            {
                if (String.IsNullOrEmpty(Request.QueryString["DonorLabResultsID"]))
                {
                    throw new Exception("More than one Record exists for deletion. Click on TrialID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM donor_labresults ";
            STRSQL += "WHERE TrialID=?TrialID ";
            if (intCountFind > 1)
            {
                STRSQL += "AND DonorLabResultsID=?DonorLabResultsID ";
            }

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            if (!String.IsNullOrEmpty(Request.QueryString["DonorLabResultsID"]))
            {
                MyCMD.Parameters.Add("?DonorLabResultsID", MySqlDbType.VarChar).Value = Request.QueryString["DonorLabResultsID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

            }

            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing delete query.";
            }


            finally //close connection
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }

            BindData();

            lblUserMessages.Text = "Data Deleted.";

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }
}