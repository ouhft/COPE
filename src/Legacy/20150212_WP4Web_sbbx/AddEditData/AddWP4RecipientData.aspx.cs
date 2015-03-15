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

public partial class AddEditData_AddWP4RecipientData : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        //private const string strMainCPH = "cplMainContents";
        //private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const Int16 INT_ACTIVE =1;//by default the recipient is active
        private const Int16 INT_INACTIVE = 0;//by default the recipient is inactive

        private const Int16 INT_DATALOCKED = 1;//by default this record is locked

    //static Random _random = new Random();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                lblDescription.Text = "Please Enter Main Recipient Details to add a Recipient.";

                pnlMain.Visible = true;
                pnlTrialIDRecipient.Visible = false;

//                string STRSQL = @"SELECT t1.* FROM
//                            (SELECT t1.* FROM (SELECT t1.TrialID,  CONCAT(t1.TrialID, ' --> DonorID ', t1.DonorID) TrialIDDetails, COUNT(*) CR FROM trialdetails t1
//                            LEFT JOIN r_identification t2 ON t1.TrialID=t2.TrialID
//                            WHERE t1.TrialID LIKE ?TrialID AND t2.TrialID IS NULL  
//                            GROUP BY t1.TrialID) t1
//                            UNION
//                            SELECT t2.* FROM (SELECT t1.TrialID, CONCAT(t1.TrialID, ' --> DonorID ', t1.DonorID) TrialIDDetails, COUNT(*) CR FROM trialdetails t1 
//                            LEFT JOIN r_identification t2 ON t1.TrialID=t2.TrialID
//                            WHERE t1.TrialID LIKE ?TrialID
//                            GROUP BY t1.TrialID
//                            HAVING CR < 2) t2) t1
//                            ORDER BY TrialID";


//                string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");

//                sqldsTrialID.SelectCommand = STRSQL;
//                sqldsTrialID.SelectParameters.Clear();
//                sqldsTrialID.SelectParameters.Add("?TrialID", strTrialIDLeadingCharacters + SessionVariablesAll.CentreCode + "%");

