using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for GeneralRoutines
/// </summary>
public class GeneralRoutines
{
    #region " Private Constants & Variables "

        private const string STRCONN = "cope4dbconn";
        private const string strKeyDataName = "dbname";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

    #endregion


    // one parameter/value
    public static string ReturnScalar(string strSQL, string strParameter, string strParameterValue, string strConn)
    {
        string strRecord = "-1";

        string CS = string.Empty;
        CS = ConfigurationManager.ConnectionStrings[strConn].ConnectionString;

        MySqlConnection MySQLC = new MySqlConnection(CS);

        try
        {

            MySqlCommand CMD = new MySqlCommand(strSQL, MySQLC);

            CMD.Parameters.Clear();
            CMD.Parameters.Add(strParameter, MySqlDbType.VarChar).Value = strParameterValue;

            CMD.Connection.Open();

            strRecord = CMD.ExecuteScalar().ToString();

            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            return strRecord;
        }

        catch
        {
            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            strRecord = "-1";
            return strRecord;


        }


    }

    //two parameters/values
    public static string ReturnScalarTwo(string strSQL, string strParameter, string strParameterValue, string strParameter2, string strParameterValue2, string strConn)
    {
        string strRecord = "-1";

        string CS = string.Empty;
        CS = ConfigurationManager.ConnectionStrings[strConn].ConnectionString;

        MySqlConnection MySQLC = new MySqlConnection(CS);

        try
        {

            MySqlCommand CMD = new MySqlCommand(strSQL, MySQLC);

            CMD.Parameters.Clear();
            CMD.Parameters.Add(strParameter, MySqlDbType.VarChar).Value = strParameterValue;
            CMD.Parameters.Add(strParameter2, MySqlDbType.VarChar).Value = strParameterValue2;

            CMD.Connection.Open();

            strRecord = CMD.ExecuteScalar().ToString();

            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            return strRecord;
        }

        catch
        {
            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            strRecord = "-1";
            return strRecord;


        }


    }


    //Three parameters/values
    public static string ReturnScalarThree(string strSQL, string strParameter, string strParameterValue, string strParameter2, string strParameterValue2, string strParameter3, string strParameterValue3, string strConn)
    {
        string strRecord = "-1";

        string CS = string.Empty;
        CS = ConfigurationManager.ConnectionStrings[strConn].ConnectionString;

        MySqlConnection MySQLC = new MySqlConnection(CS);

        try
        {

            MySqlCommand CMD = new MySqlCommand(strSQL, MySQLC);

            CMD.Parameters.Clear();
            CMD.Parameters.Add(strParameter, MySqlDbType.VarChar).Value = strParameterValue;
            CMD.Parameters.Add(strParameter2, MySqlDbType.VarChar).Value = strParameterValue2;
            CMD.Parameters.Add(strParameter3, MySqlDbType.VarChar).Value = strParameterValue3;

            CMD.Connection.Open();

            strRecord = CMD.ExecuteScalar().ToString();

            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            return strRecord;
        }

        catch
        {
            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            strRecord = "-1";
            return strRecord;


        }


    }


    public static string UpdateScalar(string strSQL, string strParameter, string strParameterValue, string strConn)
    {
        string strRecord = "-1";

        string CS = string.Empty;
        CS = ConfigurationManager.ConnectionStrings[strConn].ConnectionString;

        MySqlConnection MySQLC = new MySqlConnection(CS);

        try
        {

            MySqlCommand CMD = new MySqlCommand(strSQL, MySQLC);

            CMD.Parameters.Clear();
            CMD.Parameters.Add(strParameter, MySqlDbType.VarChar).Value = strParameterValue;

            CMD.Connection.Open();

            strRecord = CMD.ExecuteNonQuery().ToString();

            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            return strRecord;
        }

        catch
        {
            if (MySQLC.State == ConnectionState.Open)
            {
                MySQLC.Close();
            }

            strRecord = "-1";
            return strRecord;


        }


    }

