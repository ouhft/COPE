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

public partial class Report_SummaryList : System.Web.UI.Page
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";

        private const string strAccessDenied = "~/OtherArea/AccessDenied.aspx";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";
    //static Random _random = new Random();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();
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
            lblGV1.Text = string.Empty;

            string strOccasion = string.Empty;

            string STRSQL = string.Empty;

            STRSQL += "SELECT t1.TrialID, ";
            STRSQL += "IF(t2.TrialID IS NULL, 'NO', 'YES') DonorDetails,  ";
            STRSQL += "IF(t3.TrialID IS NULL, 'NO', 'YES') DonorPreopData,  ";
            STRSQL += "IF(t4.TrialID IS NULL, 'NO', 'YES') DonorLabData,  ";
            STRSQL += "IF(t5.TrialID IS NULL, 'NO', 'YES') KidneyInspection,  ";
            STRSQL += "IF(t6.TrialID IS NULL, 'NO', 'YES') Randomisation,  ";
            STRSQL += "IF(t7.TrialID IS NULL, 'NO', 'YES') RecipientIdentification,  ";
            STRSQL += "IF(t8.TrialID IS NULL, 'NO', 'YES') RecipientPeriOperativeData,  ";
            STRSQL += "IF(t9.TrialID IS NULL, 'NO', 'YES') FUDay1to14,  ";
            STRSQL += "IF(t10.TrialID IS NULL, 'NO', 'YES') FUDay1Month,  ";
            STRSQL += "IF(t11.TrialID IS NULL, 'NO', 'YES') FU3Months,  ";
            STRSQL += "IF(t12.TrialID IS NULL, 'NO', 'YES') FU6Months,  ";
            STRSQL += "IF(t13.TrialID IS NULL, 'NO', 'YES') FU1Year  ";
            
            STRSQL += "FROM trialdetails t1 LEFT JOIN donor_identification t2 ";
            STRSQL += "ON t1.TrialID=t2.TrialID ";

            STRSQL += "LEFT JOIN donor_preop_clinicaldata t3 ";
            STRSQL += "ON t1.TrialID=t3.TrialID ";

            STRSQL += "LEFT JOIN donor_labresults t4 ";
            STRSQL += "ON t1.TrialID=t4.TrialID ";

            STRSQL += "LEFT JOIN kidneyinspection t5 ";
            STRSQL += "ON t1.TrialID=t5.TrialID ";

            STRSQL += "LEFT JOIN kidneyr t6 ";
            STRSQL += "ON t1.TrialID=t6.TrialID ";

            STRSQL += "LEFT JOIN r_identification t7 ";
            STRSQL += "ON t1.TrialID=t7.TrialID ";

            STRSQL += "LEFT JOIN r_perioperative t8 ";
            STRSQL += "ON t1.TrialID=t8.TrialID ";

            //follow up 1-14 days
            strOccasion = "1-14 Days";

            STRSQL += "LEFT JOIN  ";
            STRSQL += "(SELECT * FROM r_fuposttreatment WHERE Occasion='" + strOccasion + "') t9 ";
            STRSQL += "ON t1.TrialID=t9.TrialID ";

            //follow up 1-14 days
            strOccasion = "1 Month";

            STRSQL += "LEFT JOIN   ";
            STRSQL += "(SELECT * FROM r_fuposttreatment WHERE Occasion='" + strOccasion + "') t10 ";
            STRSQL += "ON t1.TrialID=t10.TrialID ";


            //follow up 3 Months
            strOccasion = "3 Months";

            STRSQL += "LEFT JOIN   ";
            STRSQL += "(SELECT * FROM r_fuposttreatment WHERE Occasion='" + strOccasion + "') t11 ";
            STRSQL += "ON t1.TrialID=t11.TrialID ";

            //follow up 3 Months
            strOccasion = "6 Months";

            STRSQL += "LEFT JOIN ";
            STRSQL += "(SELECT * FROM r_fuposttreatment WHERE Occasion='" + strOccasion + "') t12 ";
            STRSQL += "ON t1.TrialID=t12.TrialID ";


            //follow up 3 Months
            strOccasion = "1 Year";

            STRSQL += "LEFT JOIN ";
            STRSQL += "(SELECT * FROM r_fuposttreatment WHERE Occasion='" + strOccasion + "') t13 ";
            STRSQL += "ON t1.TrialID=t13.TrialID ";


            STRSQL += "WHERE t1.TrialID LIKE ?TrialID ";

            if ((string)ViewState["SortField"] == "TrialID")
            {
                STRSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            }
            else
            {
                STRSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
                STRSQL += ", TrialID ";
            }


            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialID", SessionVariablesAll.CentreCode + "%");


            sqldsGV1.SelectCommand = STRSQL;
            
            GV1.DataSource = sqldsGV1;
            GV1.DataBind();
            lblGV1.Text = "Click on TrialID to Add/Edit Data.";

            


        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding data to the datagrid.";
        }

    }


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

    protected void GV1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int i = 0;
            for (i = 0; i <= 12; i++)
            {
                
                    if (i > 0)
                    {
                        if (e.Row.Cells[i].Text == "YES")
                        {
                            e.Row.Cells[i].ForeColor = System.Drawing.Color.Green;
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[i].ForeColor = System.Drawing.Color.Red;
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.LightGray;
                        e.Row.Cells[i].BackColor = System.Drawing.Color.LightGray;
                    }

            }

        }

    }

    //protected void GV1_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        int i = 0;
    //        for (i = 0; i <= 12; i++)
    //        {
    //            if (e.Row.RowState == DataControlRowState.Normal)
    //            {

    //                if (i > 0)
    //                {
    //                    if (e.Row.Cells[i].Text == "YES")
    //                    {
    //                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Green;
    //                        e.Row.Cells[i].BackColor = System.Drawing.Color.Green;
    //                    }
    //                    else
    //                    {
    //                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Red;
    //                        e.Row.Cells[i].BackColor = System.Drawing.Color.Red;
    //                    }
    //                }
    //                else
    //                {
    //                    e.Row.Cells[i].ForeColor = System.Drawing.Color.LightGray;
    //                    e.Row.Cells[i].BackColor = System.Drawing.Color.LightGray;
    //                }
                    
    //            }
    //            else
    //            {
    //                if (i > 0)
    //                {

    //                    if (e.Row.Cells[i].Text == "YES")
    //                    {
    //                        e.Row.Cells[i].ForeColor = System.Drawing.Color.LightGreen;
    //                        e.Row.Cells[i].BackColor = System.Drawing.Color.LightGreen;
    //                    }
    //                    else
    //                    {
    //                        e.Row.Cells[i].ForeColor = System.Drawing.Color.Salmon;
    //                        e.Row.Cells[i].BackColor = System.Drawing.Color.Salmon;
    //                    }
    //                }
    //                else
    //                {
    //                    e.Row.Cells[i].ForeColor = System.Drawing.Color.LightBlue;
    //                    e.Row.Cells[i].BackColor = System.Drawing.Color.LightBlue;
    //                }
                    
    //            }

                
    //        }
                
    //    }

    //}
}