using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Summary description for ConstantsGeneral
/// </summary>
public static class ConstantsGeneral
{

    //leading characters
    public static string LeadingChars { get { return "WP4"; } }

    //maximum length of trialid
    public static int intTrialIDMaximumLength { get { return 8; } }

    //maximum length of trialid (recipient)
    public static int intTrialIDRecipientMaximumLength { get { return 9; } }

    //Donor Age
    public static int intMinDonorAge { get { return -99; } } //oldest donor to match with range validator's minimum values
    public static int intMaxDonorAge { get { return -50; } } //youngest donor to match with range validator's maximum values

    //Recipient Age negative to subtract from curent date
    public static int intMinRecipientAge { get { return -99; } }
    public static int intMaxRecipientAge { get { return -18; } }

    //Donor Recipient weight
    public static double dblMinDonorWeight { get { return 20; } }
    public static double dblMaxDonorWeight { get { return 200; } }
    public static int intDecimalPlacesDonorWeight { get { return 2; } }


    public static double dblMinDonorWeightRec { get { return 20; } }
    public static double dblMaxDonorWeightRec { get { return 150; } }

    //Donor Recipient Height
    //public static double dblMinDonorHeight { get { return 50; } }
    //public static double dblMaxDonorHeight { get { return 250; } }
    //public static int intDecimalPlacesDonorHeight { get { return 1; } }
    public static int intMinDonorHeight { get { return 100; } }
    public static int intMaxDonorHeight { get { return 250; } }


    public static int intMinDonorHeightRec { get { return 120; } }
    public static int intMaxDonorHeightRec { get { return 250; } }


    //HLA Typing
    public static int intMinHLATyping { get { return 0; } }
    public static int intMaxHLATyping { get { return 99; } }

    //ICU Stay
    public static int intMinICUStay { get { return 0; } }
    public static int intMaxICUStay { get { return 500; } }

    //Systolic Blood Pressure Mean (mmHg)
    public static int intMinSystolicBloodPressure { get { return 10; } }
    public static int intMaxSystolicBloodPressure { get { return 200; } }

    public static int intMinSystolicBloodPressureRec { get { return 10; } }
    public static int intMaxSystolicBloodPressureRec { get { return 200; } }

    //Diastolic Blood Pressure Mean (mmHg)
    public static int intMinDiastolicBloodPressure { get { return 10; } }
    public static int intMaxDiastolicBloodPressure { get { return 200; } }
    public static int intMinDiastolicBloodPressureRec { get { return 10; } }
    public static int intMaxDiastolicBloodPressureRec { get { return 200; } }

    //dopamine last dose
    public static int intMinDopamineLastDose { get { return 0; } }
    public static int intMaxDopamineLastDose { get { return 60; } }

    public static int intMinDopamineLastDoseRec { get { return 0; } }
    public static int intMaxDopamineLastDoseRec { get { return 60; } }

    //dobutamine last dose
    public static int intMinDobutamineLastDose { get { return 0; } }
    public static int intMaxDobutamineLastDose { get { return 60; } }

    public static int intMinDobutamineLastDoseRec { get { return 0; } }
    public static int intMaxDobutamineLastDoseRec { get { return 60; } }


    //NorAdrenalin last dose
    public static int intMinNorAdrenalineLastDose { get { return 0; } }
    public static int intMaxNorAdrenalineLastDose { get { return 120; } }

    public static int intMinNorAdrenalineLastDoseRec { get { return 0; } }
    public static int intMaxNorAdrenalineLastDoseRec { get { return 120; } }

    //Diuresis
    public static int intMinDiuresis { get { return 0; } }
    public static int intMaxDiuresis { get { return 10000; } }

    public static int intMinDiuresisRec { get { return 0; } }
    public static int intMaxDiuresisRec { get { return 10000; } }

    //DurationAnuriaOliguria
    public static int intMinDurationAnuriaOliguria { get { return 0; } }
    public static int intMaxDurationAnuriaOliguria { get { return 24; } }

    public static int intMinDurationAnuriaOliguriaRec { get { return 0; } }
    public static int intMaxDurationAnuriaOliguriaRec { get { return 24; } }


