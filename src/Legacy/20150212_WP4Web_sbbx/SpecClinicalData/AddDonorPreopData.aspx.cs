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


public partial class SpecClinicalData_AddDonorPreopData : System.Web.UI.Page
{

    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string STR_DEFAULT_SELECTION = "Unknown";

        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_OTHER_SELECTION = "Other";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        //static Random _random = new Random();

        //DateTime dteMinDateBirth = DateTime.Now.AddYears(-100);
        //DateTime dteMaxDateBirth = DateTime.Now.AddYears(-20);

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";

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

                lblDescription.Text = "Add Donor PreOp Data for " + Request.QueryString["TID"].ToString();

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                
                //// get the DonorID
                //string strDonorID = string.Empty;

                //ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

                //if (mpCPH != null)
                //{
                //    Label lblMainLabel = (Label)(mpCPH.FindControl(strMainLabel));

                //    if (lblMainLabel != null)
                //    {
                //        strDonorID = lblMainLabel.Text.Replace("(", "");
                //        strDonorID = strDonorID.Replace(")", "");
                //    }
                //}

                
                ddDonorDiagnosisOptions.DataSource = XMLDonorDiagnosisOptionsDataSource;
                ddDonorDiagnosisOptions.DataBind();

                rblDiabetesMellitus.DataSource = XmlMainOptionsDataSource;
                rblDiabetesMellitus.DataBind();
                //rblDiabetesMellitus.SelectedValue = STR_DEFAULT_SELECTION;

                rblAlcoholAbuse.DataSource = XmlMainOptionsDataSource;
                rblAlcoholAbuse.DataBind();
                //rblAlcoholAbuse.SelectedValue = STR_DEFAULT_SELECTION;

                rblCardiacArrest.DataSource = XmlMainOptionsYNDataSource;
                rblCardiacArrest.DataBind();
                //rblCardiacArrest.SelectedValue = STR_DEFAULT_SELECTION;


                rv_txtSystolicBloodPressure.MinimumValue = ConstantsGeneral.intMinDiastolicBloodPressure.ToString();
                rv_txtSystolicBloodPressure.MaximumValue = ConstantsGeneral.intMaxSystolicBloodPressure.ToString();
                rv_txtSystolicBloodPressure.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinSystolicBloodPressure.ToString() + " and " + ConstantsGeneral.intMaxSystolicBloodPressure.ToString();
                txtSystolicBloodPressure.ToolTip = "Recommended value between " + ConstantsGeneral.intMinSystolicBloodPressureRec.ToString() + " and " + ConstantsGeneral.intMaxSystolicBloodPressureRec.ToString();

                rv_txtDiastolicBloodPressure.MinimumValue = ConstantsGeneral.intMinDiastolicBloodPressure.ToString();
                rv_txtDiastolicBloodPressure.MaximumValue = ConstantsGeneral.intMaxSystolicBloodPressure.ToString();
                rv_txtDiastolicBloodPressure.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinDiastolicBloodPressure.ToString() + " and " + ConstantsGeneral.intMaxDiastolicBloodPressure.ToString();
                txtDiastolicBloodPressure.ToolTip = "Recommended value between " + ConstantsGeneral.intMinDiastolicBloodPressureRec.ToString() + " and " + ConstantsGeneral.intMaxDiastolicBloodPressureRec.ToString();

                rblHypotensivePeriod.DataSource = XmlMainOptionsYNDataSource;
                rblHypotensivePeriod.DataBind();
                rblHypotensivePeriod.SelectedValue = STR_DEFAULT_SELECTION;


                //rblDonorAnuriaOliguria.DataSource = XmlMainOptionsDataSource;
                //rblDonorAnuriaOliguria.DataBind();
                //rblDonorAnuriaOliguria.SelectedValue = STR_DEFAULT_SELECTION;

                
                rblDopamine.DataSource = XmlMainOptionsDataSource;
                rblDopamine.DataBind();
                //rblDopamine.SelectedValue = STR_DEFAULT_SELECTION;

                lblDopamineLastDose.Visible = false;
                txtDopamineLastDose.Visible = false;