    // check if password adheres to the rules
    //at least a lower case alphabet, an upper case alphabet, one number, 5 unique characters
    //output 2 no lower case, 3 no upper case, 4 no numeric, 5  cannot repeat same character 3 times, 
    //6 unique characters should be 5
    public static int PCheck(string strPWord, int intMaximumLength, int intDistinct)
    {
        int intResultCheck = 0;

        try
        {
            if (strPWord.Length < intMaximumLength)
            { 
                intResultCheck = 1;
                return intResultCheck;            
            }

            if (strPWord.Distinct().Count() < intDistinct)
            {
                intResultCheck = 6; //unique characters less than what is required
                return intResultCheck;
            }
            int CountLower = 0; //2
            int CountUpper = 0; //3
            int CountNumber = 0; //4
            int CountRepeat = 0; //5
            //int CountUnique = 0; //6
            
            int CountRepetitions = 0;

            for (int i = 0; i < strPWord.Length; i++)
            {
                if (char.IsUpper(strPWord[i])) CountUpper++;
                if (char.IsLower(strPWord[i])) CountLower++;
                if (char.IsDigit(strPWord[i])) CountNumber++;

                if (i > 0)
                {
                    if (strPWord[i] == strPWord[i - 1])
                    {
                        if (strPWord[i] == strPWord[i - 1]) CountRepetitions++;

                    }
                    
                    else

                    {
                        CountRepetitions = 0;
                        
                    }

                    if (CountRepetitions > 1) CountRepeat = CountRepetitions;
                }
               



            }

            if (CountLower == 0) intResultCheck = 2;
            if (CountUpper == 0) intResultCheck = 3;
            if (CountNumber == 0) intResultCheck = 4;
            if (CountRepeat > 1) intResultCheck = 5;
            //if (CountUnique < 5) intResultCheck = 6;


            return intResultCheck;
        }
        catch
        {
            intResultCheck = -1;
            
            return intResultCheck;
        }

       

    }

    //public static int PCheckOld(string strPWord, int intMaximumLength, int intDistinct)
    //{
    //    int intResultCheck = 0;

    //    try
    //    {
    //        if (strPWord.Length < intMaximumLength) intResultCheck = 1;


    //        int CountLower = 0; //2
    //        int CountUpper = 0; //3
    //        int CountNumber = 0; //4
    //        int CountRepeat = 0; //5
    //        int CountUnique = 0; //6


    //        int CountRepetitions = 0;

    //        for (int i = 0; i < strPWord.Length; i++)
    //        {
    //            if (char.IsUpper(strPWord[i])) CountUpper++;
    //            if (char.IsLower(strPWord[i])) CountLower++;
    //            if (char.IsDigit(strPWord[i])) CountNumber++;

    //            if (i > 0)
    //            {
    //                if (strPWord[i] == strPWord[i - 1])
    //                {
    //                    if (strPWord[i] == strPWord[i - 1]) CountRepetitions++;

    //                }
    //                else
    //                {
    //                    CountRepetitions = 0;
    //                    CountUnique++;
    //                }

    //                if (CountRepetitions > 1) CountRepeat = CountRepetitions;
    //                }
    //                else
    //                { CountUnique++; }



    //        }

    //        if (CountLower == 0) intResultCheck = 2;
    //        if (CountUpper == 0) intResultCheck = 3;
    //        if (CountNumber == 0) intResultCheck = 4;
    //        if (CountRepeat > 1) intResultCheck = 5;
    //        if (CountUnique < 5) intResultCheck = 6;


    //        return intResultCheck;
    //    }
    //    catch
    //    {
    //        intResultCheck = -1;
    //        return intResultCheck;
    //    }

    //}

