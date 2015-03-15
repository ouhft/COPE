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

using AjaxControlToolkit;

public partial class SpecClinicalData_EditDonorSpecimen : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

        private const string strExcludeOccasion = "1-7 Days";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

        private const string strNestedCPH = "SpecimenContents";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";


    #endregion

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

                    if (Request.QueryString["SpecimenID"].ToString() == String.Empty)
                    {
                        throw new Exception("Could not obtain Unique identifier.");
                    }

                    lblDescription.Text = "Edit Specimen Data.";
                    cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click 'OK' the selected data will be deleted. Click 'CANCEL' if you do not wish to delete selected data.";

                    ddOccasion.DataSource = XMLOccasionsSpecimenDonorDataSource;
                    ddOccasion.DataBind();

                    ddSpecimenType.DataSource = XMLSpecimenTypesDataSource;
                    ddSpecimenType.DataBind();

                    ddTissueSource.DataSource = XMLTissueSourcesDataSource;
                    ddTissueSource.DataBind();

                    if (!string.IsNullOrEmpty(Request.QueryString["SpecimenID"]))
                    {
                        AssignData();
                    }

                    ViewState["SortField"] = "Barcode";
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

            strSQL += "SELECT t1.*, t2.DonorID MainDonorID,  ";
            strSQL += "DATE_FORMAT(t1.DateCollected, '%d/%m/%Y') Date_Collected,  ";
            strSQL += "DATE_FORMAT(t1.TimeCollected, '%H:%i') Time_Collected,  ";
            strSQL += "DATE_FORMAT(t2.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            strSQL += "FROM specimen t1 ";
            strSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = sqldsGV1;
            sqldsGV1.SelectCommand = strSQL;
            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Click on TrialID Edit/Delete Specimen Data. Number of Records " + GV1.Rows.Count.ToString();

            lblDescription.Text = "Add  Donor Specimen Data for " + Request.QueryString["TID"].ToString() + " and DonorID " + strDonorID;


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
                if (String.IsNullOrEmpty(Request.QueryString["SpecimenID"]) == false)
                {
                    if (drv["SpecimenID"].ToString() == Request.QueryString["SpecimenID"].ToString())
                    {
                        e.Row.BackColor = System.Drawing.Color.LightBlue;
                    }
                }
                ConfirmButtonExtender cmdDeleteRow_ConfirmButtonExtenderGV1 = e.Row.FindControl("cmdDeleteRow_ConfirmButtonExtender") as ConfirmButtonExtender;

                if (cmdDeleteRow_ConfirmButtonExtenderGV1 != null)
                {
                    cmdDeleteRow_ConfirmButtonExtenderGV1.ConfirmText = "If you click 'OK' the selected data will be deleted. Click 'CANCEL' if you do not wish to delete selected data.";
                }
            }
        }
    }
    protected void cmdDeleteRow_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            Button cmdDelRow = sender as Button;

            if (cmdDelRow == null)
            {
                throw new Exception("Could not obtain the Delete button.");
            }

            GridViewRow rowGRV;
            rowGRV = cmdDelRow.Parent.Parent as GridViewRow;

            Label lblSpecimenID_GV1 = rowGRV.FindControl("lblSpecimenID") as Label;

            if (lblSpecimenID_GV1.Text == null)
            {
                throw new Exception("Could not obtain the Label.");
            }

            if (string.IsNullOrEmpty(lblSpecimenID_GV1.Text))
            {
                throw new Exception("Could not obtain the Unique Identifier.");
            }
            //lblUserMessages.Text = lblSpecimenID_GV1.Text;

            string STRSQL = String.Empty;

            STRSQL += "DELETE FROM specimen WHERE SpecimenID=?SpecimenID AND TrialID=?TrialID  ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?SpecimenID", MySqlDbType.VarChar).Value = lblSpecimenID_GV1.Text;
            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                BindData();

                lblUserMessages.Text = "Data Deleted.";

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

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while clicking on Delete button.";
        }

    }


    protected void AssignData()
    {
        try
        {
            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  specimen t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID AND SpecimenID=?SpecimenID ";

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
            MyCMD.Parameters.Add("?SpecimenID", MySqlDbType.VarChar).Value = Request.QueryString["SpecimenID"];

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["Barcode"]))
                            {
                                txtBarcode.Text = (string)(myDr["Barcode"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Occasion"]))
                            {
                                ListItem liOccasion = ddOccasion.Items.FindByValue(myDr["Occasion"].ToString());

                                if (liOccasion != null)
                                {
                                    ddOccasion.SelectedValue = myDr["Occasion"].ToString();
                                }

                            }

                            if (!DBNull.Value.Equals(myDr["DateCollected"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateCollected"].ToString()) == true)
                                {
                                    txtDateCollected.Text = Convert.ToDateTime(myDr["DateCollected"]).ToShortDateString();
                                }                                
                            }
                            if (!DBNull.Value.Equals(myDr["TimeCollected"]))
                            {
                                if (myDr["TimeCollected"].ToString().Length >= 5)
                                {
                                    txtTimeCollected.Text = myDr["TimeCollected"].ToString().Substring(0, 5);
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["SpecimenType"]))
                            {
                                ListItem liSpecimenType = ddSpecimenType.Items.FindByValue(myDr["SpecimenType"].ToString());

                                if (liSpecimenType != null)
                                {
                                    ddSpecimenType.SelectedValue = myDr["SpecimenType"].ToString();
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["TissueSource"]))
                            {
                                ListItem liTissueSource = ddTissueSource.Items.FindByValue(myDr["TissueSource"].ToString());

                                if (liTissueSource != null)
                                {
                                    ddTissueSource.SelectedValue = myDr["TissueSource"].ToString();
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
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

                lblUserMessages.Text = ex.Message + " An error occured while executing assign query. ";
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assiging data.";
        }

        
    }

    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM specimen WHERE SpecimenID=?SpecimenID";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?SpecimenID", Request.QueryString["SpecimenID"].ToString(), STRCONN));

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Record exists for Updating.");
            }

            if (intCountFind == 0)
            {
                throw new Exception("No Record exists for Updating.");
            }

            //check if another barcode exists

            if (txtBarcode.Text != string.Empty)
            {
                STRSQL_FIND = "SELECT COUNT(*) CR FROM specimen WHERE Barcode=?Barcode AND SpecimenID<>?SpecimenID ";
                intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?Barcode", txtBarcode.Text, "?SpecimenID", Request.QueryString["SpecimenID"].ToString(), STRCONN));

                if (intCountFind < 0)
                {
                    throw new Exception("An error occured while checking if Barcode has been used for a different sample.");
                }

                if (intCountFind > 0)
                {
                    STRSQL_FIND = "SELECT Trial FROM specimen WHERE Barcode=?Barcode AND SpecimenID<>?SpecimenID ";
                    string strTrialID = GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?Barcode", txtBarcode.Text, "?SpecimenID", Request.QueryString["SpecimenID"].ToString(), STRCONN);
                    throw new Exception("Barcode " + txtBarcode.Text + " has already been assigned to " + strTrialID + ". Please Check Data you are updating");
                }
            }

            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select 'Occasion' when Tissue Was Collected");
            }

            if (txtDateCollected.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDateCollected.Text) == false)
                {
                    throw new Exception("Please Enter Date collected in the correct format.");
                }

                if (Convert.ToDateTime(txtDateCollected.Text) > DateTime.Today)
                {
                    throw new Exception("Date Collected cannot be greater than Today's date.");
                }


            }

            if (ddSpecimenType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select 'Specimen Type'");
            }

            string STRSQL = string.Empty;

            STRSQL += "UPDATE specimen SET ";
            STRSQL += "Barcode=?Barcode, SpecimenType=?SpecimenType, DateCollected=?DateCollected, TimeCollected=?TimeCollected,  ";
            STRSQL += "Occasion=?Occasion, TissueSource=?TissueSource, ";
            STRSQL += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL += "WHERE TrialID=?TrialID AND SpecimenID=?SpecimenID ";


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

            if (!String.IsNullOrEmpty(Request.QueryString["SpecimenID"]))
            {
                MyCMD.Parameters.Add("?SpecimenID", MySqlDbType.VarChar).Value = Request.QueryString["SpecimenID"].ToString();
            }

            

            if (txtBarcode.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Barcode", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Barcode", MySqlDbType.VarChar).Value = txtBarcode.Text;
            }

            if (ddSpecimenType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?SpecimenType", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SpecimenType", MySqlDbType.VarChar).Value = ddSpecimenType.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtDateCollected.Text) == false)
            {
                MyCMD.Parameters.Add("?DateCollected", MySqlDbType.Date).Value = DBNull.Value; 
            }
            else
            {
                MyCMD.Parameters.Add("?DateCollected", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateCollected.Text);
            }

            if (txtTimeCollected.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?TimeCollected", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TimeCollected", MySqlDbType.VarChar).Value = txtTimeCollected.Text;
            }

            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;
            }

            if (ddTissueSource.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?TissueSource", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TissueSource", MySqlDbType.VarChar).Value = ddTissueSource.SelectedValue;
            }

            if (string.IsNullOrEmpty(txtComments.Text))
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text; }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;


            MyCONN.Open();
            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();

                lblUserMessages.Text = "Data Update.";
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


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while updating data.";
        }
    }
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM specimen WHERE TrialID=?TrialID AND SpecimenID=?SpecimenID";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?SpecimenID", Request.QueryString["SpecimenID"].ToString(), STRCONN));

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
                if (String.IsNullOrEmpty(Request.QueryString["SpecimenID"]))
                {
                    throw new Exception("More than one Record exists for deletion. Click on DonorID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM specimen ";
            STRSQL += "WHERE TrialID=?TrialID AND SpecimenID=?SpecimenID ";
            

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

            if (!String.IsNullOrEmpty(Request.QueryString["SpecimenID"]))
            {
                MyCMD.Parameters.Add("?SpecimenID", MySqlDbType.VarChar).Value = Request.QueryString["SpecimenID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();

                lblUserMessages.Text = "Data Deleted.";
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

           
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }
}