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

using System.Net;
using System.Net.Mail;

public partial class SpecClinicalData_AddAE : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";

        private const int intMinimumCharactersDescriptionEvent = 20;


        private const string strMainCPH = "cplMainContents";
        private const string strSpecimenCPH = "SpecimenContents";

        private const string REDIRECTPAGE_AE = "~/SpecClinicalData/AddAdvEvent.aspx?TID=";
        private const string REDIRECTPAGE_SAE = "~/SpecClinicalData/AddSerAE.aspx?TID=";
        
        //private const string REDIRECTPAGE = "~/SpecClinicalData/EditSerAE.aspx?TID=";
        //private const string REDIRECTID = "&SerAEID=";
        //private const string REDIRECTEMAILCODE = "&ECH=";


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
                    throw new Exception("Could not obtain the TrialID.");
                }

                lblDescription.Text = "Add Adverse Event Data for " + Request.QueryString["TID_R"].ToString();

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessage"];
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

                rblResultDeath.DataSource = XMLMainOptionsYNDataSource;
                rblResultDeath.DataBind();

                rblLTInjury.DataSource = XMLMainOptionsYNDataSource;
                rblLTInjury.DataBind();

                rblPermanentImpairement.DataSource = XMLMainOptionsYNDataSource;
                rblPermanentImpairement.DataBind();

                rblInPatientCareHospitalisation.DataSource = XMLMainOptionsYNDataSource;
                rblInPatientCareHospitalisation.DataBind();

                rblSurgicalIntervention.DataSource = XMLMainOptionsYNDataSource;
                rblSurgicalIntervention.DataBind();

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";

                lblPopUpSurgicalIntervention.Text = "<b>This includes device deficiencies that might have led to a serious adverse event if: suitable action had not been taken/intervention had not been made/circumstances had been less fortunate. ";
            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }

    }
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (rblLTInjury.SelectedValue==STR_YES_SELECTION
                || rblPermanentImpairement.SelectedValue == STR_YES_SELECTION
                || rblInPatientCareHospitalisation.SelectedValue==STR_YES_SELECTION
                || rblSurgicalIntervention.SelectedValue == STR_YES_SELECTION
                || rblResultDeath.SelectedValue == STR_YES_SELECTION)
            {
                Response.Redirect(REDIRECTPAGE_SAE + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
            }
            else
            {
                Response.Redirect(REDIRECTPAGE_AE + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
            }
        }
        catch(Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
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
}