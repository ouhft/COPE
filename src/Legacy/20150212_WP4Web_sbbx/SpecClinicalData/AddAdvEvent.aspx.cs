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


public partial class SpecClinicalData_AddAdvEvent : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";

    private const string STR_UNKNOWN_SELECTION = "Unknown";
    private const string STR_DD_UNKNOWN_SELECTION = "0";
    private const string STR_YES_SELECTION = "YES";
    private const string STR_NO_SELECTION = "NO";

    private const string strMainCPH = "cplMainContents";
    private const string strSpecimenCPH = "SpecimenContents";

    //private const int CENTRECODE = 12;
    private const string REDIRECTPAGE = "~/SpecClinicalData/EditAdvEvent.aspx?TID=";
    private const string REDIRECTID = "&AEID=";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                {
                    throw new Exception("Could not obtain the TrialID (Recipient).");
                }

                

                lblDescription.Text = "Add Adverse Event Data for " + Request.QueryString["TID_R"].ToString();

                string STRSQL_TRIALIDDATE = string.Empty;
                STRSQL_TRIALIDDATE = "SELECT DateCreated FROM trialdetails WHERE TrialID=?TrialID ";
                string strTrialIDDate = string.Empty;
                strTrialIDDate = GeneralRoutines.ReturnScalar(STRSQL_TRIALIDDATE, "?TrialID", Request.QueryString["TID"], STRCONN);

                if (GeneralRoutines.IsDate(strTrialIDDate)==true)
                {
                    txtDateAE_CalendarExtender.StartDate = Convert.ToDateTime(strTrialIDDate);
                }
                txtDateAE_CalendarExtender.EndDate = DateTime.Today;

                ddAdverseEventType.DataSource = XMLAdverseEventsDataSource;
                ddAdverseEventType.DataBind();

                ddRecipientInfectionType.DataSource = XMLRecipientInfectionTypesDataSource;
                ddRecipientInfectionType.DataBind();

                ddClavienGrading.DataSource = XMLClavienGradingsDataSource;
                ddClavienGrading.DataBind();

                rblRecipientInfectionOrganismBacteria.DataSource = XMLMainOptionsDataSource;
                rblRecipientInfectionOrganismBacteria.DataBind();
                //rblRecipientInfectionOrganismBacteria.SelectedValue = STR_UNKNOWN_SELECTION;

                rblRecipientInfectionOrganismViral.DataSource = XMLMainOptionsDataSource;
                rblRecipientInfectionOrganismViral.DataBind();
                //rblRecipientInfectionOrganismViral.SelectedValue = STR_UNKNOWN_SELECTION;

                rblRecipientInfectionOrganismFungal.DataSource = XMLMainOptionsDataSource;
                rblRecipientInfectionOrganismFungal.DataBind();
                //rblRecipientInfectionOrganismFungal.SelectedValue = STR_UNKNOWN_SELECTION;

                ddBiopsyProvenAcuteRejectionBG.DataSource = XMLBanffGradingsDataSource;
                ddBiopsyProvenAcuteRejectionBG.DataBind();
                

                pnlRecipientInfectionTypeOther.Visible = false;

                pnlRecipientInfectionOrganismBacteriaDetails.Visible = false;
                pnlRecipientInfectionOrganismViralDetails.Visible = false;
                pnlRecipientInfectionOrganismFungalDetails.Visible = false;

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";

                lblClavienGrading.Text = "<b>Grade I:</b> Any deviation from the normal postoperative course without ";
                lblClavienGrading.Text += "<br/>the need for pharmacological treatment or surgical, endoscopic and ";
                lblClavienGrading.Text += "<br/>radiological interventions.";
                lblClavienGrading.Text += "<br/>Allowed therapeutic regimens are: drugs as antiemetics, antipyretics, ";
                lblClavienGrading.Text += "<br/>analgetics, diuretics and electrolytes and physiotherapy. ";
                lblClavienGrading.Text += "<br/>This grade also includes wound infections opened at the bedside.";
                lblClavienGrading.Text += "<br/><b>Grade II:</b> Requiring pharmacological treatment with drugs ";
                lblClavienGrading.Text += "<br/>other than such allowed for grade I complications. ";
                lblClavienGrading.Text += "<br/>Blood transfusions and total parenteral nutrition are also included.";
                lblClavienGrading.Text += "<br/><b>Grade III-a:</b> Requiring surgical, endoscopic or radiologicalintervention  ";
                lblClavienGrading.Text += "<br/>not under general anesthesia.";
                lblClavienGrading.Text += "<br/><b>Grade III-b:</b> Requiring surgical, endoscopic or radiological intervention ";
                lblClavienGrading.Text += "<br/>under general anesthesia.";
                lblClavienGrading.Text += "<br/><b>Grade IV-a:</b> Life-threatening complication (including ";
                lblClavienGrading.Text += "<br/>CNS complications: brain haemorrhage, ischaemic stroke, ";
                lblClavienGrading.Text += "<br/>subarachnoid bleeding, but excluding transient ischaemic attacks) ";
                //lblClavienGrading.Text += "<br/>";
                lblClavienGrading.Text += "<br/>requiring IC/ICU management with single organ dysfunction ";
                lblClavienGrading.Text += "<br/>(including dialysis).";
                lblClavienGrading.Text += "<br/><b>Grade IV-b:</b> Life-threatening complication (including ";
                lblClavienGrading.Text += "<br/>CNS complications: brain haemorrhage, ischaemic stroke, ";
                lblClavienGrading.Text += "<br/>subarachnoid bleeding, but excluding transient ischaemic attacks) ";
                //lblClavienGrading.Text += "<br/>";
                lblClavienGrading.Text += "<br/>requiring IC/ICU management with multi-organ dysfunction.";
                lblClavienGrading.Text += "<br/><b>Grade V:</b> Death of a patient.";

                //pnlClavienGrading.GroupingText = "<b>Grade I:</b> Any deviation from the normal postoperative course without ";
                //pnlClavienGrading.GroupingText += "<br/>the need for pharmacological treatment or surgical, endoscopic and ";
                //pnlClavienGrading.GroupingText += "<br/>radiological interventions.";
                //pnlClavienGrading.GroupingText += "<br/>Allowed therapeutic regimens are: drugs as antiemetics, antipyretics, ";
                //pnlClavienGrading.GroupingText += "<br/>analgetics, diuretics and electrolytes and physiotherapy. ";
                //pnlClavienGrading.GroupingText += "<br/>This grade also includes wound infections opened at the bedside.";
                //pnlClavienGrading.GroupingText += "<br/><b>Grade II:</b> Requiring pharmacological treatment with drugs ";
                //pnlClavienGrading.GroupingText += "<br/>other than such allowed for grade I complications. ";
                //pnlClavienGrading.GroupingText += "<br/>Blood transfusions and total parenteral nutrition are also included.";
                //pnlClavienGrading.GroupingText += "<br/><b>Grade III-a:</b> Requiring surgical, endoscopic or radiologicalintervention  ";
                //pnlClavienGrading.GroupingText += "<br/>not under general anesthesia.";
                //pnlClavienGrading.GroupingText += "<br/><b>Grade III-b:</b> Requiring surgical, endoscopic or radiological intervention ";
                //pnlClavienGrading.GroupingText += "<br/>under general anesthesia.";
                //pnlClavienGrading.GroupingText += "<br/><b>Grade IV-a:</b> Life-threatening complication (including ";
                //pnlClavienGrading.GroupingText += "<br/>CNS complications: brain haemorrhage, ischaemic stroke, ";
                //pnlClavienGrading.GroupingText += "<br/>subarachnoid bleeding, but excluding transient ischaemic attacks) ";
                ////pnlClavienGrading.GroupingText += "<br/>";
                //pnlClavienGrading.GroupingText += "<br/>requiring IC/ICU management with single organ dysfunction ";
                //pnlClavienGrading.GroupingText += "<br/>(including dialysis).";
                //pnlClavienGrading.GroupingText += "<br/><b>Grade IV-b:</b> Life-threatening complication (including ";
                //pnlClavienGrading.GroupingText += "<br/>CNS complications: brain haemorrhage, ischaemic stroke, ";
                //pnlClavienGrading.GroupingText += "<br/>subarachnoid bleeding, but excluding transient ischaemic attacks) ";
                ////pnlClavienGrading.GroupingText += "<br/>";
                //pnlClavienGrading.GroupingText += "<br/>requiring IC/ICU management with multi-organ dysfunction.";
                //pnlClavienGrading.GroupingText += "<br/><b>Grade V:</b> Death of a patient.";

                ViewState["SortField"] = "DateAE";
                ViewState["SortDirection"] = "DESC";

                BindData();

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessage"];
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";
                
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
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID (Recipient).");
            }
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.TrialID,  ";
            strSQL += "DATE_FORMAT(t1.DateAE, '%d/%m/%Y') Date_AE, ";
            strSQL += "DATE_FORMAT(t1.DateCreated, '%d/%m/%Y %H:%i') Date_Created ";
            //strSQL += "t2.RecipientID, DATE_FORMAT(t2.RecipientDateOfBirth, '%d/%m/%Y') Recipient_DateOfBirth ";
            strSQL += "FROM r_ae t1 ";
            strSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();


            lblGV1.Text = "Click on TrialID (Recipient) to Edit an Adverse Event Data.";

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
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            if (txtDateAE.Text == string.Empty)
            {
                throw new Exception("Please enter 'Date of Adverse Event'.");
            }

            if (GeneralRoutines.IsDate(txtDateAE.Text) == false)
            {
                throw new Exception("Please enter 'Date of Adverse Event' as DD/MM/YYYY.");
            }

            //date of Adverse Event cannnot be before the date liver was randomised
            string STRSQL_TRIALIDDATE = string.Empty;
            STRSQL_TRIALIDDATE = "SELECT DateCreated FROM trialdetails WHERE TrialID=?TrialID ";
            string strTrialIDDate = string.Empty;
            strTrialIDDate = GeneralRoutines.ReturnScalar(STRSQL_TRIALIDDATE, "?TrialID", Request.QueryString["TID"], STRCONN);

            if (GeneralRoutines.IsDate(strTrialIDDate) == false)
            {
                throw new Exception("Could not obtain the Date and Time when the TrialID was created.");
            }

            //if ((Convert.ToDateTime(txtDateAE.Text).Date - Convert.ToDateTime(strTrialIDDate).Date).TotalDays <0)
            if ((Convert.ToDateTime(txtDateAE.Text).Date < Convert.ToDateTime(strTrialIDDate).Date))
            {
               throw new Exception("The 'Date Of Adverse Event' can not be earlier than the Date (" + Convert.ToDateTime(strTrialIDDate).ToShortDateString() + ") when TrialID was created." );
            }


            //adverse event cannnot be after date withdrawn
            string STRSQL_DATEWITHDRAWN = string.Empty;
            STRSQL_DATEWITHDRAWN += "SELECT DateWithdrawn FROM trialidwithdrawn  WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strDateWithdrawn = string.Empty;
            strDateWithdrawn += GeneralRoutines.ReturnScalar(STRSQL_DATEWITHDRAWN, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

            if (GeneralRoutines.IsDate(strDateWithdrawn) == true)
            {
                if (Convert.ToDateTime(txtDateAE.Text).Date > Convert.ToDateTime(strDateWithdrawn).Date)
                {
                    throw new Exception("The 'Date of Adverse Event' can not be after 'Date Withdrawn ' (" + Convert.ToDateTime(strDateWithdrawn).ToShortDateString() + ") of the recipient.");
                }
            }

            //adverse event cannnot be after date of death
            string STRSQL_DEATHDATE = "SELECT DeathDate FROM r_deceased WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strDeathDate = string.Empty;
            strDeathDate = GeneralRoutines.ReturnScalar(STRSQL_DEATHDATE, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

            if (GeneralRoutines.IsDate(strDeathDate) == true)
            {
                if (Convert.ToDateTime(txtDateAE.Text).Date > Convert.ToDateTime(strDeathDate).Date)
                {
                    throw new Exception("The 'Date of Adverse Event' cannot be after 'Date of Death ' (" + Convert.ToDateTime(strDeathDate).ToShortDateString() + ") of the recipient.");
                }
            }

            if (Convert.ToDateTime(txtDateAE.Text) > DateTime.Today)
            {
                throw new Exception("'Date of Adverse Event' can not be greater than Today's date.");
            }

            if (ddAdverseEventType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select an Adverse Event Type.");
            }

            if (ddAdverseEventType.SelectedValue == "Recipient Infection")
            {
                if (ddRecipientInfectionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
                {
                    throw new Exception("Since " + lblAdverseEventType.Text + " selected is '" + ddAdverseEventType.SelectedValue + "' please select '" + lblRecipientInfectionType.Text + "'");
                }
                if (ddRecipientInfectionType.SelectedValue == "Other")
                {
                    if (txtRecipientInfectionTypeOther.Text == string.Empty)
                    {
                        throw new Exception("Since " + lblRecipientInfectionType.Text + " selected is '" + ddRecipientInfectionType.SelectedValue + "' please provide details for " + lblRecipientInfectionTypeOther.Text + "'");
                    }
                    //throw new Exception("Since " + lblRecipientInfectionType.Text + " selected is '" + ddRecipientInfectionType.SelectedValue + "' please provide details for " + lblRecipientInfectionTypeOther.Text + "'");
                }

                //check recipient infection bacteria has been added
                if (rblRecipientInfectionOrganismBacteria.SelectedIndex==-1)
                {
                    throw new Exception("Please Select an option for '" + lblRecipientInfectionOrganismBacteria.Text + "'.");
                }

                if (rblRecipientInfectionOrganismBacteria.SelectedValue == STR_YES_SELECTION)
                {
                    if (txtRecipientInfectionOrganismBacteriaDetails.Text == string.Empty)
                    {
                        throw new Exception("Since '" + lblRecipientInfectionOrganismBacteria.Text + "' is '" + rblRecipientInfectionOrganismBacteria.SelectedValue + "', please provide " + lblRecipientInfectionOrganismBacteriaDetails.Text);
                    }
                    
                }

                if (txtRecipientInfectionOrganismBacteriaDetails.Text!=string.Empty)
                {
                    if (rblRecipientInfectionOrganismBacteria.SelectedValue!= STR_YES_SELECTION)
                    {
                        throw new Exception("Please Select 'YES' for '" + lblRecipientInfectionOrganismBacteria.Text + "' as details have been provided for " + lblRecipientInfectionOrganismBacteriaDetails.Text);
                    }
                }

                if (rblRecipientInfectionOrganismBacteria.SelectedValue != STR_YES_SELECTION)
                {
                    txtRecipientInfectionOrganismBacteriaDetails.Text = string.Empty;
                }

                //check recipient infection viral has been added
                if (rblRecipientInfectionOrganismViral.SelectedIndex == -1)
                {
                    throw new Exception("Please Select an option for '" + lblRecipientInfectionOrganismViral.Text + "'.");
                }

                if (rblRecipientInfectionOrganismViral.SelectedValue == STR_YES_SELECTION)
                {
                    if (txtRecipientInfectionOrganismViralDetails.Text == string.Empty)
                    {
                        throw new Exception("Since '" + lblRecipientInfectionOrganismViral.Text + "' is '" + rblRecipientInfectionOrganismViral.SelectedValue + "', please provide " + lblRecipientInfectionOrganismViralDetails.Text);
                    }
                    
                }

                if (txtRecipientInfectionOrganismViralDetails.Text != string.Empty)
                {
                    if (rblRecipientInfectionOrganismViral.SelectedValue != STR_YES_SELECTION)
                    {
                        throw new Exception("Please Select 'YES' for '" + lblRecipientInfectionOrganismViral.Text + "' as details have been provided for " + lblRecipientInfectionOrganismViralDetails.Text);
                    }
                }

                if (rblRecipientInfectionOrganismViral.SelectedValue != STR_YES_SELECTION)
                {
                    txtRecipientInfectionOrganismViralDetails.Text = string.Empty;
                }

                //check recipient infection Fungal has been added
                if (rblRecipientInfectionOrganismFungal.SelectedIndex == -1)
                {
                    throw new Exception("Please Select an option for '" + lblRecipientInfectionOrganismFungal.Text + "'.");
                }

                if (rblRecipientInfectionOrganismFungal.SelectedValue == STR_YES_SELECTION)
                {
                    if (txtRecipientInfectionOrganismFungalDetails.Text == string.Empty)
                    {
                        throw new Exception("Since '" + lblRecipientInfectionOrganismFungal.Text + "' is '" + rblRecipientInfectionOrganismFungal.SelectedValue + "', please provide " + lblRecipientInfectionOrganismFungalDetails.Text);
                    }
                    
                }

                if (txtRecipientInfectionOrganismFungalDetails.Text != string.Empty)
                {
                    if (rblRecipientInfectionOrganismFungal.SelectedValue != STR_YES_SELECTION)
                    {
                        throw new Exception("Please Select 'YES' for '" + lblRecipientInfectionOrganismFungal.Text + "' as details have been provided for " + lblRecipientInfectionOrganismFungalDetails.Text);
                    }
                }

                if (rblRecipientInfectionOrganismFungal.SelectedValue != STR_YES_SELECTION)
                {
                    txtRecipientInfectionOrganismFungalDetails.Text = string.Empty;
                }

            }

            if (ddAdverseEventType.SelectedValue == "Re-Operation")
            {
                if (txtReOperationDetails.Text == string.Empty)
                {
                    throw new Exception("Since " + lblAdverseEventType.Text + " selected is '" + ddAdverseEventType.SelectedValue + "' please provide details for '" + lblReOperationDetails.Text + "'");
                }

            }

            if (ddAdverseEventType.SelectedValue == "Other Adverse Event")
            {
                if (txtOtherAdverseEvent.Text==string.Empty)
                {
                    throw new Exception("Since " + lblAdverseEventType.Text + " selected is '" + ddAdverseEventType.SelectedValue + "' please provide '" + lblOtherAdverseEvent.Text + "'");
                }
                
            }

            if (ddClavienGrading.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select " + lblCG.Text);
            }

            string STRSQL = string.Empty;

            STRSQL += "INSERT INTO r_ae ";
            STRSQL += "(TrialIDRecipient, DateAE, AdverseEventType, RecipientInfectionType, RecipientInfectionTypeOther, RecipientInfectionOrganismBacteria,";
            STRSQL += "RecipientInfectionOrganismBacteriaDetails, RecipientInfectionOrganismViral, RecipientInfectionOrganismViralDetails,";
            STRSQL += "RecipientInfectionOrganismFungal, RecipientInfectionOrganismFungalDetails, BiopsyProvenAcuteRejectionBG, ReOperationDetails, OtherAdverseEvent, ";
            STRSQL += "ClavienGrading,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?DateAE, ?AdverseEventType, ?RecipientInfectionType, ?RecipientInfectionTypeOther, ?RecipientInfectionOrganismBacteria,";
            STRSQL += "?RecipientInfectionOrganismBacteriaDetails, ?RecipientInfectionOrganismViral, ?RecipientInfectionOrganismViralDetails,";
            STRSQL += "?RecipientInfectionOrganismFungal, ?RecipientInfectionOrganismFungalDetails, ?BiopsyProvenAcuteRejectionBG, ?ReOperationDetails, ?OtherAdverseEvent,";
            STRSQL += "?ClavienGrading,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE r_ae SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "AND AEID=?AEID ";
            //STRSQL_LOCK += "AND DateAE IS NOT NULL ";
            //STRSQL_LOCK += "AND IF(AdverseEventType='Recipient Infection', RecipientInfectionType IS NOT NULL, AdverseEventType IS NOT NULL) ";
            //STRSQL_LOCK += "AND IF(RecipientInfectionType='Other', RecipientInfectionTypeOther IS NOT NULL, AdverseEventType IS NOT NULL) ";
            //STRSQL_LOCK += "AND IF(AdverseEventType='Re-Operation', ReOperationDetails IS NOT NULL, AdverseEventType IS NOT NULL) ";
            //STRSQL_LOCK += "AND IF(AdverseEventType='Other Adverse Event', OtherAdverseEvent IS NOT NULL, AdverseEventType IS NOT NULL) ";
            //STRSQL_LOCK += "AND ClavienGrading IS NOT NULL ";


            //UniqueID find
            string STR_SQLID = string.Empty;
            STR_SQLID += "SELECT AEID FROM r_ae ";
            STR_SQLID += "WHERE TrialIDRecipient=?TrialIDRecipient ORDER BY DateCreated DESC LIMIT 1 ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            if (GeneralRoutines.IsDate(txtDateAE.Text) == false)
            {
                MyCMD.Parameters.Add("?DateAE", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateAE", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateAE.Text);
            }

            if (ddAdverseEventType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?AdverseEventType", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AdverseEventType", MySqlDbType.VarChar).Value = ddAdverseEventType.SelectedValue;
            }

            if (ddRecipientInfectionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RecipientInfectionType", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionType", MySqlDbType.VarChar).Value = ddRecipientInfectionType.SelectedValue;
            }

            if (txtRecipientInfectionTypeOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RecipientInfectionTypeOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionTypeOther", MySqlDbType.VarChar).Value = txtRecipientInfectionTypeOther.Text;
            }

            if (rblRecipientInfectionOrganismBacteria.SelectedIndex == -1 )
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismBacteria", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismBacteria", MySqlDbType.VarChar).Value = rblRecipientInfectionOrganismBacteria.SelectedValue;
            }

            if (txtRecipientInfectionOrganismBacteriaDetails.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismBacteriaDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismBacteriaDetails", MySqlDbType.VarChar).Value = txtRecipientInfectionOrganismBacteriaDetails.Text;
            }

            if (rblRecipientInfectionOrganismViral.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismViral", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismViral", MySqlDbType.VarChar).Value = rblRecipientInfectionOrganismViral.SelectedValue;
            }

            if (txtRecipientInfectionOrganismViralDetails.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismViralDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismViralDetails", MySqlDbType.VarChar).Value = txtRecipientInfectionOrganismViralDetails.Text;
            }

            if (rblRecipientInfectionOrganismFungal.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismFungal", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismFungal", MySqlDbType.VarChar).Value = rblRecipientInfectionOrganismFungal.SelectedValue;
            }

            if (txtRecipientInfectionOrganismFungalDetails.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismFungalDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInfectionOrganismFungalDetails", MySqlDbType.VarChar).Value = txtRecipientInfectionOrganismFungalDetails.Text;
            }

            
            if (ddBiopsyProvenAcuteRejectionBG.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?BiopsyProvenAcuteRejectionBG", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?BiopsyProvenAcuteRejectionBG", MySqlDbType.VarChar).Value = ddBiopsyProvenAcuteRejectionBG.SelectedValue;
            }

            if (txtReOperationDetails.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ReOperationDetails", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReOperationDetails", MySqlDbType.VarChar).Value = txtReOperationDetails.Text;
            }

            if (txtOtherAdverseEvent.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?OtherAdverseEvent", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OtherAdverseEvent", MySqlDbType.VarChar).Value = txtOtherAdverseEvent.Text;
            }

            if (ddClavienGrading.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ClavienGrading", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ClavienGrading", MySqlDbType.VarChar).Value = ddClavienGrading.SelectedValue;
            }

            if (txtComments.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = 1;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                

                //get the uniqueid to redirect the page
                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = STR_SQLID;
                int intID = Convert.ToInt32(MyCMD.ExecuteScalar());

                MyCMD.Parameters.Add("?AEID", MySqlDbType.VarChar).Value = intID.ToString();


                //lock data
                if (chkAllDataAdded.Checked == true)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQL_LOCK;
                    MyCMD.ExecuteNonQuery();

                }

                
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                Response.Redirect(REDIRECTPAGE + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + REDIRECTID + intID.ToString(), false);

                BindData();
                lblUserMessages.Text = "Data Added";

            }
            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }


            //finally //close connection
            //{
            //    if (MyCONN.State == ConnectionState.Open)
            //    {
            //        MyCONN.Close();
            //    }
            //}


        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }

    protected void ddAdverseEventType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            switch (ddAdverseEventType.SelectedValue)
            {
                case "Recipient Infection":
                    pnlRecipientInfection.Visible=true;

                    pnlBiopsyProvenAcuteRejectionBG.Visible = false;
                    ddBiopsyProvenAcuteRejectionBG.SelectedValue = "0";

                    pnlReOperationAdverseEvent.Visible = false;
                    txtReOperationDetails.Text = string.Empty; 

                    pnlOtherAdverseEvent.Visible = false;
                    txtOtherAdverseEvent.Text = string.Empty;
                    break;

                case "Biopsy Proven Acute Rejection":
                    
                    pnlBiopsyProvenAcuteRejectionBG.Visible = true;

                    pnlRecipientInfection.Visible = false;
                    ddRecipientInfectionType.SelectedValue = "0";
                    txtRecipientInfectionTypeOther.Text = string.Empty;

                    rblRecipientInfectionOrganismBacteria.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismBacteriaDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismViral.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismViralDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismFungal.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismFungalDetails.Text = string.Empty;
                    
                    pnlReOperationAdverseEvent.Visible = false;
                    txtReOperationDetails.Text = string.Empty; 

                    pnlOtherAdverseEvent.Visible = false;
                    txtOtherAdverseEvent.Text = string.Empty;
                    break;

                case "Re-Operation":
                    
                    pnlReOperationAdverseEvent.Visible = true; 

                    pnlRecipientInfection.Visible = false;
                    ddRecipientInfectionType.SelectedValue = "0";
                    txtRecipientInfectionTypeOther.Text = string.Empty;

                    rblRecipientInfectionOrganismBacteria.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismBacteriaDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismViral.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismViralDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismFungal.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismFungalDetails.Text = string.Empty;

                    pnlBiopsyProvenAcuteRejectionBG.Visible = false;
                    ddBiopsyProvenAcuteRejectionBG.SelectedValue = "0";

                    
                    pnlOtherAdverseEvent.Visible = false;
                    txtOtherAdverseEvent.Text = string.Empty;
                    
                    break;

                case "Other Adverse Event":
                    pnlOtherAdverseEvent.Visible = true;

                    pnlRecipientInfection.Visible = false;
                    ddRecipientInfectionType.SelectedValue = "0";
                    txtRecipientInfectionTypeOther.Text = string.Empty;

                    rblRecipientInfectionOrganismBacteria.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismBacteriaDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismViral.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismViralDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismFungal.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismFungalDetails.Text = string.Empty;
                    
                    pnlBiopsyProvenAcuteRejectionBG.Visible = false;
                    ddBiopsyProvenAcuteRejectionBG.SelectedValue = "0";

                    pnlReOperationAdverseEvent.Visible = false;
                    txtReOperationDetails.Text = string.Empty; 

                    break;

                default:
                    pnlRecipientInfection.Visible = false;
                    ddRecipientInfectionType.SelectedValue = "0";
                    txtRecipientInfectionTypeOther.Text = string.Empty;

                    rblRecipientInfectionOrganismBacteria.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismBacteriaDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismViral.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismViralDetails.Text = string.Empty;

                    rblRecipientInfectionOrganismFungal.SelectedValue = STR_UNKNOWN_SELECTION;
                    txtRecipientInfectionOrganismFungalDetails.Text = string.Empty;

                    pnlBiopsyProvenAcuteRejectionBG.Visible = false;
                    ddBiopsyProvenAcuteRejectionBG.SelectedValue = "0";

                    pnlReOperationAdverseEvent.Visible = false;
                    txtReOperationDetails.Text = string.Empty; 

                    pnlOtherAdverseEvent.Visible = false;
                    txtOtherAdverseEvent.Text = string.Empty;

                    break;
            }
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Adverse Event Type.";
        }

    }
    protected void ddRecipientInfectionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddRecipientInfectionType.SelectedValue == "Other")
        {
            pnlRecipientInfectionTypeOther.Visible = true;
        }
        else
        {
            pnlRecipientInfectionTypeOther.Visible = false;
            txtRecipientInfectionTypeOther.Text = string.Empty;

        }

    }
    protected void rblRecipientInfectionOrganismBacteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRecipientInfectionOrganismBacteria.SelectedValue == "YES")
        {
            pnlRecipientInfectionOrganismBacteriaDetails.Visible = true;
        }
        else
        {
            pnlRecipientInfectionOrganismBacteriaDetails.Visible = false;
            txtRecipientInfectionOrganismBacteriaDetails.Text = string.Empty;
        }

    }
    protected void rblRecipientInfectionOrganismViral_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRecipientInfectionOrganismViral.SelectedValue == "YES")
        {
            pnlRecipientInfectionOrganismViralDetails.Visible = true;
        }
        else
        {
            pnlRecipientInfectionOrganismViralDetails.Visible = false;
            txtRecipientInfectionOrganismViralDetails.Text = string.Empty;
        }

    }
    protected void rblRecipientInfectionOrganismFungal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRecipientInfectionOrganismFungal.SelectedValue == "YES")
        {
            pnlRecipientInfectionOrganismFungalDetails.Visible = true;
        }
        else
        {
            pnlRecipientInfectionOrganismFungalDetails.Visible = false;
            txtRecipientInfectionOrganismFungalDetails.Text = string.Empty;
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