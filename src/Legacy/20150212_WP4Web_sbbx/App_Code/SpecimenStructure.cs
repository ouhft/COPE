using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SpecimenStructure
/// </summary>
public class SpecimenStructure
{

    private Int64 uniqueid = 0;
    private string specimentype = "";
    private string occasion = "";
    private string collected = "";
    private string barcode = "";
    private string datecollected = "";
    private string timecollected = "";
    private string datecentrifugation = "";
    private string timecentrifugation = "";
    private string tissuelocation = "";
    private string preservation = "";
    private string comments = "";
    private Int16 sqlcode;

    // Declare a UniqueID property of type string: 
    public Int64 UniqueID
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

    // Declare a Specimentype property of type string: 
    public string Specimentype
    {
        get
        {
            return specimentype;
        }
        set
        {
            specimentype = value;
        }
    }

    // Declare property Occasion: 
    public string Occasion
    {
        get
        {
            return occasion;
        }

        set
        {
            occasion = value;
        }
    }

    // Declare property collected: 
    public string Collected
    {
        get
        {
            return collected;
        }

        set
        {
            collected = value;
        }
    }

    public string Barcode
    {
        get
        {
            return barcode;
        }

        set
        {
            barcode = value;
        }
    }

    public string Datecollected
    {
        get
        {
            return datecollected;
        }

        set
        {
            datecollected = value;
        }
    }

    public string Timecollected
    {
        get
        {
            return timecollected;
        }

        set
        {
            timecollected = value;
        }
    }

    public string Datecentrifugation
    {
        get
        {
            return datecentrifugation;
        }

        set
        {
            datecentrifugation = value;
        }
    }

    public string Timecentrifugation
    {
        get
        {
            return timecentrifugation;
        }

        set
        {
            timecentrifugation = value;
        }
    }

    public string Tissuelocation
    {
        get
        {
            return tissuelocation;
        }
        set
        {
            tissuelocation = value;
        }
    }

    public string Preservation
    {
        get
        {
            return preservation;
        }
        set
        {
            preservation = value;
        }
    }

    public string Comments
    {
        get
        {
            return comments;
        }
        set
        {
            comments = value;
        }
    }
    // SqCode for add/updatedeleye property of type string: 
    public Int16 SqlCode
    {
        get
        {
            return sqlcode;
        }
        set
        {
            sqlcode = value;
        }
    }

}