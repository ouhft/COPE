using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmailListSAE
/// </summary>
public class EmailListSAE
{

    private string uniqueid = "";
    private string saeallcentre = "";
    private string saeallcentrecomments = "";
    private string trialidcreated = "";
    private string trialidcreatedcomments = "";
    // Declare a UniqueID property of type string: 
    public string UniqueID
    {
        get
        {
            return uniqueid;
        }
        set
        {
            uniqueid = value;
        }
    }

    public string SAEAllCentre
    {
        get
        {
            return saeallcentre;
        }
        set
        {
            saeallcentre = value;
        }
    }

    public string SAEAllCentreComments
    {
        get
        {
            return saeallcentrecomments;
        }
        set
        {
            saeallcentrecomments = value;
        }
    }

    public string TrialIDCreated
    {
        get
        {
            return trialidcreated;
        }
        set
        {
            trialidcreated = value;
        }
    }

    public string TrialIDCreatedComments
    {
        get
        {
            return trialidcreatedcomments;
        }
        set
        {
            trialidcreatedcomments = value;
        }
    }
}