    //Vasopressors
    public static double dblMinVasopressors { get { return 0.001; } }
    public static double dblMaxVasopressors { get { return 100; } }
    public static int intDecimalPlacesVasopressors { get { return 3; } }

    public static double dblMinVasopressorsRec { get { return 0.001; } }
    public static double dblMaxVasopressorsRec { get { return 100; } }

    //donor lab results
    public const double dblMinHb_mgdl = 12;
    public const double dblMaxHb_mgdl = 18;

    public const double dblMinHb_mgdlRec = 12;
    public const double dblMaxHb_mgdlRec = 18;

    public const double dblMinHb_mmol = 1.86;
    public const double dblMaxHb_mmol = 2.71;

    public const double dblMinHb_mmolRec = 1.86;
    public const double dblMaxHb_mmolRec = 2.71;

    //Ht
    public const double dblMinHt = 37;
    public const double dblMaxHt = 54;

    public const double dblMinHtRec = 37;
    public const double dblMaxHtRec = 54;

    public const double dblMinpH = 7.35;
    public const double dblMaxpH = 7.45;

    public const double dblMinpHRec = 7.35;
    public const double dblMaxpHRec = 7.45;

    public const double dblMinpCO2_mmhg = 32;
    public const double dblMaxpCO2_mmhg = 48;

    public const double dblMinpCO2_mmhgRec = 32;
    public const double dblMaxpCO2_mmhgRec = 48;

    public const double dblMinpCO2_kPa = 4.4;
    public const double dblMaxpCO2_kPa = 5.9;
    public const double dblMinpCO2_kPaRec = 4.4;
    public const double dblMaxpCO2_kPaRec = 5.9;

    public const double dblMinpO2_mmhg = 70;
    public const double dblMaxpO2_mmhg = 108;

    public const double dblMinpO2_mmhgRec = 70;
    public const double dblMaxpO2_mmhgRec = 108;

    public const double dblMinpO2_kPa = 10;
    public const double dblMaxpO2_kPa = 14;

    public const double dblMinpO2_kPaRec = 10;
    public const double dblMaxpO2_kPaRec = 14;

    public const double dblMinUrea_gdl = 0;
    public const double dblMaxUrea_gdl = 50;
    public const double dblMinUrea_gdlRec = 0;
    public const double dblMaxUrea_gdlRec = 50;


    public const double dblMinUrea_mmol = 0;
    public const double dblMaxUrea_mmol = 8.3;
    public const double dblMinUrea_mmolRec = 0;
    public const double dblMaxUrea_mmolRec = 8.3;

    public const double dblMinCreatinine_mgdl = 0.51;
    public const double dblMaxCreatinine_mgdl = 1.17;

    public const double dblMinCreatinine_mgdlRec = 0.51;
    public const double dblMaxCreatinine_mgdlRec = 1.17;

    public const double dblMinCreatinine_micromol = 53;
    public const double dblMaxCreatinine_micromol = 106;

    public const double dblMinCreatinine_micromolRec = 53;
    public const double dblMaxCreatinine_micromolRec = 106;


    //Creatinine
    public static int intMinCreatinine { get { return 10; } }
    public static int intMaxCreatinine { get { return 100; } }

    public static int intMinCreatinineRec { get { return 10; } }
    public static int intMaxCreatinineRec { get { return 100; } }

    //WashoutVolume
    public static int intMinWashoutVolume { get { return 1; } }
    public static int intMaxWashoutVolume { get { return 10000; } }

    public static int intMinWashoutVolumeRec { get { return 1; } }
    public static int intMaxWashoutVolumeRec { get { return 10000; } }

    //public static int intDecimalPlacesWashoutVolume { get { return 2; } }


    //PreTransplantUO
    public static int intMinPreTransplantUO { get { return 0; } }
    public static int intMaxPreTransplantUO { get { return 10000; } }

    public static int intMinPreTransplantUORec { get { return 0; } }
    public static int intMaxPreTransplantUORec { get { return 10000; } }


    //Number of Previous Transplants
    public static int intMinNumberPreviousTransplants { get { return 0; } }
    public static int intMaxNumberPreviousTransplants { get { return 10; } }

