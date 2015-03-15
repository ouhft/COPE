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

public partial class _TopMasterPage : System.Web.UI.MasterPage
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";
        private const string strKeyDataName = "dbname";
        private const string REDIRECTPAGE = "~/Default.aspx";

        private const string strLogOut = "mnuLogout";
        private const string strAdminMenu = "mnuAdminMenu";

        private const string REDIRECT_HOMEPAGE = "~/WP4HomePage.aspx";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (String.IsNullOrEmpty(SessionVariablesAll.Web_cope41))
            {
                Response.Redirect(REDIRECTPAGE, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            if (SessionVariablesAll.Web_cope41 != "True")
            {
                Response.Redirect(REDIRECTPAGE, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            if (!IsPostBack)
            {
                lblTopMastePageUserMessages.Text = string.Empty;

                string STRSQL = string.Empty;
                string strIsServer = System.Configuration.ConfigurationManager.AppSettings.Get("isserver");

                if (!string.IsNullOrEmpty(SessionVariablesAll.AdminSuperUser))
                {
                    if (SessionVariablesAll.AdminSuperUser == "YES")
                    {

                        STRSQL += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged  
                                    FROM cope_wp_four.lstcentres  t1  
                                    ORDER BY t1.CountryCode, t1.CentreCode";

                        sqldsCentreCode.SelectParameters.Clear();


                        sqldsCentreCode.SelectCommand = STRSQL;
                        ddCentreCode.DataSource = sqldsCentreCode;
                        ddCentreCode.DataBind();


                        if (Request.Url.AbsoluteUri.Contains("/SpecClinicalData/"))
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["TID"]))
                            {
                                //string strCentre = Request.QueryString["TID"].ToString();

                                string STRSQL_CENTRECODE = "SELECT CentreCode FROM trialdetails WHERE TrialID=?TrialID ";
                                string strCentreCode = GeneralRoutines.ReturnScalar(STRSQL_CENTRECODE, "?TrialID", Request.QueryString["TID"], STRCONN);
                                
                                if (strCentreCode=="-1")
                                {
                                    throw new Exception("Centre Code could not be obtained.");
                                }
                                SessionVariablesAll.CentreCode = strCentreCode;
                            }
                            //SessionVariablesAll.CentreCode
                        }

                    }

                    else
                    {
                        ////get all the centres an individual can  access
                        //STRSQL = "SELECT t1.CountryCode,  t1.Country  ";
                        //STRSQL += "FROM lstcountries t1  ";
                        //STRSQL += "INNER JOIN copewpfourother.listdbuser t3 ON t3.centrecode=t1.CountryCode ";
                        //STRSQL += "INNER JOIN copewpfourother.listusers t4 ON t3.ListUsersID=t4.ListusersID ";
                        //STRSQL += "WHERE t4.username=?UserName ";
                        ////STRSQL += "WHERE t4.username='" + SessionVariablesAll.UserName + "' ";
                        //STRSQL += "";
                        //STRSQL += "ORDER BY t1.CountryCode ";

                        //only where add/edit=YES
                        STRSQL += @"SELECT CONCAT(t1.CountryCode, t1.CentreCode) Centre, CONCAT(t1.CountryCode, t1.CentreCode, ' - ' , t1.CentreName) CentreNameMerged, t2.*  
                                    FROM cope_wp_four.lstcentres  t1  
                                    INNER JOIN copewpfourother.listdbuser t2 ON  CONCAT(t1.CountryCode, t1.CentreCode)=t2.centrecode
                                    INNER JOIN copewpfourother.listusers t3 ON  t2.ListusersID=t3.ListusersID
                                    WHERE t3.UserName=?UserName  
                                    ORDER BY t1.CountryCode, t1.CentreCode ";

                        //lblUserDetails.Text = STRSQL;

                        sqldsCentreCode.SelectParameters.Add("?UserName", SessionVariablesAll.UserName);
                        sqldsCentreCode.SelectCommand = STRSQL;
                        ddCentreCode.DataSource = sqldsCentreCode;
                        ddCentreCode.DataBind();


                        if (Request.Url.AbsoluteUri.Contains("/SpecClinicalData/"))
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["TID"]))
                            {
                                string STRSQL_CENTRECODE = "SELECT CentreCode FROM trialdetails WHERE TrialID=?TrialID ";
                                string strCentreCode = GeneralRoutines.ReturnScalar(STRSQL_CENTRECODE, "?TrialID", Request.QueryString["TID"], STRCONN);

                                if (strCentreCode != "-1")
                                {
                                    ListItem liCentre = ddCentreCode.Items.FindByValue(strCentreCode);

                                    if (liCentre != null)
                                    {

                                        //ddCentreList.SelectedValue = strCentre;

                                        Boolean blnAssignAccess = GeneralRoutines.AssignUserAccessRights(strCentreCode);

                                        if (blnAssignAccess == true)
                                        {
                                            SessionVariablesAll.CentreCode = strCentreCode;
                                        }
                                        else
                                        {
                                            throw new Exception("You do not have access rights for the Centre Code assocaited with the TrialID.");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("The Centre Code assocaited with the TrialID doesn not exist in the drop down.");
                                    }
                                    
                                }
                                else
                                {
                                    throw new Exception("Centre Code could not be obtained.");
                                }
                            }

                        }
                    }
                }
                else
                {
                    ddCentreCode.Enabled = false;
                }






                //ddCentreCode.DataSource = sqldsCentreCode;
                //ddCentreCode.DataBind();

                ListItem li = ddCentreCode.Items.FindByValue(SessionVariablesAll.CentreCode);

                if (li != null)
                {
                    ddCentreCode.SelectedValue = SessionVariablesAll.CentreCode;
                }




                lblUserDetails.Text = SessionVariablesAll.UserName;
                lblUserDetails.Text += " " + SessionVariablesAll.AdminSuperUser;
                lblUserDetails.Text += " " + SessionVariablesAll.SuperUser;
                lblUserDetails.Text += "<br/> (" + SessionVariablesAll.LastLogin + ")";

                lblDepartment.Text = "&copy;&nbsp;<a href=http://www.nds.ox.ac.uk target=_blank>";
                lblDepartment.Text += "The Nuffield Department of Surgical Sciences</a><br /> University of Oxford&nbsp;";
                lblDepartment.Text += DateTime.Today.Year.ToString() + "&nbsp;";

            }

        }

        catch (System.Exception excep)
        {

            lblTopMastePageUserMessages.Text = excep.Message + " An error occured while loading the Main Page.";
        }
    }


    protected void mnuTop_MenuItemClick(object sender, MenuEventArgs e)
    {
        switch (e.Item.Value)
        {

            case strLogOut:

                SessionVariablesAll.Web_cope41 = "False";
                Session.Remove(SessionVariablesAll.Web_cope41);

                Response.Redirect(REDIRECTPAGE);

                break;

            //case strAdminMenu:

            //    break;

        }


    }

    protected void mnuTop_MenuItemDataBound(object sender, MenuEventArgs e)
    {

        try
        {
            if (SessionVariablesAll.SuperUser == "YES")
            {
                if (e.Item.Value == strAdminMenu)
                {
                    MenuItem item = new MenuItem();
                    item.Text = "Add Users";
                    item.Value = "mnuAENUser";
                    item.NavigateUrl = "~/OtherArea/AENUser.aspx";
                    e.Item.ChildItems.Add(item);


                    item = new MenuItem();

                    item.Text = "Reset Password Other";
                    item.Value = "mnuResetOther";
                    item.NavigateUrl = "~/OtherArea/ROther.aspx";
                    e.Item.ChildItems.Add(item);

                    item = new MenuItem();

                    item.Text = "Unlock User";
                    item.Value = "mnuUUSe";
                    item.NavigateUrl = "~/OtherArea/UUSe.aspx";
                    e.Item.ChildItems.Add(item);

                }

                //else
                //{
                //    lblTopMastePageUserMessages.Text = "yahoo1";
                //}


            }

            //else
            //{
            //    lblTopMastePageUserMessages.Text = "yahoo2";
            //}
        }

        catch (System.Exception ex)
        {

            lblTopMastePageUserMessages.Text = ex.Message + " An error occured while binding item to the Menu Control.";
        }


    }

    protected Boolean InsertLogoutDetails(string strUserID)
    {
        Boolean blnLockedUser = false;

        try
        {

            string strSQL = string.Empty;

            strSQL += "INSERT INTO  copewpfourother.userexitlogs ";
            strSQL += "(UserID, DbName, LoggedOut, IPAddress, SessionID) ";
            strSQL += "VALUES ";
            strSQL += "(?UserID, ?DbName, ?LoggedOut, ?IPAddress, ?SessionID) ";

            string CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;
            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

            MyCMD.Parameters.Clear();
            MyCMD.Parameters.Add("?UserID", MySqlDbType.VarChar).Value = strUserID;
            MyCMD.Parameters.Add("?DbName", MySqlDbType.VarChar).Value = System.Configuration.ConfigurationManager.AppSettings.Get(strKeyDataName); ; //user locked
            MyCMD.Parameters.Add("?LoggedOut", MySqlDbType.DateTime).Value = DateTime.Now;
            MyCMD.Parameters.Add("?IPAddress", MySqlDbType.VarChar).Value = Request.UserHostAddress;

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = Request.Cookies["ASP.NET_SessionId"].Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SessionID", MySqlDbType.VarChar).Value = DBNull.Value;
            }

            MyCONN.Open();
            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                blnLockedUser = true;
            }
            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                blnLockedUser = false;
                lblTopMastePageUserMessages.Text = ex.Message + " An error occured while executing error log query.";
            }

        }
        catch (System.Exception ex)
        {
            blnLockedUser = false;
            lblTopMastePageUserMessages.Text = ex + " An error occured while locking user";
        }

        return blnLockedUser;
    }


    protected void ddCentreCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(SessionVariablesAll.AdminSuperUser))
                {
                    if (SessionVariablesAll.AdminSuperUser == "YES")
                    {
                        SessionVariablesAll.CentreCode = ddCentreCode.SelectedValue;
                        //lblUserDetails.Text += "Centre -" + SessionVariablesAll.CentreCode;
                        Response.Redirect(REDIRECTPAGE, false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        GeneralRoutines.AssignUserAccessRights(ddCentreCode.SelectedValue);
                        Response.Redirect(REDIRECTPAGE, false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                
            }
            catch (System.Exception ex)
            {

                lblTopMastePageUserMessages.Text = ex.Message + " An error occured while binding item to the Menu Control.";
            }

        }



}