                //rv_txtDopamineLastDose.MinimumValue = ConstantsGeneral.intMinDopamineLastDose.ToString();
                //rv_txtDopamineLastDose.MaximumValue = ConstantsGeneral.intMaxDopamineLastDose.ToString();
                //rv_txtDopamineLastDose.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinDopamineLastDose.ToString() + " and " + ConstantsGeneral.intMaxDopamineLastDose.ToString();
                //txtDopamineLastDose.ToolTip = "Recommended value between " + ConstantsGeneral.intMinDopamineLastDoseRec.ToString() + " and " + ConstantsGeneral.intMaxDopamineLastDoseRec.ToString();

                rblDobutamine.DataSource = XmlMainOptionsDataSource;
                rblDobutamine.DataBind();
                //rblDobutamine.SelectedValue = STR_DEFAULT_SELECTION;
                
                lblDobutamineLastDose.Visible = false;
                txtDobutamineLastDose.Visible = false;

                //rv_txtDobutamineLastDose.MinimumValue = ConstantsGeneral.intMinDobutamineLastDose.ToString();
                //rv_txtDobutamineLastDose.MaximumValue = ConstantsGeneral.intMaxDobutamineLastDose.ToString();
                //rv_txtDobutamineLastDose.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinDobutamineLastDose.ToString() + " and " + ConstantsGeneral.intMaxDobutamineLastDose.ToString();
                //txtDobutamineLastDose.ToolTip = "Recommended value between " + ConstantsGeneral.intMinDobutamineLastDoseRec.ToString() + " and " + ConstantsGeneral.intMaxDobutamineLastDoseRec.ToString();


                rblNorAdrenaline.DataSource = XmlMainOptionsDataSource;
                rblNorAdrenaline.DataBind();
                //rblNorAdrenaline.SelectedValue = STR_DEFAULT_SELECTION;
                
                lblNorAdrenalineLastDose.Visible = false;
                txtNorAdrenalineLastDose.Visible = false;

                //rv_txtNorAdrenalineLastDose.MinimumValue = ConstantsGeneral.intMinNorAdrenalineLastDose.ToString();
                //rv_txtNorAdrenalineLastDose.MaximumValue = ConstantsGeneral.intMaxNorAdrenalineLastDose.ToString();
                //rv_txtNorAdrenalineLastDose.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinNorAdrenalineLastDose.ToString() + " and " + ConstantsGeneral.intMaxNorAdrenalineLastDose.ToString();
                //txtNorAdrenalineLastDose.ToolTip = "Recommended value between " + ConstantsGeneral.intMinNorAdrenalineLastDoseRec.ToString() + " and " + ConstantsGeneral.intMaxNorAdrenalineLastDoseRec.ToString();


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

            strSQL += "SELECT t1.* ";
            strSQL += " ";
            strSQL += "FROM donor_preop_clinicaldata t1 ";
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
                lblDescription.Text = "Update  Donor PreOp Data for " + Request.QueryString["TID"].ToString() + "";

                AssignData();

