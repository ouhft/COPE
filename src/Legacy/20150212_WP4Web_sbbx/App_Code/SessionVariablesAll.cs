using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SessionVariablesAll
/// </summary>
public class SessionVariablesAll
{
    private static string _UserName = "UserName";
    private static string _Web_cope41 = "Web_cope41";
    private static string _CentreCode = "CentreCode";

    private static string _AdminSuperUser = "AdminSuperUser";
    private static string _SuperUser = "SuperUser";
    private static string _CreateStudyID = "CreateStudyID";
    private static string _AddEdit = "AddEdit";
    private static string _AddEditRecipient = "AddEditRecipient";
    private static string _AddEditFollowUp = "AddEditFollowUp";

    private static string _Randomise = "Randomise";
    private static string _ViewRandomise = "ViewRandomise";
    private static string _LastLogin = "LastLogin";
    //private static string _CountLocked = "CountLocked";


    
    
    public static string UserName
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._UserName] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._UserName].ToString();
            }

        }



        set
        {
            HttpContext.Current.Session[SessionVariablesAll._UserName] = value;
        }

    }


    public static string Web_cope41
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._Web_cope41] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._Web_cope41].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._Web_cope41] = value;
        }

    }

    public static string CentreCode
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._CentreCode] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._CentreCode].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._CentreCode] = value;
        }

    }

    public static string AdminSuperUser
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._AdminSuperUser] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._AdminSuperUser].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._AdminSuperUser] = value;
        }
    }
    public static string SuperUser
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._SuperUser] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._SuperUser].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._SuperUser] = value;
        }
    }

    public static string CreateStudyID
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._CreateStudyID] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._CreateStudyID].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._CreateStudyID] = value;
        }
    }

    public static string AddEdit
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._AddEdit] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._AddEdit].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._AddEdit] = value;
        }
    }

    public static string AddEditRecipient
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._AddEditRecipient] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._AddEditRecipient].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._AddEditRecipient] = value;
        }
    }

    public static string AddEditFollowUp
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._AddEditFollowUp] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._AddEditFollowUp].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._AddEditFollowUp] = value;
        }
    }


    public static string Randomise
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._Randomise] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._Randomise].ToString();
            }

        }



        set
        {
            HttpContext.Current.Session[SessionVariablesAll._Randomise] = value;
        }

    }

    public static string ViewRandomise
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._ViewRandomise] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._ViewRandomise].ToString();
            }

        }

        set
        {
            HttpContext.Current.Session[SessionVariablesAll._ViewRandomise] = value;
        }
    }

    public static string LastLogin
    {
        get
        {

            if (HttpContext.Current.Session[SessionVariablesAll._LastLogin] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpContext.Current.Session[SessionVariablesAll._LastLogin].ToString();
            }

        }



        set
        {
            HttpContext.Current.Session[SessionVariablesAll._LastLogin] = value;
        }

    }
    
}