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
public partial class SpecClinicalRecipientPage : System.Web.UI.MasterPage
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";
    private const string strRedirectDefault = "~/Default.aspx";

    private const string strRedirectOwn = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";

    private const string strRedirectError = "~/OtherArea/AccessDenied.aspx?EID=";

    private const string STR_YES_SELECTION = "YES";
    private const string strAccessDeniedAddEdit = "~/OtherArea/AccessDenied.aspx?EID=61";

    private const string strRedirectError91 = "~/OtherArea/AccessDenied.aspx?EID=91";

    private const string strRemoveMenuItem = "mnuAddRTrial";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(SessionVariablesAll.Web_cope41))
            {
                Response.Redirect(strRedirectDefault, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            if (SessionVariablesAll.Web_cope41 != "True")
            {
                Response.Redirect(strRedirectDefault, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            ////redirect is AddEdit <> Yes
            //if (string.IsNullOrEmpty(SessionVariablesAll.AddEdit))
            //{
            //    Response.Redirect(strAccessDeniedAddEdit, false);
            //    HttpContext.Current.ApplicationInstance.CompleteRequest();
            //}

            //if (SessionVariablesAll.AddEdit != STR_YES_SELECTION)
            //{
            //    Response.Redirect(strAccessDeniedAddEdit, false);
            //    HttpContext.Current.ApplicationInstance.CompleteRequest();
            //}

            if (String.IsNullOrEmpty(Request.QueryString["TID"]))
            { throw new Exception("Could not obtain the TrialID."); }

            if (String.IsNullOrEmpty(Request.QueryString["TID_R"]))
            { throw new Exception("Could not obtain the TrialID (Recipient)."); }

            //if (Request.QueryString["TID_R"].Length > 1)
            //{
            //    if (Request.QueryString["TID_R"].Substring(3, 1) != SessionVariablesAll.CentreCode)
            //    {
            //        //throw new Exception("The first characters of the TrialID (Recipient) does not match the Centre Code.");
            //        Response.Redirect(strRedirectError + "2" + "&TID_R=" + Request.QueryString["TID_R"], false);
            //        HttpContext.Current.ApplicationInstance.CompleteRequest();

            //    }

            //}
            //else
            //{
            //    throw new Exception("Please Check the TrialID (Recipient).");
            //}

            //check if trialID exists
            if (Request.QueryString["TID_R"].Length > 1)
            {

                string STRSQLTrialID = "SELECT COUNT(*) FROM trialdetails WHERE TrialID=?TrialID ";
                Int32 intFindTrialID = 0;
                intFindTrialID = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLTrialID, "?TrialID", Request.QueryString["TID"], STRCONN));

                if (intFindTrialID == 0)
                {
                    Response.Redirect(strRedirectError + "71" + "&TID=" + Request.QueryString["TID"], false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }


                if (intFindTrialID > 1)
                {
                    Response.Redirect(strRedirectError + "72" + "&TID=" + Request.QueryString["TID"], false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                string STRSQLTrialID_Recipient = "SELECT COUNT(*) FROM trialdetails_recipient WHERE TrialID=?TrialID AND TrialIDRecipient=?TrialIDRecipient ";

                Int32 intFindTrialIDRecipient = 0;
                intFindTrialIDRecipient = Convert.ToInt32(GeneralRoutines.ReturnScalarTwo(STRSQLTrialID_Recipient, "?TrialID", Request.QueryString["TID"], "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));

                if (intFindTrialIDRecipient == 0)
                {
                    Response.Redirect(strRedirectError + "81" + "&TID_R=" + Request.QueryString["TID_R"], false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }


                if (intFindTrialID > 1)
                {
                    Response.Redirect(strRedirectError + "82" + "&TID_R=" + Request.QueryString["TID_R"], false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }



                //if (Request.QueryString["TID"].Substring(0, 2) != SessionVariablesAll.CentreCode)
                //string strCentreCode = Request.QueryString["TID_R"].Substring(3, 1);

                string STRSQL_CENTRECODE_RECIPIENT = "SELECT TransplantCentre FROM trialdetails_recipient WHERE TrialIDRecipient=?TrialIDRecipient ";
                string strCentreCode = GeneralRoutines.ReturnScalar(STRSQL_CENTRECODE_RECIPIENT, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN);

                string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                //if (strIsServer == "0")
                //{
                //    mnuSpecimenSide.DataSource = XMLSpecimenMenuRecipientOnlyDataSource;
                //}
                //else
                //{
                //    mnuSpecimenSide.DataSource = XMLSpecimenMenuRecipientDataSource;
                //}

                //mnuSpecimenSide.DataBind();

                if (strCentreCode != SessionVariablesAll.CentreCode)
                {

                    if (string.IsNullOrEmpty(SessionVariablesAll.AdminSuperUser))
                    {
                        Response.Redirect(strRedirectError + "1" + "&TID_R=" + Request.QueryString["TID_R"], false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        if (SessionVariablesAll.AdminSuperUser == "YES" || strIsServer == "0")
                        {
                            //do nothing
                        }
                        else
                        {
                            ////redirect
                            //Response.Redirect(strRedirectError + "1" + "&TID=" + Request.QueryString["TID"], false);
                            //HttpContext.Current.ApplicationInstance.CompleteRequest();

                            //added by rajeev 20141001 1603
                            Boolean blnAssignAccess = GeneralRoutines.AssignUserAccessRights(strCentreCode);

                            if (blnAssignAccess == false)
                            {
                                throw new Exception("An error occured while checking access rights.");
                            }
                            if (strCentreCode != SessionVariablesAll.CentreCode)
                            {
                                //redirect
                                Response.Redirect(strRedirectError + "1" + "&TID_R=" + Request.QueryString["TID_R"], false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {
                                if (SessionVariablesAll.AddEditRecipient != STR_YES_SELECTION && SessionVariablesAll.AddEditFollowUp != STR_YES_SELECTION)
                                {
                                    Response.Redirect(strAccessDeniedAddEdit, false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                }
                                else
                                {
                                    if (SessionVariablesAll.AddEditRecipient == STR_YES_SELECTION && SessionVariablesAll.AddEditFollowUp == STR_YES_SELECTION)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                         if (SessionVariablesAll.AddEditRecipient != STR_YES_SELECTION)
                                         {
                                             UnBindMenuItems("mnuRE"); //remove Recipient Items
                                         }

                                         if (SessionVariablesAll.AddEditFollowUp != STR_YES_SELECTION)
                                         {
                                             //lblErrorMessage.Text = SessionVariablesAll.AddEditFollowUp;
                                             UnBindMenuItems("mnuFU"); //remove follow up items 
                                         }

                                    }
                                    
                                }
                            }
                        }

                    }
                    //throw new Exception("The first characters of the TrialID (Recipient) does not match the Centre Code.");

                }


            }
            else
            {
                throw new Exception("Please Check the TrialID (Recipient).");
            }

            if (!IsPostBack)
            {
                lblErrorMessage.Text = String.Empty;


                string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");
                if (strIsServer == "0")
                {
                    mnuSpecimenSide.DataSource = XMLSpecimenMenuRecipientOnlyDataSource;
                }
                else
                {
                    mnuSpecimenSide.DataSource = XMLSpecimenMenuRecipientDataSource;
                }

                mnuSpecimenSide.DataBind();

                


                //check if TrialID exists
                string STRSQLFIND = string.Empty;
                STRSQLFIND += "SELECT COUNT(*) CR FROM trialdetails_recipient  t1 ";
                STRSQLFIND += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
                STRSQLFIND += "WHERE t1.TrialIDRecipient=?TrialIDRecipient";

                int intFindSQL = 0;
                if (GeneralRoutines.IsNumeric(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN)))
                {
                    intFindSQL = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));
                }
                else
                {
                    throw new Exception("Could not check if TrialID (Recipient) exists.");
                }

                if (intFindSQL > 1) throw new Exception("More than one TrialID (Recipient) exists for " + Request.QueryString["TID"]);
                if (intFindSQL == 0) throw new Exception(Request.QueryString["TrialID"] + " (Recipient) does not exist in the database.");
                if (intFindSQL < 0) throw new Exception("An error occured while checking if " + Request.QueryString["TID"] + " (Recipient) exists in the database.");

                //chek if anyconsent is no
                STRSQLFIND = "SELECT COUNT(*) CR FROM trialdetails_recipient  t1 ";
                STRSQLFIND += " ";
                STRSQLFIND += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
                STRSQLFIND += "AND (t1.RecipientInformedConsent='NO' OR t1.Recipient18Year='NO' OR t1.RecipientMultipleDualTransplant='YES') ";

                

                intFindSQL = 0;
                if (GeneralRoutines.IsNumeric(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN)))
                {
                    intFindSQL = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], STRCONN));
                }
                else
                {
                    throw new Exception("Could not check if all the options for TrialID (Recipient) has been selected as YES.");
                }

                if (intFindSQL >= 1)
                {
                    Response.Redirect(strRedirectError91 + "&TID_R=" + Request.QueryString["TID_R"], false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                //now assign main details
                AssignDetails();

                //lblDonorID.Text = STRSQLFIND + " " + intFindSQL.ToString();

                if (!string.IsNullOrEmpty(SessionVariablesAll.Randomise))
                {
                    if (SessionVariablesAll.Randomise != "YES")
                    {
                        MenuItem miMenuItem = mnuSpecimenSide.FindItem(strRemoveMenuItem);

                        if (miMenuItem != null)
                        {
                            mnuSpecimenSide.Items.Remove(miMenuItem);
                        }
                    }
                }

                //string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");


                if (SessionVariablesAll.AdminSuperUser == "YES" || strIsServer == "0")
                {
                    //do nothing
                }
                else
                {
                    if (SessionVariablesAll.AddEditRecipient == STR_YES_SELECTION && SessionVariablesAll.AddEditFollowUp == STR_YES_SELECTION)
                    {
                        ///do nothing;
                    }
                    else
                    {
                        if (SessionVariablesAll.AddEditRecipient != STR_YES_SELECTION)
                        {
                            UnBindMenuItems("mnuRE"); //remove Recipient Items


                        }


                        if (SessionVariablesAll.AddEditFollowUp != STR_YES_SELECTION)
                        {
                            UnBindMenuItems("mnuFU"); //remove follow up items 
                        }

                    }
                }

            }
        }
        catch (System.Exception ex)
        {
            lblErrorMessage.Text = ex.Message + " An error occured while loading the main page.";
        }
    }


    //protected void Page_LoadComplete(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

    //        lblTrialID.Text = "strIsServer " + strIsServer;
    //        if (SessionVariablesAll.AdminSuperUser == "YES" || strIsServer == "0")
    //        {
    //            //do nothing
    //            lblTrialID.Text += " //do nothing";
    //        }
    //        else
    //        {
    //            if (SessionVariablesAll.AddEditRecipient == STR_YES_SELECTION && SessionVariablesAll.AddEditFollowUp == STR_YES_SELECTION)
    //            {
    //                //do nothing
    //                lblTrialID.Text += " //do nothing";
    //            }
    //            else
    //            {
    //                if (SessionVariablesAll.AddEditRecipient != STR_YES_SELECTION)
    //                {
    //                    UnBindMenuItems("mnuRE"); //remove Recipient Items


    //                }
    //                lblTrialID.Text += " AddEditRecipient" + SessionVariablesAll.AddEditRecipient;

    //                if (SessionVariablesAll.AddEditFollowUp != STR_YES_SELECTION)
    //                {
    //                    UnBindMenuItems("mnuFU"); //remove follow up items 
    //                }
    //                lblTrialID.Text += " AddEditFollowUp" + SessionVariablesAll.AddEditFollowUp;
    //            }
    //        }

    //    }
    //    catch(Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message + " An error occured while executing Load Complete event.";
    //    }

    //}
    protected void AssignDetails()
    {
        try
        {
            lblErrorMessage.Text = String.Empty;

            string STRSQL = "SELECT t1.*, t2.DonorID DonorIdentification, t3.RecipientID FROM trialdetails_recipient  t1 ";
            STRSQL += "LEFT JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "LEFT JOIN r_identification t3 ON t1.TrialID=t3.TrialID ";
            STRSQL += " WHERE t1.TrialIDRecipient=?TrialIDRecipient ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    while (myDr.Read())
                    {
                        if (myDr.HasRows)
                        {
                            //if (!DBNull.Value.Equals(myDr["TrialID"]))
                            //{
                            //    lblTrialID.Text = myDr["TrialID"].ToString();

                            //}
                            lblTrialID.Text = "TrialID (R) ";
                            if (!DBNull.Value.Equals(myDr["TrialIDRecipient"]))
                            {
                                lblTrialID.Text += myDr["TrialIDRecipient"].ToString();

                            }
                            if (!DBNull.Value.Equals(myDr["DonorIdentification"]))
                            {
                                lblDonorID.Text = "" + myDr["DonorIdentification"].ToString() + "";
                            }

                            if (!DBNull.Value.Equals(myDr["RecipientID"]))
                            {
                                lblRecipientID.Text = "" + myDr["RecipientID"].ToString() + "";
                            }
                        }
                    }



                }

                // close connection
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }




            }




            catch (System.Exception ex)
            {
                // close connection
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblErrorMessage.Text = ex.Message + " An error occured while executing query.";
            }
        }

        catch (System.Exception ex)
        {
            lblErrorMessage.Text = ex.Message + " An error occured while Assigning data.";
        }

    }

    protected void mnuSpecimenSide_MenuItemDataBound(Object sender, MenuEventArgs e)
    {
        if (e.Item.Value.Contains("mnuEmpty"))
        {
            e.Item.Enabled = false;
        }
        else
        {
            if (e.Item.NavigateUrl != "")
            {
                string strNavURL = e.Item.NavigateUrl.ToLower().Replace("~", "");
                string strURL = Request.Url.AbsoluteUri.ToLower();

                if (strURL.Contains(strNavURL))
                {
                    //e.Item.Text = "<div style='font-weight: bold'>" + e.Item.Text + "</div>";
                    e.Item.Text = "<div style='color: green'>" + e.Item.Text + "</div>";
                }

                e.Item.NavigateUrl += "?TID_R=" + Request.QueryString["TID_R"] + "&TID=" + Request.QueryString["TID"];

            }

        }

        //add resuse submenu
        if (e.Item.Value == "mnuAddResUse")
        {
            ArrayList arlOccasions = new ArrayList();

            arlOccasions.Add("3 Months");
            arlOccasions.Add("1 Year");

            for (int i = 0; i < arlOccasions.Count; i++)
            {
                string strOccasion = arlOccasions[i].ToString();

                MenuItem mi = new MenuItem();
                mi.Text = strOccasion;
                mi.Value = "mnuResUse" + strOccasion;
                mi.NavigateUrl = "~/SpecClinicalData/AddResUse.aspx?TID=" + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + "&Occasion=" + strOccasion;
                e.Item.ChildItems.Add(mi);

            }

        }

        //change the font for SAE
        if (e.Item.Value == "mnuAddSerAE")
        {
            //e.Item.Selected = true;
            //e.Item.NavigateUrl = "";
            e.Item.Text = "<div style='color: Red'>" + e.Item.Text + "</div>";
        }

    }

    protected void UnBindMenuItems(string strItemvalue)
    {
        try
        {
            MenuItem miMenuItem;

            if (strItemvalue == "mnuRE")
            {
                miMenuItem = mnuSpecimenSide.FindItem("mnuREAddRecipient");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuREAddRPeriOperativeData");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuREAddRecipientSpecimen");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }


            }

            if (strItemvalue == "mnuFU")
            {
                miMenuItem = mnuSpecimenSide.FindItem("mnuFUAddFU1to14");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuFUAddFUPostTransplant");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuFUAddResUse");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuFUAddFUReadmissions");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuFUMarkWithdrawn");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuFUAddDeceased");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

                miMenuItem = mnuSpecimenSide.FindItem("mnuREAddRecipientSpecimen");

                if (miMenuItem != null)
                {
                    mnuSpecimenSide.Items.Remove(miMenuItem);
                }

            }

            //foreach (MenuItem item in mnuSpecimenSide.Items)
            //{

            //    if (item.Value.Contains(strItemvalue))
            //    {
            //        mnuSpecimenSide.Items.Remove(item);
            //    }

            //}


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message + " An error occured while binding Menu Items";
        }
    }


    protected void cmdGoTo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMessage.Text = string.Empty;

            if (txtMainStudyID.Text == "")
            { throw new Exception("Please Enter a TrialID (Recipient)."); }
            else
            {
                Response.Redirect(strRedirectOwn + txtMainStudyID.Text.Substring(0,7) + "&TID_R=" + txtMainStudyID.Text, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }


        }
        catch (System.Exception ex)
        {
            lblErrorMessage.Text = ex.Message + " Please enter a TrialID.";
        }
    }

   
}