    // IsNumeric Function
    public static bool IsNumeric(object Expression)
    {
        // Variable to collect the Return value of the TryParse method.
        bool isNum;

        // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
        double retNum;

        // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
        // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
        isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    //isdate function
    public static bool IsDate(string inValue)
    {
        bool result;
        string strError;

        try
        {
            DateTime dateValue;
            if (DateTime.TryParse(inValue, out dateValue))
            { result = true; }
            else
            { result = false; }
            
        }
        catch (System.Exception excep)
        {
            strError = excep.Message;
            result = false;
        }
        return result;
    }

    //public static string ListAllText()
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;

    //        int intAlCurrent;
    //        int intAlPrevious;

    //        intAlCurrent = 0;
    //        intAlPrevious = 0;

    //        string strTextToSearch = String.Empty;

    //        char[] aryChar = txtSearchValue.Text.ToCharArray();

    //        for (int i = 0; i < aryChar.Length; i++)
    //        {
    //            intAlPrevious = intAlCurrent;

    //            if (i > 0)
    //            {
    //                if (char.IsLetterOrDigit(aryChar[i]))
    //                {
    //                    lblUserMessages.Text += aryChar[i] + ". ";
    //                    intAlCurrent = 1;

    //                }
    //                else
    //                { intAlCurrent = 0; }

    //                if ((intAlCurrent == intAlPrevious) & (intAlCurrent == 1))
    //                {
    //                    strTextToSearch += aryChar[i];
    //                }
    //                else
    //                    if ((intAlCurrent != intAlPrevious) & (intAlCurrent == 1))
    //                    {
    //                        strTextToSearch += aryChar[i];
    //                    }
    //                    else
    //                    {
    //                        if ((intAlCurrent != intAlPrevious) & (intAlCurrent == 0))
    //                        {
    //                            strTextToSearch += ",";
    //                        }

    //                    }

    //            }
    //            else
    //            {
    //                if (char.IsLetterOrDigit(aryChar[i]))
    //                {
    //                    intAlCurrent = 1;
    //                    strTextToSearch += aryChar[i];
    //                }
    //                else
    //                { intAlCurrent = 0; }
    //            }


    //        }
    //        lblCentres.Text = strTextToSearch;

    //    }
    //    catch (Exception excep)
    //    {
    //        lblUserMessages.Text = excep.Message + " Error while retrieving text.";
    //    }
    //}


    public static string ListAllText(string strStringToCheck)
    {
        string strValue = "-1";
        string strTextToSearch = String.Empty;
        try
        {
            //lblUserMessages.Text = string.Empty;

            int intAlCurrent;
            int intAlPrevious;

            intAlCurrent = 0;
            intAlPrevious = 0;



            char[] aryChar = strStringToCheck.ToCharArray();

            for (int i = 0; i < aryChar.Length; i++)
            {
                intAlPrevious = intAlCurrent;

                if (i > 0)
                {
                    if (char.IsLetterOrDigit(aryChar[i]))
                    {
                        //lblUserMessages.Text += aryChar[i] + ". ";
                        intAlCurrent = 1;

                    }
                    else
                    { intAlCurrent = 0; }

                    if ((intAlCurrent == intAlPrevious) & (intAlCurrent == 1))
                    {
                        strTextToSearch += aryChar[i];
                    }
                    else
                        if ((intAlCurrent != intAlPrevious) & (intAlCurrent == 1))
                        {
                            strTextToSearch += aryChar[i];
                        }
                        else
                        {
                            if ((intAlCurrent != intAlPrevious) & (intAlCurrent == 0))
                            {
                                strTextToSearch += ",";
                            }

                        }

                }
                else
                {
                    if (char.IsLetterOrDigit(aryChar[i]))
                    {
                        intAlCurrent = 1;
                        strTextToSearch += aryChar[i];
                    }
                    else
                    { intAlCurrent = 0; }
                }


            }
            //lblCentres.Text = strTextToSearch;
            return strTextToSearch;

        }
        catch
        {
            strValue = "-1";
            return strValue;
            //lblUserMessages.Text = excep.Message + " Error while retrieving text.";

        }
    }


    public static Boolean AssignUserAccessRights(string strCentreCode)
    {

        Boolean blnAccessRights = false;

        try
        {
            string strSQL = string.Empty;
            strSQL = "SELECT t1.* FROM copewpfourother.listdbuser t1 INNER JOIN copewpfourother.listusers t2 ON t1.ListUsersID=t2.ListUsersID ";
            strSQL += "WHERE t2.username=?username ";
            strSQL += "AND t1.dataname=?DataName ";
            strSQL += "AND t1.CentreCode=?CentreCode ";

            string strDataName = System.Configuration.ConfigurationManager.AppSettings.Get(strKeyDataName);

            string CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;
            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = strSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?username", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;
            MyCMD.Parameters.Add("?dataname", MySqlDbType.VarChar).Value = strDataName;
            MyCMD.Parameters.Add("?CentreCode", MySqlDbType.VarChar).Value = strCentreCode;

            MyCONN.Open();
            try
            {
                MySqlDataReader DR = MyCMD.ExecuteReader();

                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        SessionVariablesAll.Web_cope41 = "True";
                        //SessionVariablesAll.UserName = txtUsername.Text;

                        //if (DR["AdminSuperUser"] != DBNull.Value)
                        //{
                        //    SessionVariablesAll.AdminSuperUser = DR["AdminSuperUser"].ToString();
                        //}
                        //else
                        //{
                        //    SessionVariablesAll.AdminSuperUser = "NO";
                        //}

                        if (DR["SuperUser"] != DBNull.Value)
                        {
                            SessionVariablesAll.SuperUser = DR["SuperUser"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.SuperUser = "NO";
                        }

                        if (DR["CreateStudyID"] != DBNull.Value)
                        {
                            SessionVariablesAll.CreateStudyID = DR["CreateStudyID"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.CreateStudyID = "NO";
                        }

                        if (DR["AddEdit"] != DBNull.Value)
                        {
                            SessionVariablesAll.AddEdit = DR["AddEdit"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.AddEdit = "NO";
                        }

                        if (DR["AddEditRecipient"] != DBNull.Value)
                        {
                            SessionVariablesAll.AddEditRecipient = DR["AddEditRecipient"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.AddEditRecipient = "NO";
                        }

                        if (DR["AddEditFollowUp"] != DBNull.Value)
                        {
                            SessionVariablesAll.AddEditFollowUp = DR["AddEditFollowUp"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.AddEditFollowUp = "NO";
                        }

                        if (DR["Randomise"] != DBNull.Value)
                        {
                            SessionVariablesAll.Randomise = DR["Randomise"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.Randomise = "NO";
                        }

                        if (DR["ViewRandomise"] != DBNull.Value)
                        {
                            SessionVariablesAll.ViewRandomise = DR["ViewRandomise"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.ViewRandomise = "NO";
                        }

                        

                        if (DR["centrecode"] != DBNull.Value)
                        {
                            SessionVariablesAll.CentreCode = DR["centrecode"].ToString();
                        }
                        else
                        {
                            SessionVariablesAll.CentreCode = "";
                        }



                    }
                }

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                blnAccessRights = true;

                //Response.Redirect(REDIRECT_HOMEPAGE, false);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (System.Exception ex)
            {
                string strError = ex.Message;
                blnAccessRights = false;
                //lblTopMastePageUserMessages.Text = ex.Message + " An error occured while assigning user access rights.";
            }

            return blnAccessRights;

        }
        catch (System.Exception ex)
        {
            //lblTopMastePageUserMessages.Text = ex.Message + " An error occured while binding item to the Menu Control.";
            string strError = ex.Message;

            blnAccessRights = false;
            return blnAccessRights;

        }
    }


    public static string eGFR_Calculate(string strTrialIDRecipient, Double dblCreatinine,  DateTime dteCreatinineDate, string strUnit)
    {

        string streGFR = "NA";
        string strError = string.Empty;

        try
        {
            
            //check if identification data added
            string STRSQLFIND = @"SELECT COUNT(*) CR FROM r_identification  
                                    WHERE TrialIDRecipient=?TrialIDRecipient";

            Int16 intCountRecipientData = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQLFIND, "?TrialIDRecipient", strTrialIDRecipient, STRCONN));

            if (intCountRecipientData > 1)
            {
                throw new Exception("More than one Records exist in r_identification table.");
            }

            if (intCountRecipientData==0)
            {
                throw new Exception("No Records exist in r_identification table.");
            }


            if (intCountRecipientData < 0)
            {
                throw new Exception("Error while checking if Records exist in r_identification table.");
            }

            //get DOB
//            string STRSQL_DOB = @"SELECT DateOfBirth, FORMAT(DATEDIFF(TransplantationDate,DateOfBirth) / 365.25,2) DOB FROM r_identification t1
//                                    INNER JOIN r_perioperative t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient 
//                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

            string STRSQL_DOB = "SELECT FORMAT(DATEDIFF('" + dteCreatinineDate.ToString("yyyy-MM-dd") + "',DateOfBirth) / 365.25,2) DOB FROM r_identification t1 ";
            STRSQL_DOB +="  ";
            STRSQL_DOB +="WHERE t1.TrialIDRecipient=?TrialIDRecipient ";

            string strDOB = GeneralRoutines.ReturnScalar(STRSQL_DOB, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);

            if (GeneralRoutines.IsNumeric(strDOB) == false || strDOB=="-1")
            {
                throw new Exception("An error occured while calculating Date of Birth.");
            }


            //get ethnicity
            string STRSQL_BLACK = @"SELECT EthnicityBlack FROM r_identification  
                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strEthnicityBlack = GeneralRoutines.ReturnScalar(STRSQL_BLACK, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);


            //check if female
            string STRSQL_FEMALE = @"SELECT Sex FROM r_identification  
                                    WHERE TrialIDRecipient=?TrialIDRecipient ";

            string strFemale = GeneralRoutines.ReturnScalar(STRSQL_FEMALE, "?TrialIDRecipient", strTrialIDRecipient, STRCONN);

            //now calculate
            Double dbleGFR;

            if (strUnit == "µmol/l")
            {
                dbleGFR = 32788;
            }
            else if (strUnit == "mg/dL")
            {
                dbleGFR = 186;
            }
            else
            {
                throw new Exception("Unknown Unit.");
            }

            dbleGFR = dbleGFR * Math.Pow(dblCreatinine, -1.154) * Math.Pow(Convert.ToDouble(strDOB), -0.203);

            if (strEthnicityBlack == STR_YES_SELECTION)
            {
                dbleGFR = dbleGFR * 1.212;
            }

            if (strFemale == "Female")
            {
                dbleGFR = dbleGFR * 0.742;
            }

            streGFR = Math.Round(dbleGFR, 2).ToString(); //round to two decimal places
        }
        catch (Exception ex)
        {
            strError = ex.Message + " An error occured while calulating eGFR.";
            streGFR = "Error";
        }
        return streGFR;

    }

    //public GeneralRoutines()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
}