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

public partial class SpecClinicalMasterPage : System.Web.UI.MasterPage
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";
        private const string strRedirectDefault = "~/Default.aspx";

        private const string strRedirectOwn = "~/SpecClinicalData/ViewSummary.aspx?TID=";

        private const string strRedirectError = "~/OtherArea/AccessDenied.aspx?EID=";

        private const string STR_YES_SELECTION = "YES";
        private const string strAccessDeniedAddEdit = "~/OtherArea/AccessDenied.aspx?EID=61";

        private const string strRemoveMenuItem = "mnuAddRTrial";

        string strTrialIDLeadingCharacters = System.Configuration.ConfigurationManager.AppSettings.Get("TrialIDLeadingCharacters");


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
            { throw new Exception("Could not obtain the Unique Identifier."); }

            if (Request.QueryString["TID"].Length > 1)
            {

                //check if trialID exists
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

                //if (Request.QueryString["TID"].Substring(0, 2) != SessionVariablesAll.CentreCode)
                //string strCentreCode = Request.QueryString["TID"].Substring(3, 1);

                string STRSQL_CENTRECODE = "SELECT CentreCode FROM trialdetails WHERE TrialID=?TrialID ";
                string strCentreCode = GeneralRoutines.ReturnScalar(STRSQL_CENTRECODE, "?TrialID", Request.QueryString["TID"], STRCONN);

                string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                if (strCentreCode != SessionVariablesAll.CentreCode)
                {

                    if (string.IsNullOrEmpty(SessionVariablesAll.AdminSuperUser))
                    {
                        Response.Redirect(strRedirectError + "1" + "&TID=" + Request.QueryString["TID"], false);
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
                                Response.Redirect(strRedirectError + "1" + "&TID=" + Request.QueryString["TID"], false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {
                                if (SessionVariablesAll.AddEdit != STR_YES_SELECTION)
                                {
                                    Response.Redirect(strAccessDeniedAddEdit + "&Centre=" + SessionVariablesAll.CentreCode , false);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                }
                            }
                        }

                    }
                    //throw new Exception("The first characters of the TrialID (Recipient) does not match the Centre Code.");

                }

            }
            else
            {
                throw new Exception("Please Check the TrialID.");
            }

            if (!IsPostBack)
            {
                lblErrorMessage.Text = String.Empty;

                    
                

                mnuSpecimenSide.DataSource = XMLSpecimenMenuDonorDataSource;
                mnuSpecimenSide.DataBind();
                

                //check if TrialID exists
                string STRSQLFIND = "SELECT COUNT(*) CR FROM trialdetails WHERE TrialID=?TrialID";

                int intFindSQL = 0;
                if (GeneralRoutines.IsNumeric(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialID", Request.QueryString["TID"], STRCONN)))
                {
                    intFindSQL = Convert.ToInt32(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialID", Request.QueryString["TID"], STRCONN));
                }
                else
                {
                    throw new Exception("Could not check if TrialID exists.");
                }

                if (intFindSQL > 1) throw new Exception("More than one TrialID exists for " + Request.QueryString["TID"]);
                if (intFindSQL == 0) throw new Exception(Request.QueryString["TrialID"] + " does not exist in the database.");
                if (intFindSQL < 0) throw new Exception("An error occured while checking if " + Request.QueryString["TID"] + " exists in the database.");


                //now assign main details
                AssignDetails();

                string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                if (!string.IsNullOrEmpty(SessionVariablesAll.Randomise) || strIsServer=="0")
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
                //foreach (MenuItem item in mnuSpecimenSide.Items)
                //{
                //    lblErrorMessage.Text += item.Value + ". ";
                //}

                //lblErrorMessage.Text = mnuSpecimenSide.Items.Count.ToString();

            }
        }
        catch (System.Exception ex)
        {
            lblErrorMessage.Text = ex.Message + " An error occured while loading the main page.";
        }
    }

    
    protected void AssignDetails()
    {
        try
        {
            lblErrorMessage.Text = String.Empty;

            string STRSQL = "SELECT t1.* FROM trialdetails t1 ";
            //STRSQL += "LEFT JOIN r_identification t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += " WHERE t1.TrialID=?TrialID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];

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
                            if (!DBNull.Value.Equals(myDr["TrialID"]))
                            {
                                lblTrialID.Text = myDr["TrialID"].ToString();

                            }
                            if (!DBNull.Value.Equals(myDr["DonorID"]))
                            {
                                //lblDonorID.Text ="D " +  myDr["DonorID"].ToString() + "";
                            }

                            //if (!DBNull.Value.Equals(myDr["RecipientID"]))
                            //{
                            //    lblRecipientID.Text = "R " + myDr["RecipientID"].ToString() + ")";
                            //}
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
            e.Item.Enabled = false ;
        }
        else 
        {
            if (e.Item.NavigateUrl != "")
            {
                e.Item.NavigateUrl += "?TID=" + Request.QueryString["TID"];

                string strNavURL = e.Item.NavigateUrl.ToLower().Replace("~", "");
                string strURL = Request.Url.AbsoluteUri.ToLower();

                if (strURL.Contains(strNavURL))
                {
                    //e.Item.Text = "<div style='font-weight: bold'>" + e.Item.Text + "</div>";
                    e.Item.Text = "<div style='color: green'>" + e.Item.Text + "</div>";
                }
                if (e.Item.Value == "mnuAddMPGData")
                {
                    //e.Item.NavigateUrl = "";
                    MenuItem mi = new MenuItem();

                    mi.Text = "Left Kidney";
                    mi.Value = "mnuAddMPGDataLeftKidney";
                    mi.NavigateUrl = e.Item.NavigateUrl  + "&Side=Left";


                    e.Item.ChildItems.Add(mi);

                    mi = new MenuItem();

                    mi.Text = "Right Kidney";
                    mi.Value = "mnuAddMPGDataRightKidney";
                    mi.NavigateUrl = e.Item.NavigateUrl  + "&Side=Right";


                    e.Item.ChildItems.Add(mi);

                }

                if (e.Item.Value == "mnuAddKidneyProcedure")
                {
                    //e.Item.NavigateUrl = "";
                    MenuItem mi = new MenuItem();

                    mi.Text = "Left Kidney";
                    mi.Value = "mnuAddKidneyProcedureLeftKidney";
                    mi.NavigateUrl = e.Item.NavigateUrl  + "&Side=Left";


                    e.Item.ChildItems.Add(mi);

                    mi = new MenuItem();

                    mi.Text = "Right Kidney";
                    mi.Value = "mnuAddKidneyProcedureRightKidney";
                    mi.NavigateUrl = e.Item.NavigateUrl + "&Side=Right";


                    e.Item.ChildItems.Add(mi);

                }

                if (e.Item.Value == "mnuUPreservation")
                {
                    //e.Item.NavigateUrl = "";
                    MenuItem mi = new MenuItem();

                    mi.Text = "Left Kidney";
                    mi.Value = "mnuUPreservationLeftKidney";
                    mi.NavigateUrl = e.Item.NavigateUrl  + "&Side=Left";


                    e.Item.ChildItems.Add(mi);

                    mi = new MenuItem();

                    mi.Text = "Right Kidney";
                    mi.Value = "mnuUPreservationRightKidney";
                    mi.NavigateUrl = e.Item.NavigateUrl  + "&Side=Right";


                    e.Item.ChildItems.Add(mi);

                }

            }
            
        }

    }


    

    protected void cmdGoTo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMessage.Text = string.Empty;

            if (txtMainStudyID.Text == "")
            { throw new Exception("Please Enter a TrialID."); }
            else
            {
                
                Response.Redirect(strRedirectOwn + txtMainStudyID.Text,false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }


        }
        catch (System.Exception ex)
        {
            lblErrorMessage.Text = ex.Message + " Please enter a TrialID.";
        }
    }

   
    
}
