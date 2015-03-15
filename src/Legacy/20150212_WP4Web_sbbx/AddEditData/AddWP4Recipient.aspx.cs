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

public partial class AddEditData_AddWP4Recipient : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";

    private const string strMainCPH = "cplMainContents";
    private const string strMainLabel = "lblDonorID";

    private const string STR_OTHER_SELECTION = "Other";
    private const string STR_UNKNOWN_SELECTION = "Unknown";
    private const string STR_NO_SELECTION = "NO";
    private const string STR_YES_SELECTION = "YES";

    private const string STR_DD_UNKNOWN_SELECTION = "0";

    private const int intMaxAge = 100;
    private const int intMinAge = 18;

    //static Random _random = new Random();

    #endregion

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsPostBack)
            {
                //if (String.IsNullOrEmpty(Request.QueryString["TID"]))
                //{
                //    throw new Exception("Could not obtain TrialID.");
                //}

                string STRSQL = "SELECT t1.TrialID, CONCAT(t1.TrialID, ' --> DonorID ', t1.DonorID) TrialIDDetails FROM trialdetails t1 WHERE t1.TrialID LIKE ?TrialID ORDER BY t1.TrialID";

                string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");

                sqldsTrialID.SelectCommand = STRSQL;
                sqldsTrialID.SelectParameters.Clear();
                sqldsTrialID.SelectParameters.Add("?TrialID", strTrialIDLeadingCharacters + SessionVariablesAll.CentreCode + "%");

                ddTrialID.DataSource = sqldsTrialID;
                ddTrialID.DataBind();


                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                

                rblKidneyReceived.DataSource = XMLKidneySidesDataSource;
                rblKidneyReceived.DataBind();
                rblKidneyReceived.SelectedValue = STR_UNKNOWN_SELECTION;

                rblSex.DataSource = XMLSexDataSource;
                rblSex.DataBind();
                rblSex.SelectedValue = STR_UNKNOWN_SELECTION;

                rblEthnicityBlack.DataSource = XMLMainOptionsDataSource;
                rblEthnicityBlack.DataBind();
                rblEthnicityBlack.SelectedValue = STR_UNKNOWN_SELECTION;

                ddRenalDisease.DataSource = XMLRenalDiseasesDataSource;
                ddRenalDisease.DataBind();

                //rblDialysisType.DataSource = XMLDialysisTypesDataSource;
                //rblDialysisType.DataBind();
                //rblDialysisType.SelectedValue = STR_UNKNOWN_SELECTION;

                rblBloodGroup.DataSource = XmlBloodGroupDataSource;
                rblBloodGroup.DataBind();
                rblBloodGroup.SelectedValue = STR_UNKNOWN_SELECTION;

                rblHLA_A.DataSource = XMLHLAMismatchDataSource;
                rblHLA_A.DataBind();
                rblHLA_A.SelectedValue = STR_UNKNOWN_SELECTION;

                rblHLA_B.DataSource = XMLHLAMismatchDataSource;
                rblHLA_B.DataBind();
                rblHLA_B.SelectedValue = STR_UNKNOWN_SELECTION;

                rblHLA_DR.DataSource = XMLHLAMismatchDataSource;
                rblHLA_DR.DataBind();
                rblHLA_DR.SelectedValue = STR_UNKNOWN_SELECTION;

                rblET_Urgency.DataSource = XMLETUrgenciesDataSource;
                rblET_Urgency.DataBind();
                rblET_Urgency.SelectedValue = STR_UNKNOWN_SELECTION;

                rblHLA_A_Mismatch.DataSource = XMLHLAMismatchDataSource;
                rblHLA_A_Mismatch.DataBind();
                rblHLA_A_Mismatch.SelectedValue = STR_UNKNOWN_SELECTION;

                rblHLA_B_Mismatch.DataSource = XMLHLAMismatchDataSource;
                rblHLA_B_Mismatch.DataBind();
                rblHLA_B_Mismatch.SelectedValue = STR_UNKNOWN_SELECTION;

                rblHLA_DR_Mismatch.DataSource = XMLHLAMismatchDataSource;
                rblHLA_DR_Mismatch.DataBind();
                rblHLA_DR_Mismatch.SelectedValue = STR_UNKNOWN_SELECTION;

                ddMobility.DataSource = XMLQOLScoresDataSource;
                ddMobility.DataBind();

                ddSelfCare.DataSource = XMLQOLScoresDataSource;
                ddSelfCare.DataBind();

                ddUsualActivities.DataSource = XMLQOLScoresDataSource;
                ddUsualActivities.DataBind();

                ddPainDiscomfort.DataSource = XMLQOLScoresDataSource;
                ddPainDiscomfort.DataBind();

                ddAnxietyDepression.DataSource = XMLQOLScoresDataSource;
                ddAnxietyDepression.DataBind();

                //cmdDelete.Enabled = false;
                //cmdDelete.Visible = false;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                //BindData();

                lblDescription.Text = "Please Enter RecipientID Details.";
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
        }
    }

    protected void BindData(string strTrialIDRecipient)
    {
        try
        {
            string STRSQL = string.Empty;
            STRSQL += "SELECT *, ";
            STRSQL += "DATE_FORMAT(DateOfBirth, '%d/%m/%Y') Date_OfBirth ";
            STRSQL += "FROM r_identification ";
            STRSQL += "WHERE TrialIDRecipient = ?TrialIDRecipient ";
            //STRSQL += "ORDER BY " + ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialIDRecipient", strTrialIDRecipient);

            sqldsGV1.SelectCommand = STRSQL;
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();
            lblGV1.Text = "Recipient ID Details";
        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding data.";
        }

        //lblUserMessages.Text = strSQL;
    }

    //sorting main datagrid
    //protected void GV1_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
    //    {
    //        switch (ViewState["SortDirection"].ToString())
    //        {
    //            case "ASC":
    //                ViewState["SortDirection"] = "DESC";
    //                break;
    //            case "DESC":
    //                ViewState["SortDirection"] = "ASC";
    //                break;
    //        }

    //    }
    //    else
    //    {
    //        ViewState["SortField"] = e.SortExpression;
    //        ViewState["SortDirection"] = "DESC";
    //    }
    //    BindData();
    //}

    protected void AssignData()
    {

        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.*, t2.DonorID Donor, t2.DateOfBirthDonor FROM  r_identification t1 ";
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

                            //if (!DBNull.Value.Equals(myDr["KidneyReceived"]))
                            //{
                            //    rblKidneyReceived.SelectedValue = (string)(myDr["KidneyReceived"]);

                            //}

                            if (!DBNull.Value.Equals(myDr["RecipientID"]))
                            {
                                txtRecipientID.Text = (string)(myDr["RecipientID"]);

                            }

                            if (!DBNull.Value.Equals(myDr["DateOfBirth"]))
                            {

                                if (GeneralRoutines.IsDate(myDr["DateOfBirth"].ToString()))
                                {
                                    txtRecipientDateOfBirth.Text = Convert.ToDateTime(myDr["DateOfBirth"]).ToString("dd/MM/yyyy");

                                }

                            }

                            if (!DBNull.Value.Equals(myDr["Sex"]))
                            {
                                rblSex.SelectedValue = (string)(myDr["Sex"]);

                            }

                            if (!DBNull.Value.Equals(myDr["Weight"]))
                            {
                                txtWeight.Text = (string)(myDr["Weight"]);
                            }



                            if (!DBNull.Value.Equals(myDr["Height"]))
                            {
                                txtHeight.Text = (string)(myDr["Height"]);
                            }

                            if (!DBNull.Value.Equals(myDr["EthnicityBlack"]))
                            {
                                rblEthnicityBlack.SelectedValue = (string)(myDr["EthnicityBlack"]);
                            }

                            if (!DBNull.Value.Equals(myDr["RenalDisease"]))
                            {
                                ddRenalDisease.SelectedValue = (string)(myDr["RenalDisease"]);
                            }

                            if (!DBNull.Value.Equals(myDr["RenalDiseaseOther"]))
                            {
                                txtRenalDiseaseOther.Text = (string)(myDr["RenalDiseaseOther"]);
                            }

                            //if (!DBNull.Value.Equals(myDr["DateOnDialysisSince"]))
                            //{
                            //    if (GeneralRoutines.IsDate(myDr["DateOnDialysisSince"].ToString()))
                            //    {
                            //        txtDateOnDialysisSince.Text = Convert.ToDateTime(myDr["DateOnDialysisSince"]).ToString("dd/MM/yyyy");

                            //    }
                            //}

                            //if (!DBNull.Value.Equals(myDr["DialysisType"]))
                            //{
                            //    rblDialysisType.SelectedValue = (string)(myDr["DialysisType"]);
                            //}

                            if (!DBNull.Value.Equals(myDr["PreTransplantDiuresis"]))
                            {
                                txtPreTransplantDiuresis.Text = (string)(myDr["PreTransplantDiuresis"]);
                            }

                            //if (!DBNull.Value.Equals(myDr["PannelReactiveAntibodies"]))
                            //{
                            //    txtPannelReactiveAntibodies.Text = (string)(myDr["PannelReactiveAntibodies"]);
                            //}


                            if (!DBNull.Value.Equals(myDr["BloodGroup"]))
                            {
                                rblBloodGroup.SelectedValue = (string)(myDr["BloodGroup"]);

                            }

                            //if (!DBNull.Value.Equals(myDr["NumberPreviousTransplants"]))
                            //{
                            //    txtNumberPreviousTransplants.Text = (string)(myDr["NumberPreviousTransplants"]);
                            //}


                            if (!DBNull.Value.Equals(myDr["HLA_A"]))
                            {
                                rblHLA_A.SelectedValue = (string)(myDr["HLA_A_Mismatch"]);
                            }

                            if (!DBNull.Value.Equals(myDr["HLA_B"]))
                            {
                                rblHLA_B.SelectedValue = (string)(myDr["HLA_B"]);
                            }

                            if (!DBNull.Value.Equals(myDr["HLA_DR"]))
                            {
                                rblHLA_DR.SelectedValue = (string)(myDr["HLA_DR"]);
                            }

                            if (!DBNull.Value.Equals(myDr["HLA_A_Mismatch"]))
                            {
                                //txtHLA_A_Mismatch.Text = (string)(myDr["HLA_A_Mismatch"]);
                                rblHLA_A_Mismatch.SelectedValue = (string)(myDr["HLA_A_Mismatch"]);
                            }

                            if (!DBNull.Value.Equals(myDr["HLA_B_Mismatch"]))
                            {
                                //txtHLA_B_Mismatch.Text = (string)(myDr["HLA_B_Mismatch"]);
                                rblHLA_B_Mismatch.SelectedValue = (string)(myDr["HLA_B_Mismatch"]);
                            }

                            if (!DBNull.Value.Equals(myDr["HLA_DR_Mismatch"]))
                            {
                                //txtHLA_DR_Mismatch.Text = (string)(myDr["HLA_DR_Mismatch"]);
                                rblHLA_DR_Mismatch.SelectedValue = (string)(myDr["HLA_DR_Mismatch"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Mobility"]))
                            {
                                ddMobility.SelectedValue = myDr["Mobility"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["SelfCare"]))
                            {
                                ddSelfCare.SelectedValue = myDr["SelfCare"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["UsualActivities"]))
                            {
                                ddUsualActivities.SelectedValue = myDr["UsualActivities"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["PainDiscomfort"]))
                            {
                                ddPainDiscomfort.SelectedValue = myDr["PainDiscomfort"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["AnxietyDepression"]))
                            {
                                ddAnxietyDepression.SelectedValue = myDr["AnxietyDepression"].ToString();
                            }


                            if (!DBNull.Value.Equals(myDr["VASScore"]))
                            {
                                txtVASScore.Text = myDr["VASScore"].ToString();
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

    // add data
    protected void cmdAddData_Click(object sender, EventArgs e)
    {

        try
        {
            lblUserMessages.Text = "";

            if (ddTrialID.SelectedValue == "0")
            {
                throw new Exception("Please Select a Donor TrialID");
            }
            if (rblKidneyReceived.SelectedValue == STR_UNKNOWN_SELECTION)
            { throw new Exception("Please Select Side of the Kidney received."); }


            if (txtRecipientID.Text == string.Empty)
            {
                throw new Exception("Please enter the Recipient ID.");
            }

            txtRecipientID.Text.Trim();


            if (GeneralRoutines.IsDate(txtRecipientDateOfBirth.Text) == false)
            {
                throw new Exception("Please enter the Date of Birth of the Recipient.");
            }

            //if (Convert.ToDateTime(txtRecipientDateOfBirth.Text) > DateTime.Today)
            //{
            //    throw new Exception("Date of Birth of Recipient cannot be greater than Today's date.");
            //}

            if (Convert.ToDateTime(txtRecipientDateOfBirth.Text) > DateTime.Now.AddYears(-intMinAge))
            {
                throw new Exception("The minimum age of the Recipient should at least be " + intMinAge.ToString() + " years.");
            }

            if (rblSex.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select Recipient's Sex.");
            }

            if (txtHeight.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtHeight.Text) == false)
                {
                    throw new Exception("Height should be numeric.");
                }
            }

            if (txtWeight.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtWeight.Text) == false)
                {
                    throw new Exception("Weight should be numeric.");
                }
            }


            if (rblEthnicityBlack.SelectedValue == STR_OTHER_SELECTION)
            {
                throw new Exception("Please Select YES/NO for Recipient's Ethnicity if Black.");
            }

            if (ddRenalDisease.SelectedValue == "0")
            {
                throw new Exception("Please Select Renal Disease.");
            }
            
            //if (txtNumberPreviousTransplants.Text != string.Empty)
            //{
            //    if (GeneralRoutines.IsNumeric(txtNumberPreviousTransplants.Text) == false)
            //    {
            //        throw new Exception("Please Enter Number of previous transplants in numeric format.");

            //    }
            //}

            if (txtVASScore.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtVASScore.Text) == false)
                {
                    throw new Exception("Please Enter value for 'VAS Score' in Numeric Format.");
                }

                if (Convert.ToInt16(txtVASScore.Text) < 0)
                {
                    throw new Exception("'VAS Score' cannot be less than 0.");
                }

                if (Convert.ToInt16(txtVASScore.Text) > 100)
                {
                    if (Convert.ToInt16(txtVASScore.Text) != 999)
                    {
                        throw new Exception("'VAS Score' cannot be greater than 100 (Only exception is 999 for missing value).");
                    }
                }

            }

            //if (rblHLA_A.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA A Typing.");
            //}

            //if (rblHLA_B.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA B Typing.");
            //}

            //if (rblHLA_DR.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA DR Typing.");
            //}

            //if (rblET_Urgency.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'ET Urgency");
            //}

            //if (rblHLA_A_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA A Mismatch");
            //}

            //if (rblHLA_B_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA B Mismatch");
            //}

            //if (rblHLA_DR_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select 'HLA DR Mismatch");
            //}


            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_identification WHERE TrialID=?TrialID AND KidneyReceived=?KidneyReceived ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", ddTrialID.SelectedValue, "?KidneyReceived", rblKidneyReceived.SelectedValue, STRCONN));


            if (intCountFind > 0)
            {
                STRSQL_FIND = "SELECT RecipientID FROM r_identification WHERE TrialID=?TrialID AND KidneyReceived=?KidneyReceived ";
                string strRecipientID=GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", ddTrialID.SelectedValue, "?KidneyReceived", rblKidneyReceived.SelectedValue, STRCONN);
                throw new Exception("Recipient " + strRecipientID + " has already been added for TrialID " +  ddTrialID.SelectedValue + " with " + rblKidneyReceived.SelectedValue + " Kidney. Please Select another Donor/Kidney combination.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Data for " + rblKidneyReceived.SelectedValue + " Kidney already exists for this TrialID.");
            }

            //check if recipient already exists in Europe if a Europe donor/ UK if a UK donor. Country Code for UK is 3
            STRSQL_FIND = "SELECT COUNT(*) CR FROM r_identification WHERE RecipientID=?RecipientID AND TrialID LIKE '" + Request.QueryString["TID"].ToString().Substring(0,4) + "%'";

            //if (SessionVariablesAll.CentreCode.Substring(0, 1) == "1")
            //{
            //    STRSQL_FIND += "AND TrialID LIKE 'WP41%' ";
            //}
            //else
            //{
            //    STRSQL_FIND += "AND TrialID NOT LIKE 'WP41%' ";
            //}

            intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?RecipientID", txtRecipientID.Text, STRCONN));

            if (intCountFind > 0)
            {
                //get the trialid for the RecipientID
                STRSQL_FIND = "SELECT TrialID FROM r_identification WHERE RecipientID=?RecipientID AND TrialID LIKE '" + Request.QueryString["TID"].ToString().Substring(0, 4) + "%'";

                string strTrialIDRecipientID = string.Empty;
                strTrialIDRecipientID = GeneralRoutines.ReturnScalar(STRSQL_FIND, "?RecipientID", txtRecipientID.Text, STRCONN);
                throw new Exception("RecipientID " + txtRecipientID.Text + " has already been associated with " + strTrialIDRecipientID + ". Please check the RecipientID you are adding.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if another RecipientID exists.");
            }


            //now add the data
            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO r_identification ";
            STRSQL += "(TrialID, KidneyReceived, TrialIDRecipient, RecipientID, DateOfBirth, Sex, Weight,  Height, BMI, EthnicityBlack,  ";
            //STRSQL += "RenalDisease, RenalDiseaseOther, NumberPreviousTransplants, PretransplantDiuresis, BloodGroup,  ";
            STRSQL += "RenalDisease, RenalDiseaseOther, PretransplantDiuresis, BloodGroup,  ";
            STRSQL += "HLA_A, HLA_B, HLA_DR, ET_Urgency,  HLA_A_Mismatch, HLA_B_Mismatch, HLA_DR_Mismatch, ";
            STRSQL += "Mobility, SelfCare, UsualActivities, PainDiscomfort, AnxietyDepression, VASScore,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?KidneyReceived, ?TrialIDRecipient, ?RecipientID, ?DateOfBirth, ?Sex, ?Weight, ?Height, ?BMI, ?EthnicityBlack,  ";
            //STRSQL += "?RenalDisease, ?RenalDiseaseOther, ?NumberPreviousTransplants, ?PretransplantDiuresis, ?BloodGroup,  ";
            STRSQL += "?RenalDisease, ?RenalDiseaseOther, ?PretransplantDiuresis, ?BloodGroup,  ";
            STRSQL += "?HLA_A, ?HLA_B, ?HLA_DR, ?ET_Urgency,  ?HLA_A_Mismatch, ?HLA_B_Mismatch, ?HLA_DR_Mismatch, ";
            STRSQL += "?Mobility, ?SelfCare, ?UsualActivities, ?PainDiscomfort, ?AnxietyDepression, ?VASScore,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            //string STRSQL_UPDATE = String.Empty;
            //STRSQL_UPDATE += "UPDATE r_identification SET ";
            //STRSQL_UPDATE += "RecipientID=?RecipientID,  DateOfBirth=?DateOfBirth, Sex=?Sex, Weight=?Weight,   ";
            //STRSQL_UPDATE += "Height=?Height, BMI=?BMI, EthnicityBlack=?EthnicityBlack, RenalDisease=?RenalDisease, RenalDiseaseOther=?RenalDiseaseOther, ";
            //STRSQL_UPDATE += "DateOnDialysisSince=?DateOnDialysisSince, DialysisType=?DialysisType, PreTransplantUOPer24hr=?PreTransplantUOPer24hr, ";
            //STRSQL_UPDATE += "NumberPreviousTransplants=?NumberPreviousTransplants, BloodGroup=?BloodGroup, PannelReactiveAntibodies=?PannelReactiveAntibodies,";
            ////STRSQL_UPDATE += "HLA_A=?HLA_A, HLA_B=?HLA_B, HLA_DR=?HLA_DR,";
            //STRSQL_UPDATE += "HLA_A_Mismatch=?HLA_A_Mismatch, HLA_B_Mismatch=?HLA_B_Mismatch, HLA_DR_Mismatch=?HLA_DR_Mismatch,Comments=?Comments,";
            //STRSQL_UPDATE += "";
            //STRSQL_UPDATE += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            //STRSQL_UPDATE += "WHERE TrialID=?TrialID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            MyCMD.CommandText = STRSQL;



            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = ddTrialID.SelectedValue;

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = ddTrialID.SelectedValue + rblKidneyReceived.SelectedValue.Substring(0,1);

            MyCMD.Parameters.Add("?KidneyReceived", MySqlDbType.VarChar).Value = rblKidneyReceived.SelectedValue;

            MyCMD.Parameters.Add("?RecipientID", MySqlDbType.VarChar).Value = txtRecipientID.Text;

            MyCMD.Parameters.Add("?DateOfBirth", MySqlDbType.Date).Value = Convert.ToDateTime(txtRecipientDateOfBirth.Text);


            MyCMD.Parameters.Add("?Sex", MySqlDbType.VarChar).Value = rblSex.SelectedValue;

            if (txtHeight.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Height", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Height", MySqlDbType.VarChar).Value = txtHeight.Text;
            }

            if (txtWeight.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Weight", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Weight", MySqlDbType.VarChar).Value = txtWeight.Text;
            }

            //if (txtAdmissionWeight.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?AdmissionWeight", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?AdmissionWeight", MySqlDbType.VarChar).Value = txtAdmissionWeight.Text;
            //}

            //bmi uses admission weight
            if (GeneralRoutines.IsNumeric(txtHeight.Text) && GeneralRoutines.IsNumeric(txtHeight.Text))
            { MyCMD.Parameters.Add("?BMI", MySqlDbType.VarChar).Value = Math.Round(Convert.ToDouble(txtWeight.Text) / (Convert.ToDouble(txtHeight.Text)/100 * Convert.ToDouble(txtHeight.Text)/100), 3); }
            else
            { MyCMD.Parameters.Add("?BMI", MySqlDbType.VarChar).Value = DBNull.Value; }

            if (rblEthnicityBlack.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?EthnicityBlack", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?EthnicityBlack", MySqlDbType.VarChar).Value = rblEthnicityBlack.SelectedValue;
            }

            if (ddRenalDisease.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RenalDisease", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RenalDisease", MySqlDbType.VarChar).Value = ddRenalDisease.SelectedValue;
            }

            if (txtRenalDiseaseOther.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RenalDiseaseOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RenalDiseaseOther", MySqlDbType.VarChar).Value = txtRenalDiseaseOther.Text;
            }

            //if (txtNumberPreviousTransplants.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?NumberPreviousTransplants", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?NumberPreviousTransplants", MySqlDbType.VarChar).Value = txtNumberPreviousTransplants.Text;
            //}

            if (txtPreTransplantDiuresis.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PreTransplantDiuresis", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PreTransplantDiuresis", MySqlDbType.VarChar).Value = txtPreTransplantDiuresis.Text;
            }

            if (rblBloodGroup.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?BloodGroup", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?BloodGroup", MySqlDbType.VarChar).Value = rblBloodGroup.SelectedValue;
            }

            if (rblHLA_A.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?HLA_A", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_A", MySqlDbType.VarChar).Value = rblHLA_A.SelectedValue;
            }

            if (rblHLA_B.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?HLA_B", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_B", MySqlDbType.VarChar).Value = rblHLA_B.SelectedValue;
            }

            if (rblHLA_DR.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?HLA_DR", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_DR", MySqlDbType.VarChar).Value = rblHLA_DR.SelectedValue;
            }

            if (rblET_Urgency.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ET_Urgency", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ET_Urgency", MySqlDbType.VarChar).Value = rblET_Urgency.SelectedValue;
            }

            if (rblHLA_A_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?HLA_A_Mismatch", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_A_Mismatch", MySqlDbType.VarChar).Value = rblHLA_A_Mismatch.SelectedValue;
            }

            if (rblHLA_B_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?HLA_B_Mismatch", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_B_Mismatch", MySqlDbType.VarChar).Value = rblHLA_B_Mismatch.SelectedValue;
            }

            if (rblHLA_DR_Mismatch.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?HLA_DR_Mismatch", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?HLA_DR_Mismatch", MySqlDbType.VarChar).Value = rblHLA_DR_Mismatch.SelectedValue;
            }

            if (ddMobility.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?Mobility", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Mobility", MySqlDbType.VarChar).Value = ddMobility.SelectedValue;
            }

            if (ddSelfCare.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?SelfCare", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SelfCare", MySqlDbType.VarChar).Value = ddSelfCare.SelectedValue;
            }

            if (ddUsualActivities.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?UsualActivities", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?UsualActivities", MySqlDbType.VarChar).Value = ddUsualActivities.SelectedValue;
            }

            if (ddPainDiscomfort.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?PainDiscomfort", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PainDiscomfort", MySqlDbType.VarChar).Value = ddPainDiscomfort.SelectedValue;
            }

            if (ddAnxietyDepression.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?AnxietyDepression", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AnxietyDepression", MySqlDbType.VarChar).Value = ddAnxietyDepression.SelectedValue;
            }

            if (txtVASScore.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?VASScore", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?VASScore", MySqlDbType.VarChar).Value = txtVASScore.Text;
            }

            if (string.IsNullOrEmpty(txtComments.Text))
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

            }

            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing insert/update query.";
            }


            finally //close connection
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }

            BindData(ddTrialID.SelectedValue + rblKidneyReceived.SelectedValue.Substring(0, 1));

            lblUserMessages.Text = "RecipientID Added.";

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }

    protected void ddTrialID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            lblDonorIDDetails.Text = string.Empty;

            if (ddTrialID.SelectedValue != "0")
            {
                //populate txtDonorID
                string STRSQL = string.Empty; 
                STRSQL +="SELECT t1.TrialID, t1.DonorID, t1.DateOfBirthDonor, t2.KidneyLeftDonated, t2.KidneyRightDonated ";
                STRSQL += "FROM trialdetails t1  ";
                STRSQL += "LEFT JOIN donor_identification t2 ON t1.TrialID=t2.TrialID ";
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

                MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = ddTrialID.SelectedValue;

                MyCONN.Open();

                try
                {
                    using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                    {
                        if (myDr.HasRows)
                        {
                            while (myDr.Read())
                            {
                                lblDonorIDDetails.Text = "DonorID- ";
                                if (!DBNull.Value.Equals(myDr["DonorID"]))
                                {
                                    lblDonorIDDetails.Text +="<b>" + (string)(myDr["DonorID"]) + "</b>";
                                }
                                lblDonorIDDetails.Text += "; Left - ";
                                if (!DBNull.Value.Equals(myDr["KidneyLeftDonated"]))
                                {
                                    lblDonorIDDetails.Text += "<b>" + (string)(myDr["KidneyLeftDonated"]) + "</b>";
                                }
                                else
                                {
                                    lblDonorIDDetails.Text += "<b>Not Entered</b>";
                                }

                                lblDonorIDDetails.Text += "; Right - ";
                                if (!DBNull.Value.Equals(myDr["KidneyRightDonated"]))
                                {
                                    lblDonorIDDetails.Text += "<b>" + (string)(myDr["KidneyRightDonated"]) + "</b>";
                                }
                                else
                                {
                                    lblDonorIDDetails.Text += "<b>Not Entered</b>";
                                }
                            }
                        }
                    }

                    if (MyCONN.State == ConnectionState.Open)
                    { 
                        MyCONN.Close(); 
                    }
                }

                catch (System.Exception ex)
                {
                    if (MyCONN.State == ConnectionState.Open)
                    { 
                        MyCONN.Close(); 
                    }
                    lblUserMessages.Text = ex.Message + " An error occured while assiging Donor Data";
                }

                //recipient details add

                string STRSQL_R = string.Empty;
                STRSQL_R += "SELECT t1.TrialID, t1.DonorID, t2.TrialIDRecipient, t2.RecipientID, t2.KidneyReceived ";
                STRSQL_R += "FROM trialdetails t1  ";
                STRSQL_R += "INNER JOIN r_identification t2 ON t1.TrialID=t2.TrialID ";
                STRSQL_R += "WHERE t1.TrialID=?TrialID ORDER BY KidneyReceived ";

                MyCMD.CommandText = STRSQL_R;

                if (MyCONN.State == ConnectionState.Closed)
                {
                    MyCONN.Open();
                }


                try
                {
                    using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                    {
                        if (myDr.HasRows)
                            lblDonorIDDetails.Text += " ; Recipient- ";
                        {
                            int iLoop = 0;

                            while (myDr.Read())
                            {
                                
                                if (!DBNull.Value.Equals(myDr["TrialIDRecipient"]))
                                {
                                    lblDonorIDDetails.Text += " " + (string)(myDr["TrialIDRecipient"]) + " ";
                                }

                                if (!DBNull.Value.Equals(myDr["RecipientID"]))
                                {
                                    lblDonorIDDetails.Text += " " + (string)(myDr["RecipientID"]) + " ";
                                }

                                if (!DBNull.Value.Equals(myDr["KidneyReceived"]))
                                {
                                    lblDonorIDDetails.Text += "<b>" + (string)(myDr["KidneyReceived"]) + "</b>";
                                }

                                if (iLoop == 0)
                                {
                                    lblDonorIDDetails.Text += "; ";
                                }
                                iLoop += 1;
                            }
                        }
                    }

                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }
                }

                catch (System.Exception ex)
                {
                    if (MyCONN.State == ConnectionState.Open)
                    {
                        MyCONN.Close();
                    }
                    lblUserMessages.Text += ex.Message + " An error occured while assiging Recipient Details";
                }

            }
            else
            {
                lblDonorIDDetails.Text = string.Empty;
            }
        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting a TrialID.";
        }
    }
}