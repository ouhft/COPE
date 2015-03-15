using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;

using AjaxControlToolkit;


public partial class SpecClinicalData_UPreservation : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        //access denied cannot randomise
        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx?EID=51";
        private const string STRSAVEPATH = "E:\\SITU\\COPE\\WP4\\OtherList\\PreservationFiles\\";//F:\\SITU\\COPE\\WP3\\PreservationFiles\\";
        private const string STRBACKUPPATH = "C:\\SITU\\Cope\\WP4\\OtherList\\PreservationFilesBackUp\\";

        //private const string STRSAVEPATH = "C:\\SITU\\Cope\\WP4\\OtherList\\PreservationFiles\\";
        
    
        //private const string STRDELETEPATH = "F:\\SITU\\COPE\WP3\PreservationFiles\";

        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblUserMessages.Text = String.Empty;

                lblPageDescription.Text = "Upload Perfusion Parameter File obtained from the machine.";

                lblUserMessages.Text = string.Empty;

                if (string.IsNullOrEmpty(Request.QueryString["TID"]))
                {
                    throw new Exception("Could not obtain the trialID.");
                }

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

                ddSide.DataSource = XMLKidneySidesDataSource;
                ddSide.DataBind();

                ListItem li = ddSide.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if(li!=null)
                {
                    ddSide.Items.Remove(li);
                }
                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";


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

                //if (string.IsNullOrEmpty(Request.QueryString["RowIndex"]) == false || ddSide.SelectedValue!=STR_DD_UNKNOWN_SELECTION)
                //{
                //    pnlFileUpload.Visible = false;
                //    pnlEditComments.Visible = true;

                //    AssignData();
                //}
                //else
                //{
                //    pnlFileUpload.Visible = true;
                //    pnlEditComments.Visible = false;
                //}

            }
        }
        catch (Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while loading the page.";
        }
    }

    protected void AssignData()
    {
        try
        {
            string STRSQL = string.Empty;

            //STRSQL += "SELECT * FROM tblfileuploads WHERE TrialID=?TrialID AND RowIndex=?RowIndex AND Side=?Side";

            STRSQL += "SELECT * FROM tblfileuploads WHERE TrialID=?TrialID AND Side=?Side";

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

            MyCMD.Parameters.Add("?RowIndex", MySqlDbType.VarChar).Value = Request.QueryString["RowIndex"];

            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;

            MyCONN.Open();
            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["FileName"]))
                            {
                                lblUploadedFileName.Text = (string)(myDr["FileName"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
                            }

                            chkRecordExist.Checked = true;

                        }
                    }
                    else
                    {
                        chkRecordExist.Checked = false;
                        //throw new Exception("The record you have selected does not exist.");
                    }
                }
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                //cmdUpload.Text = "Update";
            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                lblUserMessages.Text = ex.Message + " An error occured while executing assign query. ";
            }

        }
        catch (Exception ex)
        { 
            lblUserMessages.Text = ex.Message + " An error occured while assigning data."; 
        }
    }

    protected void BindData()
    {
        try
        {
           
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, CONCAT('E:\\\\SITU\\\\COPE\\\\WP4\\\\OtherList\\\\PreservationFiles\\\\', t1.FileName) FullName, ";
            //strSQL += "SELECT t1.*, CONCAT('C:\\\\SITU\\\\COPE\\\\WP4\\\\OtherList\\\\PreservationFiles\\\\', t1.FileName) FullName, ";
            strSQL += "CONCAT(DATE_FORMAT(t1.DateCreated, '%d/%m/%Y %H:%i')) Date_Created ";
            strSQL += "FROM tblfileuploads t1 ";
            strSQL += "WHERE t1.TrialID=?TrialID   ";
            //strSQL += "WHERE t1.TrialID='" + Request.QueryString["TID"].ToString() + "' ";
            strSQL += "ORDER BY t1." + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            

            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            
            lblGV1.Text = "List of Files uploaded " + GV1.Rows.Count.ToString() + ".";
            

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


   //gv1 rowdatabond
    protected void GV1_RowCommand(Object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Download")
            {
                string[] fileInfo = e.CommandArgument.ToString().Split(Convert.ToChar(";"));

                string FileName = fileInfo[0];

                string FullPath = fileInfo[1];

                Downloadfile(FileName, FullPath);

            }
        }
        catch (Exception ex)
        { lblUserMessages.Text = ex.Message + " An error occured while downlaoding file"; }
    }

    protected void GV1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.DataItem != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = (DataRowView)(e.Row.DataItem);
                    if (String.IsNullOrEmpty(Request.QueryString["RowIndex"]) == false)
                    {
                        {
                            if (drv["RowIndex"].ToString() == Request.QueryString["RowIndex"].ToString())
                            {
                                e.Row.BackColor = System.Drawing.Color.LightBlue;
                            }
                        }
                    }
                }

                ConfirmButtonExtender cmdDelete_ConfirmButtonExtenderGV1=(ConfirmButtonExtender)(e.Row.FindControl("cmdDelete_ConfirmButtonExtender"));
                if (cmdDelete_ConfirmButtonExtenderGV1 != null)
                {
                    cmdDelete_ConfirmButtonExtenderGV1.ConfirmText="If you click 'OK' the Selected File will be deleted. Click 'CANCEL' if you do not wish to Delete Selected file";
                }


            }
        }
        catch (Exception ex)
        { 
            lblUserMessages.Text = ex.Message + " An error occured while downlaoding file"; 
        }        
    }

    protected void Downloadfile(string fileName, string FullFilePath)
    {
        try
        {
            //lblFileUpload.Text = fileName + " " + FullFilePath;

            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            Response.TransmitFile(FullFilePath);
            Response.End();
        }

        catch (Exception excep)
        { lblUserMessages.Text = excep.Message + " An error occured while downlaoding file"; }

    }

    protected void cmdReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.Url.AbsoluteUri, false);

        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while resetting the page.";
        }
    }
    protected void cmdUpload_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddSide.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select Side of kidney.");

            }
            if (chkRecordExist.Checked == true)
            {
                UpdateData();

            }

            else
            {
                if (string.IsNullOrEmpty(fileUpload1.PostedFile.FileName))
                {
                    throw new Exception("Please Select a File to Upload.");
                }
                string strFile = fileUpload1.PostedFile.FileName;

                if (strFile == String.Empty) throw new Exception("Please Select a File using Browse buttom.");

                //check if the file exists in the database

                string strFindFile = "SELECT COUNT(*) CR FROM tblfileuploads WHERE FileName=?FileName ";

                int intCountFiles = Convert.ToInt32(GeneralRoutines.ReturnScalar(strFindFile, "?FileName", strFile, STRCONN));

                if (intCountFiles < 0) throw new Exception("Could not check if File exists");

                if (intCountFiles > 0) throw new Exception("A file with the same name already exists in the database.");


                strFindFile = "SELECT COUNT(*) CR FROM tblfileuploads WHERE TrialID=?TrialID AND  Side=?Side ";

                intCountFiles = Convert.ToInt32(GeneralRoutines.ReturnScalarTwo(strFindFile, "?TrialID", Request.QueryString["TID"],  "?Side", ddSide.SelectedValue, STRCONN));

                if (intCountFiles < 0) throw new Exception("Could not check if File exists for selected side");

                if (intCountFiles > 0) throw new Exception("File has already been uploaded for the selected kidney.");

            
                if (fileUpload1.HasFile)
                {
                    string Extension = Path.GetExtension(fileUpload1.PostedFile.FileName); /* this will give u the extension*/

                    switch (Extension)
                    {
                        case ".txt":
                            //do nothing
                            break;
                        case ".xls":
                            //do nothing
                            break;

                        case ".xlsx":
                            //do nothing
                            break;
                        case ".pkl":
                            //do nothing
                            break;

                        default:
                            throw new Exception("Only txt, xls, xlsx and pkl file types can be uploaded.");
                            //break;
                    }

                    fileUpload1.SaveAs(STRSAVEPATH + strFile);
                    fileUpload1.Dispose();
                }
                else
                {
                    throw new Exception("No File available for Upload.");
                }

                // add file if it does not exist

                string strSQL = string.Empty;

                strSQL += "INSERT INTO tblfileuploads ";
                strSQL += "(TrialID, Side, FileName, IPAddress, Comments, DateCreated, CreatedBy) ";
                strSQL += "VALUES ";
                strSQL += "(?TrialID, ?Side, ?FileName, ?IPAddress, ?Comments, ?DateCreated, ?CreatedBy) ";

                string CS = string.Empty;
                CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

                MySqlConnection MyCONN = new MySqlConnection();
                MyCONN.ConnectionString = CS;

                MySqlCommand MyCMD = new MySqlCommand();
                MyCMD.Connection = MyCONN;

                MyCMD.CommandType = CommandType.Text;
                MyCMD.CommandText = strSQL;

                MyCMD.Parameters.Clear();

                MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];

                MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;

                MyCMD.Parameters.Add("?FileName", MySqlDbType.VarChar).Value = fileUpload1.PostedFile.FileName;
            
                MyCMD.Parameters.Add("?IPAddress", MySqlDbType.VarChar).Value = Request.UserHostAddress;
            

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

                MyCONN.Open();

                try
                {
                    MyCMD.ExecuteNonQuery();

                    if (MyCONN.State == ConnectionState.Open) MyCONN.Close();

                    BindData();

                }
                catch (Exception excep)
                {
                    if (MyCONN.State == ConnectionState.Open) MyCONN.Close();

                    lblUserMessages.Text = excep.Message + " An error occured while executing query.";
                }
            }
        }

        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while uploading file.";
        }
    }

    protected void UpdateData()
    {
        try
        {
            string strSQL = string.Empty;

            strSQL += "UPDATE tblfileuploads SET ";
            strSQL += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy, IPAddressUpdatedBy=?IPAddressUpdatedBy ";
            strSQL += "WHERE TrialID=?TrialID AND Side=?Side ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];
            MyCMD.Parameters.Add("?RowIndex", MySqlDbType.VarChar).Value = Request.QueryString["RowIndex"];
            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;

            MyCMD.Parameters.Add("?IPAddressUpdatedBy", MySqlDbType.VarChar).Value = Request.UserHostAddress;

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

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                if (MyCONN.State == ConnectionState.Open) MyCONN.Close();

                BindData();

                lblUserMessages.Text = "Data Updated";
            }
            catch (Exception excep)
            {
                if (MyCONN.State == ConnectionState.Open) MyCONN.Close();

                lblUserMessages.Text = excep.Message + " An error occured while executing update query.";
            }


        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while Updating comments.";
        }

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        try 
        {
            lblUserMessages.Text = string.Empty;

            Button cmdDelete=(Button)(sender);
            GridViewRow rowGRV = (GridViewRow)(cmdDelete.Parent.Parent);

            Label lblRowIndex_GV1 = (Label)(rowGRV.FindControl("lblRowIndex"));
            Label lblFileName_GV1 = (Label)(rowGRV.FindControl("lblFileName"));

            if (string.IsNullOrEmpty(lblRowIndex_GV1.Text))
            {
                throw new Exception("Could not obtain Unique Identifier.");
            }

            if (string.IsNullOrEmpty(lblFileName_GV1.Text))
            {
                throw new Exception("Could not obtain File Name.");
            }


            string STRSQL = String.Empty;

            STRSQL += "DELETE FROM tblfileuploads WHERE TrialID=?TrialID AND RowIndex=?RowIndex ";

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
            MyCMD.Parameters.Add("?RowIndex", MySqlDbType.VarChar).Value = lblRowIndex_GV1.Text;


            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();
                //File.Delete(STRSAVEPATH + lblFileName_GV1.Text);

                File.Move(STRSAVEPATH + lblFileName_GV1.Text, STRBACKUPPATH + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + lblFileName_GV1.Text);
                lblUserMessages.Text = "Selected File Deleted";
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


        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while resetting the page.";
        }

    }

    protected void ddSide_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddSide.SelectedValue!=STR_DD_UNKNOWN_SELECTION)
            {

                pnlSideSelected.Visible = true;


                AssignData();

                if (chkRecordExist.Checked == true)
                {
                    cmdUpload.Text = "Update";

                    pnlFileUpload.Visible = false;
                    pnlEditComments.Visible = true;


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
                    pnlFileUpload.Visible = true;
                    pnlEditComments.Visible = false;
                    cmdUpload.Text = "Upload File";
                }

            }
            else
            {
                pnlSideSelected.Visible = false;
            }
            
            
        }
        catch (System.Exception excep)
        {
            lblUserMessages.Text = excep.Message + " An error occured while selecting side of Kidney.";
        }
    }
}