                lblGV1.Text = "Summary of Donor Pre Op Data";
            }
            else if (GV1.Rows.Count == 0)
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblDescription.Text = "Add  Donor PreOp Data for " + Request.QueryString["TID"].ToString() + "";
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

            string STRSQL = "SELECT t1.*, t2.DonorID Donor FROM  donor_preop_clinicaldata t1 ";
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

                            //if (!DBNull.Value.Equals(myDr["Diagnosis"]))
                            //{
                            //    string[] strSC_Sets = myDr["Diagnosis"].ToString().Split(',');

                            //    for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                            //    {
                            //        string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                            //        ListItem currentCheckBox = cblDonorDiagnosisOptions.Items.FindByValue(strSC_Contents[0].ToString());

                            //        if (currentCheckBox != null)
                            //        {
                            //            if (strSC_Contents[1].ToString() == STR_YES_SELECTION)
                            //            {
                            //                currentCheckBox.Selected = true;
                            //            }

                            //        }

                            //    }
                            //}

                            if (!DBNull.Value.Equals(myDr["Diagnosis"]))
                            {
                                ddDonorDiagnosisOptions.Text = (string)(myDr["Diagnosis"]);
                            }

                            if (lblDonorDiagnosisOptions.Font.Bold == true)
                            {
                                if (ddDonorDiagnosisOptions.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblDonorDiagnosisOptions.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DiagnosisOtherDetails"]))
                            {
                                txtDiagnosisOtherDetails.Text = (string)(myDr["DiagnosisOtherDetails"]);
                            }


                            if (lblDiagnosisOtherDetails.Font.Bold == true)
                            {
                                if (txtDiagnosisOtherDetails.Text == String.Empty)
                                {
                                    lblDiagnosisOtherDetails.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DiabetesMellitus"]))
                            {
                                rblDiabetesMellitus.SelectedValue = (string)(myDr["DiabetesMellitus"]);
                            }

                            if (lblDiabetesMellitus.Font.Bold == true)
                            {
                                if (rblDiabetesMellitus.SelectedIndex==-1)
                                {
                                    lblDiabetesMellitus.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["AlcoholAbuse"]))
                            {
                                rblAlcoholAbuse.SelectedValue = (string)(myDr["AlcoholAbuse"]);
                            }

                            if (lblAlcoholAbuse.Font.Bold == true)
                            {
                                if (rblAlcoholAbuse.SelectedIndex == -1)
                                {
                                    lblAlcoholAbuse.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CardiacArrest"]))
                            {
                                rblCardiacArrest.SelectedValue = (string)(myDr["CardiacArrest"]);
                            }

                            if (lblCardiacArrest.Font.Bold == true)
                            {
                                if (rblCardiacArrest.SelectedIndex == -1)
                                {
                                    lblCardiacArrest.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["SystolicBloodPressure"]))
                            {
                                txtSystolicBloodPressure.Text = (string)(myDr["SystolicBloodPressure"]);
                            }

                            if (lblSystolicBloodPressure.Font.Bold == true)
                            {
                                if (txtSystolicBloodPressure.Text == String.Empty)
                                {
                                    lblSystolicBloodPressure.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DiastolicBloodPressure"]))
                            {
                                txtDiastolicBloodPressure.Text = (string)(myDr["DiastolicBloodPressure"]);
                            }

                            if (lblDiastolicBloodPressure.Font.Bold == true)
                            {
                                if (txtDiastolicBloodPressure.Text == String.Empty)
                                {
                                    lblDiastolicBloodPressure.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["HypotensivePeriod"]))
                            {
                                rblHypotensivePeriod.SelectedValue = (string)(myDr["HypotensivePeriod"]);
                            }

                            if (lblHypotensivePeriod.Font.Bold == true)
                            {
                                if (rblHypotensivePeriod.SelectedIndex == -1)
                                {
                                    lblHypotensivePeriod.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Diuresis"]))
                            {
                                if ((string)(myDr["Diuresis"])==STR_UNKNOWN_SELECTION)
                                {
                                    chkDiuresis.Checked=true;
                                }
                                
                                if (chkDiuresis.Checked!=true)
                                {
                                    txtDiuresis.Text = (string)(myDr["Diuresis"]);
                                }
                                
                            }

                            if (lblDiuresis.Font.Bold == true)
                            {
                                if (chkDiuresis.Checked == false && txtDiuresis.Text == string.Empty)
                                {
                                    lblDiuresis.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            //if (!DBNull.Value.Equals(myDr["DonorAnuriaOliguria"]))
                            //{
                            //    rblDonorAnuriaOliguria.SelectedValue = (string)(myDr["DonorAnuriaOliguria"]);
                            //}

                            
                            if (!DBNull.Value.Equals(myDr["Dopamine"]))
                            {
                                rblDopamine.SelectedValue = (string)(myDr["Dopamine"]);
                            }

                            if (lblDopamine.Font.Bold == true)
                            {
                                if (rblDopamine.SelectedIndex==-1)
                                {
                                    lblDopamine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DopamineLastDose"]))
                            {
                                txtDopamineLastDose.Text = (string)(myDr["DopamineLastDose"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Dobutamine"]))
                            {
                                rblDobutamine.SelectedValue = (string)(myDr["Dobutamine"]);
                            }

                            if (lblDobutamine.Font.Bold == true)
                            {
                                if (rblDobutamine.SelectedIndex == -1)
                                {
                                    lblDobutamine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DobutamineLastDose"]))
                            {
                                txtDobutamineLastDose.Text = (string)(myDr["DobutamineLastDose"]);
                            }

                            if (!DBNull.Value.Equals(myDr["NorAdrenaline"]))
                            {
                                rblNorAdrenaline.SelectedValue = (string)(myDr["NorAdrenaline"]);
                            }

                            if (lblNorAdrenaline.Font.Bold == true)
                            {
                                if (rblNorAdrenaline.SelectedIndex == -1)
                                {
                                    lblNorAdrenaline.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NorAdrenalineLastDose"]))
                            {
                                txtNorAdrenalineLastDose.Text = (string)(myDr["NorAdrenalineLastDose"]);
                            }

                            if (!DBNull.Value.Equals(myDr["OtherMedication"]))
                            {
                                txtOtherMedication.Text = (string)(myDr["OtherMedication"]);
                            }

                            if (!DBNull.Value.Equals(myDr["OtherMedicationLastDose"]))
                            {
                                txtOtherMedicationLastDose.Text = (string)(myDr["OtherMedicationLastDose"]);
                            }

                            if (!DBNull.Value.Equals(myDr["OtherMedication2"]))
                            {
                                txtOtherMedication2.Text = (string)(myDr["OtherMedication2"]);
                            }

                            if (!DBNull.Value.Equals(myDr["OtherMedication2LastDose"]))
                            {
                                txtOtherMedication2LastDose.Text = (string)(myDr["OtherMedication2LastDose"]);
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

                rblDopamine_SelectedIndexChanged(this, EventArgs.Empty);
                rblDobutamine_SelectedIndexChanged(this, EventArgs.Empty);
                rblNorAdrenaline_SelectedIndexChanged(this, EventArgs.Empty);

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
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Assigning Data.";
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
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //if (Page.IsValid == false)
            //{
            //    throw new Exception("Please check data you have entered.");
            //}

            if (ddDonorDiagnosisOptions.SelectedValue==STR_OTHER_SELECTION)
            {
                //throw new Exception("Please Select at least one Option from Donor Diagnosis.");

                Page.Validate("Diagnosis");

                if (Page.IsValid == false)
                {
                    throw new Exception("Please enter " + lblDiagnosisOtherDetails.Text);
                }

            }

            //check the options selected
            //int i = 0; //count the number of options selected

            //foreach (ListItem item in cblDonorDiagnosisOptions.Items)
            //{

            //    if (item.Selected == true)

            //    {
            //        if (item.Value == "Other")
            //        {
            //            if (txtDiagnosisOtherDetails.Text == string.Empty)
            //            {
            //                throw new Exception("Since you have selected 'Other' in Donor Diagnosis, Please provide details for Diagnosis (If Other).");
            //            }
            //        }
            //        i += 1;
            //    }
               
            //}

            //if (i == 0)
            //{
            //    throw new Exception("Please Select at least one Option from Donor Diagnosis.");
            //}

            //if (rblDiabetesMellitus.SelectedIndex == -1 || rblDiabetesMellitus.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for Diabetes Mellitus."); }

            //if (rblAlcoholAbuse.SelectedIndex == -1 || rblAlcoholAbuse.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for Alcohol Abuse."); }

            //if (rblCardiacArrest.SelectedIndex == -1 || rblCardiacArrest.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for Cardiac Arrest."); }

            //if (!GeneralRoutines.IsNumeric(txtSystolicBloodPressure.Text))
            //{ throw new Exception("Please Enter a numeric value for Systolic Blood Pressure."); }

            //if (!GeneralRoutines.IsNumeric(txtDiastolicBloodPressure.Text))
            //{ throw new Exception("Please Enter a numeric value for Diastolic Blood Pressure."); }
            
            //if (rblHypotensivePeriod.SelectedIndex == -1 || rblHypotensivePeriod.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for Hypotensive Period."); }

            //if (!GeneralRoutines.IsNumeric(txtDiuresis.Text))
            //{ throw new Exception("Please Enter a numeric value for Diuresis."); }

            //if (rblDonorAnuriaOliguria.SelectedIndex == -1 || rblDonorAnuriaOliguria.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for Donor Anuria/Oliguria."); }

            //if (rblDopamine.SelectedIndex == -1 || rblDopamine.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for Dopamine."); }

            //if (txtDopamineLastDose.Text!=string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtDopamineLastDose.Text) == false)
            //    {
            //        throw new Exception("Since the option selected for Dopamine is YES, please enter a numeric value for last Dose for Dopamine."); 
            //    }
            //}
            //else 
            //{
            //    if (txtDopamineLastDose.Text != string.Empty)
            //    {
            //        throw new Exception("Since the option selected for Dopamine is NO, the Text Box for last Dose of Dopamine should be empty.");
            //    }
            //}

            //if (rblDobutamine.SelectedIndex == -1 || rblDobutamine.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for Dobutamine."); }

            //if (txtDobutamineLastDose.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtDobutamineLastDose.Text) == false)
            //    {
            //        throw new Exception("Since the option selected for Dobutamine is YES, please enter a numeric value for last Dose for Dobutamine.");
            //    }
            //}
            //else
            //{
            //    if (txtDobutamineLastDose.Text != string.Empty)
            //    {
            //        throw new Exception("Since the option selected for Dobutamine is NO, the Text Box for last Dose of Dobutamine should be empty.");
            //    }
            //}

            //if (rblNorAdrenaline.SelectedIndex == -1 || rblNorAdrenaline.SelectedValue == STR_DEFAULT_SELECTION)
            //{ throw new Exception("Please Select an option for NorAdrenaline."); }

            //if (txtNorAdrenalineLastDose.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtNorAdrenalineLastDose.Text) == false)
            //    {
            //        throw new Exception("Since the option selected for NorAdrenaline is YES, please enter a numeric value for last Dose for NorAdrenaline.");
            //    }
            //}
            //else
            //{
            //    if (txtNorAdrenalineLastDose.Text != string.Empty)
            //    {
            //        throw new Exception("Since the option selected for NorAdrenaline is NO, the Text Box for Last Dose of NorAdrenaline should be empty.");
            //    }
            //}

            if (txtOtherMedicationLastDose.Text != string.Empty)
            {
                if (txtOtherMedication.Text == string.Empty)
                {
                    throw new Exception("Since you have entered the Lase Dose for 'Other Medication (1)', Please enter details of 'Other Medication (1)'.");
                }
            }

            if (txtOtherMedication2LastDose.Text != string.Empty)
            {
                if (txtOtherMedication2.Text == string.Empty)
                {
                    throw new Exception("Since you have entered the Lase Dose for 'Other Medication (2)', Please enter details of 'Other Medication (2)'.");
                }
            }

            //if (txtOtherMedicationLastDose.Text != string.Empty)
            //{
            //    if (txtOtherMedication.Text == string.Empty)
            //        throw new Exception("As you have entered the Last Dose for Other medication, Please Enter Details of Other Medication.");
            //    {
            //    }
            //}

            if (Page.IsValid == false)
            {
                throw new Exception("Please check data you have entered.");
            }

            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }
                      

            //add the data
            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO donor_preop_clinicaldata ";
            STRSQL += "(TrialID,";
            STRSQL += "Diagnosis, DiagnosisOtherDetails,";
            STRSQL += "DiabetesMellitus, AlcoholAbuse, CardiacArrest, SystolicBloodPressure, DiastolicBloodPressure, HypotensivePeriod, Diuresis, ";
            STRSQL += "Dopamine, DopamineLastDose, Dobutamine, DobutamineLastDose, NorAdrenaline,NorAdrenalineLastDose, ";
            STRSQL += "OtherMedication, OtherMedicationLastDose,OtherMedication2, OtherMedication2LastDose,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID,";
            STRSQL += "?Diagnosis, ?DiagnosisOtherDetails,";
            STRSQL += "?DiabetesMellitus, ?AlcoholAbuse, ?CardiacArrest, ?SystolicBloodPressure, ?DiastolicBloodPressure, ?HypotensivePeriod, ?Diuresis, ";
            STRSQL += "?Dopamine, ?DopamineLastDose, ?Dobutamine, ?DobutamineLastDose, ?NorAdrenaline, ?NorAdrenalineLastDose,";
            STRSQL += "?OtherMedication, ?OtherMedicationLastDose,?OtherMedication2, ?OtherMedication2LastDose,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE donor_preop_clinicaldata SET ";
            STRSQL_UPDATE += "Diagnosis=?Diagnosis, DiagnosisOtherDetails=?DiagnosisOtherDetails,";
            STRSQL_UPDATE += "DiabetesMellitus=?DiabetesMellitus, AlcoholAbuse=?AlcoholAbuse, CardiacArrest=?CardiacArrest, SystolicBloodPressure=?SystolicBloodPressure,  ";
            STRSQL_UPDATE += "DiastolicBloodPressure=?DiastolicBloodPressure, HypotensivePeriod=?HypotensivePeriod, Diuresis=?Diuresis, ";
            STRSQL_UPDATE += "Dopamine=?Dopamine, DopamineLastDose=?DopamineLastDose, Dobutamine=?Dobutamine,";
            STRSQL_UPDATE += "DobutamineLastDose=?DobutamineLastDose, NorAdrenaline=?NorAdrenaline, NorAdrenalineLastDose=?NorAdrenalineLastDose, ";
            STRSQL_UPDATE += "OtherMedication=?OtherMedication, OtherMedicationLastDose=?OtherMedicationLastDose,  ";
            STRSQL_UPDATE += "OtherMedication2=?OtherMedication2, OtherMedication2LastDose=?OtherMedication2LastDose,  ";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


            //lock data
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE donor_preop_clinicaldata SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE donor_preop_clinicaldata SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_preop_clinicaldata WHERE TrialID=?TrialID ";

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
                throw new Exception("More than One Donor PreOp Data exists for this TrialID. Click on TrialID in the table below to select data to delete. ");
            }
            else
            { 
                throw new Exception("An error occured while check if Donor PreOp Data already exist in the database.");
            }


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();


            //append Diagnosis selection
            string strDiagnosis = string.Empty;

            //appened data
            //i = 0;

            //for (i = 0; i < cblDonorDiagnosisOptions.Items.Count; i++)
            //{
            //    strDiagnosis += cblDonorDiagnosisOptions.Items[i].Value + ":";
            //    if (cblDonorDiagnosisOptions.Items[i].Selected)
            //    {
            //        strDiagnosis += STR_YES_SELECTION;
            //    }
            //    else
            //    {
            //        strDiagnosis += STR_NO_SELECTION;
            //    }

            //    if (i < cblDonorDiagnosisOptions.Items.Count - 1)
            //    {
            //        strDiagnosis += ",";
            //    }

            //}

            if (ddDonorDiagnosisOptions.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?Diagnosis", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Diagnosis", MySqlDbType.VarChar).Value = ddDonorDiagnosisOptions.SelectedValue;
            }
                       
            
            // add remaining parameters
            if (txtDiagnosisOtherDetails.Text == string.Empty)
            { MyCMD.Parameters.Add("?DiagnosisOtherDetails", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?DiagnosisOtherDetails", MySqlDbType.VarChar).Value = txtDiagnosisOtherDetails.Text; }

            if (rblDiabetesMellitus.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?DiabetesMellitus", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DiabetesMellitus", MySqlDbType.VarChar).Value = rblDiabetesMellitus.SelectedValue;
            }

            if (rblAlcoholAbuse.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?AlcoholAbuse", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AlcoholAbuse", MySqlDbType.VarChar).Value = rblAlcoholAbuse.SelectedValue;
            }

            if (rblCardiacArrest.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?CardiacArrest", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CardiacArrest", MySqlDbType.VarChar).Value = rblCardiacArrest.SelectedValue;
            }

            if (txtSystolicBloodPressure.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?SystolicBloodPressure", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            {
                MyCMD.Parameters.Add("?SystolicBloodPressure", MySqlDbType.VarChar).Value = txtSystolicBloodPressure.Text; 
            }

            if (txtDiastolicBloodPressure.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?DiastolicBloodPressure", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DiastolicBloodPressure", MySqlDbType.VarChar).Value = txtDiastolicBloodPressure.Text;
            }

            if (rblHypotensivePeriod.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?HypotensivePeriod", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HypotensivePeriod", MySqlDbType.VarChar).Value = rblHypotensivePeriod.SelectedValue;
            }


            if (chkDiuresis.Checked==true)
            {
                MyCMD.Parameters.Add("?Diuresis", MySqlDbType.VarChar).Value = STR_UNKNOWN_SELECTION;
            }
            else
            {
                if (txtDiuresis.Text == string.Empty)
                {
                    MyCMD.Parameters.Add("?Diuresis", MySqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    MyCMD.Parameters.Add("?Diuresis", MySqlDbType.VarChar).Value = txtDiuresis.Text;
                }
            }

            //if (rblDonorAnuriaOliguria.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?DonorAnuriaOliguria", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?DonorAnuriaOliguria", MySqlDbType.VarChar).Value = rblDonorAnuriaOliguria.SelectedValue;
            //}


            if (rblDopamine.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?Dopamine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Dopamine", MySqlDbType.VarChar).Value = rblDopamine.SelectedValue;
            }

            if (txtDopamineLastDose.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?DopamineLastDose", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            {
                MyCMD.Parameters.Add("?DopamineLastDose", MySqlDbType.VarChar).Value = txtDopamineLastDose.Text; 
            }


            if (rblDobutamine.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?Dobutamine", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Dobutamine", MySqlDbType.VarChar).Value = rblDobutamine.SelectedValue;
            }

            
            if (txtDobutamineLastDose.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?DobutamineLastDose", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            {
                MyCMD.Parameters.Add("?DobutamineLastDose", MySqlDbType.VarChar).Value = txtDobutamineLastDose.Text; 
            }

            if (rblNorAdrenaline.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?NorAdrenaline", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NorAdrenaline", MySqlDbType.VarChar).Value = rblNorAdrenaline.SelectedValue;
            }
            
            if (txtNorAdrenalineLastDose.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?NorAdrenalineLastDose", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?NorAdrenalineLastDose", MySqlDbType.VarChar).Value = txtNorAdrenalineLastDose.Text; 
            }

            if (txtOtherMedication.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?OtherMedication", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?OtherMedication", MySqlDbType.VarChar).Value = txtOtherMedication.Text;
            }

            if (txtOtherMedicationLastDose.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?OtherMedicationLastDose", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?OtherMedicationLastDose", MySqlDbType.VarChar).Value = txtOtherMedicationLastDose.Text; 
            }

            if (txtOtherMedication2.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?OtherMedication2", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?OtherMedication2", MySqlDbType.VarChar).Value = txtOtherMedication2.Text; 
            }

            if (txtOtherMedication2LastDose.Text == string.Empty)
            { 
                MyCMD.Parameters.Add("?OtherMedication2LastDose", MySqlDbType.VarChar).Value = DBNull.Value; 
            }
            else
            { 
                MyCMD.Parameters.Add("?OtherMedication2LastDose", MySqlDbType.VarChar).Value = txtOtherMedication2LastDose.Text; 
            }
            
            if (string.IsNullOrEmpty(txtComments.Text))
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
                strSQLCOMPLETE += "IF(IF(t2.Diagnosis ='Other', t2.DiagnosisOtherDetails IS NOT NULL, t2.Diagnosis IS NOT NULL)  ";
                strSQLCOMPLETE += "AND t2.DiabetesMellitus IS NOT NULL AND t2.AlcoholAbuse IS NOT NULL AND t2.CardiacArrest IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.SystolicBloodPressure IS NOT NULL AND t2.DiastolicBloodPressure IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.Diuresis IS NOT NULL AND t2.Dopamine IS NOT NULL AND t2.Dobutamine IS NOT NULL  ";
                strSQLCOMPLETE += " ";
                strSQLCOMPLETE += "AND t2.NorAdrenaline IS NOT NULL ";
                //strSQLCOMPLETE += "AND t2.DonorRiskIndex IS NOT NULL, 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM donor_preop_clinicaldata t2 ";
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

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while adding data.";
        }
    }
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_identification WHERE TrialID=?TrialID ";

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
                if (String.IsNullOrEmpty(Request.QueryString["DonorIdentificationID"]))
                {
                    throw new Exception("More than one Record exists for deletion. Click on TrialID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM donor_preop_clinicaldata ";
            STRSQL += "WHERE TrialID=?TrialID ";
            if (intCountFind > 1)
            {
                STRSQL += "AND DonorPreOpClinicalDataID=?DonorPreOpClinicalDataID ";
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

            if (!String.IsNullOrEmpty(Request.QueryString["DonorPreOpClinicalDataID"]))
            {
                MyCMD.Parameters.Add("?DonorPreOpClinicalDataID", MySqlDbType.VarChar).Value = Request.QueryString["DonorPreOpClinicalDataID"].ToString();
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
    protected void txtSystolicBloodPressure_TextChanged(object sender, EventArgs e)
    {
        if (GeneralRoutines.IsNumeric(txtSystolicBloodPressure.Text))
        {
            if (Convert.ToInt32(txtSystolicBloodPressure.Text)<100)
            {
                rblHypotensivePeriod.SelectedValue = STR_YES_SELECTION;
            }
            else
            {
                rblHypotensivePeriod.SelectedValue = STR_NO_SELECTION;
            }
            //else
            //{

            //}

        }
        else
        {
            rblHypotensivePeriod.SelectedIndex = -1;
        }
    }
    //protected void txtDiuresis_TextChanged(object sender, EventArgs e)
    //{
    //    if (GeneralRoutines.IsNumeric(txtDiuresis.Text))
    //    {
    //        if (Convert.ToDouble(txtDiuresis.Text) < 500)
    //        {
    //           rblDonorAnuriaOliguria.SelectedValue = STR_YES_SELECTION;
    //        }
    //        else
    //        {
    //            rblDonorAnuriaOliguria.SelectedValue = STR_NO_SELECTION;
    //        }

    //    }
    //}
    protected void ddDonorDiagnosisOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddDonorDiagnosisOptions.SelectedValue==STR_OTHER_SELECTION)
            {
                pnlDiagnosis.Visible=true;
            }
            else
            {
                pnlDiagnosis.Visible = false;
                txtDiagnosisOtherDetails.Text = string.Empty;

            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting an option for Diagnosis.";
        }
    }
    protected void chkDiuresis_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            txtDiuresis.Text = string.Empty;

        }
        catch(Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting an option for Checkbox " + chkDiuresis.Text;
        }
    }
    protected void rblDopamine_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblDopamine.SelectedValue==STR_YES_SELECTION)
        {
            lblDopamineLastDose.Visible = true;
            txtDopamineLastDose.Visible = true;
        }
        else
        {
            lblDopamineLastDose.Visible = false;
            txtDopamineLastDose.Visible = false;
            txtDopamineLastDose.Text = string.Empty;
        }
    }
    protected void rblDobutamine_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblDobutamine.SelectedValue == STR_YES_SELECTION)
        {
            lblDobutamineLastDose.Visible = true;
            txtDobutamineLastDose.Visible = true;
        }
        else
        {
            lblDobutamineLastDose.Visible = false;
            txtDobutamineLastDose.Visible = false;
            txtDobutamineLastDose.Text = string.Empty;
        }
    }
    protected void rblNorAdrenaline_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblNorAdrenaline.SelectedValue == STR_YES_SELECTION)
        {
            lblNorAdrenalineLastDose.Visible = true;
            txtNorAdrenalineLastDose.Visible = true;
        }
        else
        {
            lblNorAdrenalineLastDose.Visible = false;
            txtNorAdrenalineLastDose.Visible = false;
            txtNorAdrenalineLastDose.Text = string.Empty;
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