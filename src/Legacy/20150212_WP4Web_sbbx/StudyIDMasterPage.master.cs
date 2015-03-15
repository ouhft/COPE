using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _StudyIDMasterPage : System.Web.UI.MasterPage
{
    #region " Private Constants & Variables "

        private const string REDIRECTPAGE_DONOR = "~/AddEditData/WP4TList.aspx?";
        private const string REDIRECTPAGE_RECIPIENT = "~/AddEditData/WP4ListRecipient.aspx?";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["QT"]))
            {
                ddSearchType.SelectedValue = Request.QueryString["QT"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["QV"]))
            {
                txtSearch.Text = Request.QueryString["QV"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ES"]))
            {
                if (Request.QueryString["ES"] == "1")
                {
                    chkExactSearch.Checked = true;
                }
            }

        }
    }
    protected void cmdSearchContents_Click(object sender, EventArgs e)
    {
        try 
        {
            

            lblErrorMessage.Text = string.Empty;

            if (ddSearchType.SelectedValue == "0")
            {
                throw new Exception("Please Select a Search Criteria.");
            }

            if (txtSearch.Text == string.Empty)
            {
                throw new Exception("Please Enter Search text.");
            }

            string strES = "0";

            if (chkExactSearch.Checked == true)
            {
                strES = "1";
            }

            if (ddSearchType.SelectedValue == "TID_R" || ddSearchType.SelectedValue == "RID")
            {

                Response.Redirect(REDIRECTPAGE_RECIPIENT + "QT=" + ddSearchType.SelectedValue + "&QV=" + txtSearch.Text + "&ES=" + strES, false);
            }
            else
            {

                Response.Redirect(REDIRECTPAGE_DONOR + "QT=" + ddSearchType.SelectedValue + "&QV=" + txtSearch.Text + "&ES=" + strES, false);
            }
           


        }
        catch (System.Exception ex)
        {
            lblErrorMessage.Text = ex.Message + " An error occured when clicking 'Search' button.";
        }
    }
}