//                ddTrialID.DataSource = sqldsTrialID;
//                ddTrialID.DataBind();


                                


                string STRSQL_TransplantCentres = string.Empty;
                string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                //if adminsuperuser then all access
                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION || strIsServer == "0")
                {
                    STRSQL_TransplantCentres += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged  
                                    FROM cope_wp_four.lstcentres  t1  
                                    ORDER BY t1.CountryCode, t1.CentreCode ";
                }
                else
                {
                    //only where add/edit=YES
                    STRSQL_TransplantCentres += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM cope_wp_four.lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    AND (t2.AddEditRecipient='" + STR_YES_SELECTION + "' OR  t2.AddEditFollowUp='" + STR_YES_SELECTION + "') ORDER BY t1.CountryCode, t1.CentreCode ";


                }


                sqldsCentreLists.SelectCommand = STRSQL_TransplantCentres;
                sqldsCentreLists.SelectParameters.Clear();
                sqldsCentreLists.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);

                if (!string.IsNullOrEmpty(SessionVariablesAll.CentreCode))
                {

                    sqldsCentreLists.SelectParameters.Add("?CountryCode", SessionVariablesAll.CentreCode);
                    ddTransplantCentre.DataSource = sqldsCentreLists;
                    ddTransplantCentre.DataBind();
                    //ddCountry.SelectedValue = SessionVariablesAll.CentreCode;

                }


                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdAddData_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK a new Recipient will be created.";

                rblKidneyReceived.DataSource = XMLKidneySidesDataSource;
                rblKidneyReceived.DataBind();

                ListItem li = rblKidneyReceived.Items.FindByValue(STR_UNKNOWN_SELECTION);

                if (li != null)
                {
                    rblKidneyReceived.Items.Remove(li);
                }

                rblRecipientInformedConsent.DataSource = XMLMainOptionsYNDataSource;
                rblRecipientInformedConsent.DataBind();

                rblRecipient18Year.DataSource = XMLMainOptionsYNDataSource;
                rblRecipient18Year.DataBind();

                rblRecipientMultipleDualTransplant.DataSource = XMLMainOptionsYNDataSource;
                rblRecipientMultipleDualTransplant.DataBind();


            }
        }
        catch(Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }

    // reset page
    protected void cmdReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.Url.AbsoluteUri, false);
            //lblUserMessages.Text = "";
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

            if (rblRecipientInformedConsent.SelectedValue==STR_YES_SELECTION)
            {
                Page.Validate("Inclusion");
            }
            
            if (Page.IsValid == false)
            {
                throw new Exception("Please Check the data you have added.");
            }
            if (txtTrialID.Text==string.Empty)
            {
                throw new Exception("Please Enter TrialID.");
            }

            txtTrialID.Text.ToUpper();

            if (rblKidneyReceived.SelectedValue == STR_UNKNOWN_SELECTION || rblKidneyReceived.SelectedIndex==-1)
            { 
                throw new Exception("Please Select Side of the Kidney received."); 
            }

            //if (rblRecipientInformedConsent.SelectedValue != STR_YES_SELECTION)
            //{
            //    throw new Exception("Please Select YES for " + lblRecipientInformedConsent.Text + ".");
            //}

            //if (rblRecipient18Year.SelectedValue != STR_YES_SELECTION)
            //{
            //    throw new Exception("Please Select YES for " + lblRecipient18Year.Text + ".");
            //}

            //if (rblRecipientMultipleDualTransplant.SelectedValue != STR_NO_SELECTION)
            //{
            //    throw new Exception("Please Select NO for " + lblRecipientMultipleDualTransplant.Text + ".");
            //}

            if (rblRecipientInformedConsent.SelectedIndex==-1)
            {
                throw new Exception("Please Select an option for " + lblRecipientInformedConsent.Text + ".");
            }

            //if (rblRecipient18Year.SelectedIndex == -1)
            //{
            //    throw new Exception("Please Select an option for " + lblRecipient18Year.Text + ".");
            //}

            //if (rblRecipientMultipleDualTransplant.SelectedIndex == -1)
            //{
            //    throw new Exception("Please Select an option for " + lblRecipientMultipleDualTransplant.Text + ".");
            //}


            //check if kidney has been assigned
            string STRSQL_FIND_TrialID = "SELECT COUNT(*) CR FROM trialdetails WHERE TrialID=?TrialID  ";
            int intCountFind_TrialID = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND_TrialID, "?TrialID", txtTrialID.Text, STRCONN));

            if (intCountFind_TrialID==0)
            {
                throw new Exception("The TrialID " + txtTrialID.Text +  " does not exist in the database.");
            }

            if (intCountFind_TrialID >1)
            {
                throw new Exception("More than one TrialID " + txtTrialID.Text + " exist in the database.");
            }
            if (intCountFind_TrialID < 0)
            {
                throw new Exception("An error occured while checking if TrialID " + txtTrialID.Text + " exists in the database.");
            }

            //check if a kidney from mainland europe is not assigned to a recipient in UK

            if (txtTrialID.Text.Substring(3,1) == "1")
            {
                if (txtTrialID.Text.Substring(3,1) != ddTransplantCentre.SelectedValue.Substring(0,1))
                {
                    throw new Exception("Country Code for the TrialID (Donor) should match with the Country code of the selected Transplant Centre.");
                }

            }

            if (txtTrialID.Text.Substring(3, 1) != "1")
            {
                if (ddTransplantCentre.SelectedValue.Substring(0, 1) == "1")
                {
                    throw new Exception("A kidney from mainland Europe cannot be transplanted in the UK.");
                }

            }

            //check if kidney has been assigned
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM trialdetails_recipient WHERE TrialID=?TrialID AND KidneyReceived=?KidneyReceived ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", txtTrialID.Text, "?KidneyReceived", rblKidneyReceived.SelectedValue, STRCONN));


            if (intCountFind > 0)
            {
                STRSQL_FIND = "SELECT DonorID FROM trialdetails t1 INNER JOIN trialdetails_recipient t2 ON t1.TrialID=t2.TrialID WHERE t2.TrialID=?TrialID AND t2.KidneyReceived=?KidneyReceived ";
                string strDonorID = GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", txtTrialID.Text, "?KidneyReceived", rblKidneyReceived.SelectedValue, STRCONN);
                throw new Exception(" " + rblKidneyReceived.SelectedValue + " Kidney from ET Donor/ NHSBT Number " + strDonorID + " has already been added for TrialID " + txtTrialID.Text + ". Please Select another Donor/Kidney combination.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Data for " + rblKidneyReceived.SelectedValue + " Kidney already exists for this TrialID.");
            }


            //STRSQL_FIND = "SELECT COUNT(*) CR FROM machineperfusion WHERE TrialID=?TrialID AND KidneyReceived=?KidneyReceived AND KidneyOnMachine='YES' ";
            STRSQL_FIND = "SELECT COUNT(*) CR FROM machineperfusion WHERE TrialID=?TrialID AND Side=?KidneyReceived AND KidneyOnMachine='YES' ";
            
            intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", txtTrialID.Text, "?KidneyReceived", rblKidneyReceived.SelectedValue, STRCONN));


            string strTrialIDRecipient = txtTrialID.Text + rblKidneyReceived.SelectedValue.Substring(0, 1);
            if (intCountFind == 0)
            {
                throw new Exception("Machine Perfusion data has not been added for " + rblKidneyReceived.SelectedValue +  " from " +  txtTrialID.Text + ". Please add the data first.");

            }
            if (intCountFind < 0)
            {
                throw new Exception("An Error occured while checking if Machine Perfusion data has not been added for " + rblKidneyReceived.SelectedValue + " from " + txtTrialID.Text + ".");

            }

            //check if kidney has been assigned
            STRSQL_FIND = "SELECT COUNT(*) CR FROM trialdetails_recipient WHERE TrialIDRecipient=?TrialIDRecipient ";

            intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialIDRecipient", strTrialIDRecipient, STRCONN));

            if (intCountFind > 0)
            {
                throw new Exception("TrialID Recipient " + strTrialIDRecipient + " has already been added.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if " + strTrialIDRecipient + " has already been added to the database.");
            }

            //now add the data
            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO trialdetails_recipient ";
            STRSQL += "(TrialID, KidneyReceived, TrialIDRecipient, TransplantCentre, RecipientInformedConsent, Recipient18Year, RecipientMultipleDualTransplant,  ";
            STRSQL += "Active,  ";
            STRSQL += "DataLocked, DateLocked, LockedBy, ";
            STRSQL += "";
            STRSQL += "DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?KidneyReceived, ?TrialIDRecipient, ?TransplantCentre, ?RecipientInformedConsent, ?Recipient18Year, ?RecipientMultipleDualTransplant,  ";
            STRSQL += "?Active,  ";
            STRSQL += "?DataLocked, ?DateLocked, ?LockedBy, ";
            STRSQL += "";
            STRSQL += "?DateCreated, ?CreatedBy) ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            MyCMD.CommandText = STRSQL;



            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = txtTrialID.Text;
            MyCMD.Parameters.Add("?KidneyReceived", MySqlDbType.VarChar).Value = rblKidneyReceived.SelectedValue;
            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = strTrialIDRecipient;
            //MyCMD.Parameters.Add("?TransplantCentre", MySqlDbType.VarChar).Value = txtTransplantCentre.Text.Trim();
            MyCMD.Parameters.Add("?TransplantCentre", MySqlDbType.VarChar).Value = ddTransplantCentre.SelectedValue;


            if (rblRecipientInformedConsent.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?RecipientInformedConsent", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientInformedConsent", MySqlDbType.VarChar).Value = rblRecipientInformedConsent.SelectedValue;
            }

            if (rblRecipient18Year.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?Recipient18Year", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Recipient18Year", MySqlDbType.VarChar).Value = rblRecipient18Year.SelectedValue;
            }

            if (rblRecipientMultipleDualTransplant.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?RecipientMultipleDualTransplant", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RecipientMultipleDualTransplant", MySqlDbType.VarChar).Value = rblRecipientMultipleDualTransplant.SelectedValue;
            }

            


            if (rblRecipientInformedConsent.SelectedValue == STR_NO_SELECTION || rblRecipient18Year.SelectedValue == STR_NO_SELECTION || rblRecipientMultipleDualTransplant.SelectedValue == STR_YES_SELECTION)
            {

                MyCMD.Parameters.Add("?Active", MySqlDbType.VarChar).Value = INT_INACTIVE.ToString();
            }
            else
            {
                MyCMD.Parameters.Add("?Active", MySqlDbType.VarChar).Value = INT_ACTIVE.ToString();

            }

            


            DateTime dteDateTimeNow = DateTime.Now;

            MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = INT_DATALOCKED.ToString();
            MyCMD.Parameters.Add("?DateLocked", MySqlDbType.DateTime).Value = dteDateTimeNow;
            MyCMD.Parameters.Add("?LockedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = dteDateTimeNow;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;


            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                BindData(strTrialIDRecipient);

                lblUserMessages.Text = "Recipient Added.";

                if (rblRecipientInformedConsent.SelectedValue == STR_NO_SELECTION || rblRecipient18Year.SelectedValue == STR_NO_SELECTION || rblRecipientMultipleDualTransplant.SelectedValue == STR_YES_SELECTION)
                {
                    lblGV1.Text = ConstantsGeneral.RecipientConsentMessage + strTrialIDRecipient;
                }
                pnlMain.Visible = false;
                pnlTrialIDRecipient.Visible = true;

            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                
                lblUserMessages.Text = ex.Message + " An error occured while executing insert query.";
            }


            //finally //close connection
            //{
            //    if (MyCONN.State == ConnectionState.Open)
            //    { MyCONN.Close(); }
            //}

            


        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }


    protected void BindData(string strTrialIDRecipient)
    {
        try
        {
            string STRSQL = string.Empty;
            STRSQL += "SELECT * ";
            //STRSQL += "DATE_FORMAT(DateOfBirth, '%d/%m/%Y') Date_OfBirth ";
            STRSQL += "FROM trialdetails_recipient ";
            STRSQL += "WHERE TrialIDRecipient = ?TrialIDRecipient ";
            //STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialIDRecipient", strTrialIDRecipient);

            sqldsGV1.SelectCommand = STRSQL;
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();
            lblGV1.Text = "Recipient Main Details. Please click on TrialID (Recipient) to add Recipient Data";
        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding data.";
        }

        //lblUserMessages.Text = strSQL;
    }

    protected void rblRecipientInformedConsent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRecipientInformedConsent.SelectedValue == STR_NO_SELECTION)
        {
            lblRecipientInformedConsentMessage.Text = ConstantsGeneral.RecipientConsentMessage;

            pnlInclusionCriteria.Visible = false;

        }
        else
        {
            lblRecipientInformedConsentMessage.Text = string.Empty;

            pnlInclusionCriteria.Visible = true;
        }
    }
    protected void rblRecipient18Year_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRecipient18Year.SelectedValue == STR_NO_SELECTION)
        {
            lblRecipient18YearMessage.Text = ConstantsGeneral.RecipientInclusionMessage;
        }
        else
        {
            lblRecipient18YearMessage.Text = string.Empty;
        }
    }
    protected void rblRecipientMultipleDualTransplant_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRecipientMultipleDualTransplant.SelectedValue == STR_NO_SELECTION)
        {
            lblRecipientMultipleDualTransplantMessage.Text = ConstantsGeneral.RecipientInclusionMessage;
        }
        else
        {
            lblRecipientMultipleDualTransplantMessage.Text = string.Empty;
        }
    }
}