    public static int intMinNumberPreviousTransplantsRec { get { return 0; } }
    public static int intMaxNumberPreviousTransplantsRec { get { return 10; } }


    //HLA Mismatch
    public static int intMinHLAMismatch { get { return 0; } }
    public static int intMaxHLAMismatch { get { return 2; } }


    //Anastomosis Time
    public static int intMinAnastomosisTime { get { return 0; } }
    public static int intMaxAnastomosisTime { get { return 120; } }

    public static int intMinAnastomosisTimeRec { get { return 0; } }
    public static int intMaxAnastomosisTimeRec { get { return 120; } }


    //Maximum numbr of days post Operation Start
    public static int intMaxDaysPostOperationStart { get { return 5; } }

    //SerumCreatinine
    public static int intMinSerumCreatinine { get { return 0; } }
    public static int intMaxSerumCreatinine { get { return 2000; } }

    public static int intMinSerumCreatinineRec { get { return 0; } }
    public static int intMaxSerumCreatininRece { get { return 2000; } }



    //Number acute rejection episodes
    public static int intMinNumberAcuteRejections { get { return 0; } }
    public static int intMaxNumberAcuteRejections { get { return 20; } }

    public static int intMinNumberAcuteRejectionsRec { get { return 0; } }
    public static int intMaxNumberAcuteRejectionsRec { get { return 20; } }


    //Number Biopsy Proven Rejections
    public static int intMinNumberBiopsyProvenRejections { get { return 0; } }
    public static int intMaxNumberBiopsyProvenRejections { get { return 20; } }


    public static int intMinNumberBiopsyProvenRejectionsRec { get { return 0; } }
    public static int intMaxNumberBiopsyProvenRejectionsRec { get { return 20; } }


    //follow updays
    //1-14 Days
    public static int intMin1_14Days { get { return 0; } }
    public static int intMax1_14Days { get { return 30; } }

    //3 Months
    public static int intMin3Months { get { return -30; } }
    public static int intMax3Months { get { return 30; } }


    //6 Months
    public static int intMin6Months { get { return -45; } }
    public static int intMax6Months { get { return 45; } }

    //1 Year 
    public static int intMin1Year { get { return -60; } }
    public static int intMax1Year { get { return 60; } }


    //1 Year
    public static int intMinResUse { get { return 0; } }
    public static int intMaxResUse { get { return 100; } }

    //Generic messages

    #region
        //LockedMessage
        public static string LockedMessage { get { return "This record is now locked. If corrections are required please notify the COPE Trials office &lt;a href=&quot;mailto:cope.trials@nds.ox.ac.uk&quot;>cope.trials@nds.ox.ac.uk&lt;/a>"; } }

        //FinalMessage
        public static string FinalMessage { get { return "This record is now Final. If corrections are required please notify the COPE Trials office &lt;a href=&quot;mailto:cope.trials@nds.ox.ac.uk&quot;>cope.trials@nds.ox.ac.uk&lt;/a>"; } }

        //Locked and Final Message
        public static string LockedFinalMessage { get { return "This record is now locked/marked as Final. If corrections are required please notify the COPE Trials office &lt;a href=&quot;mailto:cope.trials@nds.ox.ac.uk&quot;>cope.trials@nds.ox.ac.uk&lt;/a>"; } }

        //MandatoryMessage
        public static string MandatoryMessage { get { return "Fields with an Asterisk must be entered. Fields in Bold/Red are Mandatory for data to be classified as Complete."; } }


        //TimeMessage
        public static string TimeMessage { get { return "Please Enter Time in 24 hour format"; } }

        //DateMessage
        public static string strDateMessage { get { return "Please Enter Date as dd/mm/yyyy"; } }

        //IncompleteCSS
        public static string IncompleteCSS { get { return "Incomplete"; } }

        //IncompleteColour
        public static string IncompleteColour { get { return "Red"; } }


        //SCode1
        public static string SCode1 { get { return "Please complete fields highlighted in Red if available for Data to be classified as complete"; } }

        //SCode1
        public static string RecipientConsentMessage { get { return "No recipient sampling or data will be collected. "; } }

        public static string RecipientInclusionMessage { get { return "No further data will be collected. "; } }
        
    #endregion
}