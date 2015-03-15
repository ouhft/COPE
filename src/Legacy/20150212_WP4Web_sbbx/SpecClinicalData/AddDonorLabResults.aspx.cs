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

public partial class SpecClinicalData_AddDonorLabResults : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

    //Hb
        private const double dblMinHb_mgdl=12;
        private const double dblMaxHb_mgdl = 18;

        private const double dblMinHb_mgdlRec=12;
        private const double dblMaxHb_mgdlRec = 18;

        private const double dblMinHb_mmol = 1.86;
        private const double dblMaxHb_mmol = 2.71;

        private const double dblMinHb_mmolRec = 1.86;
        private const double dblMaxHb_mmolRec = 2.71;

    //Ht
        private const double dblMinHt = 37;
        private const double dblMaxHt = 54;

        private const double dblMinHtRec = 37;
        private const double dblMaxHtRec = 54;

        private const double dblMinpH = 7.35;
        private const double dblMaxpH = 7.45;

        private const double dblMinpHRec = 7.35;
        private const double dblMaxpHRec = 7.45;

        private const double dblMinpCO2_mmhg = 32;
        private const double dblMaxpCO2_mmhg = 48;

        private const double dblMinpCO2_mmhgRec = 32;
        private const double dblMaxpCO2_mmhgRec = 48;

        private const double dblMinpCO2_kPa = 4.4;
        private const double dblMaxpCO2_kPa = 5.9;
        private const double dblMinpCO2_kPaRec = 4.4;
        private const double dblMaxpCO2_kPaRec = 5.9;

        private const double dblMinpO2_mmhg = 70;
        private const double dblMaxpO2_mmhg = 108;

        private const double dblMinpO2_mmhgRec = 70;
        private const double dblMaxpO2_mmhgRec = 108;

        private const double dblMinpO2_kPa = 10;
        private const double dblMaxpO2_kPa = 14;

        private const double dblMinpO2_kPaRec = 10;
        private const double dblMaxpO2_kPaRec = 14;

        private const double dblMinUrea_gdl = 0;
        private const double dblMaxUrea_gdl = 50;
        private const double dblMinUrea_gdlRec = 0;
        private const double dblMaxUrea_gdlRec = 50;
        

        private const double dblMinUrea_mmol = 0;
        private const double dblMaxUrea_mmol = 8.3;
        private const double dblMinUrea_mmolRec = 0;
        private const double dblMaxUrea_mmolRec = 8.3;

        private const double dblMinCreatinine_mgdl = 0.51;
        private const double dblMaxCreatinine_mgdl = 1.17;

        private const double dblMinCreatinine_mgdlRec = 0.51;
        private const double dblMaxCreatinine_mgdlRec = 1.17;

        private const double dblMinCreatinine_micromol = 53;
        private const double dblMaxCreatinine_micromol = 106;

        private const double dblMinCreatinine_micromolRec = 53;
        private const double dblMaxCreatinine_micromolRec = 106;


        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";
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

                lblDescription.Text = "Add Donor Lab Results Data for " + Request.QueryString["TID"].ToString();
                rblHbUnits.DataSource = XMLHbDataSource;
                rblHbUnits.DataBind();
                rblHbUnits.Items[0].Selected = true;

                rblpCO2Units.DataSource = XMLpCO2UnitsDataSource;
                rblpCO2Units.DataBind();
                rblpCO2Units.Items[0].Selected = true;

                rblpO2Units.DataSource = XMLpO2UnitsDataSource;
                rblpO2Units.DataBind();
                rblpO2Units.Items[0].Selected = true;

                rblUreaUnits.DataSource = XMLUreaUnitsDataSource;
                rblUreaUnits.DataBind();
                rblUreaUnits.Items[0].Selected = true;

                rblCreatinineUnits.DataSource = XMLCreatinineUnitsDataSource;
                rblCreatinineUnits.DataBind();
                rblCreatinineUnits.Items[0].Selected = true;

                rblMeanCreatinineUnits.DataSource = XMLCreatinineUnitsDataSource;
                rblMeanCreatinineUnits.DataBind();
                rblMeanCreatinineUnits.Items[0].Selected = true;

                rblMaxCreatinineUnits.DataSource = XMLCreatinineUnitsDataSource;
                rblMaxCreatinineUnits.DataBind();
                rblMaxCreatinineUnits.Items[0].Selected = true;


                ValidatorInitialise();

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                    
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();


                //lock data
                pnlReasonModified.Visible = false;
                txtReasonModified.ToolTip = "Enter Reasons if you are Modifying any Data on this page";
                lblReasonModifiedOldDetails.ToolTip = "Displays Reasons that have been entered for modifying data in the past.";
                if (chkDataLocked.Checked == true || chkDataFinal.Checked == true)
                {

                    if (chkDataLocked.Checked == true && chkDataFinal.Checked == true)
                    {
                        string strDataLockedFinalMessage = ConfigurationManager.AppSettings["LockedFinalMessage"];
                        lblDescription.Text += "<br/>" + strDataLockedFinalMessage;
                    }
                    else
                    {
                        if (chkDataFinal.Checked == true)
                        {
                            string strDataFinalMessage = ConfigurationManager.AppSettings["FinalMessage"];
                            lblDescription.Text += "<br/>" + strDataFinalMessage;
                        }
                        else
                        {
                            string strDataLocked = ConfigurationManager.AppSettings["LockedMessage"];
                            lblDescription.Text += "<br/>" + strDataLocked;
                        }
                    }


                    if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                    {
                        if (chkDataLocked.Checked == true)
                        {
                            pnlReasonModified.Visible = true;
                        }
                        else
                        {
                            pnlReasonModified.Visible = false;
                        }
                        pnlFinal.Visible = true;
                        cmdAddData.Enabled = true;
                        cmdDelete.Enabled = true;
                        cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        cmdAddData.Enabled = false;
                        cmdDelete.Enabled = false;
                        cmdReset.Enabled = false;

                    }
                }

                //data final checkbox visible
                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    pnlFinal.Visible = true;
                    //cmdUpdate.Enabled = true;
                    ////cmdDelete.Enabled = true;
                    //cmdReset.Enabled = true;
                }
                else
                {
                    pnlFinal.Visible = false;
                    //cmdUpdate.Enabled = false;
                    ////cmdDelete.Enabled = false;
                    //cmdReset.Enabled = false;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["SCode"]))
                {
                    if (Request.QueryString["SCode"].ToString() == "1")
                    {
                        string strSCode1Message = string.Empty;
                        strSCode1Message = ConfigurationManager.AppSettings["SCode1"];
                        lblUserMessages.Text = strSCode1Message;
                    }
                }

                hlkSummaryPage.NavigateUrl = strRedirectSummary + Request.QueryString["TID"];

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessageNoAsterisk"];
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

                //locked button
                if (chkDataLocked.Checked == false)
                {
                    pnlAllDataAdded.Visible = true;
                }
                else
                {
                    pnlAllDataAdded.Visible = false;
                }

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
            strSQL += "DATE_FORMAT(t1.DateCreated, '%d/%m/%Y') Date_Created ";
            strSQL += "FROM donor_labresults t1 ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            

            if (GV1.Rows.Count == 1)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                //cmdAddData.Text = "Update Data";
                lblDescription.Text = "Update  Donor Lab Results for " + Request.QueryString["TID"].ToString() + "";

                AssignData();

                lblGV1.Text = "Summary of Donor Lab Results";

            }
            else if (GV1.Rows.Count == 0)
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblDescription.Text = "Add  Donor Lab Results for " + Request.QueryString["TID"].ToString() + "";
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
                        string strIncompleteColour = ConfigurationManager.AppSettings["IncompleteColour"];

                        while (myDr.Read())
                        {

                            if (!DBNull.Value.Equals(myDr["Hb"]))
                            {
                                txtHb.Text = (string)(myDr["Hb"]);
                            }

                            if (lblHb.Font.Bold == true)
                            {
                                if (txtHb.Text==string.Empty)
                                {
                                    lblHb.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["HbUnit"]))
                            {
                                rblHbUnits.SelectedValue = (string)(myDr["HbUnit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Ht"]))
                            {
                                txtHt.Text = (string)(myDr["Ht"]);
                            }

                            if (lblHt.Font.Bold == true)
                            {
                                if (txtHt.Text == string.Empty)
                                {
                                    lblHt.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["pH"]))
                            {
                                txtpH.Text = (string)(myDr["pH"]);
                            }

                            if (lblpH.Font.Bold == true)
                            {
                                if (txtpH.Text == string.Empty)
                                {
                                    lblpH.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["pCO2"]))
                            {
                                txtpCO2.Text = (string)(myDr["pCO2"]);
                            }


                            if (lblpCO2.Font.Bold == true)
                            {
                                if (txtpCO2.Text == string.Empty)
                                {
                                    lblpCO2.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["pCO2Unit"]))
                            {
                                rblpCO2Units.SelectedValue = (string)(myDr["pCO2Unit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["pO2"]))
                            {
                                txtpO2.Text = (string)(myDr["pO2"]);
                            }

                            if (lblpO2.Font.Bold == true)
                            {
                                if (txtpO2.Text == string.Empty)
                                {
                                    lblpO2.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["pO2Unit"]))
                            {
                                rblpO2Units.SelectedValue = (string)(myDr["pO2Unit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Urea"]))
                            {
                                txtUrea.Text = (string)(myDr["Urea"]);
                            }

                            if (lblUrea.Font.Bold == true)
                            {
                                if (txtUrea.Text == string.Empty)
                                {
                                    lblUrea.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["UreaUnit"]))
                            {
                                rblUreaUnits.SelectedValue = (string)(myDr["UreaUnit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Creatinine"]))
                            {
                                txtCreatinine.Text = (string)(myDr["Creatinine"]);
                            }


                            if (lblCreatinine.Font.Bold == true)
                            {
                                if (txtCreatinine.Text == string.Empty)
                                {
                                    lblCreatinine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CreatinineUnit"]))
                            {
                                rblCreatinineUnits.SelectedValue = (string)(myDr["CreatinineUnit"]);
                            }


                            if (!DBNull.Value.Equals(myDr["MeanCreatinine"]))
                            {
                                txtMeanCreatinine.Text = (string)(myDr["MeanCreatinine"]);
                            }


                            if (lblMeanCreatinine.Font.Bold == true)
                            {
                                if (txtMeanCreatinine.Text == string.Empty)
                                {
                                    lblMeanCreatinine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["MeanCreatinineUnit"]))
                            {
                                rblMeanCreatinineUnits.SelectedValue = (string)(myDr["MeanCreatinineUnit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["MaxCreatinine"]))
                            {
                                txtMaxCreatinine.Text = (string)(myDr["MaxCreatinine"]);
                            }

                            if (lblMaxCreatinine.Font.Bold == true)
                            {
                                if (txtMaxCreatinine.Text == string.Empty)
                                {
                                    lblMaxCreatinine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["MaxCreatinineUnit"]))
                            {
                                rblMaxCreatinineUnits.SelectedValue = (string)(myDr["MaxCreatinineUnit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
                            }


                            //lock data if DataLocked=1
                            if (!DBNull.Value.Equals(myDr["DataLocked"]))
                            {
                                if (myDr["DataLocked"].ToString() == "1")
                                {
                                    chkDataLocked.Checked = true;
                                }
                                else
                                {
                                    chkDataLocked.Checked = false;
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["ReasonModified"]))
                            {
                                lblReasonModifiedOldDetails.Text = myDr["ReasonModified"].ToString().Replace("\r\n", "<br />");
                            }

                            //Mark Data Final Assign 
                            if (!DBNull.Value.Equals(myDr["DataFinal"]))
                            {
                                if (myDr["DataFinal"].ToString() == "1")
                                {
                                    chkDataFinal.Checked = true;
                                }
                                else
                                {
                                    chkDataFinal.Checked = false;
                                }
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

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
        }
    }
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //if (txtHb.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtHb.Text) == false)
            //    {
            //        throw new Exception("Please enter Hb in numeric format");
            //    }                    
            //}
            //else
            //{
            //    throw new Exception("Please enter Hb.");
            //}


            //if (GeneralRoutines.IsNumeric(txtHb.Text))
            //{
            //    if (rblHbUnits.SelectedValue == "mmol/l")
            //    {
            //        if (Convert.ToDouble(txtHb.Text) < 1.86)
            //        {
            //            throw new Exception(lblHb.Text + " cannot be less than " + "'1.86'.");
            //        }

            //        if (Convert.ToDouble(txtHb.Text) > 2.71)
            //        {
            //            throw new Exception(lblHb.Text + " cannot be greater than " + "'2.71'.");
            //        }

            //    }
            //    else
            //    {
            //        if (Convert.ToDouble(txtHb.Text) < 12)
            //        {
            //            throw new Exception(lblHb.Text + " cannot be less than " + "12.");
            //        }

            //        if (Convert.ToDouble(txtHb.Text) > 18)
            //        {
            //            throw new Exception(lblHb.Text + " cannot be greater than " + "18.");
            //        }
            //    }
            //}
            
            //if (txtHt.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtHt.Text) == false)
            //    {
            //        throw new Exception("Please enter Ht in numeric format");
            //    }
            //}
            //else
            //{
            //    throw new Exception("Please enter Ht.");
            //}

            //if (txtpH.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtpH.Text) == false)
            //    {
            //        throw new Exception("Please enter pH in numeric format");
            //    }
            //}
            //else
            //{
            //    throw new Exception("Please enter pH.");
            //}

            //if (txtpCO2.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtpCO2.Text) == false)
            //    {
            //        throw new Exception("Please enter pCO2 in numeric format");
            //    }
            //}
            //else
            //{
            //    throw new Exception("Please enter pCO2.");
            //}


            //if (txtpO2.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtpO2.Text) == false)
            //    {
            //        throw new Exception("Please enter pO2 in numeric format");
            //    }
            //}
            //else
            //{
            //    throw new Exception("Please enter pO2.");
            //}

            //if (GeneralRoutines.IsNumeric(txtpCO2.Text))
            //{
            //    if (rblpCO2Units.SelectedValue == "kPa")
            //    {
            //        if (Convert.ToDouble(txtpCO2.Text) < 4.4)
            //        {
            //            throw new Exception(lblpCO2.Text + " cannot be less than " + "'4.4'.");
            //        }

            //        if (Convert.ToDouble(txtpCO2.Text) > 5.9)
            //        {
            //            throw new Exception(lblpCO2.Text + " cannot be greater than " + "'5.9'.");
            //        }

            //    }
            //    else
            //    {
            //        if (Convert.ToDouble(txtpCO2.Text) < 32)
            //        {
            //            throw new Exception(lblpCO2.Text + " cannot be less than " + "'32'.");
            //        }

            //        if (Convert.ToDouble(txtpCO2.Text) > 48)
            //        {
            //            throw new Exception(lblpCO2.Text + " cannot be greater than " + "'48'.");
            //        }
            //    }
            //}


            //if (GeneralRoutines.IsNumeric(txtpO2.Text))
            //{
            //    if (rblpO2Units.SelectedValue == "kPa")
            //    {
            //        if (Convert.ToDouble(txtpO2.Text) < 10)
            //        {
            //            throw new Exception(lblpO2.Text + " cannot be less than " + "'10'.");
            //        }

            //        if (Convert.ToDouble(txtpO2.Text) > 14)
            //        {
            //            throw new Exception(lblpO2.Text + " cannot be greater than " + "'14'.");
            //        }

            //    }
            //    else
            //    {
            //        if (Convert.ToDouble(txtpO2.Text) < 70)
            //        {
            //            throw new Exception(lblpO2.Text + " cannot be less than " + "'70'.");
            //        }

            //        if (Convert.ToDouble(txtpO2.Text) > 108)
            //        {
            //            throw new Exception(lblpO2.Text + " cannot be greater than " + "'108'.");
            //        }
            //    }
            //}

            //if (txtUrea.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtUrea.Text) == false)
            //    {
            //        throw new Exception("Please enter Urea in numeric format");
            //    }
            //}
            //else
            //{
            //    throw new Exception("Please enter Urea.");
            //}

            //if (GeneralRoutines.IsNumeric(txtUrea.Text))
            //{
            //    if (rblUreaUnits.SelectedValue == "mmol/l")
            //    {
            //        if (Convert.ToDouble(txtUrea.Text) < 0)
            //        {
            //            throw new Exception(lblUrea.Text + " cannot be less than " + "'0'.");
            //        }

            //        if (Convert.ToDouble(txtUrea.Text) > 8.3)
            //        {
            //            throw new Exception(lblUrea.Text + " cannot be greater than " + "'8.3'.");
            //        }

            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(txtUrea.Text) < 0)
            //        {
            //            throw new Exception(lblUrea.Text + " cannot be less than " + "'0'.");
            //        }

            //        if (Convert.ToInt32(txtUrea.Text) > 50)
            //        {
            //            throw new Exception(lblUrea.Text + " cannot be greater than " + "'50'.");
            //        }
            //    }
            //}


            //if (GeneralRoutines.IsNumeric(txtMeanCreatinine.Text))
            //{
            //    if (rblMeanCreatinineUnits.SelectedValue == "mg/dL")
            //    {
            //        if (Convert.ToDouble(txtMeanCreatinine.Text) < 0.51)
            //        {
            //            throw new Exception(lblMeanCreatinine.Text + " cannot be less than " + "'0.51'.");
            //        }

            //        if (Convert.ToDouble(txtMeanCreatinine.Text) > 1.17)
            //        {
            //            throw new Exception(lblMeanCreatinine.Text + " cannot be greater than " + "'1.17'.");
            //        }

            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(txtMeanCreatinine.Text) < 53)
            //        {
            //            throw new Exception(lblMeanCreatinine.Text + " cannot be less than " + "'53'.");
            //        }

            //        if (Convert.ToInt32(txtMeanCreatinine.Text) > 106)
            //        {
            //            throw new Exception(lblMeanCreatinine.Text + " cannot be greater than " + "'106'.");
            //        }
            //    }
            //}


            //if (GeneralRoutines.IsNumeric(txtMaxCreatinine.Text))
            //{
            //    if (rblMaxCreatinineUnits.SelectedValue == "mg/dL")
            //    {
            //        if (Convert.ToDouble(txtMaxCreatinine.Text) < 0.51)
            //        {
            //            throw new Exception(lblMaxCreatinine.Text + " cannot be less than " + "'0.51'.");
            //        }

            //        if (Convert.ToDouble(txtMaxCreatinine.Text) > 1.17)
            //        {
            //            throw new Exception(lblMaxCreatinine.Text + " cannot be greater than " + "'1.17'.");
            //        }

            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(txtMaxCreatinine.Text) < 53)
            //        {
            //            throw new Exception(lblMaxCreatinine.Text + " cannot be less than " + "'53'.");
            //        }

            //        if (Convert.ToInt32(txtMaxCreatinine.Text) > 106)
            //        {
            //            throw new Exception(lblMaxCreatinine.Text + " cannot be greater than " + "'106'.");
            //        }
            //    }
            //}

            if (GeneralRoutines.IsNumeric(txtCreatinine.Text) && GeneralRoutines.IsNumeric(txtMaxCreatinine.Text))
            {
                if (Convert.ToDouble(txtCreatinine.Text) > Convert.ToDouble(txtMaxCreatinine.Text))
                {
                    throw new Exception(lblCreatinine.Text + " cannot be greater than " + lblMaxCreatinine.Text + ". Please check the data you have entered.");
                }
            }


            if (GeneralRoutines.IsNumeric(txtMeanCreatinine.Text) && GeneralRoutines.IsNumeric(txtMaxCreatinine.Text))
            {
                if (Convert.ToDouble(txtMeanCreatinine.Text) > Convert.ToDouble(txtMaxCreatinine.Text))
                {
                    throw new Exception(lblMeanCreatinine.Text + " cannot be greater than " + lblMaxCreatinine.Text + ". Please check the data you have entered.");
                }
            }
            //if (txtMeanCreatinine.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtMeanCreatinine.Text) == false)
            //    {
            //        throw new Exception("Please enter Mean Creatinine in numeric format");
            //    }
            //}
            //else
            //{
            //    throw new Exception("Please enter Mean Creatinine.");
            //}


            //if (txtMaxCreatinine.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtMaxCreatinine.Text) == false)
            //    {
            //        throw new Exception("Please enter Max Creatinine in numeric format");
            //    }
            //}
            //else
            //{
            //    throw new Exception("Please enter Max Creatinine.");
            //}

            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }


            string STRSQL = "";

            STRSQL += "INSERT INTO donor_labresults ";
            //STRSQL += "(TrialID, Hb, HbUnit, Ht, pH, pCO2, pO2, Urea, UreaUnit, MeanCreatinine, MeanCreatinineUnit, MaxCreatinine, MaxCreatinineUnit, ";
            STRSQL += "(TrialID, Hb, HbUnit, Ht, pH, pCO2, pCO2Unit, pO2, pO2Unit,";
            STRSQL += "Urea, UreaUnit, Creatinine, CreatinineUnit, MeanCreatinine, MeanCreatinineUnit, MaxCreatinine, MaxCreatinineUnit, ";
            STRSQL += " Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            //STRSQL += "(?TrialID, ?Hb, ?HbUnit, ?Ht, ?pH, ?pCO2, ?pO2, ?Urea, ?UreaUnit, ?MeanCreatinine, ?MeanCreatinineUnit, ?MaxCreatinine,  ?MaxCreatinineUnit, ";
            STRSQL += "(?TrialID, ?Hb, ?HbUnit, ?Ht,  ?pH, ?pCO2, ?pCO2Unit, ?pO2, ?pO2Unit,";
            STRSQL += "?Urea, ?UreaUnit, ?Creatinine, ?CreatinineUnit, ?MeanCreatinine, ?MeanCreatinineUnit, ?MaxCreatinine,  ?MaxCreatinineUnit,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";


            string STRSQL_UPDATE = "";
            STRSQL_UPDATE += "UPDATE donor_labresults SET ";
            STRSQL_UPDATE += "Hb=?Hb, HbUnit=?HbUnit, Ht=?Ht, pH=?pH, pCO2=?pCO2, pCO2Unit=?pCO2Unit, pO2=?pO2,  pO2Unit=?pO2Unit, ";
            STRSQL_UPDATE += "Urea=?Urea, UreaUnit=?UreaUnit, Creatinine=?Creatinine, CreatinineUnit=?CreatinineUnit, ";
            STRSQL_UPDATE += "MeanCreatinine=?MeanCreatinine, MeanCreatinineUnit=?MeanCreatinineUnit, MaxCreatinine=?MaxCreatinine, MaxCreatinineUnit=?MaxCreatinineUnit, ";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


            //lock data
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE donor_labresults SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE donor_labresults SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";

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
                throw new Exception("More than One Donor Lab Results exists for this TrialID. Click on TrialID in the table below to select data to delete. ");
            }
            else
            {
                throw new Exception("An error occured while check if Donor Lab Results Data already exist in the database.");
            }


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            if (txtHb.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Hb", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Hb", MySqlDbType.VarChar).Value = txtHb.Text;
            }

            if (rblHbUnits.SelectedIndex == -1 )
            {
                MyCMD.Parameters.Add("?HbUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HbUnit", MySqlDbType.VarChar).Value = rblHbUnits.SelectedValue;
            }

            if (txtHt.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Ht", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Ht", MySqlDbType.VarChar).Value = txtHt.Text;
            }

            if (txtpH.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?pH", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?pH", MySqlDbType.VarChar).Value = txtpH.Text;
            }

            if (txtpCO2.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?pCO2", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?pCO2", MySqlDbType.VarChar).Value = txtpCO2.Text;
            }


            if (rblpCO2Units.SelectedIndex == -1 || rblpCO2Units.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?pCO2Unit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?pCO2Unit", MySqlDbType.VarChar).Value = rblpCO2Units.SelectedValue;
            }

            if (txtpO2.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?pO2", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?pO2", MySqlDbType.VarChar).Value = txtpO2.Text;
            }

            if (rblpO2Units.SelectedIndex == -1 )
            {
                MyCMD.Parameters.Add("?pO2Unit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?pO2Unit", MySqlDbType.VarChar).Value = rblpO2Units.SelectedValue;
            }

            if (txtUrea.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Urea", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Urea", MySqlDbType.VarChar).Value = txtUrea.Text;
            }

            if (rblUreaUnits.SelectedIndex == -1 )
            {
                MyCMD.Parameters.Add("?UreaUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UreaUnit", MySqlDbType.VarChar).Value = rblUreaUnits.SelectedValue;
            }

            if (txtCreatinine.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Creatinine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Creatinine", MySqlDbType.VarChar).Value = txtCreatinine.Text;
            }

            if (rblCreatinineUnits.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?CreatinineUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CreatinineUnit", MySqlDbType.VarChar).Value = rblCreatinineUnits.SelectedValue;
            }

            if (txtMeanCreatinine.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?MeanCreatinine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MeanCreatinine", MySqlDbType.VarChar).Value = txtMeanCreatinine.Text;
            }

            if (rblMeanCreatinineUnits.SelectedIndex == -1 )
            {
                MyCMD.Parameters.Add("?MeanCreatinineUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MeanCreatinineUnit", MySqlDbType.VarChar).Value = rblMeanCreatinineUnits.SelectedValue;
            }

            if (txtMaxCreatinine.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?MaxCreatinine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MaxCreatinine", MySqlDbType.VarChar).Value = txtMaxCreatinine.Text;
            }

            if (rblMaxCreatinineUnits.SelectedIndex == -1 || rblMaxCreatinineUnits.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?MaxCreatinineUnit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MaxCreatinineUnit", MySqlDbType.VarChar).Value = rblMaxCreatinineUnits.SelectedValue;
            }

            if (txtComments.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            if (txtReasonModified.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = DBNull.Value;

            }
            else
            {
                string strReasonModified = "";
                strReasonModified += DateTime.Now.ToString() + " Modified By " + SessionVariablesAll.UserName + System.Environment.NewLine;
                strReasonModified += txtReasonModified.Text;
                if (lblReasonModifiedOldDetails.Text != string.Empty)
                {
                    strReasonModified += System.Environment.NewLine + lblReasonModifiedOldDetails.Text;
                }

                MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = strReasonModified;
            }

            MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = 1;

            if (chkDataFinal.Checked == true)
            {
                MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 1;
            }
            else
            {
                MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 0;
            }


            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                //lock data
                if (chkAllDataAdded.Checked == true)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQL_LOCK;
                    MyCMD.ExecuteNonQuery();

                }

                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQL_FINAL;
                    MyCMD.ExecuteNonQuery();
                }



                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                BindData();

                string strSQLCOMPLETE = string.Empty;
                strSQLCOMPLETE += "SELECT ";
                strSQLCOMPLETE += "IF(t2.Hb IS NOT NULL AND t2.HbUnit IS NOT NULL  ";
                strSQLCOMPLETE += "AND t2.Ht IS NOT NULL AND t2.pH IS NOT NULL AND t2.pCO2 IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.pO2 IS NOT NULL AND t2.Urea IS NOT NULL AND t2.UreaUnit IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.Creatinine IS NOT NULL AND t2.CreatinineUnit IS NOT NULL ";
                //strSQLCOMPLETE += "AND t2.MeanCreatinine IS NOT NULL AND t2.MeanCreatinineUnit IS NOT NULL ";
                //strSQLCOMPLETE += "AND t2.MaxCreatinine IS NOT NULL  AND t2.MaxCreatinineUnit IS NOT NULL   ";
                strSQLCOMPLETE += " ";
                //strSQLCOMPLETE += "AND t2.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM donor_labresults t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialID=?TrialID ";
                strSQLCOMPLETE += "";

                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialID", Request.QueryString["TID"], STRCONN);

                //lblDonorRiskIndex.Text = strComplete;

                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);
                }
                else
                {
                    if (Request.Url.AbsoluteUri.Contains("&SCode=1"))
                    {
                        Response.Redirect(Request.Url.AbsoluteUri);
                        
                    }
                    else
                    {
                        Response.Redirect(Request.Url.AbsoluteUri + "&SCode=1", false);
                    }
                    lblUserMessages.Text = "Data Submitted";
                }
            }

            

            catch (System.Exception ex)
            {

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                lblUserMessages.Text = ex.Message + " An error occured while executing add/update query.";
            }

            

            


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
        }
    }
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
    
    
    protected void rblHbUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            ValidatorInitialise();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Hb Unit.";
        }
    }
    protected void rblpCO2Units_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            ValidatorInitialise();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting pCO2 Unit.";
        }
    }
    protected void rblpO2Units_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            ValidatorInitialise();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting pO2 Unit.";
        }
    }
    protected void rblUreaUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            ValidatorInitialise();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Urea Unit.";
        }
    }

    protected void rblCreatinineUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            ValidatorInitialise();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting  Creatinine Unit.";
        }
    }

    protected void rblMeanCreatinineUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            ValidatorInitialise();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Mean Creatinine Unit.";
        }
    }
    protected void rblMaxCreatinineUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            ValidatorInitialise();
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Max Creatinine Unit.";
        }
    }


    protected void ValidatorInitialise()
    {
        try
        {
            lblUserMessages.Text=string.Empty;


            if (rblHbUnits.SelectedValue == "mmol/l")
            {
                cv_txtHb.Type = ValidationDataType.Double;
                cv_txtHb.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtHb.MaximumValue = dblMinHb_mmol.ToString();
                rv_txtHb.MaximumValue = dblMaxHb_mmol.ToString();

                rv_txtHb.ErrorMessage = "Value should be between " + dblMinHb_mmol.ToString() + " and " + dblMaxHb_mmol.ToString();

                txtHb.ToolTip="Recommended value between " + ConstantsGeneral.dblMinHb_mmolRec.ToString() + " and " + ConstantsGeneral.dblMaxHb_mmolRec.ToString();
                //rv_txtHb.Type = ValidationDataType.Double;
                //rv_txtHb.MinimumValue = "1.86";
                //rv_txtHb.MaximumValue = "2.71";

                //lblUserMessages.Text = rblHbUnits.SelectedValue + " " + rv_txtHb.MinimumValue + " " + rv_txtHb.MaximumValue + " ";
            }
            else
            {
                //cv_txtHb.Type = ValidationDataType.Integer;
                //cv_txtHb.ErrorMessage = "Please enter Numeric Values. Decimals are not allowed.";

                cv_txtHb.Type = ValidationDataType.Double;
                cv_txtHb.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtHb.MaximumValue = dblMinHb_mgdl.ToString();
                rv_txtHb.MaximumValue = dblMaxHb_mgdl.ToString();

                rv_txtHb.ErrorMessage = "Value should be between " + dblMinHb_mgdl.ToString() + " and " + dblMaxHb_mgdl.ToString();
                txtHb.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinHb_mgdlRec.ToString() + " and " + ConstantsGeneral.dblMaxHb_mgdlRec.ToString();
                //rv_txtHb.Type = ValidationDataType.Integer;
                //rv_txtHb.MinimumValue = "12";
                //rv_txtHb.MaximumValue = "18";
                //lblUserMessages.Text = rblHbUnits.SelectedValue + " " + rv_txtHb.MinimumValue + " " + rv_txtHb.MaximumValue + " ";
                //lblUserMessages.Text = rblHbUnits.SelectedValue;
            }

            rv_txtHt.MinimumValue = dblMinHt.ToString();
            rv_txtHt.MaximumValue = dblMaxHt.ToString();

            rv_txtHt.ErrorMessage = "Value should be between " + rv_txtHt.MinimumValue + " and " + rv_txtHt.MaximumValue;
            txtHt.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinHtRec.ToString() + " and " + ConstantsGeneral.dblMaxHtRec.ToString();

            rv_txtpH.MinimumValue = dblMinpH.ToString();
            rv_txtpH.MaximumValue = dblMaxpH.ToString();

            rv_txtpH.ErrorMessage = "Value should be between " + rv_txtpH.MinimumValue + " and " + rv_txtpH.MaximumValue;
            txtpH.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinpHRec.ToString() + " and " + ConstantsGeneral.dblMaxpHRec.ToString();

            if (rblpCO2Units.SelectedValue == "kPa")
            {
                cv_txtpCO2.Type = ValidationDataType.Double;
                cv_txtpCO2.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtpCO2.MinimumValue = dblMinpCO2_kPa.ToString();
                rv_txtpCO2.MaximumValue = dblMaxpCO2_kPa.ToString();

                rv_txtpCO2.ErrorMessage = "Value should be between " + rv_txtpCO2.MinimumValue + " and " + rv_txtpCO2.MaximumValue;
                txtpCO2.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinpCO2_kPaRec.ToString() + " and " + ConstantsGeneral.dblMaxpCO2_kPaRec.ToString();
            }
            else
            {
                
                cv_txtpCO2.Type = ValidationDataType.Double;
                cv_txtpCO2.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtpCO2.MinimumValue = dblMinpCO2_mmhg.ToString();
                rv_txtpCO2.MaximumValue = dblMaxpCO2_mmhg.ToString();

                rv_txtpCO2.ErrorMessage = "Value should be between " + rv_txtpCO2.MinimumValue + " and " + rv_txtpCO2.MaximumValue;
                txtpCO2.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinpCO2_mmhgRec.ToString() + " and " + ConstantsGeneral.dblMaxpCO2_mmhgRec.ToString();
               
            }


            if (rblpO2Units.SelectedValue == "kPa")
            {
                cv_txtpO2.Type = ValidationDataType.Double;
                cv_txtpO2.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtpO2.MinimumValue = dblMinpO2_kPa.ToString();
                rv_txtpO2.MaximumValue = dblMaxpO2_kPa.ToString();

                rv_txtpO2.ErrorMessage = "Value should be between " + rv_txtpO2.MinimumValue + " and " + rv_txtpO2.MaximumValue;

                txtpCO2.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinpO2_kPaRec.ToString() + " and " + ConstantsGeneral.dblMaxpO2_kPaRec.ToString();

            }
            else
            {

                cv_txtpO2.Type = ValidationDataType.Double;
                cv_txtpO2.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtpO2.MinimumValue = dblMinpO2_mmhg.ToString();
                rv_txtpO2.MaximumValue = dblMaxpO2_mmhg.ToString();

                rv_txtpO2.ErrorMessage = "Value should be between " + rv_txtpO2.MinimumValue + " and " + rv_txtpO2.MaximumValue;

                txtpCO2.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinpO2_mmhgRec.ToString() + " and " + ConstantsGeneral.dblMaxpO2_mmhgRec.ToString();
            }

            if (rblUreaUnits.SelectedValue == "mmol/l")
            {
                cv_txtUrea.Type = ValidationDataType.Double;
                cv_txtUrea.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtUrea.MinimumValue = dblMinUrea_mmol.ToString();
                rv_txtUrea.MaximumValue = dblMaxUrea_mmol.ToString();

                rv_txtUrea.ErrorMessage = "Value should be between " + rv_txtUrea.MinimumValue + " and " + rv_txtUrea.MaximumValue;
                txtUrea.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinUrea_mmolRec.ToString() + " and " + ConstantsGeneral.dblMaxUrea_mmolRec.ToString();
            }
            else
            {
                cv_txtUrea.Type = ValidationDataType.Integer;
                cv_txtUrea.ErrorMessage = "Please enter Numeric Values. Decimals are not allowed.";

                rv_txtUrea.MinimumValue = dblMinUrea_gdl.ToString();
                rv_txtUrea.MaximumValue = dblMaxUrea_gdl.ToString();

                rv_txtUrea.ErrorMessage = "Value should be between " + rv_txtUrea.MinimumValue + " and " + rv_txtUrea.MaximumValue;
                txtUrea.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinUrea_gdlRec.ToString() + " and " + ConstantsGeneral.dblMaxUrea_gdlRec.ToString();

            }

            if (rblCreatinineUnits.SelectedValue == "mg/dL")
            {
                cv_txtCreatinine.Type = ValidationDataType.Double;
                cv_txtCreatinine.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtCreatinine.MinimumValue = dblMinCreatinine_mgdl.ToString();
                rv_txtCreatinine.MaximumValue = dblMaxCreatinine_mgdl.ToString();

                rv_txtCreatinine.ErrorMessage = "Value should be between " + rv_txtCreatinine.MinimumValue + " and " + rv_txtCreatinine.MaximumValue;

                txtCreatinine.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinCreatinine_mgdlRec.ToString() + " and " + ConstantsGeneral.dblMaxCreatinine_mgdlRec.ToString();

            }
            else
            {
                cv_txtCreatinine.Type = ValidationDataType.Integer;
                cv_txtCreatinine.ErrorMessage = "Please enter Numeric Values. Decimals are not allowed.";

                rv_txtCreatinine.MinimumValue = dblMinCreatinine_micromol.ToString();
                rv_txtCreatinine.MaximumValue = dblMaxCreatinine_micromol.ToString();

                rv_txtCreatinine.ErrorMessage = "Value should be between " + rv_txtCreatinine.MinimumValue + " and " + rv_txtCreatinine.MaximumValue;

                txtCreatinine.ToolTip = "Recommended value between " + ConstantsGeneral.dblMinCreatinine_micromolRec.ToString() + " and " + ConstantsGeneral.dblMaxCreatinine_micromolRec.ToString();
            }

            if (rblMeanCreatinineUnits.SelectedValue == "mg/dL")
            {
                cv_txtMeanCreatinine.Type = ValidationDataType.Double;
                cv_txtMeanCreatinine.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtMeanCreatinine.MinimumValue = dblMinCreatinine_mgdl.ToString();
                rv_txtMeanCreatinine.MaximumValue = dblMaxCreatinine_mgdl.ToString();

                rv_txtMeanCreatinine.ErrorMessage = "Value should be between " + rv_txtMeanCreatinine.MinimumValue + " and " + rv_txtMeanCreatinine.MaximumValue;
                txtMeanCreatinine.ToolTip = "Recommended value between " + dblMinCreatinine_mgdlRec.ToString() + " and " + dblMaxCreatinine_mgdlRec.ToString();

            }
            else
            {
                cv_txtMeanCreatinine.Type = ValidationDataType.Integer;
                cv_txtMeanCreatinine.ErrorMessage = "Please enter Numeric Values. Decimals are not allowed.";

                rv_txtMeanCreatinine.MinimumValue = dblMinCreatinine_micromol.ToString();
                rv_txtMeanCreatinine.MaximumValue = dblMaxCreatinine_micromol.ToString();

                rv_txtMeanCreatinine.ErrorMessage = "Value should be between " + rv_txtMeanCreatinine.MinimumValue + " and " + rv_txtMeanCreatinine.MaximumValue;
                txtMeanCreatinine.ToolTip = "Recommended value between " + dblMinCreatinine_micromolRec.ToString() + " and " + dblMaxCreatinine_micromolRec.ToString();
            }

            if (rblMaxCreatinineUnits.SelectedValue == "mg/dL")
            {
                cv_txtMaxCreatinine.Type = ValidationDataType.Double;
                cv_txtMaxCreatinine.ErrorMessage = "Please enter Numeric Values. Decimals are allowed.";

                rv_txtMaxCreatinine.MinimumValue = dblMinCreatinine_mgdl.ToString();
                rv_txtMaxCreatinine.MaximumValue = dblMaxCreatinine_mgdl.ToString();

                rv_txtMaxCreatinine.ErrorMessage = "Value should be between " + rv_txtMaxCreatinine.MinimumValue + " and " + rv_txtMaxCreatinine.MaximumValue;
                txtMaxCreatinine.ToolTip = "Recommended value between " + dblMinCreatinine_mgdlRec.ToString() + " and " + dblMaxCreatinine_mgdlRec.ToString();
            }
            else
            {
                cv_txtMaxCreatinine.Type = ValidationDataType.Integer;
                cv_txtMaxCreatinine.ErrorMessage = "Please enter Numeric Values. Decimals are not allowed.";

                rv_txtMaxCreatinine.MinimumValue = dblMinCreatinine_micromol.ToString();
                rv_txtMaxCreatinine.MaximumValue = dblMaxCreatinine_micromol.ToString();

                rv_txtMaxCreatinine.ErrorMessage = "Value should be between " + rv_txtMaxCreatinine.MinimumValue + " and " + rv_txtMaxCreatinine.MaximumValue;
                txtMaxCreatinine.ToolTip = "Recommended value between " + dblMinCreatinine_micromolRec.ToString() + " and " + dblMaxCreatinine_micromolRec.ToString();

            }

        }

        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Initialising Validation values. ";
        }
    }


    protected void chkAllDataAdded_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            if (chkAllDataAdded.Checked == true)
            {

                string strDataLockedOther = ConfigurationManager.AppSettings["LockedMessageManual"];
                //strDataLockedOther = "";
                lblAllDataAddedMessage.Text = strDataLockedOther;
                lblAllDataAddedMessage.Visible = true;


            }
            else
            {
                lblAllDataAddedMessage.Visible = false;

            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while clicking Data Entry Complete CheckBox";
        }
    }
}