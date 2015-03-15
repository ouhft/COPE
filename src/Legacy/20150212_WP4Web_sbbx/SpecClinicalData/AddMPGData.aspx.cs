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


public partial class SpecClinicalData_AddMPGData : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string strHMPCode = "B"; //with oxygen

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx?EID=51";

        private const string strDefaultFontColor = "Black";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";

    //static Random _random = new Random();

    #endregion

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["TID"].ToString() == String.Empty)
                {
                    throw new Exception("Could not obtain TrialID.");
                }

                //ViewState["RanCategory"] = string.Empty; //should be B i.e. HMP with oxygen
                //ViewState["KidneySuitableTransplant"] = string.Empty; //should be YES

                ////check if user has access to randomisation data
                //if (string.IsNullOrEmpty(SessionVariablesAll.Randomise))
                //{
                //    Response.Redirect(strAccessDenied + "&TID=" + Request.QueryString["TID"], false);
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //}

                //if (SessionVariablesAll.Randomise != STR_YES_SELECTION)
                //{
                //    Response.Redirect(strAccessDenied + "&TID=" + Request.QueryString["TID"], false);
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //}

                //pnlKidneyOnMachineSelected.Visible = false;


                //hide field
                if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
                {

                    pnlValuepO2Perfusate.Visible = true;
                    lblValuepO2Perfusate.Visible = true;
                    txtValuepO2Perfusate.Visible = true;

                    pnlOxygenBottleFullOpen.Visible = true;
                }
                else
                {
                    pnlValuepO2Perfusate.Visible = false;
                    lblValuepO2Perfusate.Visible = false;
                    txtValuepO2Perfusate.Visible = false;

                    pnlOxygenBottleFullOpen.Visible = false;
                }
                pnlKidneyOnMachineSelectedNo.Visible = false;

                pnlKidneyOnMachineSelectedYes.Visible = false;

                lblDescription.Text = "Add Machine Perfusion Data for " + Request.QueryString["TID"];


                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                ddSide.DataSource = XMLKidneySidesDataSource;
                ddSide.DataBind();

                ListItem li = ddSide.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (li!=null)
                {
                    ddSide.Items.Remove(li);
                }
                rblKidneyOnMachine.DataSource = XMLMainOptionsYNDataSource;
                rblKidneyOnMachine.DataBind();
                //rblKidneyOnMachine.SelectedValue = STR_UNKNOWN_SELECTION;

                


                txtPerfusionStartDate_CalendarExtender.EndDate = DateTime.Today;

                ddUsedPatchHolder.DataSource = XMLUsedPatchHolderSizesDataSource;
                ddUsedPatchHolder.DataBind();

                ddArtificialPatchUsed.DataSource = XMLMainOptionsYNDataSource;
                ddArtificialPatchUsed.DataBind();

                ddArtificialPatchSize.DataSource = XMLArtificialPatchSizesDataSource;
                ddArtificialPatchSize.DataBind();

                ddArtificialPatchNumber.DataSource = XMLArtificialPatchNumbersDataSource;
                ddArtificialPatchNumber.DataBind();

                ddOxygenBottleFull.DataSource = XMLMainOptionsYNDataSource;
                ddOxygenBottleFull.DataBind();

                ddOxygenBottleOpened.DataSource = XMLMainOptionsYNDataSource;
                ddOxygenBottleOpened.DataBind();

                ddOxygenTankChanged.DataSource = XMLMainOptionsYNDataSource;
                ddOxygenTankChanged.DataBind();

                txtOxygenTankChangedDate_CalendarExtender.EndDate = DateTime.Today;


                ddIceContainerReplenished.DataSource = XMLMainOptionsYNDataSource;
                ddIceContainerReplenished.DataBind();

                txtIceContainerReplenishedDate_CalendarExtender.EndDate = DateTime.Today;


                ddLogisticallyPossibleMeasurepO2Perfusate.DataSource = XMLMainOptionsYNDataSource;
                ddLogisticallyPossibleMeasurepO2Perfusate.DataBind();


                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

                //loop though rows to highlight selected occasion
                ddSide.SelectedIndex = -1;

                if (string.IsNullOrEmpty(Request.QueryString["Side"]) == false)
                {
                    ListItem liSide = ddSide.Items.FindByValue(Request.QueryString["Side"]);

                    if (liSide != null)
                    {
                        liSide.Selected = true;

                        ddSide_SelectedIndexChanged(this, EventArgs.Empty);
                    }


                }

                //if (ddSide.SelectedValue != STR_DD_UNKNOWN_SELECTION)
                //{
                //    Label lbl;

                //    foreach (GridViewRow row in GV1.Rows)
                //    {
                //        lbl = (Label)row.FindControl("lblSide");

                //        if (lbl != null)
                //        {
                //            if (row.RowType == DataControlRowType.DataRow)
                //            {
                //                if (lbl.Text == ddSide.SelectedValue)
                //                {
                //                    row.BackColor = System.Drawing.Color.LightBlue;
                //                }
                //            }
                //        }
                //    }

                //    AssignData();
                //}

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
                        //cmdUpdate.Enabled = true;
                        cmdAddData.Enabled = true;
                        cmdDelete.Enabled = true;
                        cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        //cmdUpdate.Enabled = false;
                        cmdAddData.Enabled = false;
                        cmdDelete.Enabled = false;
                        cmdReset.Enabled = false;

                    }
                }

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConstantsGeneral.MandatoryMessage;
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

                //data final checkbox visible
                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    pnlFinal.Visible = true;
                    //cmdAddData.Enabled = true;
                    //cmdDelete.Enabled = true;
                    //cmdReset.Enabled = true;
                }
                else
                {
                    pnlFinal.Visible = false;
                    //cmdAddData.Enabled = false;
                    //cmdDelete.Enabled = false;
                    //cmdReset.Enabled = false;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["SCode"]))
                {
                    if (Request.QueryString["SCode"].ToString() == "1")
                    {
                        string strSCode1Message = string.Empty;
                        strSCode1Message = ConstantsGeneral.SCode1;
                        lblUserMessages.Text = strSCode1Message;
                    }
                }


                hlkSummaryPage.NavigateUrl = strRedirectSummary + Request.QueryString["TID"];

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
            strSQL += "DATE_FORMAT(t1.PerfusionStartDate, '%d/%m/%Y') PerfusionStart_Date, ";
            strSQL += "TIME_FORMAT(t1.PerfusionStartTime, '%H:%i') PerfusionStart_Time ";
            strSQL += "FROM machineperfusion t1 ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();

            lblGV1.Text = "Summary of  Machine Perfusion Data for " + Request.QueryString["TID"].ToString() + " ";
            //if (GV1.Rows.Count == 1)
            //{
            //    cmdDelete.Enabled = true;
            //    //cmdDelete.Visible = true;
            //    //cmdAddData.Text = "Update Data";
            //    lblDescription.Text = "Update  Machine Perfusion Data for " + Request.QueryString["TID"].ToString() + " ";
            //    lblGV1.Text = "Summary of  Machine Perfusion Data for " + Request.QueryString["TID"].ToString() + " ";
            //    AssignData();

            //}
            //else if (GV1.Rows.Count == 0)
            //{
            //    cmdDelete.Enabled = false;
            //    //cmdDelete.Visible = false;
            //    //cmdAddData.Text = "Add Data";
            //    lblDescription.Text = "Add  Machine Perfusion Data for " + Request.QueryString["TID"].ToString() + " ";
            //}
            //else
            //{
            //    throw new Exception("More than one Records exist.");
            //}

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

    // Reset Page
    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;


            string STRSQL = string.Empty;

            STRSQL += "SELECT t1.*, t2.DonorID Donor FROM  machineperfusion t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID AND Side=?Side ";
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
            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;
            MyCONN.Open();


            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        string strIncompleteColour = ConstantsGeneral.IncompleteColour;
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["KidneyOnMachine"]))
                            {
                                rblKidneyOnMachine.SelectedValue = (string)(myDr["KidneyOnMachine"]);
                            }

                            if (lblKidneyOnMachine.Font.Bold == true)
                            {
                                if (rblKidneyOnMachine.SelectedIndex==-1)
                                {
                                    lblKidneyOnMachine.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["KidneyOnMachineNo"]))
                            {
                                txtKidneyOnMachineNo.Text = (string)(myDr["KidneyOnMachineNo"]);
                            }

                            if (lblKidneyOnMachineNo.Font.Bold == true)
                            {
                                if (txtKidneyOnMachineNo.Text == string.Empty)
                                {
                                    lblKidneyOnMachineNo.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PerfusionStartDate"]))
                            {

                                if (GeneralRoutines.IsDate(myDr["PerfusionStartDate"].ToString()))
                                {
                                    txtPerfusionStartDate.Text = Convert.ToDateTime(myDr["PerfusionStartDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PerfusionStartTime"]))
                            {

                                if (myDr["PerfusionStartTime"].ToString().Length >= 5)
                                {
                                    txtPerfusionStartTime.Text = myDr["PerfusionStartTime"].ToString().Substring(0, 5);
                                }
                            }


                            if (lblPerfusionStartDate.Font.Bold == true)
                            {
                                if (txtPerfusionStartDate.Text == string.Empty || txtPerfusionStartTime.Text == string.Empty)
                                {
                                    lblPerfusionStartDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            //if (rblKidneyOnMachine.SelectedValue == STR_NO_SELECTION)
                            //{
                            //    pnlKidneyOnMachineSelected.Visible = true;

                            //    pnlKidneyOnMachineSelectedNo.Visible = true;

                            //    pnlKidneyOnMachineSelectedYes.Visible = false;
                            //}

                            //else if (rblKidneyOnMachine.SelectedValue == STR_YES_SELECTION)
                            //{
                            //    pnlKidneyOnMachineSelected.Visible = true;

                            //    pnlKidneyOnMachineSelectedNo.Visible = false;

                            //    pnlKidneyOnMachineSelectedYes.Visible = true;
                            //}

                            //else
                            //{
                            //    pnlKidneyOnMachineSelected.Visible = false;

                            //    pnlKidneyOnMachineSelectedNo.Visible = false;

                            //    pnlKidneyOnMachineSelectedYes.Visible = false;
                            //}

                            if (!DBNull.Value.Equals(myDr["MachineSerialNumber"]))
                            {
                                txtMachineSerialNumber.Text = (string)(myDr["MachineSerialNumber"]);
                            }

                            if (lblMachineSerialNumber.Font.Bold == true)
                            {
                                if (txtMachineSerialNumber.Text == string.Empty)
                                {
                                    lblMachineSerialNumber.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["MachineReferenceModelNumber"]))
                            {
                                txtMachineReferenceModelNumber.Text = (string)(myDr["MachineReferenceModelNumber"]);
                            }

                            if (lblMachineReferenceModelNumber.Font.Bold == true)
                            {
                                if (txtMachineReferenceModelNumber.Text == string.Empty)
                                {
                                    lblMachineReferenceModelNumber.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["LotNumberDisposables"]))
                            {
                                txtLotNumberDisposables.Text = (string)(myDr["LotNumberDisposables"]);
                            }

                            if (lblLotNumberDisposables.Font.Bold == true)
                            {
                                if (txtLotNumberDisposables.Text == string.Empty)
                                {
                                    lblLotNumberDisposables.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["LotNumberPerfusionSolution"]))
                            {
                                txtLotNumberPerfusionSolution.Text = (string)(myDr["LotNumberPerfusionSolution"]);
                            }

                            if (lblLotNumberPerfusionSolution.Font.Bold == true)
                            {
                                if (txtLotNumberPerfusionSolution.Text == string.Empty)
                                {
                                    lblLotNumberPerfusionSolution.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["UsedPatchHolder"]))
                            {
                                ddUsedPatchHolder.SelectedValue = (string)(myDr["UsedPatchHolder"]);
                            }

                            if (lblUsedPatchHolder.Font.Bold == true)
                            {
                                if (ddUsedPatchHolder.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblUsedPatchHolder.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArtificialPatchUsed"]))
                            {
                                ddArtificialPatchUsed.SelectedValue = (string)(myDr["ArtificialPatchUsed"]);
                            }

                            if (lblArtificialPatchUsed.Font.Bold == true)
                            {
                                if (ddArtificialPatchUsed.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblArtificialPatchUsed.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["ArtificialPatchSize"]))
                            {
                                ddArtificialPatchSize.SelectedValue = (string)(myDr["ArtificialPatchSize"]);
                            }

                            if (lblArtificialPatchSize.Font.Bold == true)
                            {
                                if (ddArtificialPatchSize.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblArtificialPatchSize.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArtificialPatchNumber"]))
                            {
                                ddArtificialPatchNumber.SelectedValue = (string)(myDr["ArtificialPatchNumber"]);
                            }

                            if (lblArtificialPatchNumber.Font.Bold == true)
                            {
                                if (ddArtificialPatchNumber.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblArtificialPatchNumber.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
                            {
                                if (!DBNull.Value.Equals(myDr["OxygenBottleFull"]))
                                {
                                    ddOxygenBottleFull.SelectedValue = (string)(myDr["OxygenBottleFull"]);
                                }

                                if (lblOxygenBottleFull.Font.Bold == true)
                                {
                                    if (ddOxygenBottleFull.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                    {
                                        lblOxygenBottleFull.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                    }
                                }

                                if (!DBNull.Value.Equals(myDr["OxygenBottleOpened"]))
                                {
                                    ddOxygenBottleOpened.SelectedValue = (string)(myDr["OxygenBottleOpened"]);
                                }

                                if (lblOxygenBottleOpened.Font.Bold == true)
                                {
                                    if (ddOxygenBottleOpened.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                    {
                                        lblOxygenBottleOpened.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                    }
                                }

                            }
                            


                            if (!DBNull.Value.Equals(myDr["OxygenTankChanged"]))
                            {
                                ddOxygenTankChanged.SelectedValue = (string)(myDr["OxygenTankChanged"]);
                            }

                            if (lblOxygenTankChanged.Font.Bold==true)
                            {
                                if (ddOxygenTankChanged.SelectedValue==STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblOxygenTankChanged.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["OxygenTankChangedDate"]))
                            {

                                if (GeneralRoutines.IsDate(myDr["OxygenTankChangedDate"].ToString()))
                                {
                                    txtOxygenTankChangedDate.Text = Convert.ToDateTime(myDr["OxygenTankChangedDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["OxygenTankChangedTime"]))
                            {

                                if (myDr["OxygenTankChangedTime"].ToString().Length >= 5)
                                {
                                    txtOxygenTankChangedTime.Text = myDr["OxygenTankChangedTime"].ToString().Substring(0, 5);
                                }
                            }


                            if (lblOxygenTankChangedDateTime.Font.Bold == true)
                            {
                                if (txtOxygenTankChangedDate.Text == string.Empty || txtOxygenTankChangedTime.Text == string.Empty)
                                {
                                    lblOxygenTankChangedDateTime.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["IceContainerReplenished"]))
                            {
                                ddIceContainerReplenished.SelectedValue = (string)(myDr["IceContainerReplenished"]);
                            }

                            if (lblIceContainerReplenished.Font.Bold==true)
                            {
                                if (ddIceContainerReplenished.SelectedValue==STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblIceContainerReplenished.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["IceContainerReplenishedDate"]))
                            {

                                if (GeneralRoutines.IsDate(myDr["IceContainerReplenishedDate"].ToString()))
                                {
                                    txtIceContainerReplenishedDate.Text = Convert.ToDateTime(myDr["IceContainerReplenishedDate"]).ToString("dd/MM/yyyy");
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["IceContainerReplenishedTime"]))
                            {

                                if (myDr["IceContainerReplenishedTime"].ToString().Length >= 5)
                                {
                                    txtIceContainerReplenishedTime.Text = myDr["IceContainerReplenishedTime"].ToString().Substring(0, 5);
                                }
                            }


                            if (lblIceContainerReplenishedDateTime.Font.Bold == true)
                            {
                                if (txtIceContainerReplenishedDate.Text == string.Empty || txtIceContainerReplenishedTime.Text == string.Empty)
                                {
                                    lblIceContainerReplenishedDateTime.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["LogisticallyPossibleMeasurepO2Perfusate"]))
                            {
                                ddLogisticallyPossibleMeasurepO2Perfusate.SelectedValue = (string)(myDr["LogisticallyPossibleMeasurepO2Perfusate"]);
                            }

                            if (lblLogisticallyPossibleMeasurepO2Perfusate.Font.Bold == true)
                            {
                                if (ddLogisticallyPossibleMeasurepO2Perfusate.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                                {
                                    lblLogisticallyPossibleMeasurepO2Perfusate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            
                            if (SessionVariablesAll.ViewRandomise==STR_YES_SELECTION)
                            {
                                if (!DBNull.Value.Equals(myDr["ValuepO2Perfusate"]))
                                {
                                    txtValuepO2Perfusate.Text = (string)(myDr["ValuepO2Perfusate"]);
                                }

                                if (lblValuepO2Perfusate.Font.Bold == true)
                                {
                                    if (txtValuepO2Perfusate.Text == string.Empty)
                                    {
                                        lblValuepO2Perfusate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                    }
                                }
                            }

                            

                            if (!DBNull.Value.Equals(myDr["ValuepO2PerfusateMeasured"]))
                            {
                                txtValuepO2PerfusateMeasured.Text = (string)(myDr["ValuepO2PerfusateMeasured"]);
                            }

                            if (lblValuepO2PerfusateMeasured.Font.Bold == true)
                            {
                                if (txtValuepO2PerfusateMeasured.Text == string.Empty)
                                {
                                    lblValuepO2PerfusateMeasured.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
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

                            rblKidneyOnMachine_SelectedIndexChanged(this, EventArgs.Empty);
                            ddArtificialPatchUsed_SelectedIndexChanged(this, EventArgs.Empty);
                            ddOxygenTankChanged_SelectedIndexChanged(this, EventArgs.Empty);
                            ddIceContainerReplenished_SelectedIndexChanged(this, EventArgs.Empty);
                            ddLogisticallyPossibleMeasurepO2Perfusate_SelectedIndexChanged(this, EventArgs.Empty);

                        }

                        cmdDelete.Visible = true;

                        lblDescription.Text = "Update Machine Perfusion Data for " + Request.QueryString["TID"] + " (" + ddSide.SelectedValue + ")";
                    }
                    else
                    {
                        rblKidneyOnMachine.SelectedValue = STR_NO_SELECTION;

                        rblKidneyOnMachine_SelectedIndexChanged(this, EventArgs.Empty);

                        pnlKidneyOnMachineSelectedNo.Visible = false;

                        rblKidneyOnMachine.SelectedIndex = -1;

                        cmdDelete.Visible = false;

                        lblDescription.Text = "Add Machine Perfusion Data for " + Request.QueryString["TID"] + " (" + ddSide.SelectedValue + ")";
                    }

                    

                }

                

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                Label lblSide;

                foreach (GridViewRow row in GV1.Rows)
                {
                    lblSide = (Label)row.FindControl("lblSide");

                    if (lblSide != null)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (lblSide.Text == ddSide.SelectedValue)
                            {
                                row.BackColor = System.Drawing.Color.LightBlue;
                            }
                            else
                            {
                                if (row.RowState == DataControlRowState.Alternate)
                                {
                                    row.BackColor = GV1.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    row.BackColor = GV1.RowStyle.BackColor;
                                }


                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing Assign Query.";
            }
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
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

            if (ddSide.SelectedIndex == -1 || ddSide.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select Kidney Side.");
            }
            if (rblKidneyOnMachine.SelectedValue == STR_NO_SELECTION)
            {
                Page.Validate("KidneyOnMachineNO");
            }

            if (rblKidneyOnMachine.SelectedValue==STR_YES_SELECTION)
            {
                Page.Validate("KidneyOnMachineYes");
            }

            if (Page.IsValid==false)
            {
                throw new Exception("Please Check the data you have entered.");
            }

            //DateTime dteBaggedColdStorage = DateTime.MinValue;
            DateTime dteArrivalTransplantCentre = DateTime.MinValue;
            DateTime dtePerfusionStart = DateTime.MinValue;
            DateTime dteOxygenTankChanged = DateTime.MinValue;
            DateTime dteIceContainerReplenished=DateTime.MinValue;

            if (rblKidneyOnMachine.SelectedValue == STR_UNKNOWN_SELECTION)
            { 
                throw new Exception("Please Select if Kidney can be placed on Machine."); 
            }

            if (rblKidneyOnMachine.SelectedValue == STR_YES_SELECTION)
            {
                


                ////get datetime arrival Transplant Centre

                //string STRSQL_DATEARRIVAL = "SELECT DateArrivalTransplantCentre FROM kidneyinspection WHERE TrialID=?TrialID ";
                //string stDateArrivalTransplantCentre = GeneralRoutines.ReturnScalar(STRSQL_DATEARRIVAL, "?TrialID", Request.QueryString["TID"], STRCONN);

                //if (GeneralRoutines.IsDate(stDateArrivalTransplantCentre) == false)
                //{
                //    throw new Exception("Please Enter Date Arrival of Kidney at the Transplant Centre in the 'Inspection' page. ");
                //}


                //string STRSQL_TIMEARRIVAL = "SELECT TimeArrivalTransplantCentre FROM kidneyinspection WHERE TrialID=?TrialID ";
                //string strTimeArrivalTransplantCentre = GeneralRoutines.ReturnScalar(STRSQL_TIMEARRIVAL, "?TrialID", Request.QueryString["TID"], STRCONN);

                //if (GeneralRoutines.IsDate(strTimeArrivalTransplantCentre) == false)
                //{
                //    throw new Exception("Please Enter Time Arrival of Kidney at the Transplant Centre in the 'Inspection' page. ");
                //}

                //if (GeneralRoutines.IsDate(stDateArrivalTransplantCentre.Substring(0, 10) + " " + strTimeArrivalTransplantCentre) == true)
                //{
                //    dteArrivalTransplantCentre = Convert.ToDateTime(stDateArrivalTransplantCentre.Substring(0, 10) + " " + strTimeArrivalTransplantCentre);
                //}
                //else
                //{
                //    throw new Exception("Please Enter DateTime Arrival of Kidney at the Transplant Centre in the 'Inspection' page. ");
                //}


                string STRSQL_DATECREATED = "SELECT DATECREATED FROM trialdetails WHERE TrialID=?TrialID ";
                DateTime dteDateCreated = DateTime.MinValue;

                if (GeneralRoutines.IsDate(GeneralRoutines.ReturnScalar(STRSQL_DATECREATED, "?TrialID", Request.QueryString["TID"], STRCONN)) == true)
                {
                    dteDateCreated = Convert.ToDateTime(GeneralRoutines.ReturnScalar(STRSQL_DATECREATED, "?TrialID", Request.QueryString["TID"], STRCONN));
                }

                string STRSQL_RANDOMISED = "SELECT DateCreated FROM kidneyr WHERE TrialID=?TrialID ";
                DateTime dteDateRandomised = DateTime.MinValue;

                if (GeneralRoutines.IsDate(GeneralRoutines.ReturnScalar(STRSQL_RANDOMISED, "?TrialID", Request.QueryString["TID"], STRCONN)) == true)
                {
                    dteDateRandomised = Convert.ToDateTime(GeneralRoutines.ReturnScalar(STRSQL_DATECREATED, "?TrialID", Request.QueryString["TID"], STRCONN));
                }

                if (GeneralRoutines.IsDate(txtPerfusionStartDate.Text) == true)
                {

                    //if (dteBaggedColdStorage != DateTime.MinValue)
                    //{
                    //    if (Convert.ToDateTime(txtPerfusionStartDate.Text).Date < dteBaggedColdStorage.Date)
                    //    {
                    //        throw new Exception("" + lblPerfusionStartDate.Text + " cannot be earlier than the Date " + dteBaggedColdStorage.ToShortDateString() + " when Kidney was bagged for cold storage.");
                    //    }
                    //}

                    //if (dteArrivalTransplantCentre != DateTime.MinValue)
                    //{
                    //    if (Convert.ToDateTime(txtPerfusionStartDate.Text).Date < dteArrivalTransplantCentre.Date)
                    //    {
                    //        throw new Exception("" + lblPerfusionStartDate.Text + " cannot be earlier than the Date " + dteArrivalTransplantCentre.ToShortDateString() + " when Kindey Arrived at the Transplant Centre.");
                    //    }
                    //}

                    if (dteDateCreated != DateTime.MinValue)
                    {
                        if (Convert.ToDateTime(txtPerfusionStartDate.Text).Date < dteDateCreated.Date)
                        {
                            throw new Exception("" + lblPerfusionStartDate.Text + " cannot be earlier than the Date " + dteDateCreated.ToShortDateString() + " when TrialID was created.");
                        }
                    }


                    if (dteDateRandomised != DateTime.MinValue)
                    {
                        if (Convert.ToDateTime(txtPerfusionStartDate.Text).Date < dteDateRandomised.Date)
                        {
                            throw new Exception("" + lblPerfusionStartDate.Text + " cannot be earlier than the Date " + dteDateRandomised.ToShortDateString() + " when kidneys from this TrialID were randomised.");
                        }
                    }

                    if (Convert.ToDateTime(txtPerfusionStartDate.Text).Date > DateTime.Now.Date)
                    {
                        throw new Exception("Perfusion Start Date cannot be later than Today's date.");
                    }


                }
                else
                {
                    throw new Exception("Please enter Perfusion Start Date.");
                }

                if (txtPerfusionStartTime.Text == string.Empty || txtPerfusionStartTime.Text =="__:__")
                {
                    throw new Exception("Please enter the Time of Perfusion Start. ");
                }
                else
                {
                    if (GeneralRoutines.IsNumeric(txtPerfusionStartTime.Text.Substring(0, 2)) == false)
                    {
                        throw new Exception("Time Hour of Perfusion Start should be numeric.");
                    }

                    if (Convert.ToInt16(txtPerfusionStartTime.Text.Substring(0, 2)) > 23)
                    {
                        throw new Exception("Time Hour of Perfusion Start should not be greater than 23.");
                    }

                    if (GeneralRoutines.IsNumeric(txtPerfusionStartTime.Text.Substring(3, 2)) == false)
                    {
                        throw new Exception("Time Minute of Perfusion Start should be numeric.");
                    }

                    if (Convert.ToInt16(txtPerfusionStartTime.Text.Substring(3, 2)) > 59)
                    {
                        throw new Exception("Time Minute of Perfusion Start cannot be greater than 59.");
                    }


                    dtePerfusionStart = Convert.ToDateTime(txtPerfusionStartDate.Text + " " + txtPerfusionStartTime.Text);


                    //if (dteBaggedColdStorage != DateTime.MinValue)
                    //{
                    //    if (dtePerfusionStart < dteBaggedColdStorage)
                    //    {
                    //        throw new Exception("" + lblPerfusionStartDate.Text + " cannot be earlier than the Date " + dteBaggedColdStorage.ToString() + " when Kidney was bagged for cold storage.");
                    //    }
                    //}

                    //if (dteArrivalTransplantCentre != DateTime.MinValue)
                    //{
                    //    if (dtePerfusionStart < dteArrivalTransplantCentre)
                    //    {
                    //        throw new Exception("" + lblPerfusionStartDate.Text + " cannot be earlier than the Date " + dteArrivalTransplantCentre.ToString() + " when Kidney Arrived at the Transplant Centre.");
                    //    }
                    //}

                    //if (dtePerfusionStart > DateTime.Now.AddHours(1))
                    //{
                    //    throw new Exception("Perfusion Start Date cannot be later than current date/time.");
                    //}

                    if (dtePerfusionStart > DateTime.Now)
                    {
                        throw new Exception("Perfusion Start Date/Time cannot be later than current date/time.");
                    }


                    if (dteDateCreated != DateTime.MinValue)
                    {
                        if (dtePerfusionStart < dteDateCreated)
                        {
                            throw new Exception("Date/Time for " + lblPerfusionStartDate.Text + " cannot be earlier than the Date Time " + dteDateCreated.ToString() + " when this TrialID was created.");
                        }
                    }

                    if (dteDateRandomised != DateTime.MinValue)
                    {
                        if (dtePerfusionStart < dteDateRandomised.Date)
                        {
                            throw new Exception("Date/Time for " + lblPerfusionStartDate.Text + " cannot be earlier than the Date Time " + dteDateRandomised.ToShortDateString() + " when kidneys from this TrialID were randomised.");
                        }
                    }
                    
                }


                if (txtOxygenTankChangedDate.Text!=string.Empty)
                {
                    if (GeneralRoutines.IsDate(txtOxygenTankChangedDate.Text) == false)
                    {
                        throw new Exception("Please enter '" + lblOxygenTankChangedDateTime.Text + "' Date as DD/MM/YYYY.");
                    }

                    dteOxygenTankChanged = Convert.ToDateTime(txtOxygenTankChangedDate.Text);

                    if (dteOxygenTankChanged.Date> DateTime.Now.Date)
                    {
                        throw new Exception("'" + lblOxygenTankChangedDateTime.Text + "' Date cannot be later than current date.");
                    }
                    
                    if (dtePerfusionStart !=DateTime.MinValue)
                    {
                        if (dteOxygenTankChanged.Date < dtePerfusionStart.Date)
                        {
                            throw new Exception("Date for '" + lblOxygenTankChangedDateTime.Text + "' cannot be earlier than " + lblPerfusionStartDate.Text + "' date. ");
                        }
                    }
                }

                if (txtOxygenTankChangedTime.Text != string.Empty && txtOxygenTankChangedTime.Text != "__:__")
                {
                    if (GeneralRoutines.IsNumeric(txtOxygenTankChangedTime.Text.Substring(0, 2)) == false)
                    {
                        throw new Exception("Time Hour of " + lblOxygenTankChangedDateTime.Text + " should be numeric.");
                    }

                    if (Convert.ToInt16(txtOxygenTankChangedTime.Text.Substring(0, 2)) > 23)
                    {
                        throw new Exception("Time Hour of " + lblOxygenTankChangedDateTime.Text + " should not be greater than 23.");
                    }

                    if (GeneralRoutines.IsNumeric(txtOxygenTankChangedTime.Text.Substring(3, 2)) == false)
                    {
                        throw new Exception("Time Minute of " + lblOxygenTankChangedDateTime.Text + " should be numeric.");
                    }

                    if (Convert.ToInt16(txtOxygenTankChangedTime.Text.Substring(3, 2)) > 59)
                    {
                        throw new Exception("Time Minute of " + lblOxygenTankChangedDateTime.Text + " cannot be greater than 59.");
                    }


                    dteOxygenTankChanged = Convert.ToDateTime(txtOxygenTankChangedDate.Text + " " + txtOxygenTankChangedTime.Text);

                    if (dteOxygenTankChanged > DateTime.Now)
                    {
                        throw new Exception("Date/Time '" + lblOxygenTankChangedDateTime.Text + "' cannot be later than current Date/Time.");
                    }

                    if (dtePerfusionStart != DateTime.MinValue)
                    {
                        if (dteOxygenTankChanged < dtePerfusionStart)
                        {
                            throw new Exception("Date/Time for '" + lblOxygenTankChangedDateTime.Text + "' cannot be earlier than " + lblPerfusionStartDate.Text + "'. ");
                        }
                    }

                }


                if (txtIceContainerReplenishedDate.Text!=string.Empty)
                {
                    if (GeneralRoutines.IsDate(txtIceContainerReplenishedDate.Text) == false)
                    {
                        throw new Exception("Please enter '" + lblIceContainerReplenishedDateTime.Text + "' Date as DD/MM/YYYY.");
                    }

                    dteIceContainerReplenished = Convert.ToDateTime(txtIceContainerReplenishedDate.Text);

                    if (dteIceContainerReplenished.Date> DateTime.Now.Date)
                    {
                        throw new Exception("'" + lblIceContainerReplenishedDateTime.Text + "' Date cannot be later than current date.");
                    }
                    
                    if (dtePerfusionStart !=DateTime.MinValue)
                    {
                        if (dteIceContainerReplenished.Date < dtePerfusionStart.Date)
                        {
                            throw new Exception("Date for '" + lblIceContainerReplenishedDateTime.Text + "' cannot be earlier than " + lblPerfusionStartDate.Text + "' date. ");
                        }
                    }
                }

                if (txtIceContainerReplenishedTime.Text != string.Empty && txtIceContainerReplenishedTime.Text != "__:__")
                {
                    if (GeneralRoutines.IsNumeric(txtIceContainerReplenishedTime.Text.Substring(0, 2)) == false)
                    {
                        throw new Exception("Time Hour of " + lblIceContainerReplenishedDateTime.Text + " should be numeric.");
                    }

                    if (Convert.ToInt16(txtIceContainerReplenishedTime.Text.Substring(0, 2)) > 23)
                    {
                        throw new Exception("Time Hour of " + lblIceContainerReplenishedDateTime.Text + " should not be greater than 23.");
                    }

                    if (GeneralRoutines.IsNumeric(txtIceContainerReplenishedTime.Text.Substring(3, 2)) == false)
                    {
                        throw new Exception("Time Minute of " + lblIceContainerReplenishedDateTime.Text + " should be numeric.");
                    }

                    if (Convert.ToInt16(txtIceContainerReplenishedTime.Text.Substring(3, 2)) > 59)
                    {
                        throw new Exception("Time Minute of " + lblIceContainerReplenishedDateTime.Text + " cannot be greater than 59.");
                    }


                    dteIceContainerReplenished = Convert.ToDateTime(txtIceContainerReplenishedDate.Text + " " + txtIceContainerReplenishedTime.Text);

                    if (dteIceContainerReplenished > DateTime.Now)
                    {
                        throw new Exception("Date/Time '" + lblIceContainerReplenishedDateTime.Text + "' cannot be later than current Date/Time.");
                    }

                    if (dtePerfusionStart != DateTime.MinValue)
                    {
                        if (dteIceContainerReplenished < dtePerfusionStart)
                        {
                            throw new Exception("Date/Time for '" + lblIceContainerReplenishedDateTime.Text + "' cannot be earlier than " + lblPerfusionStartDate.Text + "'. ");
                        }
                    }

                }
                
            
                
            }

            if (rblKidneyOnMachine.SelectedValue == STR_NO_SELECTION)
            {
                if (txtKidneyOnMachineNo.Text == string.Empty)
                {
                    throw new Exception("Since Kidney can be placed on Machine is 'NO', please provide details Why Kidney placed on machine is 'NO'.");
                }

                

            }


            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }
            //now add the data

            string STRSQL = string.Empty;
            STRSQL += "INSERT INTO machineperfusion ";
            STRSQL += "(TrialID, Side, KidneyOnMachine, KidneyOnMachineNo,  PerfusionStartDate, PerfusionStartTime, ";
            STRSQL += "MachineSerialNumber, MachineReferenceModelNumber, LotNumberPerfusionSolution, LotNumberDisposables, ";
            STRSQL += "UsedPatchHolder, ArtificialPatchUsed, ArtificialPatchSize, ArtificialPatchNumber,  ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL += "OxygenBottleFull, OxygenBottleOpened,";
            }
            STRSQL += "OxygenTankChanged, OxygenTankChangedDate, OxygenTankChangedTime, ";
            STRSQL += "IceContainerReplenished, IceContainerReplenishedDate, IceContainerReplenishedTime, ";
            STRSQL += "LogisticallyPossibleMeasurepO2Perfusate, ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL += "ValuepO2Perfusate, ";
            }
            STRSQL += "ValuepO2PerfusateMeasured, ";
            STRSQL += "Comments, ";
            STRSQL += "DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?Side, ?KidneyOnMachine, ?KidneyOnMachineNo, ?PerfusionStartDate, ?PerfusionStartTime, ";
            STRSQL += "?MachineSerialNumber, ?MachineReferenceModelNumber, ?LotNumberPerfusionSolution, ?LotNumberDisposables, ";
            STRSQL += "?UsedPatchHolder, ?ArtificialPatchUsed, ?ArtificialPatchSize, ?ArtificialPatchNumber, ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL += "?OxygenBottleFull, ?OxygenBottleOpened,";
            }
            STRSQL += " ?OxygenTankChanged, ?OxygenTankChangedDate, ?OxygenTankChangedTime,";
            STRSQL += "?IceContainerReplenished, ?IceContainerReplenishedDate, ?IceContainerReplenishedTime, ";
            STRSQL += "?LogisticallyPossibleMeasurepO2Perfusate, ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL += "?ValuepO2Perfusate, ";
            }
            STRSQL += "?ValuepO2PerfusateMeasured, ";

            STRSQL += "?Comments,";
            STRSQL += "?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = string.Empty;
            STRSQL_UPDATE += "UPDATE machineperfusion SET ";
            STRSQL_UPDATE += "KidneyOnMachine=?KidneyOnMachine, KidneyOnMachineNo=?KidneyOnMachineNo, ";
            STRSQL_UPDATE += "PerfusionStartDate=?PerfusionStartDate, PerfusionStartTime=?PerfusionStartTime, MachineSerialNumber=?MachineSerialNumber,";
            STRSQL_UPDATE += " MachineReferenceModelNumber=?MachineReferenceModelNumber, LotNumberPerfusionSolution=?LotNumberPerfusionSolution, LotNumberDisposables=?LotNumberDisposables,";
            STRSQL_UPDATE += "UsedPatchHolder=?UsedPatchHolder, ArtificialPatchUsed =?ArtificialPatchUsed, ";
            STRSQL_UPDATE += "ArtificialPatchSize=?ArtificialPatchSize, ArtificialPatchNumber=?ArtificialPatchNumber, ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL_UPDATE += "OxygenBottleFull=?OxygenBottleFull, OxygenBottleOpened=?OxygenBottleOpened,";
            }            
            STRSQL_UPDATE += "OxygenTankChanged=?OxygenTankChanged, OxygenTankChangedDate=?OxygenTankChangedDate, OxygenTankChangedTime=?OxygenTankChangedTime,";
            STRSQL_UPDATE += "IceContainerReplenished=?IceContainerReplenished, IceContainerReplenishedDate=?IceContainerReplenishedDate, IceContainerReplenishedTime=?IceContainerReplenishedTime,";
            STRSQL_UPDATE += "LogisticallyPossibleMeasurepO2Perfusate=?LogisticallyPossibleMeasurepO2Perfusate,   ";
            if (SessionVariablesAll.ViewRandomise == STR_YES_SELECTION)
            {
                STRSQL_UPDATE += "ValuepO2Perfusate=?ValuepO2Perfusate,";
            }
            STRSQL_UPDATE += "ValuepO2PerfusateMeasured=?ValuepO2PerfusateMeasured,";
            STRSQL_UPDATE += "Comments=?Comments, ";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID AND Side=?Side ";


            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE machineperfusion SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) AND Side=?Side  ";
            



            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE machineperfusion SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID AND Side=?Side ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM machineperfusion WHERE TrialID=?TrialID AND Side=?Side";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?Side", ddSide.SelectedValue, STRCONN));

            if (intCountFind == 1)
            {
                //lblKidneyOnMachine.Text = intCountFind.ToString();
                MyCMD.CommandText = STRSQL_UPDATE;
            }
            else if (intCountFind == 0)
            {
                MyCMD.CommandText = STRSQL;
            }
            else
            {
                throw new Exception("An error occured while check if Machine Perfusion Data already exists in the database.");
            }


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue ;

            MyCMD.Parameters.Add("?KidneyOnMachine", MySqlDbType.VarChar).Value = rblKidneyOnMachine.SelectedValue;

            if (txtKidneyOnMachineNo.Text == string.Empty )
            { MyCMD.Parameters.Add("?KidneyOnMachineNo", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?KidneyOnMachineNo", MySqlDbType.VarChar).Value = txtKidneyOnMachineNo.Text; }

            if (GeneralRoutines.IsDate(txtPerfusionStartDate.Text) == true)
            {
                MyCMD.Parameters.Add("?PerfusionStartDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtPerfusionStartDate.Text);
            }
            else
            {
                MyCMD.Parameters.Add("?PerfusionStartDate", MySqlDbType.Date).Value = DBNull.Value;
            }

            if (txtPerfusionStartTime.Text == string.Empty || txtPerfusionStartTime.Text == "__:__")
            { MyCMD.Parameters.Add("?PerfusionStartTime", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?PerfusionStartTime", MySqlDbType.VarChar).Value = txtPerfusionStartTime.Text; }

            if (txtMachineSerialNumber.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?MachineSerialNumber", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MachineSerialNumber", MySqlDbType.VarChar).Value = txtMachineSerialNumber.Text;
            }

            if (txtMachineReferenceModelNumber.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?MachineReferenceModelNumber", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?MachineReferenceModelNumber", MySqlDbType.VarChar).Value = txtMachineReferenceModelNumber.Text;
            }

            if (txtLotNumberPerfusionSolution.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?LotNumberPerfusionSolution", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?LotNumberPerfusionSolution", MySqlDbType.VarChar).Value = txtLotNumberPerfusionSolution.Text;
            }

            if (txtLotNumberDisposables.Text==string.Empty)
            {
                MyCMD.Parameters.Add("?LotNumberDisposables", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?LotNumberDisposables", MySqlDbType.VarChar).Value = txtLotNumberDisposables.Text;
            }

            if (ddUsedPatchHolder.SelectedValue == STR_DD_UNKNOWN_SELECTION )
            {
                MyCMD.Parameters.Add("?UsedPatchHolder", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UsedPatchHolder", MySqlDbType.VarChar).Value = ddUsedPatchHolder.SelectedValue;
            }

            if (ddArtificialPatchUsed.SelectedValue == STR_DD_UNKNOWN_SELECTION )
            {
                MyCMD.Parameters.Add("?ArtificialPatchUsed", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArtificialPatchUsed", MySqlDbType.VarChar).Value = ddArtificialPatchUsed.SelectedValue;
            }

            if (ddArtificialPatchSize.SelectedValue == STR_DD_UNKNOWN_SELECTION )
            {
                MyCMD.Parameters.Add("?ArtificialPatchSize", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArtificialPatchSize", MySqlDbType.VarChar).Value = ddArtificialPatchSize.SelectedValue;
            }

            if (ddArtificialPatchNumber.SelectedValue == STR_DD_UNKNOWN_SELECTION )
            {
                MyCMD.Parameters.Add("?ArtificialPatchNumber", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArtificialPatchNumber", MySqlDbType.VarChar).Value = ddArtificialPatchNumber.SelectedValue;
            }

            if (ddOxygenBottleFull.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?OxygenBottleFull", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OxygenBottleFull", MySqlDbType.VarChar).Value = ddOxygenBottleFull.SelectedValue;
            }

            if (ddOxygenBottleOpened.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?OxygenBottleOpened", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OxygenBottleOpened", MySqlDbType.VarChar).Value = ddOxygenBottleOpened.SelectedValue;
            }

            if (ddOxygenTankChanged.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?OxygenTankChanged", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OxygenTankChanged", MySqlDbType.VarChar).Value = ddOxygenTankChanged.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtOxygenTankChangedDate.Text)==false)
            {
                MyCMD.Parameters.Add("?OxygenTankChangedDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OxygenTankChangedDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtOxygenTankChangedDate.Text);
            }

            if (txtOxygenTankChangedTime.Text == string.Empty || txtOxygenTankChangedTime.Text=="__:__")
            {
                MyCMD.Parameters.Add("?OxygenTankChangedTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OxygenTankChangedTime", MySqlDbType.VarChar).Value = txtOxygenTankChangedTime.Text;
            }

            if (ddIceContainerReplenished.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?IceContainerReplenished", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?IceContainerReplenished", MySqlDbType.VarChar).Value = ddIceContainerReplenished.SelectedValue;
            }

            if (GeneralRoutines.IsDate(txtIceContainerReplenishedDate.Text) == false)
            {
                MyCMD.Parameters.Add("?IceContainerReplenishedDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?IceContainerReplenishedDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtIceContainerReplenishedDate.Text);
            }

            if (txtIceContainerReplenishedTime.Text == string.Empty || txtIceContainerReplenishedTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?IceContainerReplenishedTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?IceContainerReplenishedTime", MySqlDbType.VarChar).Value = txtIceContainerReplenishedTime.Text;
            }

            if (ddLogisticallyPossibleMeasurepO2Perfusate.SelectedValue==STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?LogisticallyPossibleMeasurepO2Perfusate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?LogisticallyPossibleMeasurepO2Perfusate", MySqlDbType.VarChar).Value = ddLogisticallyPossibleMeasurepO2Perfusate.SelectedValue;
            }

            if (txtValuepO2Perfusate.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ValuepO2Perfusate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ValuepO2Perfusate", MySqlDbType.VarChar).Value = txtValuepO2Perfusate.Text;
            }

            if (txtValuepO2PerfusateMeasured.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ValuepO2PerfusateMeasured", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ValuepO2PerfusateMeasured", MySqlDbType.VarChar).Value = txtValuepO2PerfusateMeasured.Text;
            }


            if (txtComments.Text == string.Empty)
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text; }


            if (txtReasonModified.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = DBNull.Value;
                // MyCMD.Parameters.Add("?DateModified", MySqlDbType.VarChar).Value = DBNull.Value;
                //MyCMD.Parameters.Add("?ModifiedBy", MySqlDbType.VarChar).Value = DBNull.Value;
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
                //MyCMD.Parameters.Add("?DateModified", MySqlDbType.VarChar).Value = DateTime.Now;
                //MyCMD.Parameters.Add("?ModifiedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;
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

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

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

                myTrans.Commit();

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }


                //redirect to summary page if data complete is not no
                string strSQLCOMPLETE = string.Empty;
                strSQLCOMPLETE += "SELECT ";
                strSQLCOMPLETE += " IF(IF(t2.KidneyOnMachine='" + STR_NO_SELECTION + "', t2.KidneyOnMachineNo  IS NOT NULL,  ";
                strSQLCOMPLETE += "t2.PerfusionStartDate IS NOT NULL AND t2.PerfusionStartTime IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.MachineSerialNumber IS NOT NULL AND t2.MachineReferenceModelNumber  IS NOT NULL AND t2.LotNumberPerfusionSolution IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.LotNumberDisposables IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.UsedPatchHolder IS NOT NULL AND t2.ArtificialPatchUsed  IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.OxygenBottleFull IS NOT NULL AND t2.OxygenBottleOpened IS NOT NULL) ";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM machineperfusion t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialID=?TrialID AND Side=?Side";
                strSQLCOMPLETE += "";


                string strComplete = GeneralRoutines.ReturnScalarTwo(strSQLCOMPLETE, "?TrialID", Request.QueryString["TID"],"?Side", ddSide.SelectedValue, STRCONN);

                //lblDonorRiskIndex.Text = strComplete;

                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);
                }
                else
                {
                    //Response.Redirect(Request.Url.AbsoluteUri + "&SCode=1", false);
                    //lblUserMessages.Text = "Data Submitted";
                    string strUri = Request.Url.AbsoluteUri;

                    if (strUri.Contains("SCode=1"))
                    {
                        //Response.Redirect(strUri, false);
                        
                    }
                    else
                    {
                        //Response.Redirect(strUri + "&SCode=1", false);
                        strUri += "&SCode=1";
                        
                    }

                    if (strUri.Contains("Side"))
                    {
                        if (strUri.Contains("Side=Left") )
                        {
                            strUri.Replace("Left", ddSide.SelectedValue);
                        }
                        if ( strUri.Contains("Side=Right"))
                        {
                            strUri.Replace("Right", ddSide.SelectedValue);
                        }

                    }
                    else
                    {
                        strUri += "&Side=" + ddSide.SelectedValue;
                    }
                    Response.Redirect(strUri, false);
                    lblUserMessages.Text = "Data Submitted";

                }


            }

            catch (System.Exception ex)
            {
                myTrans.Rollback();

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing add/update query.";
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

            if (ddSide.SelectedValue==STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select Side of Kidney.");
            }

            string STRSQL = string.Empty;
            STRSQL += "DELETE FROM machineperfusion WHERE TrialID=?TrialID AND Side=?Side ";

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
            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;
            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }


                Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);

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
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }

    }
    protected void rblKidneyOnMachine_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (rblKidneyOnMachine.SelectedValue == STR_YES_SELECTION)
            {
                //pnlKidneyOnMachineSelected.Visible = true;

                pnlKidneyOnMachineSelectedNo.Visible = false;

                pnlKidneyOnMachineSelectedYes.Visible = true;

                txtKidneyOnMachineNo.Text = string.Empty;

                

            }

            else 
            {

                pnlKidneyOnMachineSelectedNo.Visible = true;

                pnlKidneyOnMachineSelectedYes.Visible = false;

                txtPerfusionStartDate.Text = string.Empty;
                txtPerfusionStartTime.Text = string.Empty;

                txtMachineSerialNumber.Text = string.Empty;
                txtMachineReferenceModelNumber.Text = string.Empty;

                txtLotNumberDisposables.Text = string.Empty;


                ddUsedPatchHolder.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                ddArtificialPatchUsed.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                ddArtificialPatchSize.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                ddArtificialPatchNumber.SelectedValue = STR_DD_UNKNOWN_SELECTION;

                ddOxygenTankChanged.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                txtOxygenTankChangedDate.Text = string.Empty;
                txtOxygenTankChangedTime.Text = string.Empty;

                ddIceContainerReplenished.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                txtIceContainerReplenishedDate.Text = string.Empty;
                txtIceContainerReplenishedTime.Text = string.Empty;

                ddLogisticallyPossibleMeasurepO2Perfusate.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                txtValuepO2Perfusate.Text = string.Empty;
                txtValuepO2PerfusateMeasured.Text = string.Empty;
                
            }

            //else
            //{
            //    //pnlKidneyOnMachineSelected.Visible = false;

            //    pnlKidneyOnMachineSelectedNo.Visible = false;

            //    pnlKidneyOnMachineSelectedYes.Visible = false;
            //}


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured when selecting if " + lblKidneyOnMachine.Text + ".";
        }
    }

    protected void ddArtificialPatchUsed_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddArtificialPatchUsed.SelectedValue == STR_YES_SELECTION)
            {
                pnlArtificialPatchUsedYes.Visible = true;
            }
            else
            {
                pnlArtificialPatchUsedYes.Visible = false;
                ddArtificialPatchSize.SelectedValue = STR_DD_UNKNOWN_SELECTION;
                ddArtificialPatchNumber.SelectedValue = STR_DD_UNKNOWN_SELECTION;


            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting if Artificial Patch Used.";
        }
    }
    protected void ddOxygenTankChanged_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddOxygenTankChanged.SelectedValue == STR_YES_SELECTION)
            {
                pnlOxygenTankChanged.Visible = true;
            }
            else
            {
                pnlOxygenTankChanged.Visible = false;
                txtOxygenTankChangedDate.Text = string.Empty;
                txtOxygenTankChangedTime.Text = string.Empty;
                


            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting if " + lblOxygenTankChanged.Text + ".";
        }
    }

    protected void ddIceContainerReplenished_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddIceContainerReplenished.SelectedValue == STR_YES_SELECTION)
            {
                pnlIceContainerReplenished.Visible = true;
            }
            else
            {
                pnlIceContainerReplenished.Visible = false;
                txtIceContainerReplenishedDate.Text = string.Empty;
                txtIceContainerReplenishedTime.Text = string.Empty;
                


            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting if " + lblIceContainerReplenished.Text + ".";
        }
    }

    protected void ddLogisticallyPossibleMeasurepO2Perfusate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddLogisticallyPossibleMeasurepO2Perfusate.SelectedValue == STR_YES_SELECTION)
            {
                pnlLogisticallyPossibleMeasurepO2Perfusate.Visible = true;
            }
            else
            {
                pnlLogisticallyPossibleMeasurepO2Perfusate.Visible = false;

                if (SessionVariablesAll.ViewRandomise==STR_YES_SELECTION)
                {
                    txtValuepO2Perfusate.Text = string.Empty;
                }
                
                txtValuepO2PerfusateMeasured.Text = string.Empty;

            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting if " + lblLogisticallyPossibleMeasurepO2Perfusate.Text + ".";
        }
    }

    
    protected void ddSide_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddSide.SelectedValue!=STR_UNKNOWN_SELECTION)
            {
                pnlSideSelected.Visible = true;

                AssignData();

                Label lbl;

                foreach (GridViewRow row in GV1.Rows)
                {
                    lbl = (Label)row.FindControl("lblSide");

                    if (lbl != null)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (lbl.Text == ddSide.SelectedValue)
                            {
                                row.BackColor = System.Drawing.Color.LightBlue;
                            }
                            else
                            {
                                if (row.RowState == DataControlRowState.Alternate)
                                {
                                    row.BackColor = GV1.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    row.BackColor = GV1.RowStyle.BackColor;
                                }


                            }
                        }
                    }
                }

            }
            else
            {
                pnlSideSelected.Visible = false;
                throw new Exception("Please Select 'Side' of Kidney.");
            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Side of Kidney.";
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