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

public partial class SpecClinicalData_AddDonorDetails : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string STR_DEFAULT_SELECTION = "Unknown";

        private const string strMainCPH  = "cplMainContents" ;
        private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        //number of days before the Date when a TrialID was created Start Date for calendar control should be
        private const int intDaysMinDate = -30;

        private const int intDaysMinDateOperation = 2;


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

               
                
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                //// get the DonorID
                //string strDonorID = string.Empty;

                //ContentPlaceHolder mpCPH  = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

                //if (mpCPH != null)
                //{
                //    Label lblMainLabel = (Label)(mpCPH.FindControl(strMainLabel));

                //    if (lblMainLabel != null)
                //    {
                //        strDonorID = lblMainLabel.Text.Replace("(", "");
                //        strDonorID = strDonorID.Replace(")", "");
                //    }
                    
                //}

                lblDescription.Text = "Add Donor Details for " + Request.QueryString["TID"].ToString() + "";

                //declare View State variables
                ViewState["DateCreated"] = string.Empty;
                //get Date Created
                string STRSQL_DATECREATED = "SELECT DateCreated FROM trialdetails WHERE TrialID=?TrialID ";
                ViewState["DateCreated"] = GeneralRoutines.ReturnScalar(STRSQL_DATECREATED, "?TrialID", Request.QueryString["TID"], STRCONN);

                rv_txtDateDonorAdmission.MinimumValue = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate).ToShortDateString();
                rv_txtDateDonorAdmission.MaximumValue = DateTime.Today.AddDays(2).ToShortDateString();
                rv_txtDateDonorAdmission.ErrorMessage = "Date should be between " + Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate).ToShortDateString() + " and " + DateTime.Today.ToShortDateString();
                txtDateDonorAdmission_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate);
                //txtDateDonorAdmission_CalendarExtender.EndDate = DateTime.Today.AddDays(2);
                txtDateDonorAdmission_CalendarExtender.EndDate = DateTime.Today;

                rv_txtDateDonorOperation.MinimumValue = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(-1*intDaysMinDateOperation).ToShortDateString();
                rv_txtDateDonorOperation.MaximumValue = DateTime.Today.AddDays(1 * intDaysMinDateOperation).ToShortDateString();
                rv_txtDateDonorOperation.ErrorMessage = "Date should be between " + rv_txtDateDonorOperation.MinimumValue + " and " + rv_txtDateDonorOperation.MaximumValue;
                txtDateDonorOperation_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(-1 * intDaysMinDateOperation).Date;
                //txtDateDonorOperation_CalendarExtender.EndDate = DateTime.Today.AddDays(1 * intDaysMinDateOperation).Date;
                txtDateDonorOperation_CalendarExtender.EndDate = DateTime.Today;

                rblSex.DataSource = XMLSexDataSource;
                rblSex.DataBind();
                //rblSex.SelectedValue = STR_UNKNOWN_SELECTION;

                rv_txtWeight.MinimumValue = ConstantsGeneral.dblMinDonorWeight.ToString();
                rv_txtWeight.MaximumValue = ConstantsGeneral.dblMaxDonorWeight.ToString();
                rv_txtWeight.ErrorMessage = "Value should be between " + ConstantsGeneral.dblMinDonorWeight.ToString() + " and " + ConstantsGeneral.dblMaxDonorWeight.ToString();
                txtWeight.ToolTip = "Recommended Value between " + ConstantsGeneral.dblMinDonorWeightRec.ToString() + " and " + ConstantsGeneral.dblMaxDonorWeightRec.ToString();

                rv_txtHeight.MinimumValue = ConstantsGeneral.intMinDonorHeight.ToString();
                rv_txtHeight.MaximumValue = ConstantsGeneral.intMaxDonorHeight.ToString();
                rv_txtHeight.ErrorMessage = "Value should be between " + ConstantsGeneral.intMinDonorHeight.ToString() + " and " + ConstantsGeneral.intMaxDonorHeight.ToString();
                txtHeight.ToolTip = "Recommended Value between " + ConstantsGeneral.intMinDonorHeightRec.ToString() + " and " + ConstantsGeneral.intMaxDonorHeightRec.ToString();
                
                rblBloodGroup.DataSource = XmlBloodGroupDataSource;
                rblBloodGroup.DataBind();
                rblBloodGroup.SelectedValue = STR_DEFAULT_SELECTION;

                

                cblOtherOrgansDonated.DataSource = XMLOtherOrgansDataSource;
                cblOtherOrgansDonated.DataBind();

                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "DESC";

                BindData();

                //check if donor has been added
                string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_identification WHERE TrialID=?TrialID ";

                int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

                if (intCountFind > 1)
                { throw new Exception("More than One Record Exists in the Database for TrialID " + Request.QueryString["TID"].ToString()  +  "."); }

                if (intCountFind == 1)
                {
                    AssignData();

                    lblDescription.Text = "Edit Donor Details for " + Request.QueryString["TID"].ToString() + "";

                    //cmdAddData.Text = "Update Data";

                    cmdDelete.Enabled=true;
                    cmdDelete.Visible = true;

                }

                if (intCountFind < 0)
                { 
                    throw new Exception("Error while checking if Donor Exists in the Database for TrialID " + Request.QueryString["TID"].ToString() + " ."); 
                }

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
                //strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessage"];
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
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.DonorID MainDonorID,  ";
            strSQL += "TIMESTAMPDIFF(YEAR,t2.DateOfBirthDonor,t1.DateDonorOperation) AgeOperation, ";
            strSQL += "DATE_FORMAT(t1.DateDonorOperation, '%d/%m/%Y') Date_DonorOperation, ";
            strSQL += "DATE_FORMAT(t2.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            strSQL += "FROM donor_identification t1 ";
            strSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource2;
            SqlDataSource2.SelectCommand = strSQL;
            SqlDataSource2.SelectParameters.Clear();
            SqlDataSource2.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            

            if (GV1.Rows.Count == 1)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                lblGV1.Text = "Summary of Donor Details";                
            }
            else
            {
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
            }
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

    

    protected void AssignData()
    {
        try 
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  donor_identification t1 ";
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
                            //if (!DBNull.Value.Equals(myDr["DonorID"]))
                            //{
                            //    txtDonorID.Text = (string)(myDr["DonorID"]);
                            //}

                            //if (!DBNull.Value.Equals(myDr["DateOfBirth"]))
                            //{

                            //    if (GeneralRoutines.IsDate(myDr["DateOfBirth"].ToString()))
                            //   {
                            //        txtDonorDateOfBirth.Text = Convert.ToDateTime(myDr["DateOfBirth"]).ToString("dd/MM/yyyy");
                                    
                            //   }
                                

                            //}

                            //if (!DBNull.Value.Equals(myDr["DonorCentre"]))
                            //{
                            //    ddDonorCentre.SelectedValue = (string)(myDr["DonorCentre"]);
                            //}

                            if (!DBNull.Value.Equals(myDr["DateDonorAdmission"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateDonorAdmission"].ToString()) == true)
                                {
                                    txtDateDonorAdmission.Text = Convert.ToDateTime(myDr["DateDonorAdmission"]).ToShortDateString();
                                }
                            }

                            if (lblDateDonorAdmission.Font.Bold==true)
                            {
                                if (txtDateDonorAdmission.Text==string.Empty)
                                {
                                    lblDateDonorAdmission.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["DateDonorOperation"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DateDonorOperation"].ToString()) == true)
                                {
                                    txtDateDonorOperation.Text = Convert.ToDateTime(myDr["DateDonorOperation"]).ToShortDateString();
                                }
                            }

                            if (lblDateDonorOperation.Font.Bold == true)
                            {
                                if (txtDateDonorOperation.Text == string.Empty)
                                {
                                    lblDateDonorOperation.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Sex"]))
                            {
                                rblSex.SelectedValue = (string)(myDr["Sex"]);

                            }

                            if (lblSex.Font.Bold==true)
                            {
                                if (rblSex.SelectedIndex == -1)
                                {
                                    lblSex.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            
                            if (!DBNull.Value.Equals(myDr["Weight"]))
                            {
                                txtWeight.Text = (string)(myDr["Weight"]);
                            }

                            if (lblWeight.Font.Bold == true)
                            {
                                if (txtWeight.Text == string.Empty)
                                {
                                    lblWeight.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Height"]))
                            {
                                txtHeight.Text = (string)(myDr["Height"]);
                            }

                            if (lblHeight.Font.Bold == true)
                            {
                                if (txtHeight.Text == string.Empty)
                                {
                                    lblHeight.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["BloodGroup"]))
                            {
                                rblBloodGroup.SelectedValue = (string)(myDr["BloodGroup"]);
                            }

                            if (lblBloodGroup.Font.Bold==true)
                            {
                                if (rblBloodGroup.SelectedIndex==-1)
                                {
                                    lblBloodGroup.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            


                            if (!DBNull.Value.Equals(myDr["OtherOrgansDonated"]))
                            {
                                //lblCentre.Text += myDr["OtherOrgansDonated"].ToString();
                                string[] strSC_Sets = myDr["OtherOrgansDonated"].ToString().Split(',');

                                for (int i = 0; i <= strSC_Sets.GetUpperBound(0); i++)
                                {
                                    string[] strSC_Contents = strSC_Sets[i].ToString().Split(':');

                                    ListItem currentCheckBox = cblOtherOrgansDonated.Items.FindByValue(strSC_Contents[0].ToString());

                                    if (currentCheckBox != null)
                                    {
                                        if (strSC_Contents[1].ToString() == STR_YES_SELECTION)
                                        {
                                            currentCheckBox.Selected = true;
                                        }
                                        
                                    }

                                }
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

                lblUserMessages.Text = ex.Message + " An error occured while executing assign query. ";
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assiging data.";
        }
    }
    //reset page
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
            lblUserMessages.Text = String.Empty;

            if (Page.IsValid==false)
            {
                throw new Exception("Plese check the data you have added.");
            }
            //if (!rvDonorDateOfBirth.IsValid)
            //{
            //    rvDonorDateOfBirth.ErrorMessage = "The Date range should be between " + DateTime.Now.AddYears(-100).ToShortDateString() + 
            //        " and " + DateTime.Now.AddYears(-20).ToShortDateString();
            //}

            // valdiate data

            //if (ddDonorCentre.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'Donor Centre'.");
            //}

            

            //if (GeneralRoutines.IsDate(txtDateDonorOperation.Text) == false)
            //{
            //    throw new Exception("Please Enter Donor Operation Date in the correct format.");
            //}
            if (txtDateDonorAdmission.Text!=string.Empty)
            {
                if (Convert.ToDateTime(txtDateDonorAdmission.Text) > DateTime.Today)
                {
                    throw new Exception(lblDateDonorAdmission.Text + " cannot be later than Today's date.");
                }
            }

            if (txtDateDonorOperation.Text != string.Empty)
            {

                if (Convert.ToDateTime(txtDateDonorOperation.Text) > DateTime.Today)
                {
                    throw new Exception(lblDateDonorOperation.Text + " cannot be later than Today's date.");
                }

                if (GeneralRoutines.IsDate(txtDateDonorAdmission.Text)==true)
                {
                    if (Convert.ToDateTime(txtDateDonorAdmission.Text).Date > Convert.ToDateTime(txtDateDonorOperation.Text).Date)
                    {
                        throw new Exception(lblDateDonorOperation.Text + " cannot be ealier than " + lblDateDonorAdmission.Text + ".");
                    }
                }
            }

            ListItem li = cblOtherOrgansDonated.Items.FindByValue("None");

            if (li!=null)
            {
                if (li.Selected==true)
                {
                    throw new Exception("Since None has been selected/ticked for '" + lblOtherOrgansDonated.Text + " , please deselect/untick other options.");
                }
            }

            //if (rblSex.SelectedIndex == -1 || rblSex.SelectedValue == "Unknown")
            //{ throw new Exception("Please Select Sex."); }


            //if (!GeneralRoutines.IsNumeric(txtWeight.Text))
            //{ throw new Exception("Please Enter a numeric for weight."); }

            //if (!GeneralRoutines.IsNumeric(txtHeight.Text))
            //{ throw new Exception("Please Enter a numeric for Height."); }

            //if (txtWeight.Text != string.Empty || txtHeight.Text != string.Empty)
            //{
            //    Page.Validate("Secondary");

            //    if (Page.IsValid == false)
            //    {
            //        throw new Exception("Please check the data you have entered.");
            //    }
            //}


            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }


            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO donor_identification ";
            STRSQL += "(TrialID,  DateDonorAdmission, DateDonorOperation, Sex, Weight, Height, BMI, BloodGroup,  ";
            //STRSQL += "HLA_A, HLA_B, HLA_DR, KidneyLeftDonated, KidneyRightDonated, OtherOrgansDonated,";
            STRSQL += "OtherOrgansDonated,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID,  ?DateDonorAdmission, ?DateDonorOperation, ?Sex, ?Weight, ?Height, ?BMI, ?BloodGroup,  ";
            //STRSQL += "?HLA_A, ?HLA_B, ?HLA_DR, ?KidneyLeftDonated, ?KidneyRightDonated, ?OtherOrgansDonated,";
            STRSQL += "?OtherOrgansDonated,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = String.Empty;
            STRSQL_UPDATE += "UPDATE donor_identification SET ";
            STRSQL_UPDATE += "DateDonorAdmission=?DateDonorAdmission, DateDonorOperation=?DateDonorOperation,  ";
            STRSQL_UPDATE += "Sex=?Sex, Weight=?Weight, Height=?Height, BMI=?BMI, BloodGroup=?BloodGroup,";
            //STRSQL_UPDATE += "HLA_A=?HLA_A, HLA_B=?HLA_B, HLA_DR=?HLA_DR,";
            //STRSQL_UPDATE += "KidneyLeftDonated=?KidneyLeftDonated, KidneyRightDonated=?KidneyRightDonated, OtherOrgansDonated=?OtherOrgansDonated,";
            STRSQL_UPDATE += "OtherOrgansDonated=?OtherOrgansDonated,";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


            //lock data locked in every case
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE donor_identification SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE donor_identification SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_identification WHERE TrialID=?TrialID ";

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
                throw new Exception("More than One Donor Details Data exists for this TrialID. Please Delete an existing Donor Details Data.");
            }

            else
            { 
                throw new Exception("An error occured while checking if Donor Details already exist in the database."); 
            }
            

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            //MyCMD.Parameters.Add("?DonorID", MySqlDbType.VarChar).Value = txtDonorID.Text;

            //MyCMD.Parameters.Add("?DonorCentre", MySqlDbType.VarChar).Value = ddCountry.SelectedValue;

            //MyCMD.Parameters.Add("?DateOfBirth", MySqlDbType.Date).Value = Convert.ToDateTime(txtDonorDateOfBirth.Text);

            //if (ddDonorCentre.SelectedValue==STR_UNKNOWN_SELECTION)
            //{
            //    MyCMD.Parameters.Add("?DonorCentre", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?DonorCentre", MySqlDbType.VarChar).Value = ddDonorCentre.SelectedValue;
            //}

            if (GeneralRoutines.IsDate(txtDateDonorAdmission.Text))
            {
                MyCMD.Parameters.Add("?DateDonorAdmission", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateDonorAdmission.Text);
            }
            else
            {
                MyCMD.Parameters.Add("?DateDonorAdmission", MySqlDbType.Date).Value =DBNull.Value; 
            }

            if (GeneralRoutines.IsDate(txtDateDonorOperation.Text))
            {
                MyCMD.Parameters.Add("?DateDonorOperation", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateDonorOperation.Text);
            }
            else
            {
                MyCMD.Parameters.Add("?DateDonorOperation", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (rblSex.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?Sex", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Sex", MySqlDbType.VarChar).Value = rblSex.SelectedValue;
            }
            

            if (txtWeight.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Weight", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else 
            {
                MyCMD.Parameters.Add("?Weight", MySqlDbType.VarChar).Value = txtWeight.Text;
            }

            if (txtHeight.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Height", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Height", MySqlDbType.VarChar).Value = txtHeight.Text;
            }

            if (GeneralRoutines.IsNumeric(txtHeight.Text) && GeneralRoutines.IsNumeric(txtHeight.Text))
            { MyCMD.Parameters.Add("?BMI", MySqlDbType.VarChar).Value = Math.Round(Convert.ToDouble(txtWeight.Text) / (Convert.ToDouble(txtHeight.Text) / 100 * Convert.ToDouble(txtHeight.Text) / 100), 3); }
            else
            { MyCMD.Parameters.Add("?BMI", MySqlDbType.VarChar).Value = DBNull.Value; }

            
            MyCMD.Parameters.Add("?BloodGroup", MySqlDbType.VarChar).Value = rblBloodGroup.SelectedValue;


            

            //append selection
            string strOtherOrgansDonated = string.Empty;
            //Set up connection and command objects
            //Open connection
            for (int i = 0; i < cblOtherOrgansDonated.Items.Count; i++)
            {
                strOtherOrgansDonated += cblOtherOrgansDonated.Items[i].Value + ":";
                if (cblOtherOrgansDonated.Items[i].Selected)
                {
                    strOtherOrgansDonated += STR_YES_SELECTION;
                }
                else
                {
                    strOtherOrgansDonated += STR_NO_SELECTION;
                }

                if (i < cblOtherOrgansDonated.Items.Count - 1)
                {
                    strOtherOrgansDonated += ",";
                }
            }

            if (strOtherOrgansDonated == string.Empty)
            {
                MyCMD.Parameters.Add("?OtherOrgansDonated", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            {
                MyCMD.Parameters.Add("?OtherOrgansDonated", MySqlDbType.VarChar).Value = strOtherOrgansDonated;
            }
                        
            if (string.IsNullOrEmpty(txtComments.Text))
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text; }

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
                {
                    MyCONN.Close();
                }


                //redirect to summary page if data complete is not no
                string strSQLCOMPLETE = string.Empty;

                strSQLCOMPLETE += "SELECT  ";
                strSQLCOMPLETE += "IF(t2.DateDonorAdmission IS NOT NULL AND t2.DateDonorOperation IS NOT NULL AND t2.Sex IS NOT NULL  ";
                strSQLCOMPLETE += "AND t2.Weight IS NOT NULL AND t2.Height IS NOT NULL ";
                strSQLCOMPLETE += " ";
                strSQLCOMPLETE += ",'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM donor_identification t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialID=?TrialID ";


                string strComplete = GeneralRoutines.ReturnScalar(strSQLCOMPLETE, "?TrialID", Request.QueryString["TID"], STRCONN);

                //redirect to summary page
                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
                }
                else
                {
                    if (Request.Url.AbsoluteUri.Contains("SCode=1"))
                    {
                        Response.Redirect(Request.Url.AbsoluteUri, false);
                    }
                    else
                    {
                        Response.Redirect(Request.Url.AbsoluteUri + "&SCode=1", false);
                    }

                    lblUserMessages.Text = "Data Submitted";
                }

                //if (chkAllDataAdded.Checked == true || chkDataLocked.Checked==true || chkDataFinal.Checked==true)
                //{
                //    Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);
                //}
                //else
                //{
                //    Response.Redirect(Request.Url.AbsoluteUri, false);
                //}

                BindData();
                //lblUserMessages.Text = "Data Added/Updated";
            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }
            
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
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
                    throw new Exception("More than one Record exists for deletion. Click on DonorID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM donor_identification ";
            STRSQL += "WHERE TrialID=?TrialID ";
            if (intCountFind > 1)
            {
                STRSQL += "AND DonorIdentificationID=?DonorIdentificationID ";
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

            if (!String.IsNullOrEmpty(Request.QueryString["DonorIdentificationID"]))
            {
                MyCMD.Parameters.Add("?DonorIdentificationID", MySqlDbType.VarChar).Value = Request.QueryString["DonorIdentificationID"].ToString();
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

    protected void cblOtherOrgansDonated_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem li = cblOtherOrgansDonated.Items.FindByValue("None");

            if (li != null)
            {
                if (li.Selected == true)
                {
                    cblOtherOrgansDonated.SelectedIndex = -1;
                    li.Selected = true;
                }
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting options in " + lblOtherOrgansDonated.Text + ". ";
        }
    }
}