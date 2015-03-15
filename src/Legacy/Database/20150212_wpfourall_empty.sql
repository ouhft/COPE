-- MySQL dump 10.13  Distrib 5.6.17, for Win64 (x86_64)
--
-- Host: localhost    Database: copewpfourother
-- ------------------------------------------------------
-- Server version	5.6.12-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `copewpfourother`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `copewpfourother` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `copewpfourother`;

--
-- Table structure for table `listdbuser`
--

DROP TABLE IF EXISTS `listdbuser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `listdbuser` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `dataname` varchar(20) NOT NULL,
  `ListusersID` int(10) unsigned NOT NULL,
  `centrecode` tinyint(3) unsigned NOT NULL,
  `AdminSuperUser` varchar(45) DEFAULT NULL COMMENT 'Admin Super User',
  `SuperUser` varchar(45) DEFAULT NULL,
  `CreateStudyID` varchar(45) DEFAULT NULL COMMENT 'Create StudyID',
  `AddEdit` varchar(45) DEFAULT NULL COMMENT '0 No, 1 Yes',
  `AddEditRecipient` varchar(45) DEFAULT NULL COMMENT 'Add/Edit/View Follow Up Data',
  `AddEditFollowUp` varchar(45) DEFAULT NULL COMMENT 'Add/Edit/View Follow Up Data',
  `PersonalData` varchar(45) DEFAULT NULL,
  `Randomise` varchar(45) DEFAULT NULL COMMENT 'Can the person randomise',
  `ViewRandomise` varchar(45) DEFAULT NULL,
  `SAECentre` varchar(45) DEFAULT NULL,
  `SAECentreComments` varchar(300) DEFAULT NULL,
  `TrialIDCentre` varchar(45) DEFAULT NULL,
  `TrialIDCentreComments` varchar(300) DEFAULT NULL,
  `Comments` varchar(300) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(100) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(100) DEFAULT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  KEY `Index_listusersID` (`ListusersID`),
  KEY `Index_centrecode` (`centrecode`),
  KEY `Index_4` (`dataname`)
) ENGINE=InnoDB AUTO_INCREMENT=849 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `listusers`
--

DROP TABLE IF EXISTS `listusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `listusers` (
  `ListusersID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL COMMENT 'Minimum 8 characters',
  `Active` tinyint(3) unsigned DEFAULT NULL COMMENT '0 Inactive, 1 Active, 2 Suspended',
  `LockedUser` tinyint(3) unsigned DEFAULT NULL COMMENT '1 Locked',
  `LockedUserTimestamp` datetime DEFAULT NULL COMMENT 'TIimestamp Locked',
  `UnlockUserBy` varchar(45) DEFAULT NULL,
  `UnlockUserTimestamp` datetime DEFAULT NULL,
  `UserPword` char(128) DEFAULT NULL,
  `FirstName` varchar(100) DEFAULT NULL,
  `LastName` varchar(100) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `SAEAllCentre` varchar(45) DEFAULT NULL COMMENT 'If Serious Adverse Emails need to be sent for all Centres (YES/NO/NULL)',
  `SAEAllCentreComments` varchar(500) DEFAULT NULL COMMENT 'Comments for SAE All Centre',
  `TrialIDCreated` varchar(45) DEFAULT NULL COMMENT 'If an email has to to be sent to this user wehn TrialID is crteated/ Liver Randomised',
  `TrialIDCreatedComments` varchar(500) DEFAULT NULL,
  `JobTitle` varchar(100) DEFAULT NULL,
  `Centre` varchar(45) DEFAULT NULL,
  `Comments` varchar(500) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(100) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(100) DEFAULT NULL,
  `PassUpdated` datetime DEFAULT NULL,
  `PassUpdatedBy` varchar(100) DEFAULT NULL,
  `DatePassUpdatedSuperUser` datetime DEFAULT NULL,
  `PassUpdatedSuperUser` varchar(100) DEFAULT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ListusersID`),
  KEY `Index_username` (`username`),
  KEY `Index_3` (`Centre`)
) ENGINE=InnoDB AUTO_INCREMENT=182 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usererrors`
--

DROP TABLE IF EXISTS `usererrors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usererrors` (
  `UserErrorsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `UserID` varchar(45) DEFAULT NULL,
  `ErrorCode` tinyint(3) unsigned DEFAULT NULL COMMENT 'Error Code',
  `DbName` varchar(100) DEFAULT NULL,
  `LoggedIn` datetime DEFAULT NULL,
  `IPAddress` varchar(45) DEFAULT NULL,
  `SessionID` varchar(300) DEFAULT NULL COMMENT 'ASP.NET_SessionId',
  PRIMARY KEY (`UserErrorsID`),
  KEY `Index_2` (`UserID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=996 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userexitlogs`
--

DROP TABLE IF EXISTS `userexitlogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userexitlogs` (
  `UserExitLogsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `UserID` varchar(45) DEFAULT NULL,
  `DbName` varchar(100) DEFAULT NULL,
  `LoggedOut` datetime NOT NULL,
  `IPAddress` varchar(45) DEFAULT NULL,
  `SessionID` varchar(300) DEFAULT NULL COMMENT 'ASPNet Session ID',
  PRIMARY KEY (`UserExitLogsID`),
  KEY `Index_2` (`UserID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userlogs`
--

DROP TABLE IF EXISTS `userlogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userlogs` (
  `UserLogsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `UserID` varchar(45) DEFAULT NULL,
  `DbName` varchar(100) DEFAULT NULL,
  `LoggedIn` datetime NOT NULL,
  `IPAddress` varchar(45) DEFAULT NULL,
  `SessionID` varchar(300) DEFAULT NULL COMMENT 'ASPNet Session ID',
  PRIMARY KEY (`UserLogsID`),
  KEY `Index_2` (`UserID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=725 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'copewpfourother'
--

--
-- Dumping routines for database 'copewpfourother'
--

--
-- Current Database: `cope_wp_four`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `cope_wp_four` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `cope_wp_four`;

--
-- Table structure for table `copy_donor_generalproceduredata`
--

DROP TABLE IF EXISTS `copy_donor_generalproceduredata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_donor_generalproceduredata` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `DonorGeneralProcedureDataID` int(10) unsigned NOT NULL COMMENT 'Unique ID for the Table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `TransplantTechnician` varchar(100) DEFAULT NULL COMMENT 'Name of Transplant Technician earlier -  perfusion ',
  `TransplantCoordinatorPhoneDate` date DEFAULT NULL COMMENT 'Date phone call from transplant coordinator received ',
  `TransplantCoordinatorPhoneTime` time DEFAULT NULL COMMENT 'Time phone call from transplant coordinator received ',
  `TransplantCoordinator` varchar(100) DEFAULT NULL COMMENT 'Name of transplant coordinator',
  `TelephoneTransplantCoordinator` varchar(100) DEFAULT NULL COMMENT 'Telephone number of Transplant Coordinator ',
  `RetrievalHospital` varchar(100) DEFAULT NULL COMMENT 'Hospital of retrieval',
  `ScheduledStartWithdrawlDate` date DEFAULT NULL COMMENT 'Scheduled date for start retrieval procedure ',
  `ScheduledStartWithdrawlTime` time DEFAULT NULL COMMENT 'Scheduled  time for start retrieval procedure ',
  `TechnicianArrivalHubDate` date DEFAULT NULL COMMENT ' Arrival date of technician at Hub earlier perfusion centre ',
  `TechnicianArrivalHubTime` time DEFAULT NULL COMMENT ' Arrival time of technician at Hub earlier perfusion centre ',
  `IceBoxesFilledDate` date DEFAULT NULL COMMENT 'Date Ice Boxes filled with Sufficient ice ',
  `IceBoxesFilledTime` time DEFAULT NULL COMMENT 'Time Ice Boxes filled with Sufficient ice ',
  `DepartHubDate` date DEFAULT NULL COMMENT 'Date of departure from Hub earlier perfusion centre ',
  `DepartHubTime` time DEFAULT NULL COMMENT 'Time of departure from Hub earlier perfusion centre ',
  `ArrivalDonorHospitalDate` date DEFAULT NULL COMMENT 'Date of arrival at donor hospital ',
  `ArrivalDonorHospitalTime` time DEFAULT NULL COMMENT 'Time of arrival at donor hospital ',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Remarks Free Text; Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'Date Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp',
  `EventCode` tinyint(4) NOT NULL DEFAULT '10',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`DonorGeneralProcedureDataID`) USING BTREE,
  KEY `Index_2` (`TrialID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Donor General procedure data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_donor_identification`
--

DROP TABLE IF EXISTS `copy_donor_identification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_donor_identification` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `DonorIdentificationID` int(10) unsigned NOT NULL COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT '6 Digit/Characters',
  `DonorCentre` varchar(100) DEFAULT NULL COMMENT 'Donor Centre',
  `DateDonorAdmission` date DEFAULT NULL COMMENT 'Date of admission ',
  `DateDonorOperation` date DEFAULT NULL COMMENT 'Date of donor operation ',
  `DateOfBirth` date DEFAULT NULL COMMENT 'Donor Date Of Birth',
  `Sex` varchar(45) DEFAULT NULL COMMENT 'Donor sex Male/Female',
  `Weight` varchar(45) DEFAULT NULL COMMENT 'Donor weight kg',
  `Height` varchar(45) DEFAULT NULL COMMENT 'Donor Height in centimetres',
  `BMI` varchar(45) DEFAULT NULL COMMENT 'Donor BMI Derived Value',
  `BloodGroup` varchar(45) DEFAULT NULL COMMENT 'Donor blood group O/A/B/AB',
  `HLA_A` varchar(45) DEFAULT NULL COMMENT 'HLA-typing A ; Not mandatory for Transplant technician Mandatory for national CRA',
  `HLA_B` varchar(45) DEFAULT NULL COMMENT 'HLA-typing B ; Not mandatory for Transplant technician Mandatory for national CRA',
  `HLA_DR` varchar(45) DEFAULT NULL COMMENT 'HLA-typing DR ; Not mandatory for Transplant technician Mandatory for national CRA',
  `KidneyLeftDonated` varchar(45) DEFAULT NULL COMMENT 'Kidney left donated YES/NO',
  `KidneyRightDonated` varchar(45) DEFAULT NULL COMMENT ' Kidney right donated YES/NO',
  `OtherOrgansDonated` varchar(300) DEFAULT NULL COMMENT 'Bowel-YES/No; Heart-YES/No;Liver-YES/No;Lung-YES/No;Pancreas-YES/No;Tissue-YES/No',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (free text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Last Update DateTIme',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp',
  `EventCode` tinyint(4) NOT NULL DEFAULT '11',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`DonorIdentificationID`) USING BTREE,
  KEY `Index_2` (`TrialID`) USING BTREE,
  KEY `Index_3` (`DonorID`) USING BTREE,
  KEY `Index_4` (`DateOfBirth`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Donor identification';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_donor_labresults`
--

DROP TABLE IF EXISTS `copy_donor_labresults`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_donor_labresults` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `DonorLabResultsID` int(10) unsigned NOT NULL,
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Donor ID',
  `Hb` varchar(45) DEFAULT NULL COMMENT 'Hb (mmol/l OR mg/dL) ??? Units country specific',
  `HbUnit` varchar(45) DEFAULT NULL COMMENT 'Hb Unit (mmol/l or mg/dL)',
  `Ht` varchar(45) DEFAULT NULL COMMENT 'Ht (%)',
  `pH` varchar(45) DEFAULT NULL COMMENT 'pH  Not required removed Sarah Email 20140418',
  `pCO2` varchar(45) DEFAULT NULL COMMENT 'pCO2  Not required removed Sarah Email 20140418',
  `pCO2Unit` varchar(45) DEFAULT NULL,
  `pO2` varchar(45) DEFAULT NULL COMMENT 'pO2  Not required removed Sarah Email 20140418',
  `pO2Unit` varchar(45) DEFAULT NULL,
  `Urea` varchar(45) DEFAULT NULL COMMENT 'Urea (mmol/l OR mg/dL) ??? Units country specific',
  `UreaUnit` varchar(45) DEFAULT NULL COMMENT 'Urea Unit (mmol/l or mg/dL)',
  `MeanCreatinine` varchar(45) DEFAULT NULL COMMENT 'Mean Creatinine (umol/l OR mg/dL) ??? Units country specific',
  `MeanCreatinineUnit` varchar(45) DEFAULT NULL COMMENT 'Mean Creatinine Unit (µmol/l or mg/dL)',
  `MaxCreatinine` varchar(45) DEFAULT NULL COMMENT 'Max Creatinine (umol/l OR mg/dL) ??? Units country specific',
  `MaxCreatinineUnit` varchar(45) DEFAULT NULL COMMENT 'Max Creatinine Unit (µmol/l or mg/dL)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (free text); Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the Table',
  `EventCode` tinyint(4) DEFAULT '13',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`DonorLabResultsID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Donor laboratory values';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_donor_operationdata`
--

DROP TABLE IF EXISTS `copy_donor_operationdata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_donor_operationdata` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `DonorOperatationDataID` int(10) unsigned NOT NULL COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Foreign Key from donor_identification table',
  `WithdrawlLifeSupportTreatmentDate` date DEFAULT NULL COMMENT 'date of withdrawal of life supporting treatment (dd/mm/yyyy hh:mm)',
  `WithdrawlLifeSupportTreatmentTime` time DEFAULT NULL COMMENT 'Time of withdrawal of life supporting treatment (dd/mm/yyyy hh:mm)',
  `SystolicArterialPressureBelow50Date` date DEFAULT NULL COMMENT 'Date of Systolic arterial pressure below 50 mmHg/inadequate organ perfusion (dd/mm/yyyy hh:mm)',
  `SystolicArterialPressureBelow50Time` time DEFAULT NULL COMMENT 'Time of Systolic arterial pressure below 50 mmHg/inadequate organ perfusion (dd/mm/yyyy hh:mm)',
  `StartNoTouchPeriodDate` date DEFAULT NULL COMMENT 'Date of start no touch period (dd/mm/yyyy hh:mm)',
  `StartNoTouchPeriodTime` time DEFAULT NULL COMMENT 'Time of start no touch period (dd/mm/yyyy hh:mm)',
  `CirculatoryArrestDate` date DEFAULT NULL COMMENT 'Date of circulatory arrest (dd/mm/yyyy hh:mm)',
  `CirculatoryArrestTime` time DEFAULT NULL COMMENT 'Time of circulatory arrest (dd/mm/yyyy hh:mm)',
  `LengthNoTouchPeriod` int(10) unsigned DEFAULT NULL COMMENT 'Length of no touch period (minutes)',
  `ConfirmationDeathDate` date DEFAULT NULL COMMENT 'Date of confirmation of death: (dd/mm/yyyy hh:mm)',
  `ConfirmationDeathTime` time DEFAULT NULL COMMENT 'Time of confirmation of death: (dd/mm/yyyy hh:mm)',
  `StartInSituColdPerfusionDate` date DEFAULT NULL COMMENT 'Date of start in-situ cold perfusion (dd/mm/yyyy hh:mm)',
  `StartInSituColdPerfusionTime` time DEFAULT NULL COMMENT 'Time of start in-situ cold perfusion (dd/mm/yyyy hh:mm)',
  `SystemicFlushSolutionUsed` varchar(45) DEFAULT NULL COMMENT 'Systemic flush solution used (UW/HTK/Marshall/Other)',
  `SystemicFlushSolutionUsedOther` varchar(100) DEFAULT NULL COMMENT 'If Systemic flush solution used is Other)',
  `PreservationSolutionColdPerfusion` varchar(45) DEFAULT NULL COMMENT 'Preservation solution used for cold perfusion(UW/HTK/Marshall/Other)',
  `PreservationSolutionColdPerfusionOther` varchar(100) DEFAULT NULL COMMENT 'If Other Preservation solution used for cold perfusion(UW/HTK/Marshall/Other)',
  `VolumeSolutionColdPerfusion` int(10) unsigned DEFAULT NULL COMMENT 'Volume of solution used for cold perfusion (ml)',
  `Heparin` varchar(45) DEFAULT NULL COMMENT 'Heparin (YES/NO)',
  `TotalWarmIschaemicPeriod` varchar(45) DEFAULT NULL COMMENT 'total warm ischaemic period WithdrawlLifeSupportTreatment-StartInSituColdPerfusion in Minutes',
  `WithdrawalPeriod` varchar(45) DEFAULT NULL COMMENT 'withdrawal period WithdrawlLifeSupportTreatment-CirculatoryArrest in Minutes',
  `FunctionalWarmIschaemicPeriod` varchar(45) DEFAULT NULL COMMENT 'functional warm ischaemic period MeanArterialPressureBelow50-StartInSituColdPerfusion in Minutes',
  `AsystolicWarmIschaemicPeriod` varchar(45) DEFAULT NULL COMMENT 'asystolic warm ischaemic period CirculatoryArrest-StartInSituColdPerfusion',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments (Free Text); Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '14',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`DonorOperatationDataID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Donor operation data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_donor_preop_clinicaldata`
--

DROP TABLE IF EXISTS `copy_donor_preop_clinicaldata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_donor_preop_clinicaldata` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `DonorPreOpClinicalDataID` int(10) unsigned NOT NULL COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Foreign Key from Donor Identification',
  `Diagnosis` varchar(300) DEFAULT NULL COMMENT 'Diagnosis multi trauma if YES/NO',
  `DiagnosisOtherDetails` varchar(100) DEFAULT NULL COMMENT 'If Diagnosis Other',
  `DiabetesMellitus` varchar(45) DEFAULT NULL COMMENT 'Diabetes Mellitus (IDDM) YES/NO',
  `AlcoholAbuse` varchar(45) DEFAULT NULL COMMENT 'If Alcohol Abuse YES/NO ',
  `CardiacArrest` varchar(45) DEFAULT NULL COMMENT 'Cardiac arrest (during ICU stay prior to retrieval procedure)  YES/NO',
  `SystolicBloodPressure` varchar(45) DEFAULT NULL COMMENT 'Systolic blood pressure (Before Switch Off) mm/Hg',
  `DiastolicBloodPressure` varchar(45) DEFAULT NULL COMMENT 'Systolic blood pressure (Before Switch Off) mm/Hg',
  `HypotensivePeriod` varchar(45) DEFAULT NULL COMMENT 'Hypotensive period (syst. < 100 mmHg) YES/NO',
  `Diuresis` varchar(45) DEFAULT NULL COMMENT 'Mean diuresis / hr. last 24 hrs',
  `DonorAnuriaOliguria` varchar(45) DEFAULT NULL COMMENT 'Donor anuria / oliguria (< 500 ml/24h) YES/NO',
  `DurationAnuriaOliguria` varchar(45) DEFAULT NULL COMMENT 'duration an-/oliguria in hours',
  `Dopamine` varchar(45) DEFAULT NULL COMMENT 'Dopamine YES/NO',
  `DopamineLastDose` varchar(45) DEFAULT NULL COMMENT 'Dopamine Last Does (mg); Not Mandatory',
  `Dobutamine` varchar(45) DEFAULT NULL COMMENT 'Dobutamine YES/NO',
  `DobutamineLastDose` varchar(45) DEFAULT NULL COMMENT 'Dobutamine Last Dose (mg); Not Mandatory',
  `NorAdrenaline` varchar(45) DEFAULT NULL COMMENT '(Nor)adrenaline YES/NO',
  `NorAdrenalineLastDose` varchar(45) DEFAULT NULL COMMENT 'NorAdrenaline Last Dose (mg); Not Mandatory',
  `OtherMedication` varchar(100) DEFAULT NULL COMMENT 'Other medication Details; Not Mandatory',
  `OtherMedicationLastDose` varchar(45) DEFAULT NULL COMMENT 'Other Medication Last Dose (mg); Not Mandatory',
  `OtherMedication2` varchar(100) DEFAULT NULL COMMENT 'Other Medication (Second)',
  `OtherMedication2LastDose` varchar(45) DEFAULT NULL COMMENT 'Other Medication (Second) last Dose',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (Free Text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '12',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`DonorPreOpClinicalDataID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Donor pre-operative clinical data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_kidneyinspection`
--

DROP TABLE IF EXISTS `copy_kidneyinspection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_kidneyinspection` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `KidneyInspectionID` int(10) unsigned NOT NULL COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL,
  `Side` varchar(45) DEFAULT NULL COMMENT 'LEFT/RIGHT',
  `PreservationModality` varchar(45) DEFAULT NULL COMMENT 'Preservation modality (HMP with oxygen/ HMP without oxygen )',
  `RandomisationComplete` varchar(45) DEFAULT NULL COMMENT 'YES/NO valve left open or closed and gauge concealed',
  `NumberRenalArteries` varchar(45) DEFAULT NULL COMMENT 'Number of renal arteries ',
  `ArterialProblems` varchar(300) DEFAULT NULL COMMENT 'Arterial damage (YES/NO); Venous damage (YES/NO); Ureteral damage (YES/NO); Parenchymal damage (YES/NO)',
  `WashoutPerfusion` varchar(45) DEFAULT NULL COMMENT 'Washout Perfusion (Homogenous/Patchy/Blue)',
  `RemovalDate` date DEFAULT NULL COMMENT 'Removal Time (hh:mm)',
  `RemovalTime` time DEFAULT NULL COMMENT 'Removal Time (hh:mm)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (free text)',
  `PreservationModality_R` varchar(45) DEFAULT NULL COMMENT 'Preservation modality (HMP with oxygen/ HMP without oxygen ) Right Kidney',
  `RandomisationComplete_R` varchar(45) DEFAULT NULL COMMENT 'YES/NO valve left open or closed and gauge concealed Right Kidney',
  `NumberRenalArteries_R` varchar(45) DEFAULT NULL COMMENT 'Number of renal arteries  Right Kidney',
  `ArterialProblems_R` varchar(300) DEFAULT NULL COMMENT 'Arterial damage (YES/NO); Venous damage (YES/NO); Ureteral damage (YES/NO); Parenchymal damage (YES/NO) Right Kidney',
  `WashoutPerfusion_R` varchar(45) DEFAULT NULL COMMENT 'Washout Perfusion (Homogenous/Patchy/Blue) Right Kidney',
  `Removal_RDate` date DEFAULT NULL COMMENT 'Removal Time (hh:mm) Right Kidney',
  `Removal_RTime` time DEFAULT NULL COMMENT 'Removal Time (hh:mm) Right Kidne',
  `Comments_R` varchar(500) DEFAULT NULL COMMENT 'General Comments Right Kidney (free text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '27',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`KidneyInspectionID`) USING BTREE,
  KEY `Index_3` (`DonorID`),
  KEY `Index_2` (`TrialID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Donor operation kidney data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_kidneyproceduredata`
--

DROP TABLE IF EXISTS `copy_kidneyproceduredata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_kidneyproceduredata` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `KidneyProcedureDataID` int(10) unsigned NOT NULL,
  `TrialID` varchar(45) NOT NULL COMMENT 'TrialID',
  `Side` varchar(45) NOT NULL COMMENT 'Side of Kindey (Left/Right)',
  `TransplantTechnician` varchar(100) DEFAULT NULL COMMENT 'Name of transplant technician',
  `DonorTechnicianPhoneDate` date DEFAULT NULL COMMENT 'Date phone call received from colleague technician involved in donor procedure ',
  `DonorTechnicianPhoneTime` time DEFAULT NULL COMMENT 'Time phone call received from colleague technician involved in donor procedure ',
  `TransplantHospital` varchar(100) DEFAULT NULL COMMENT 'Transplant hospital',
  `TechnicianDonorProcedure` varchar(100) DEFAULT NULL COMMENT 'Name of colleague technician involved in donor procedure',
  `TransplantHospitalContact` varchar(100) DEFAULT NULL COMMENT 'Name of recipient hospital’s operating theatre contact person/supervisor ',
  `TransplantHospitalContactPhone` varchar(100) DEFAULT NULL COMMENT 'Telephone number of recipient hospital’s operating theatre contact person/supervisor ',
  `ScheduledTransplantStartDate` date DEFAULT NULL COMMENT 'Transplant Start Date',
  `ScheduledTransplantStartTime` time DEFAULT NULL COMMENT 'Transplant Start Time',
  `TechnicianArrivalPerfusionCentreDate` date DEFAULT NULL COMMENT 'Arrival date of technician at perfusion centre ',
  `TechnicianArrivalPerfusionCentreTime` time DEFAULT NULL COMMENT 'Arrival time of technician at perfusion centre ',
  `TechnicianDeparturePerfusionCentreDate` date DEFAULT NULL COMMENT 'Date of departure from perfusion centre ',
  `TechnicianDeparturePerfusionCentreTime` time DEFAULT NULL COMMENT 'Time of departure from perfusion centre ',
  `ArrivalTransplantHospitalDate` date DEFAULT NULL COMMENT 'Date of arrival at transplant hospital ',
  `ArrivalTransplantHospitalTime` time DEFAULT NULL COMMENT 'Time of arrival at transplant hospital ',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Remarks',
  `Reallocated` varchar(100) DEFAULT NULL COMMENT 'Kidney re-allocated to another transplant centre YES/NO',
  `ReasonReallocated` varchar(100) DEFAULT NULL COMMENT 'Reason for re-allocation if positive crossmatch',
  `ReasonReallocatedOther` varchar(100) DEFAULT NULL COMMENT 'Reason for re-allocation if Other',
  `NewRecipientHospitalContact` varchar(100) DEFAULT NULL COMMENT 'Name of new recipient hospital’s operating theatre contact person/supervisor ',
  `NewRecipientHospitalContactPhone` varchar(100) DEFAULT NULL COMMENT 'Telephone number of new recipient hospital’s operating theatre contact person/supervisor ',
  `NewTransplantHospital` varchar(100) DEFAULT NULL COMMENT 'New transplant hospital',
  `NewScheduledTransplantStartDate` date DEFAULT NULL COMMENT 'New scheduled date for start transplant procedure ',
  `NewScheduledTransplantStartTime` time DEFAULT NULL COMMENT 'New scheduled time for start transplant procedure ',
  `DepartureFirstTransplantHospitalDate` date DEFAULT NULL COMMENT 'Date of departure from first transplant hospital ',
  `DepartureFirstTransplantHospitalTime` time DEFAULT NULL COMMENT 'Time of departure from first transplant hospital ',
  `ArrivalNewTransplantHospitalDate` date DEFAULT NULL COMMENT 'Date of arrival at new transplant hospital ',
  `ArrivalNewTransplantHospitalTime` time DEFAULT NULL COMMENT 'Time of arrival at new transplant hospital ',
  `NewComments` varchar(500) DEFAULT NULL COMMENT 'Additional Comments',
  `TechnicianDepartureTransplantDate` date DEFAULT NULL COMMENT 'Date of technician’s departure from transplant hospital',
  `TechnicianDepartureTransplantTime` time DEFAULT NULL COMMENT 'Time of technician’s departure from transplant hospital',
  `FinalArrivalPerfusionCentreDate` date DEFAULT NULL COMMENT 'Date of technician’s arrival at perfusion centre',
  `FinalArrivalPerfusionCentreTime` time DEFAULT NULL COMMENT 'Time of technician’s arrival at perfusion centre ',
  `TechnicianEndProcessDate` date DEFAULT NULL COMMENT 'End time of entire procedure for this technician',
  `TechnicianEndProcessTime` time DEFAULT NULL COMMENT 'End time of entire procedure for this technician',
  `TechnicianDepartureComments` varchar(500) DEFAULT NULL COMMENT 'Technician Departure Remarks',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `DateAdded` datetime DEFAULT NULL,
  `EventCode` tinyint(3) DEFAULT '30' COMMENT 'Machine Perfusion Dara default event code is 20',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`KidneyProcedureDataID`),
  KEY `Index_2` (`TrialID`) USING BTREE,
  KEY `Index_3` (`Side`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Recipient General Procedure Data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_kidneyr`
--

DROP TABLE IF EXISTS `copy_kidneyr`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_kidneyr` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `KidneyRID` int(10) unsigned NOT NULL,
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique TrialID',
  `DonorID` varchar(45) DEFAULT NULL,
  `LeftKidneyDonate` varchar(45) DEFAULT NULL COMMENT 'Left Kidney Donated',
  `RightKidneyDonate` varchar(45) DEFAULT NULL COMMENT 'Right Kidney Donates',
  `InclusionCriteriaChecked` varchar(45) DEFAULT NULL COMMENT 'Inclusion Criteria Checked (YES/NO)',
  `ExclusionCriteriaChecked` varchar(45) DEFAULT NULL COMMENT 'Exclusion Criteria Checked (YES/NO)',
  `ConsentChecked` varchar(45) DEFAULT NULL COMMENT 'Consent Checked (YES/NO)',
  `LeftRanCategory` varchar(45) DEFAULT NULL COMMENT 'Left Kidney Randomised To',
  `LeftRandomisationArm` varchar(100) DEFAULT NULL,
  `RightRanCategory` varchar(45) DEFAULT NULL COMMENT 'Right Kidney Randomised To',
  `RightRandomisationArm` varchar(100) DEFAULT NULL,
  `WPFour_RandomID` int(10) unsigned DEFAULT NULL COMMENT 'Unique Identifier from Randomisation Table',
  `Comments` varchar(500) DEFAULT NULL,
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Time stamp',
  `EventCode` tinyint(3) unsigned NOT NULL DEFAULT '25' COMMENT 'Default Event Code for the Table',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`KidneyRID`) USING BTREE,
  KEY `Index_2` (`TrialID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_machineperfusion`
--

DROP TABLE IF EXISTS `copy_machineperfusion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_machineperfusion` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `MachinePerfusionID` int(10) unsigned NOT NULL COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Foreign Key from donor_identification table',
  `Side` varchar(45) DEFAULT NULL COMMENT 'Side of Kidney (Left/Right)',
  `KidneyOnMachine` varchar(45) DEFAULT NULL COMMENT 'Is Machine Perfusion Possible (YES/NO)',
  `KidneyOnMachineNo` varchar(500) DEFAULT NULL COMMENT 'If kidney can be placed on machine NO, details (Free Text) ',
  `PerfusionStartDate` date DEFAULT NULL COMMENT 'Date Perfusion Started (dd/mm/yyyy)',
  `PerfusionStartTime` time DEFAULT NULL COMMENT 'Time Perfusion Started (hh:mm)',
  `MachineSerialNumber` varchar(45) DEFAULT NULL COMMENT 'Machine Seria lNumber',
  `MachineReferenceModelNumber` varchar(45) DEFAULT NULL COMMENT 'Reference Mode lNumber for the machine',
  `LotNumberPerfusionSolution` varchar(100) DEFAULT NULL,
  `LotNumberDisposables` varchar(100) DEFAULT NULL COMMENT 'Lot Number of Disposables',
  `Cannulae` varchar(45) DEFAULT NULL COMMENT 'Type of Cannulae Used (Sealring/Straight Cannula)',
  `CannulaeNumber` varchar(45) DEFAULT NULL COMMENT 'Cannulae Number Used 1/2/3',
  `UsedPatchHolder` varchar(45) DEFAULT NULL COMMENT 'Small/ Large/ Double Artery',
  `ArtificialPatchUsed` varchar(45) DEFAULT NULL COMMENT 'YES/NO',
  `ArtificialPatchSize` varchar(45) DEFAULT NULL COMMENT 'Small/ Large',
  `ArtificialPatchNumber` varchar(45) DEFAULT NULL COMMENT '1/2',
  `OxygenBottleFull` varchar(45) DEFAULT NULL,
  `OxygenBottleOpened` varchar(45) DEFAULT NULL,
  `OxygenTankChanged` varchar(45) DEFAULT NULL COMMENT 'Oxygen tank changed',
  `OxygenTankChangedDate` date DEFAULT NULL,
  `OxygenTankChangedTime` time DEFAULT NULL,
  `IceContainerReplenished` varchar(45) DEFAULT NULL COMMENT 'Oxygen tank changed',
  `IceContainerReplenishedDate` date DEFAULT NULL,
  `IceContainerReplenishedTime` time DEFAULT NULL,
  `LogisticallyPossibleMeasurepO2Perfusate` varchar(45) DEFAULT NULL COMMENT 'Logistically possible to measure pO2 of perfusate (YES/NO)',
  `ValuepO2Perfusate` varchar(45) DEFAULT NULL COMMENT 'Value pO2 of Perfusate',
  `ValuepO2PerfusateMeasured` varchar(100) DEFAULT NULL COMMENT 'How was pO2 measured',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments (Free Text); Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the table',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `EventCode` tinyint(3) unsigned NOT NULL DEFAULT '29' COMMENT 'Default Event Code for the Table',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`MachinePerfusionID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Machine Perfusion';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_r_ae`
--

DROP TABLE IF EXISTS `copy_r_ae`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_r_ae` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `AEID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'TrialIDRecipient',
  `DateAE` date NOT NULL COMMENT 'Date of Adverse Event (Date)',
  `AdverseEventType` varchar(100) DEFAULT NULL COMMENT 'Adverse Event Type (Recipient infection/ Biopsy proven acute rejection/ Bile leak/ Biliary stricture (anastomotic)/ Biliary stricture (non-anastomotic)/\nBleeding/ Hepatic artery thrombosis/ Portal vein thrombosis/ Hepatic artery stenosis/ Portal vein stenosis/ Re-operation/ Other)',
  `RecipientInfectionType` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Type (Pneumonia/ Surgical Site Infection/ Intra-abdominal Collection/ Other)',
  `RecipientInfectionTypeOther` varchar(100) DEFAULT NULL COMMENT 'If Recipient Infection Type is Other (Free Text)',
  `RecipientInfectionOrganismBacteria` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Organism Bacteria (YES/NO)',
  `RecipientInfectionOrganismBacteriaDetails` varchar(100) DEFAULT NULL COMMENT 'Details if Recipient Infection Organism Bacteria is YES (Free text)',
  `RecipientInfectionOrganismViral` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Organism Viral (YES/NO)',
  `RecipientInfectionOrganismViralDetails` varchar(100) DEFAULT NULL COMMENT 'Details if Recipient Infection Organism Viral is YES (Free text)',
  `RecipientInfectionOrganismFungal` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Organism Fungal (YES/NO)',
  `RecipientInfectionOrganismFungalDetails` varchar(100) DEFAULT NULL COMMENT 'Details if Recipient Infection Organism Fungal is YES (Free text)',
  `RecipientInfectionCG` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BiopsyProvenAcuteRejectionBG` varchar(100) DEFAULT NULL COMMENT 'Biopsy Proven Acute Rejection Banff Grading (Indeterminate/ Mild/ Moderate/ Severe)',
  `BiopsyProvenAcuteRejectionCG` varchar(100) DEFAULT NULL COMMENT 'Biopsy Proven Acute Rejection Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BileLeakCG` varchar(100) DEFAULT NULL COMMENT 'Bile Leak Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BiliaryStrictureAnastomoticCG` varchar(100) DEFAULT NULL COMMENT 'Biliary Stricture Anastomotic Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BiliaryStrictureNonAnastomoticCG` varchar(100) DEFAULT NULL COMMENT 'Biliary Stricture NonA nastomotic Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BleedingCG` varchar(100) DEFAULT NULL COMMENT 'Bleeding Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `HepaticArteryThrombosisCG` varchar(100) DEFAULT NULL COMMENT 'Hepatic Artery Thrombosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `PortalVeinThrombosisCG` varchar(100) DEFAULT NULL COMMENT 'Portal Vein Thrombosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `HepaticArteryStenosisCG` varchar(100) DEFAULT NULL COMMENT 'Hepatic Artery Stenosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `PortalVeinStenosisCG` varchar(100) DEFAULT NULL COMMENT 'Portal VeinStenosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `ReOperationCG` varchar(100) DEFAULT NULL COMMENT 'ReOperation Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `ReOperationDetails` varchar(500) DEFAULT NULL COMMENT 'ReOperation Details if Adverse Event Type= ReOperation',
  `OtherAdverseEvent` varchar(500) DEFAULT NULL COMMENT 'Other Adverse Event (Free Text)',
  `ClavienGrading` varchar(45) DEFAULT NULL COMMENT 'Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `OtherAdverseEventCG` varchar(100) DEFAULT NULL COMMENT 'Other Adverse Event Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the Table',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`AEID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COMMENT='Adverse Events';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_r_deceased`
--

DROP TABLE IF EXISTS `copy_r_deceased`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_r_deceased` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `RDeceasedID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(10) NOT NULL COMMENT 'Unique ID for Liver being transplanted (ABCCC -A is country code, B Centre number, where the Transplant is done, CCC running number)',
  `DeathDate` date DEFAULT NULL COMMENT 'Date of Death (Date)',
  `DeathTime` time DEFAULT NULL COMMENT 'Date of Death (Time)',
  `CauseDeath` varchar(100) DEFAULT NULL COMMENT 'Tx/Non Tx',
  `CauseDeathDetails` varchar(500) DEFAULT NULL COMMENT 'From Multiple Selection',
  `CauseDeathDetailsOther` varchar(100) DEFAULT NULL COMMENT 'If other choses in cause of death details (Free Text)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments Withdrawn',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Time stamp',
  `Source` varchar(45) DEFAULT '0',
  KEY `Index_1` (`RDeceasedID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`),
  KEY `Index_3` (`DeathDate`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COMMENT='Deceased Details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_r_fuposttreatment`
--

DROP TABLE IF EXISTS `copy_r_fuposttreatment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_r_fuposttreatment` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `RFUPostTreatmentID` int(10) unsigned NOT NULL,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `RIdentificationID` varchar(45) DEFAULT NULL COMMENT 'RecipientID',
  `Occasion` varchar(45) NOT NULL COMMENT '3 Months/ 6 months/ 1 Year',
  `FollowUpDate` date NOT NULL COMMENT 'Date of Follow Up',
  `GraftFailure` varchar(45) NOT NULL COMMENT 'Graft Failure (YES/NO)',
  `DateGraftFailure` date DEFAULT NULL COMMENT 'Date of Graft Failure (Date)',
  `PrimaryCause` varchar(100) DEFAULT NULL COMMENT 'Primary Cause (Immunologic/ Preservation/ Technical - Arterial/ Technical - Venous/ Infection - Bacterial/ Infection - Viral/ Other)',
  `PrimaryCauseOther` varchar(100) DEFAULT NULL COMMENT 'If Primary Cause if Other',
  `GraftRemoval` varchar(45) DEFAULT NULL COMMENT 'Graft Removal (YES/NO)',
  `DateGraftRemoval` date DEFAULT NULL COMMENT 'Date of Graft Removal (Date)',
  `Death` varchar(45) DEFAULT NULL COMMENT 'Death (YES/NO)',
  `DateDeath` date DEFAULT NULL COMMENT 'Date of Death (Date)',
  `CauseDeath` varchar(45) DEFAULT NULL COMMENT 'Cause of Death (Tx Related/ Non Tx Related)',
  `RequiredHyperkalemia` varchar(100) DEFAULT NULL COMMENT 'Required for hyperkalemia or fluid overload, If only 1 dialysis session  (YES/NO)',
  `HypotensivePeriod1` varchar(100) DEFAULT NULL COMMENT 'Hypotensive period I First 24 Hours Post Transplant (YES/NO)',
  `HypotensivePeriod1Duration` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period I Duration (Minutes)',
  `HypotensivePeriod1LowestSystolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period I Lowest Systolic Blood Pressure (mmHg)',
  `HypotensivePeriod1LowestDiastolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period I LowestDiastolic Blood Pressure (mmHg)',
  `HypotensivePeriod2` varchar(100) DEFAULT NULL COMMENT 'Hypotensive period II First 24 Hours Post Transplant (YES/NO)',
  `HypotensivePeriod2Duration` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period II Duration (Minutes)',
  `HypotensivePeriod2LowestSystolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period II Lowest Systolic Blood Pressure (mmHg)',
  `HypotensivePeriod2LowestDiastolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period II LowestDiastolic Blood Pressure (mmHg)',
  `SerumCreatinine` varchar(300) DEFAULT NULL COMMENT 'Serum Creatinine (µmol/l or mg/dL)',
  `CreatinineUnit` varchar(45) DEFAULT NULL COMMENT 'Creatine Unit (µmol/l or mg/dL)',
  `UrineCreatinine` varchar(300) DEFAULT NULL COMMENT 'UrineCreatinine (mmol/l or mg/dL)',
  `UrineUnit` varchar(45) DEFAULT NULL COMMENT 'Urine Unit (mmol/l or mg/dL)',
  `CreatinineClearance` varchar(300) DEFAULT NULL COMMENT 'Creatinine Clearance (ml/min)',
  `CreatinineClearanceUnit` varchar(45) DEFAULT NULL COMMENT 'CreatinineClearanceUnit ml/min',
  `EGFR` varchar(500) DEFAULT NULL COMMENT 'eGFR (Derived Value)',
  `CurrentlyDialysis` varchar(45) DEFAULT NULL COMMENT 'Currently on Dialysis (YES/NO)',
  `DialysisType` varchar(45) DEFAULT NULL COMMENT 'Dialysis Type (CAPD/Hemodialysis)',
  `DateDialysisSince` date DEFAULT NULL COMMENT 'Date Last Dialysis Since, if currently on Dialysis (Date)',
  `DateLastDialysis` date DEFAULT NULL COMMENT 'Date of last dialysis (Date)',
  `DialysisSessions` varchar(45) DEFAULT NULL COMMENT 'Number of Dialysis Session Since Last Follow Up (Number)',
  `InductionTherapy` varchar(45) DEFAULT NULL COMMENT 'Induction Therapy',
  `NumberRejectionPeriods` varchar(45) DEFAULT NULL COMMENT 'Number of rejection period (Number)',
  `PostTxImmunosuppressive` varchar(500) DEFAULT NULL COMMENT 'Post Tx Immunosuppressive Drugs (Azathioprine/Cyclosporin/MMF/Prednisolone/Sirolomus/Tacrolimus/Other)',
  `PostTxImmunosuppressiveOther` varchar(100) DEFAULT NULL COMMENT 'Post Tx Immunosuppressive Drugs If Other',
  `RejectionTreatmentsPostTx` varchar(45) DEFAULT NULL COMMENT 'Number of treatments for rejection post transplant, since last follow up (Number)',
  `Rejection` varchar(45) DEFAULT NULL COMMENT 'Rejection (YES/NO)',
  `PostTxPrednisolon` varchar(45) DEFAULT NULL COMMENT 'Post TX Rejection Treated with Prednisolon (YES/NO)',
  `PostTxOther` varchar(300) DEFAULT NULL COMMENT 'Post TX Rejection Treated with Other Drug (YES/NO)',
  `PostTxOtherDetails` varchar(100) DEFAULT NULL COMMENT 'Post TX Rejection Treated with Other Drug Details (Free Text)',
  `RejectionBiopsyProven` varchar(45) DEFAULT NULL COMMENT ' Rejection Biopsy Proven (YES/NO)',
  `CalcineurinInhibitorToxicity` varchar(45) DEFAULT NULL COMMENT 'Calcineurin Inhibitor Toxicity (YES/NO)',
  `NeedDialysis` varchar(300) DEFAULT NULL COMMENT 'Need Dialysis For First Follow Up, Days 1-7, 10 and 14 (YES/NO)',
  `DatePrimaryPostTxDischarge` date DEFAULT NULL COMMENT 'Date of primary post-Tx discharge ',
  `ComplicationsGraftFunction` varchar(500) DEFAULT NULL COMMENT 'Complications interfering with graft function not mentioned above',
  `QOLFilledAt` varchar(45) DEFAULT NULL,
  `Mobility` varchar(45) DEFAULT NULL COMMENT 'Mobility Quality of Life (1-5, 9 missing)',
  `SelfCare` varchar(45) DEFAULT NULL COMMENT 'Self Care Quality of Life (1-5, 9 missing)',
  `UsualActivities` varchar(45) DEFAULT NULL COMMENT 'Usual Activities Quality of Life (1-5, 9 missing)',
  `PainDiscomfort` varchar(45) DEFAULT NULL COMMENT 'Pain/Discomfort of Life (1-5, 9 missing)',
  `AnxietyDepression` varchar(45) DEFAULT NULL COMMENT 'Anxiety Depression Quality of Life (1-5, 9 missing)',
  `VASScore` varchar(45) DEFAULT NULL COMMENT 'VAS Score Quality of Life (0-100, 999 missing)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (Free Text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the Table',
  `EventCode` tinyint(3) unsigned DEFAULT NULL COMMENT 'Default Event Code for the Table. Blank as mutliple event codes assigned',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`RFUPostTreatmentID`),
  KEY `Index_3` (`RIdentificationID`),
  KEY `Index_2` (`TrialIDRecipient`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Follow up post transplant 1month, 3 months, 6 months, 1 year ';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_r_identification`
--

DROP TABLE IF EXISTS `copy_r_identification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_r_identification` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `RIdentificationID` int(10) unsigned NOT NULL,
  `TrialID` varchar(45) DEFAULT NULL COMMENT 'UniqueID',
  `KidneyReceived` varchar(45) DEFAULT NULL COMMENT 'Kidney received (LEFT/RIGHT)',
  `TrialIDRecipient` varchar(45) NOT NULL,
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Unique Donor ID',
  `RecipientID` varchar(45) DEFAULT NULL COMMENT 'Unique RecipientID',
  `RecipientCentre` varchar(45) DEFAULT NULL COMMENT 'Name recipient transplantation center (Free  Text)',
  `DateOfBirth` date DEFAULT NULL COMMENT 'Recipient date of birth ',
  `Sex` varchar(45) DEFAULT NULL COMMENT 'Recipient Sex: (Male/ Female)',
  `Weight` varchar(45) DEFAULT NULL COMMENT 'Donor weight kg',
  `Height` varchar(45) DEFAULT NULL COMMENT 'Donor Height in centimetre',
  `BMI` varchar(45) DEFAULT NULL COMMENT 'Donor BMI',
  `EthnicityBlack` varchar(45) DEFAULT NULL COMMENT 'If Ethnicity is Black',
  `RenalDisease` varchar(100) DEFAULT NULL COMMENT 'Renal disease (Glomerular diseases, Polycystic kidneys, Uncertain etiology, Tubular and interstitial diseases, Retransplant/Graft failure,Diabetes,Hypertensive nephroangiosclerosis, Congenital, rare familial/ metabolic disorders, Renovascular and other renal vascular diseases, Neoplasms, Other)',
  `RenalDiseaseOther` varchar(100) DEFAULT NULL COMMENT 'If Renal Disease Other',
  `NumberPreviousTransplants` varchar(45) DEFAULT NULL COMMENT 'Number of previous transplants Not required removed Sarah Email 20140418',
  `PreTransplantDiuresis` varchar(45) DEFAULT NULL COMMENT 'Pre-transplant diuresis (ml/24h)',
  `BloodGroup` varchar(45) DEFAULT NULL COMMENT 'Recipient blood group',
  `HLA_A` varchar(45) DEFAULT NULL COMMENT 'HLA Type A (HLA A)',
  `HLA_B` varchar(45) DEFAULT NULL COMMENT 'HLA Type B (HLA B)',
  `HLA_DR` varchar(45) DEFAULT NULL COMMENT 'HLA Type DR (HLA DR)',
  `ET_Urgency` varchar(45) DEFAULT NULL COMMENT 'ETUrgency 0/1/2/4',
  `HLA_A_Mismatch` varchar(45) DEFAULT NULL COMMENT 'Number of HLA mismatches (HLA A)',
  `HLA_B_Mismatch` varchar(45) DEFAULT NULL COMMENT 'Number of HLA mismatches (HLA B)',
  `HLA_DR_Mismatch` varchar(45) DEFAULT NULL COMMENT 'Number of HLA mismatches (HLA DR)',
  `Occasion` varchar(45) DEFAULT NULL COMMENT 'For Quality of Life Data',
  `DateQOLFIlled` varchar(45) DEFAULT NULL,
  `QOLFilledAt` varchar(45) DEFAULT NULL,
  `Mobility` varchar(45) DEFAULT NULL COMMENT 'Mobility Quality of Life (1-5, 9 missing)',
  `SelfCare` varchar(45) DEFAULT NULL COMMENT 'Self Care Quality of Life (1-5, 9 missing)',
  `UsualActivities` varchar(45) DEFAULT NULL COMMENT 'Usual Activities Quality of Life (1-5, 9 missing)',
  `PainDiscomfort` varchar(45) DEFAULT NULL COMMENT 'Pain/Discomfort of Life (1-5, 9 missing)',
  `AnxietyDepression` varchar(45) DEFAULT NULL COMMENT 'Anxiety Depression Quality of Life (1-5, 9 missing)',
  `VASScore` varchar(45) DEFAULT NULL COMMENT 'VAS Score Quality of Life (0-100, 999 missing)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (Free Text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '31',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`RIdentificationID`),
  KEY `Index_3` (`DonorID`),
  KEY `Index_4` (`RecipientID`),
  KEY `Index_2` (`TrialID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Recipient identification';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_r_perioperative`
--

DROP TABLE IF EXISTS `copy_r_perioperative`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_r_perioperative` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `RPerioperativeID` int(10) unsigned NOT NULL,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `RIdentificationID` int(10) unsigned DEFAULT NULL COMMENT 'Foreign Key r_identification',
  `TransplantationDate` date DEFAULT NULL COMMENT 'Transplantation date (Date)',
  `MachinePerfusionStopDate` date DEFAULT NULL COMMENT 'Date Stop machine perfusion (dd-mm-yyyy hh:mm)',
  `MachinePerfusionStopTime` time DEFAULT NULL COMMENT 'Time Stop machine perfusion (dd-mm-yyyy hh:mm)',
  `TimeOnMachine` varchar(45) DEFAULT NULL COMMENT 'calculate from interval ‘time stop machine perfusion to time left/right kidney on machine in donor data connect left or right to Kidney received in recipient identification',
  `TapeBroken` varchar(45) DEFAULT NULL COMMENT 'Was the Tape over the reagulator Broken?',
  `KidneyRemovedFromIceDate` date DEFAULT NULL,
  `KidneyRemovedFromIceTime` time DEFAULT NULL,
  `OxygenBottleFullAndTurnedOpen` varchar(45) DEFAULT NULL,
  `ColdIschemiaPeriod` varchar(45) DEFAULT NULL,
  `ColdIschemiaPeriodHours` varchar(45) DEFAULT NULL COMMENT 'Cold Ischemia Period (Hours) `ReclampingTime` varchar(45) DEFAULT NULL COMMENT Reclamping time (minutes)',
  `ColdIschemiaPeriodMinutes` varchar(45) DEFAULT NULL COMMENT 'Cold Ischemia Period (Minutes)',
  `KidneyDiscarded` varchar(45) DEFAULT NULL COMMENT 'Kidney Discarded /untransplantable (YES/NO)',
  `KidneyDiscardedYes` varchar(500) DEFAULT NULL COMMENT 'If Kidney Discarded is YES, Reason???',
  `OperationStartDate` date DEFAULT NULL COMMENT 'Start Date operation (induction of anesthesia) (yyyy-mm-dd)',
  `OperationStartTime` time DEFAULT NULL COMMENT 'Start time operation (induction of anesthesia) (hh:mm)',
  `CVPReperfusion` varchar(45) DEFAULT NULL COMMENT 'CVP at Reperfusion (mmHg)',
  `Incision` varchar(45) DEFAULT NULL COMMENT 'Incision (Med. laparotomy/extraperitoneal ie hockey stick incision)',
  `TransplantSide` varchar(45) DEFAULT NULL COMMENT 'Transplant Side (Left/Right)',
  `ArterialProblems` varchar(45) DEFAULT NULL COMMENT 'Arterial problems (no/ligated polar artery/reconstructed polar or hilar artery/repaired intima dissection/other)',
  `ArterialProblemsOther` varchar(500) DEFAULT NULL COMMENT 'If Arterial Problems Other',
  `VenousProblems` varchar(45) DEFAULT NULL COMMENT 'Venous problems (YES/NO)',
  `StartAnastomosisDate` date DEFAULT NULL COMMENT 'Start anastomosis Date',
  `StartAnastomosisTime` time DEFAULT NULL COMMENT 'Start anastomosis Time(hh:mm)',
  `ReperfusionDate` date DEFAULT NULL COMMENT 'Time of reperfusion (hh:mm)',
  `ReperfusionTime` time DEFAULT NULL COMMENT 'Time of reperfusion (hh:mm)',
  `TotalAnastomosisTime` varchar(45) DEFAULT NULL COMMENT 'Total Anastomosis time (Minutes)',
  `MannitolUsed` varchar(45) DEFAULT NULL COMMENT 'Mannitol (YES/NO)',
  `DiureticsUsed` varchar(45) DEFAULT NULL COMMENT 'Diuretics (YES/NO)',
  `HypotensivePeriod` varchar(45) DEFAULT NULL COMMENT 'Hypotensive period - syst. < 100 mmHg (YES/NO)',
  `IntraoperativeDiuresis` varchar(45) DEFAULT NULL COMMENT 'Intra-operative diuresis (YES/ NO/ Unknown)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Remarks Second',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Record Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Record Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp',
  `EventCode` tinyint(4) DEFAULT '31',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`RPerioperativeID`) USING BTREE,
  KEY `Index_3` (`RIdentificationID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Recipient operation peri-operative data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_r_readmissions`
--

DROP TABLE IF EXISTS `copy_r_readmissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_r_readmissions` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `RReadmissionsID` int(10) unsigned NOT NULL,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Trial ID',
  `RecipientID` varchar(45) DEFAULT NULL COMMENT 'Recipient ID',
  `RFUPostTreatmentID` int(10) unsigned NOT NULL,
  `Occasion` varchar(45) DEFAULT NULL COMMENT 'Occasion',
  `DateAdmission` date DEFAULT NULL COMMENT 'Date of Admission (Date)',
  `DateDischarge` date DEFAULT NULL COMMENT 'Date of Discharge (Date)',
  `ICU` varchar(45) DEFAULT NULL COMMENT 'If admitted to ICU YES/NO',
  `NeedDialysis` varchar(45) DEFAULT NULL COMMENT 'if Dialysis Needed YES/NO',
  `BiopsyTaken` varchar(45) DEFAULT NULL COMMENT 'if Biopsy Taken YES/NO',
  `Surgery` varchar(45) DEFAULT NULL COMMENT 'If Surgery requried YES/NO',
  `ReasonAdmission` varchar(500) DEFAULT NULL COMMENT 'Reason for Admission',
  `DataLocked` tinyint(4) DEFAULT '0',
  `DateLocked` datetime DEFAULT NULL,
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `ReasonModified` varchar(10000) DEFAULT NULL,
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the Table',
  KEY `Index_1` (`RReadmissionsID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`),
  KEY `Index_3` (`RecipientID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Readmissions During Follow Up';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_r_serae`
--

DROP TABLE IF EXISTS `copy_r_serae`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_r_serae` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `SerAEID` int(10) unsigned NOT NULL,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'TrialIDRecipient',
  `DateOnset` date NOT NULL COMMENT 'Date of Onset (Date)',
  `SerialNumber` varchar(45) DEFAULT NULL COMMENT 'Sequential number of the Serious Adverse Event for the indivudal',
  `Ongoing` varchar(45) DEFAULT NULL COMMENT 'Still Ongoing (YES/NO)',
  `DateResolution` date DEFAULT NULL COMMENT 'Date of Resolution (Date)',
  `DescriptionEvent` varchar(500) DEFAULT NULL COMMENT 'Description of Event (Free Text)',
  `ActionTaken` varchar(500) DEFAULT NULL COMMENT 'Other Complications',
  `Outcome` varchar(500) DEFAULT NULL COMMENT 'Outcome (Free Text)',
  `ContactName` varchar(100) DEFAULT NULL COMMENT 'Name of the Contact Person',
  `ContactPhone` varchar(45) DEFAULT NULL COMMENT 'Phone Number of the Contact Person',
  `ContactEmail` varchar(100) DEFAULT NULL COMMENT 'Email of Contact Person',
  `OtherDetails` varchar(500) DEFAULT NULL COMMENT 'Is this AE serious YES/NO; Did it result in death YES/NO, Did it result in permanent disability YES/NO; Did it result in incapacity/inability to do work YES/NO; Did it result in a sign/symptom that interferes with subject’s usual activity YES/NO; Did it cause signs/symptoms that resolved with no sequelae YES/NO; Did this AE arise from device deficiency YES/NO; Did this AE arise from device user error YES/NO;',
  `ContactDetails` varchar(500) DEFAULT NULL COMMENT 'Name, Phone Number and Email of the person to Contact for this Serious Adverse Event',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the Table',
  KEY `Index_1` (`SerAEID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Serious Adverse Events';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_resuse`
--

DROP TABLE IF EXISTS `copy_resuse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_resuse` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `ResUseID` int(10) unsigned NOT NULL,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Trial ID for the Recipient',
  `DateEntered` date DEFAULT NULL COMMENT 'Date of Adverse Event (Date)',
  `Occasion` varchar(100) DEFAULT NULL COMMENT 'Occasion (Day 30/ Month 6)',
  `GPAppointment` varchar(100) DEFAULT NULL COMMENT 'GP Practice Appointment (0-20)',
  `GPHomeVisit` varchar(100) DEFAULT NULL COMMENT 'GP Home Visit (0-20)',
  `GPTelConversation` varchar(100) DEFAULT NULL COMMENT 'GP Telephone Conversation (0-20)',
  `SpecConsultantAppointment` varchar(100) DEFAULT NULL COMMENT 'Specialist/consultant appointment (0-20)',
  `AETreatment` varchar(100) DEFAULT NULL COMMENT 'Treated in A&E (0-20)',
  `AmbulanceAEVisit` varchar(100) DEFAULT NULL COMMENT 'Ambulance to A&E/hospital (0-20)',
  `NurseHomeVisit` varchar(100) DEFAULT NULL COMMENT 'Nurse Home Visit (0-20)',
  `NursePracticeAppointment` varchar(100) DEFAULT NULL COMMENT 'Nurse Practice Appointment (0-20)',
  `PhysiotherapistAppointment` varchar(100) DEFAULT NULL COMMENT 'Physiotherapist appointment (0-20)',
  `OccupationalTherapistAppointment` varchar(100) DEFAULT NULL COMMENT 'Occupational therapist appointment (0-20)',
  `PsychologistAppointment` varchar(100) DEFAULT NULL COMMENT 'Psychologist Appointment (0-20)',
  `CounsellorAppointment` varchar(100) DEFAULT NULL COMMENT 'Counsellor Appointment (0-20)',
  `AttendedDayHospital` varchar(100) DEFAULT NULL COMMENT 'Attended Day Hospital (0-20)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp for the Table',
  `EventCode` tinyint(4) DEFAULT NULL COMMENT '65 for Day 30 66 for Month 6',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`ResUseID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Resource Use';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_specimen`
--

DROP TABLE IF EXISTS `copy_specimen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_specimen` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `SpecimenID` int(10) unsigned NOT NULL,
  `TrialID` varchar(15) DEFAULT NULL,
  `TrialIDRecipient` varchar(15) DEFAULT NULL,
  `Centre` tinyint(3) unsigned DEFAULT NULL COMMENT 'CentreCode',
  `Collected` tinyint(4) DEFAULT NULL COMMENT '0 No, 1 Yes',
  `Barcode` varchar(15) DEFAULT NULL COMMENT 'Unique Barcode',
  `SpecimenType` varchar(45) DEFAULT NULL COMMENT 'Serum, LHPST, K2E,WB, Citrate',
  `SpecimenTypeOther` varchar(45) DEFAULT NULL,
  `TissueSource` varchar(100) DEFAULT NULL COMMENT 'Left/ Right',
  `CollectedBy` varchar(100) DEFAULT NULL COMMENT 'Collected By',
  `CollectedByOther` varchar(100) DEFAULT NULL COMMENT 'If Other Collected By',
  `Occasion` varchar(100) DEFAULT NULL COMMENT 'Extra column to add when the sample was collected',
  `OccasionOther` varchar(100) DEFAULT NULL,
  `DateCollected` date DEFAULT NULL COMMENT 'Date Collected',
  `TimeCollected` time DEFAULT NULL COMMENT 'Time Collected',
  `DateCentrifugation` date DEFAULT NULL,
  `TimeCentrifugation` time DEFAULT NULL,
  `DateFrozen` date DEFAULT NULL COMMENT 'Date Frozen',
  `TimeFrozen` time DEFAULT NULL,
  `Protocol` varchar(45) DEFAULT NULL COMMENT 'Protocol',
  `State` varchar(45) DEFAULT NULL,
  `StateOther` varchar(45) DEFAULT NULL,
  `EstimatedVolume` decimal(10,2) DEFAULT NULL COMMENT 'Estimated Volume',
  `AliquoteNo` varchar(3) DEFAULT '1' COMMENT 'If more than One aliquote',
  `BoxType` varchar(10) DEFAULT NULL COMMENT 'BoxNumber 4 digit',
  `BoxState` varchar(3) DEFAULT NULL,
  `BoxOrder` varchar(3) DEFAULT NULL,
  `BoxDestination` varchar(2) DEFAULT NULL COMMENT 'S-Sheffield, C-Cambridge - Carryover from Protect',
  `BoxNumber` varchar(10) DEFAULT NULL COMMENT 'BoxNumber 4 digit',
  `Position` varchar(45) DEFAULT NULL COMMENT 'Position of Sample in the Box/Rack',
  `Status` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '0-Not Used, 1 - Processed Robot, 2-Data Marked as Deleted, 3- Destroyed, 4-Data Updated, 5-Perished, 10-Consumed',
  `Destroyed` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '0-No, 1 Yes',
  `CodeDestroyed` varchar(45) DEFAULT NULL,
  `CodeDestroyedOther` varchar(100) DEFAULT NULL,
  `DateDestroyed` date DEFAULT NULL COMMENT 'Date When the Sample Was Destroyed',
  `DestroyedBy` varchar(100) DEFAULT NULL COMMENT 'Peson who destroyed the samples',
  `DestroyedByAuto` varchar(45) DEFAULT NULL COMMENT 'User entering destroyed by data',
  `DestroyedByTimeStamp` datetime DEFAULT NULL,
  `NotDestroyedBy` varchar(100) DEFAULT NULL,
  `NotDestroyedByAuto` varchar(45) DEFAULT NULL,
  `NotDestroyedByTimestamp` datetime DEFAULT NULL,
  `ReasonDestroyed` varchar(300) DEFAULT NULL COMMENT 'Reason Specimen was Destroyed',
  `FreezeThaw` int(10) unsigned DEFAULT NULL COMMENT 'Number of freeze thaw cycles, default 0',
  `FreezeThawUnknown` varchar(45) DEFAULT NULL COMMENT 'if freeze thaw numbers are not accurate this column is used, e.g. 2+',
  `ManualProcessed` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '1 Yes was Processed by Hand',
  `RobotProcessed` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '1 Was Processed by Robot',
  `Consumed` tinyint(3) unsigned DEFAULT '0' COMMENT '0-No, 1 Yes',
  `PageVersion` varchar(45) DEFAULT NULL COMMENT 'To keep track of the versions of page',
  `DateAdded` datetime DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `EventCode` varchar(45) NOT NULL DEFAULT '50' COMMENT 'default eventcode for samples',
  `Source` tinyint(3) unsigned DEFAULT NULL,
  KEY `Index_SpecimenID` (`SpecimenID`),
  KEY `Index_Barcode` (`Barcode`),
  KEY `Index_TrialID` (`TrialID`),
  KEY `Index_TrialIDRecipient` (`TrialIDRecipient`),
  KEY `Index_DateCollectedl` (`DateCollected`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Specimen Data ';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_specimenmaindetails`
--

DROP TABLE IF EXISTS `copy_specimenmaindetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_specimenmaindetails` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `SpecimenMainDetailsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(15) DEFAULT NULL,
  `TrialIDRecipient` varchar(15) DEFAULT NULL,
  `ConsentQuod` varchar(45) DEFAULT NULL COMMENT 'changed to If the Donor is included in recipient',
  `ConsentAdditionalSamples` varchar(45) DEFAULT NULL COMMENT 'If Recipient Consent for Additional Samples (YES/NO)',
  `WorksheetBarcode` varchar(45) DEFAULT NULL COMMENT 'Barcode on the Worksheet',
  `ResearcherName` varchar(100) DEFAULT NULL COMMENT 'name of Researcher',
  `ResearcherTelephoneNumber` varchar(45) DEFAULT NULL COMMENT 'Telephone Number of Researcher',
  `SamplesStoredOxfordDate` date DEFAULT NULL COMMENT 'Date Sample stored in -80 freezer oxford',
  `SamplesStoredOxfordTime` time DEFAULT NULL COMMENT 'Time Sample stored in -80 freezer oxford',
  `InitiationNMPTime` time DEFAULT NULL COMMENT 'Time Initiation of normothermic machine preservation Time of placement of liver on NMP device (Time)',
  `LiverRemovedMachineTime` time DEFAULT NULL COMMENT 'Time If NMP Liver removed from machine Time of removal of liver from NMP device  (hh:mm)',
  `ReperfusionTime` time DEFAULT NULL COMMENT 'Time of reperfusion   (hh:mm)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `AllDataAdded` varchar(45) DEFAULT NULL COMMENT 'if all data has been added and Lock routine executed',
  `AllDataAddedLogs` varchar(5000) DEFAULT NULL COMMENT 'Track if AllDataAdded status is changed',
  `DataLocked` tinyint(3) unsigned DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'if all data has been added and Lock routine executed',
  `ReasonModified` varchar(10000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(3) unsigned DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `DateAdded` datetime DEFAULT NULL,
  `Source` tinyint(4) DEFAULT '0' COMMENT 'where the data has come from 0 is if entered in the server directly',
  `EventCode` varchar(45) DEFAULT NULL COMMENT 'default eventcode for samples',
  KEY `Index_SpecimenMainDetailsID` (`SpecimenMainDetailsID`),
  KEY `Index_TrialID` (`TrialID`),
  KEY `Index_WorksheetBarcode` (`WorksheetBarcode`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COMMENT='Specimen Main Details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_tblfileuploads`
--

DROP TABLE IF EXISTS `copy_tblfileuploads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_tblfileuploads` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `RowIndex` int(10) unsigned NOT NULL,
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique TrialID',
  `Side` varchar(45) NOT NULL COMMENT 'Side of Kidney',
  `FileName` varchar(100) DEFAULT NULL COMMENT 'File Name',
  `IPAddress` varchar(100) DEFAULT NULL COMMENT 'IP address from where it was uploaded.',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Free Text',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime when file was uploaded to the server',
  `CreatedBy` varchar(100) DEFAULT NULL COMMENT 'User who uplaoded the file',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when Keyword/Description was last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'User who updated Keyword/Description',
  `IPAddressUpdatedBy` varchar(100) DEFAULT NULL,
  `DateAdded` datetime DEFAULT NULL COMMENT 'Timestamp',
  `Source` tinyint(4) DEFAULT '0' COMMENT 'where the data has come from 0 is if entered in the server directly',
  KEY `Index_1` (`RowIndex`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`FileName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Perfusion File Upload';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_trialdetails`
--

DROP TABLE IF EXISTS `copy_trialdetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_trialdetails` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `TrialDetailsID` int(10) unsigned NOT NULL,
  `TrialID` varchar(10) NOT NULL COMMENT 'Unique ID for Kidney being transplanted',
  `CentreCode` varchar(45) DEFAULT NULL COMMENT 'Centre where donor was added',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Unique ID of Donor ACCC (A is country code, CCC running number',
  `AgeOrDateOfBirth` varchar(45) DEFAULT NULL COMMENT 'If Age selected or Date Of Birth selected',
  `DonorAge` varchar(45) DEFAULT NULL COMMENT 'Age of Donor',
  `DateOfBirthDonor` date DEFAULT NULL COMMENT 'Unique ID of Donor',
  `DonorAccept` enum('YES','NO') DEFAULT NULL COMMENT 'Check if accepted',
  `KidneySide` enum('Left','Right') DEFAULT NULL COMMENT 'Check if accepted',
  `RanCategory` varchar(45) DEFAULT NULL COMMENT 'Randomised To',
  `WP3RandomID` int(10) unsigned DEFAULT NULL COMMENT 'Unique Identifier from Randomisation Table',
  `Active` tinyint(3) unsigned NOT NULL DEFAULT '1' COMMENT '0 Inactive, 1 Active',
  `Comments` varchar(500) DEFAULT NULL,
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Time stamp',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`TrialDetailsID`) USING BTREE,
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Main Details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_trialdetails_recipient`
--

DROP TABLE IF EXISTS `copy_trialdetails_recipient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_trialdetails_recipient` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `TrialDetails_RecipientID` int(10) unsigned NOT NULL,
  `TrialID` varchar(10) NOT NULL COMMENT 'Unique ID for Kidney being transplanted',
  `KidneyReceived` varchar(45) DEFAULT NULL COMMENT 'Kidney received (LEFT/RIGHT)',
  `TrialIDRecipient` varchar(45) DEFAULT NULL,
  `TransplantCentre` varchar(100) DEFAULT NULL COMMENT 'Name of recipient transplantation center (Free  Text)',
  `RecipientInformedConsent` varchar(45) DEFAULT NULL COMMENT 'Has the recipient signed an informed consent form? (YES/NO)',
  `Recipient18Year` varchar(45) DEFAULT NULL COMMENT 'Is the recipient >18y old? (YES/NO)',
  `RecipientMultipleDualTransplant` varchar(45) DEFAULT NULL COMMENT 'Recipient will not undergo a multiple/dual transplant (YES/NO)',
  `Active` tinyint(3) unsigned NOT NULL DEFAULT '1' COMMENT '0 Inactive, 1 Active',
  `Comments` varchar(500) DEFAULT NULL,
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Time stamp',
  `Source` tinyint(3) DEFAULT '0',
  KEY `Index_1` (`TrialDetails_RecipientID`) USING BTREE,
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`TrialIDRecipient`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Main Details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `copy_trialidwithdrawn`
--

DROP TABLE IF EXISTS `copy_trialidwithdrawn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `copy_trialidwithdrawn` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'New Timestamp for the table',
  `TrialIDWithdrawnID` int(10) unsigned NOT NULL,
  `TrialIDRecipient` varchar(10) NOT NULL COMMENT 'Unique ID for Kidney being transplanted (WP3ABCCC -A is country code, B Centre number, where the Transplant is done, CCC running number)',
  `DateWithdrawn` date DEFAULT NULL COMMENT 'If Withdrawn, Date Withdrawn from the Study',
  `ReasonWithdrawn` varchar(100) DEFAULT NULL COMMENT 'Reason person withdrew from teh Study',
  `ReasonWithdrawnOther` varchar(100) DEFAULT NULL COMMENT 'If Reason Withdrawn is Other',
  `WithdrawnComments` varchar(500) DEFAULT NULL COMMENT 'Comments Withdrawn',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Time stamp',
  `Source` varchar(45) DEFAULT '0',
  KEY `Index_1` (`TrialIDWithdrawnID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`),
  KEY `Index_3` (`DateWithdrawn`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Withdrawn details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_bioresource`
--

DROP TABLE IF EXISTS `donor_bioresource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_bioresource` (
  `BioresourceID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `CopeW4ID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) NOT NULL,
  `BloodSamplesTaken` varchar(45) DEFAULT NULL COMMENT 'Donor blood samples taken (2 tubes, 1 pearl, 1 gold) (YES/NO)',
  `BloodSamplesCentrifuged` varchar(45) DEFAULT NULL COMMENT 'Donor blood samples centrifuged (YES/NO)',
  `LeftKidneyBiopsyTaken` varchar(45) DEFAULT NULL COMMENT 'Left kidney biopsy taken (YES/NO)',
  `LeftKidneyBiopsySplitAndProcessed` varchar(45) DEFAULT NULL COMMENT 'Left kidney biopsy split and processed (YES/NO)',
  `LeftKidneyReasonNo` varchar(500) DEFAULT NULL COMMENT 'If Left Kidney Biopsy Not taken Rason (Free Text)',
  `RightKidneyBiopsyTaken` varchar(45) DEFAULT NULL COMMENT 'Right kidney biopsy taken (YES/NO)',
  `RightKidneyBiopsySplitAndProcessed` varchar(45) DEFAULT NULL COMMENT 'Right kidney biopsy split and processed (YES/NO)',
  `RightKidneyReasonNo` varchar(500) DEFAULT NULL COMMENT 'If Right Kidney Biopsy Not taken Rason (Free Text)',
  `TechnicianDepartureHospitalDate` date DEFAULT NULL COMMENT 'Date of departure technician from donor hospital',
  `TechnicianDepartureHospitalTime` time DEFAULT NULL COMMENT 'Time of departure technician from donor hospital',
  `TechnicianArrivalPerfusionDate` date DEFAULT NULL COMMENT 'Date of arrival at perfusion centre ',
  `TechnicianArrivalPerfusionTime` time DEFAULT NULL COMMENT 'Time of arrival at perfusion centre ',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (free text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the Table',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`BioresourceID`) USING BTREE,
  KEY `Index_2` (`CopeW4ID`) USING BTREE,
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Bio resource';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_generalproceduredata`
--

DROP TABLE IF EXISTS `donor_generalproceduredata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_generalproceduredata` (
  `DonorGeneralProcedureDataID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique ID for the Table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `TransplantTechnician` varchar(100) DEFAULT NULL COMMENT 'Name of Transplant Technician earlier -  perfusion ',
  `TransplantCoordinatorPhoneDate` date DEFAULT NULL COMMENT 'Date phone call from transplant coordinator received ',
  `TransplantCoordinatorPhoneTime` time DEFAULT NULL COMMENT 'Time phone call from transplant coordinator received ',
  `TransplantCoordinator` varchar(100) DEFAULT NULL COMMENT 'Name of transplant coordinator',
  `TelephoneTransplantCoordinator` varchar(100) DEFAULT NULL COMMENT 'Telephone number of Transplant Coordinator ',
  `RetrievalHospital` varchar(100) DEFAULT NULL COMMENT 'Hospital of retrieval',
  `ScheduledStartWithdrawlDate` date DEFAULT NULL COMMENT 'Scheduled date for start retrieval procedure ',
  `ScheduledStartWithdrawlTime` time DEFAULT NULL COMMENT 'Scheduled  time for start retrieval procedure ',
  `TechnicianArrivalHubDate` date DEFAULT NULL COMMENT ' Arrival date of technician at Hub earlier perfusion centre ',
  `TechnicianArrivalHubTime` time DEFAULT NULL COMMENT ' Arrival time of technician at Hub earlier perfusion centre ',
  `IceBoxesFilledDate` date DEFAULT NULL COMMENT 'Date Ice Boxes filled with Sufficient ice ',
  `IceBoxesFilledTime` time DEFAULT NULL COMMENT 'Time Ice Boxes filled with Sufficient ice ',
  `DepartHubDate` date DEFAULT NULL COMMENT 'Date of departure from Hub earlier perfusion centre ',
  `DepartHubTime` time DEFAULT NULL COMMENT 'Time of departure from Hub earlier perfusion centre ',
  `ArrivalDonorHospitalDate` date DEFAULT NULL COMMENT 'Date of arrival at donor hospital ',
  `ArrivalDonorHospitalTime` time DEFAULT NULL COMMENT 'Time of arrival at donor hospital ',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Remarks Free Text; Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'Date Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp',
  `EventCode` tinyint(4) NOT NULL DEFAULT '10',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`DonorGeneralProcedureDataID`) USING BTREE,
  KEY `Index_2` (`TrialID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1 COMMENT='Donor General procedure data';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_generalproceduredata_AFTER_UPDATE` AFTER UPDATE ON `donor_generalproceduredata` FOR EACH ROW
 INSERT INTO  copy_donor_generalproceduredata
(parentrow_action, DonorGeneralProcedureDataID, TrialID, TransplantTechnician, TransplantCoordinatorPhoneDate, TransplantCoordinatorPhoneTime, TransplantCoordinator,
TelephoneTransplantCoordinator, RetrievalHospital, ScheduledStartWithdrawlDate, ScheduledStartWithdrawlTime, TechnicianArrivalHubDate,
TechnicianArrivalHubTime, IceBoxesFilledDate, IceBoxesFilledTime, DepartHubDate, DepartHubTime, ArrivalDonorHospitalDate, ArrivalDonorHospitalTime,
Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.DonorGeneralProcedureDataID, OLD.TrialID, OLD.TransplantTechnician, OLD.TransplantCoordinatorPhoneDate, OLD.TransplantCoordinatorPhoneTime,
OLD.TransplantCoordinator, OLD.TelephoneTransplantCoordinator, OLD.RetrievalHospital, OLD.ScheduledStartWithdrawlDate, OLD.ScheduledStartWithdrawlTime, OLD.TechnicianArrivalHubDate,
OLD.TechnicianArrivalHubTime, OLD.IceBoxesFilledDate, OLD.IceBoxesFilledTime, OLD.DepartHubDate, OLD.DepartHubTime, OLD.ArrivalDonorHospitalDate, OLD.ArrivalDonorHospitalTime,
OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_generalproceduredata_AFTER_DELETE` AFTER DELETE ON `donor_generalproceduredata` FOR EACH ROW
    INSERT INTO  copy_donor_generalproceduredata
(parentrow_action, DonorGeneralProcedureDataID, TrialID, TransplantTechnician, TransplantCoordinatorPhoneDate, TransplantCoordinatorPhoneTime, TransplantCoordinator,
TelephoneTransplantCoordinator, RetrievalHospital, ScheduledStartWithdrawlDate, ScheduledStartWithdrawlTime, TechnicianArrivalHubDate,
TechnicianArrivalHubTime, IceBoxesFilledDate, IceBoxesFilledTime, DepartHubDate, DepartHubTime, ArrivalDonorHospitalDate, ArrivalDonorHospitalTime,
Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete',OLD.DonorGeneralProcedureDataID, OLD.TrialID, OLD.TransplantTechnician, OLD.TransplantCoordinatorPhoneDate, OLD.TransplantCoordinatorPhoneTime,
OLD.TransplantCoordinator, OLD.TelephoneTransplantCoordinator, OLD.RetrievalHospital, OLD.ScheduledStartWithdrawlDate, OLD.ScheduledStartWithdrawlTime, OLD.TechnicianArrivalHubDate,
OLD.TechnicianArrivalHubTime, OLD.IceBoxesFilledDate, OLD.IceBoxesFilledTime, OLD.DepartHubDate, OLD.DepartHubTime, OLD.ArrivalDonorHospitalDate, OLD.ArrivalDonorHospitalTime,
OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `donor_identification`
--

DROP TABLE IF EXISTS `donor_identification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_identification` (
  `DonorIdentificationID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT '6 Digit/Characters',
  `DonorCentre` varchar(100) DEFAULT NULL COMMENT 'Donor Centre',
  `DateDonorAdmission` date DEFAULT NULL COMMENT 'Date of admission ',
  `DateDonorOperation` date DEFAULT NULL COMMENT 'Date of donor operation ',
  `DateOfBirth` date DEFAULT NULL COMMENT 'Donor Date Of Birth',
  `Sex` varchar(45) DEFAULT NULL COMMENT 'Donor sex Male/Female',
  `Weight` varchar(45) DEFAULT NULL COMMENT 'Donor weight kg',
  `Height` varchar(45) DEFAULT NULL COMMENT 'Donor Height in centimetres',
  `BMI` varchar(45) DEFAULT NULL COMMENT 'Donor BMI Derived Value',
  `BloodGroup` varchar(45) DEFAULT NULL COMMENT 'Donor blood group O/A/B/AB',
  `HLA_A` varchar(45) DEFAULT NULL COMMENT 'HLA-typing A ; Not mandatory for Transplant technician Mandatory for national CRA',
  `HLA_B` varchar(45) DEFAULT NULL COMMENT 'HLA-typing B ; Not mandatory for Transplant technician Mandatory for national CRA',
  `HLA_DR` varchar(45) DEFAULT NULL COMMENT 'HLA-typing DR ; Not mandatory for Transplant technician Mandatory for national CRA',
  `KidneyLeftDonated` varchar(45) DEFAULT NULL COMMENT 'Kidney left donated YES/NO',
  `KidneyRightDonated` varchar(45) DEFAULT NULL COMMENT ' Kidney right donated YES/NO',
  `OtherOrgansDonated` varchar(300) DEFAULT NULL COMMENT 'Bowel-YES/No; Heart-YES/No;Liver-YES/No;Lung-YES/No;Pancreas-YES/No;Tissue-YES/No',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (free text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Last Update DateTIme',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp',
  `EventCode` tinyint(4) NOT NULL DEFAULT '11',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`DonorIdentificationID`) USING BTREE,
  KEY `Index_2` (`TrialID`) USING BTREE,
  KEY `Index_3` (`DonorID`) USING BTREE,
  KEY `Index_4` (`DateOfBirth`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1 COMMENT='Donor identification';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_identification_AFTER_UPDATE` AFTER UPDATE ON `donor_identification` FOR EACH ROW
INSERT INTO  copy_donor_identification
(parentrow_action, DonorIdentificationID, TrialID, DonorID, DonorCentre, DateDonorAdmission, DateDonorOperation, DateOfBirth,
Sex, Weight, Height, BMI, BloodGroup, HLA_A, HLA_B, HLA_DR, KidneyLeftDonated, KidneyRightDonated, OtherOrgansDonated, Comments,
DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.DonorIdentificationID, OLD.TrialID, OLD.DonorID, OLD.DonorCentre, OLD.DateDonorAdmission, OLD.DateDonorOperation, OLD.DateOfBirth,
OLD.Sex, OLD.Weight, OLD.Height, OLD.BMI, OLD.BloodGroup, OLD.HLA_A, OLD.HLA_B, OLD.HLA_DR, OLD.KidneyLeftDonated, OLD.KidneyRightDonated, OLD.OtherOrgansDonated, OLD.Comments,
OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_identification_AFTER_DELETE` AFTER DELETE ON `donor_identification` FOR EACH ROW
 INSERT INTO  copy_donor_identification
(parentrow_action, DonorIdentificationID, TrialID, DonorID, DonorCentre, DateDonorAdmission, DateDonorOperation, DateOfBirth,
Sex, Weight, Height, BMI, BloodGroup, HLA_A, HLA_B, HLA_DR, KidneyLeftDonated, KidneyRightDonated, OtherOrgansDonated, Comments,
DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete',OLD.DonorIdentificationID, OLD.TrialID, OLD.DonorID, OLD.DonorCentre, OLD.DateDonorAdmission, OLD.DateDonorOperation, OLD.DateOfBirth,
OLD.Sex, OLD.Weight, OLD.Height, OLD.BMI, OLD.BloodGroup, OLD.HLA_A, OLD.HLA_B, OLD.HLA_DR, OLD.KidneyLeftDonated, OLD.KidneyRightDonated, OLD.OtherOrgansDonated, OLD.Comments,
OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `donor_labresults`
--

DROP TABLE IF EXISTS `donor_labresults`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_labresults` (
  `DonorLabResultsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Donor ID',
  `Hb` varchar(45) DEFAULT NULL COMMENT 'Hb (mmol/l OR mg/dL) ??? Units country specific',
  `HbUnit` varchar(45) DEFAULT NULL COMMENT 'Hb Unit (mmol/l or mg/dL)',
  `Ht` varchar(45) DEFAULT NULL COMMENT 'Ht (%)',
  `pH` varchar(45) DEFAULT NULL COMMENT 'pH  Not required removed Sarah Email 20140418',
  `pCO2` varchar(45) DEFAULT NULL COMMENT 'pCO2  Not required removed Sarah Email 20140418',
  `pCO2Unit` varchar(45) DEFAULT NULL,
  `pO2` varchar(45) DEFAULT NULL COMMENT 'pO2  Not required removed Sarah Email 20140418',
  `pO2Unit` varchar(45) DEFAULT NULL,
  `Urea` varchar(45) DEFAULT NULL COMMENT 'Urea (mmol/l OR mg/dL) ??? Units country specific',
  `UreaUnit` varchar(45) DEFAULT NULL COMMENT 'Urea Unit (mmol/l or mg/dL)',
  `Creatinine` varchar(45) DEFAULT NULL,
  `CreatinineUnit` varchar(45) DEFAULT NULL,
  `MeanCreatinine` varchar(45) DEFAULT NULL COMMENT 'Mean Creatinine (umol/l OR mg/dL) ??? Units country specific',
  `MeanCreatinineUnit` varchar(45) DEFAULT NULL COMMENT 'Mean Creatinine Unit (µmol/l or mg/dL)',
  `MaxCreatinine` varchar(45) DEFAULT NULL COMMENT 'Max Creatinine (umol/l OR mg/dL) ??? Units country specific',
  `MaxCreatinineUnit` varchar(45) DEFAULT NULL COMMENT 'Max Creatinine Unit (µmol/l or mg/dL)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (free text); Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the Table',
  `EventCode` tinyint(4) DEFAULT '13',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`DonorLabResultsID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1 COMMENT='Donor laboratory values';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_labresults_AFTER_UPDATE` AFTER UPDATE ON `donor_labresults` FOR EACH ROW
INSERT INTO  copy_donor_labresults
(parentrow_action, DonorLabResultsID, TrialID, DonorID, Hb, HbUnit, Ht, pH, pCO2, pCO2Unit, pO2, pO2Unit, Urea, UreaUnit, MeanCreatinine,
MeanCreatinineUnit, MaxCreatinine, MaxCreatinineUnit, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.DonorLabResultsID, OLD.TrialID, OLD.DonorID, OLD.Hb, OLD.HbUnit, OLD.Ht, OLD.pH, OLD.pCO2, OLD.pCO2Unit, OLD.pO2, OLD.pO2Unit, OLD.Urea, OLD.UreaUnit, OLD.MeanCreatinine,
OLD.MeanCreatinineUnit, OLD.MaxCreatinine, OLD.MaxCreatinineUnit, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal,OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_labresults_AFTER_DELETE` AFTER DELETE ON `donor_labresults` FOR EACH ROW
INSERT INTO  copy_donor_labresults
(parentrow_action, DonorLabResultsID, TrialID, DonorID, Hb, HbUnit, Ht, pH, pCO2, pCO2Unit, pO2, pO2Unit, Urea, UreaUnit, MeanCreatinine,
MeanCreatinineUnit, MaxCreatinine, MaxCreatinineUnit, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete',OLD.DonorLabResultsID, OLD.TrialID, OLD.DonorID, OLD.Hb, OLD.HbUnit, OLD.Ht, OLD.pH, OLD.pCO2, OLD.pCO2Unit, OLD.pO2, OLD.pO2Unit, OLD.Urea, OLD.UreaUnit, OLD.MeanCreatinine,
OLD.MeanCreatinineUnit, OLD.MaxCreatinine, OLD.MaxCreatinineUnit, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal,OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `donor_operationdata`
--

DROP TABLE IF EXISTS `donor_operationdata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_operationdata` (
  `DonorOperatationDataID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Foreign Key from donor_identification table',
  `WithdrawlLifeSupportTreatmentDate` date DEFAULT NULL COMMENT 'date of withdrawal of life supporting treatment (dd/mm/yyyy hh:mm)',
  `WithdrawlLifeSupportTreatmentTime` time DEFAULT NULL COMMENT 'Time of withdrawal of life supporting treatment (dd/mm/yyyy hh:mm)',
  `SystolicArterialPressureBelow50Date` date DEFAULT NULL COMMENT 'Date of Systolic arterial pressure below 50 mmHg/inadequate organ perfusion (dd/mm/yyyy hh:mm)',
  `SystolicArterialPressureBelow50Time` time DEFAULT NULL COMMENT 'Time of Systolic arterial pressure below 50 mmHg/inadequate organ perfusion (dd/mm/yyyy hh:mm)',
  `StartNoTouchPeriodDate` date DEFAULT NULL COMMENT 'Date of start no touch period (dd/mm/yyyy hh:mm)',
  `StartNoTouchPeriodTime` time DEFAULT NULL COMMENT 'Time of start no touch period (dd/mm/yyyy hh:mm)',
  `CirculatoryArrestDate` date DEFAULT NULL COMMENT 'Date of circulatory arrest (dd/mm/yyyy hh:mm)',
  `CirculatoryArrestTime` time DEFAULT NULL COMMENT 'Time of circulatory arrest (dd/mm/yyyy hh:mm)',
  `LengthNoTouchPeriod` int(10) unsigned DEFAULT NULL COMMENT 'Length of no touch period (minutes)',
  `ConfirmationDeathDate` date DEFAULT NULL COMMENT 'Date of confirmation of death: (dd/mm/yyyy hh:mm)',
  `ConfirmationDeathTime` time DEFAULT NULL COMMENT 'Time of confirmation of death: (dd/mm/yyyy hh:mm)',
  `StartInSituColdPerfusionDate` date DEFAULT NULL COMMENT 'Date of start in-situ cold perfusion (dd/mm/yyyy hh:mm)',
  `StartInSituColdPerfusionTime` time DEFAULT NULL COMMENT 'Time of start in-situ cold perfusion (dd/mm/yyyy hh:mm)',
  `SystemicFlushSolutionUsed` varchar(45) DEFAULT NULL COMMENT 'Systemic flush solution used (UW/HTK/Marshall/Other)',
  `SystemicFlushSolutionUsedOther` varchar(100) DEFAULT NULL COMMENT 'If Systemic flush solution used is Other)',
  `PreservationSolutionColdPerfusion` varchar(45) DEFAULT NULL COMMENT 'Preservation solution used for cold perfusion(UW/HTK/Marshall/Other)',
  `PreservationSolutionColdPerfusionOther` varchar(100) DEFAULT NULL COMMENT 'If Other Preservation solution used for cold perfusion(UW/HTK/Marshall/Other)',
  `VolumeSolutionColdPerfusion` int(10) unsigned DEFAULT NULL COMMENT 'Volume of solution used for cold perfusion (ml)',
  `Heparin` varchar(45) DEFAULT NULL COMMENT 'Heparin (YES/NO)',
  `TotalWarmIschaemicPeriod` varchar(45) DEFAULT NULL COMMENT 'total warm ischaemic period WithdrawlLifeSupportTreatment-StartInSituColdPerfusion in Minutes',
  `WithdrawalPeriod` varchar(45) DEFAULT NULL COMMENT 'withdrawal period WithdrawlLifeSupportTreatment-CirculatoryArrest in Minutes',
  `FunctionalWarmIschaemicPeriod` varchar(45) DEFAULT NULL COMMENT 'functional warm ischaemic period MeanArterialPressureBelow50-StartInSituColdPerfusion in Minutes',
  `AsystolicWarmIschaemicPeriod` varchar(45) DEFAULT NULL COMMENT 'asystolic warm ischaemic period CirculatoryArrest-StartInSituColdPerfusion',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments (Free Text); Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '14',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`DonorOperatationDataID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1 COMMENT='Donor operation data';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_operationdata_AFTER_UPDATE` AFTER UPDATE ON `donor_operationdata` FOR EACH ROW
 INSERT INTO  copy_donor_operationdata
(parentrow_action, DonorOperatationDataID, TrialID, DonorID, WithdrawlLifeSupportTreatmentDate, WithdrawlLifeSupportTreatmentTime,
SystolicArterialPressureBelow50Date, SystolicArterialPressureBelow50Time, StartNoTouchPeriodDate, StartNoTouchPeriodTime,
CirculatoryArrestDate, CirculatoryArrestTime, LengthNoTouchPeriod, ConfirmationDeathDate, ConfirmationDeathTime,
StartInSituColdPerfusionDate, StartInSituColdPerfusionTime, SystemicFlushSolutionUsed, SystemicFlushSolutionUsedOther,
PreservationSolutionColdPerfusion, PreservationSolutionColdPerfusionOther, VolumeSolutionColdPerfusion, Heparin,
TotalWarmIschaemicPeriod, WithdrawalPeriod, FunctionalWarmIschaemicPeriod, AsystolicWarmIschaemicPeriod, Comments, 
DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.DonorOperatationDataID, OLD.TrialID, OLD.DonorID, OLD.WithdrawlLifeSupportTreatmentDate, OLD.WithdrawlLifeSupportTreatmentTime,
OLD.SystolicArterialPressureBelow50Date, OLD.SystolicArterialPressureBelow50Time, OLD.StartNoTouchPeriodDate, OLD.StartNoTouchPeriodTime,
OLD.CirculatoryArrestDate, OLD.CirculatoryArrestTime, OLD.LengthNoTouchPeriod, OLD.ConfirmationDeathDate, OLD.ConfirmationDeathTime,
OLD.StartInSituColdPerfusionDate, OLD.StartInSituColdPerfusionTime, OLD.SystemicFlushSolutionUsed, OLD.SystemicFlushSolutionUsedOther,
OLD.PreservationSolutionColdPerfusion, OLD.PreservationSolutionColdPerfusionOther, OLD.VolumeSolutionColdPerfusion, OLD.Heparin,
OLD.TotalWarmIschaemicPeriod, OLD.WithdrawalPeriod, OLD.FunctionalWarmIschaemicPeriod, OLD.AsystolicWarmIschaemicPeriod, OLD.Comments,
OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_operationdata_AFTER_DELETE` AFTER DELETE ON `donor_operationdata` FOR EACH ROW
   INSERT INTO  copy_donor_operationdata
(parentrow_action, DonorOperatationDataID, TrialID, DonorID, WithdrawlLifeSupportTreatmentDate, WithdrawlLifeSupportTreatmentTime,
SystolicArterialPressureBelow50Date, SystolicArterialPressureBelow50Time, StartNoTouchPeriodDate, StartNoTouchPeriodTime,
CirculatoryArrestDate, CirculatoryArrestTime, LengthNoTouchPeriod, ConfirmationDeathDate, ConfirmationDeathTime,
StartInSituColdPerfusionDate, StartInSituColdPerfusionTime, SystemicFlushSolutionUsed, SystemicFlushSolutionUsedOther,
PreservationSolutionColdPerfusion, PreservationSolutionColdPerfusionOther, VolumeSolutionColdPerfusion, Heparin,
TotalWarmIschaemicPeriod, WithdrawalPeriod, FunctionalWarmIschaemicPeriod, AsystolicWarmIschaemicPeriod, Comments, 
DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete',OLD.DonorOperatationDataID, OLD.TrialID, OLD.DonorID, OLD.WithdrawlLifeSupportTreatmentDate, OLD.WithdrawlLifeSupportTreatmentTime,
OLD.SystolicArterialPressureBelow50Date, OLD.SystolicArterialPressureBelow50Time, OLD.StartNoTouchPeriodDate, OLD.StartNoTouchPeriodTime,
OLD.CirculatoryArrestDate, OLD.CirculatoryArrestTime, OLD.LengthNoTouchPeriod, OLD.ConfirmationDeathDate, OLD.ConfirmationDeathTime,
OLD.StartInSituColdPerfusionDate, OLD.StartInSituColdPerfusionTime, OLD.SystemicFlushSolutionUsed, OLD.SystemicFlushSolutionUsedOther,
OLD.PreservationSolutionColdPerfusion, OLD.PreservationSolutionColdPerfusionOther, OLD.VolumeSolutionColdPerfusion, OLD.Heparin,
OLD.TotalWarmIschaemicPeriod, OLD.WithdrawalPeriod, OLD.FunctionalWarmIschaemicPeriod, OLD.AsystolicWarmIschaemicPeriod, OLD.Comments,
OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `donor_preop_clinicaldata`
--

DROP TABLE IF EXISTS `donor_preop_clinicaldata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_preop_clinicaldata` (
  `DonorPreOpClinicalDataID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Foreign Key from Donor Identification',
  `Diagnosis` varchar(300) DEFAULT NULL COMMENT 'Diagnosis multi trauma if YES/NO',
  `DiagnosisOtherDetails` varchar(100) DEFAULT NULL COMMENT 'If Diagnosis Other',
  `DiabetesMellitus` varchar(45) DEFAULT NULL COMMENT 'Diabetes Mellitus (IDDM) YES/NO',
  `AlcoholAbuse` varchar(45) DEFAULT NULL COMMENT 'If Alcohol Abuse YES/NO ',
  `CardiacArrest` varchar(45) DEFAULT NULL COMMENT 'Cardiac arrest (during ICU stay prior to retrieval procedure)  YES/NO',
  `SystolicBloodPressure` varchar(45) DEFAULT NULL COMMENT 'Systolic blood pressure (Before Switch Off) mm/Hg',
  `DiastolicBloodPressure` varchar(45) DEFAULT NULL COMMENT 'Systolic blood pressure (Before Switch Off) mm/Hg',
  `HypotensivePeriod` varchar(45) DEFAULT NULL COMMENT 'Hypotensive period (syst. < 100 mmHg) YES/NO',
  `Diuresis` varchar(45) DEFAULT NULL COMMENT 'Mean diuresis / hr. last 24 hrs',
  `DonorAnuriaOliguria` varchar(45) DEFAULT NULL COMMENT 'Donor anuria / oliguria (< 500 ml/24h) YES/NO',
  `DurationAnuriaOliguria` varchar(45) DEFAULT NULL COMMENT 'duration an-/oliguria in hours',
  `Dopamine` varchar(45) DEFAULT NULL COMMENT 'Dopamine YES/NO',
  `DopamineLastDose` varchar(45) DEFAULT NULL COMMENT 'Dopamine Last Does (mg); Not Mandatory',
  `Dobutamine` varchar(45) DEFAULT NULL COMMENT 'Dobutamine YES/NO',
  `DobutamineLastDose` varchar(45) DEFAULT NULL COMMENT 'Dobutamine Last Dose (mg); Not Mandatory',
  `NorAdrenaline` varchar(45) DEFAULT NULL COMMENT '(Nor)adrenaline YES/NO',
  `NorAdrenalineLastDose` varchar(45) DEFAULT NULL COMMENT 'NorAdrenaline Last Dose (mg); Not Mandatory',
  `OtherMedication` varchar(100) DEFAULT NULL COMMENT 'Other medication Details; Not Mandatory',
  `OtherMedicationLastDose` varchar(45) DEFAULT NULL COMMENT 'Other Medication Last Dose (mg); Not Mandatory',
  `OtherMedication2` varchar(100) DEFAULT NULL COMMENT 'Other Medication (Second)',
  `OtherMedication2LastDose` varchar(45) DEFAULT NULL COMMENT 'Other Medication (Second) last Dose',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (Free Text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '12',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`DonorPreOpClinicalDataID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1 COMMENT='Donor pre-operative clinical data';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_preop_clinicaldata_AFTER_UPDATE` AFTER UPDATE ON `donor_preop_clinicaldata` FOR EACH ROW
INSERT INTO  copy_donor_preop_clinicaldata
(parentrow_action, DonorPreOpClinicalDataID, TrialID, DonorID, Diagnosis, DiagnosisOtherDetails, DiabetesMellitus, AlcoholAbuse,
CardiacArrest, SystolicBloodPressure, DiastolicBloodPressure, HypotensivePeriod, Diuresis, DonorAnuriaOliguria, DurationAnuriaOliguria,
Dopamine, DopamineLastDose, Dobutamine, DobutamineLastDose, NorAdrenaline, NorAdrenalineLastDose, OtherMedication, OtherMedicationLastDose,
OtherMedication2, OtherMedication2LastDose, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.DonorPreOpClinicalDataID, OLD.TrialID, OLD.DonorID, OLD.Diagnosis, OLD.DiagnosisOtherDetails, OLD.DiabetesMellitus, OLD.AlcoholAbuse,
OLD.CardiacArrest, OLD.SystolicBloodPressure, OLD.DiastolicBloodPressure, OLD.HypotensivePeriod, OLD.Diuresis, OLD.DonorAnuriaOliguria, OLD.DurationAnuriaOliguria,
OLD.Dopamine, OLD.DopamineLastDose, OLD.Dobutamine, OLD.DobutamineLastDose, OLD.NorAdrenaline, OLD.NorAdrenalineLastDose, OLD.OtherMedication, OLD.OtherMedicationLastDose,
OLD.OtherMedication2, OLD.OtherMedication2LastDose, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`donor_preop_clinicaldata_AFTER_DELETE` AFTER DELETE ON `donor_preop_clinicaldata` FOR EACH ROW
    INSERT INTO  copy_donor_preop_clinicaldata
(parentrow_action, DonorPreOpClinicalDataID, TrialID, DonorID, Diagnosis, DiagnosisOtherDetails, DiabetesMellitus, AlcoholAbuse,
CardiacArrest, SystolicBloodPressure, DiastolicBloodPressure, HypotensivePeriod, Diuresis, DonorAnuriaOliguria, DurationAnuriaOliguria,
Dopamine, DopamineLastDose, Dobutamine, DobutamineLastDose, NorAdrenaline, NorAdrenalineLastDose, OtherMedication, OtherMedicationLastDose,
OtherMedication2, OtherMedication2LastDose, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete',OLD.DonorPreOpClinicalDataID, OLD.TrialID, OLD.DonorID, OLD.Diagnosis, OLD.DiagnosisOtherDetails, OLD.DiabetesMellitus, OLD.AlcoholAbuse,
OLD.CardiacArrest, OLD.SystolicBloodPressure, OLD.DiastolicBloodPressure, OLD.HypotensivePeriod, OLD.Diuresis, OLD.DonorAnuriaOliguria, OLD.DurationAnuriaOliguria,
OLD.Dopamine, OLD.DopamineLastDose, OLD.Dobutamine, OLD.DobutamineLastDose, OLD.NorAdrenaline, OLD.NorAdrenalineLastDose, OLD.OtherMedication, OLD.OtherMedicationLastDose,
OLD.OtherMedication2, OLD.OtherMedication2LastDose, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `eventcodes`
--

DROP TABLE IF EXISTS `eventcodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `eventcodes` (
  `EventRowIndex` int(11) NOT NULL AUTO_INCREMENT,
  `EventCode` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'Event Code',
  `OrderSequence` tinyint(4) DEFAULT NULL COMMENT 'Ordering sequence',
  `EventName` varchar(45) DEFAULT NULL COMMENT 'Event Name',
  `PageLink` varchar(100) DEFAULT NULL COMMENT 'Name of Edit page',
  `PageIdentifier` varchar(100) DEFAULT NULL COMMENT 'Name of the Unique Identifier for the Event',
  `DateLink` varchar(45) DEFAULT NULL COMMENT 'Date For the Event',
  `Comments` varchar(100) DEFAULT NULL COMMENT 'Comments',
  PRIMARY KEY (`EventRowIndex`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `generallist`
--

DROP TABLE IF EXISTS `generallist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `generallist` (
  `GeneralListID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TableName` varchar(100) DEFAULT NULL,
  `Description` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`GeneralListID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1 COMMENT='General List';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `kidneyinspection`
--

DROP TABLE IF EXISTS `kidneyinspection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `kidneyinspection` (
  `KidneyInspectionID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL,
  `Side` varchar(45) DEFAULT NULL COMMENT 'LEFT/RIGHT',
  `PreservationModality` varchar(45) DEFAULT NULL COMMENT 'Preservation modality (HMP with oxygen/ HMP without oxygen )',
  `RandomisationComplete` varchar(45) DEFAULT NULL COMMENT 'YES/NO valve left open or closed and gauge concealed',
  `NumberRenalArteries` varchar(45) DEFAULT NULL COMMENT 'Number of renal arteries ',
  `ArterialProblems` varchar(300) DEFAULT NULL COMMENT 'RenalGraftDamage Left Kidney',
  `KidneyTransplantable` varchar(45) DEFAULT NULL COMMENT 'Kidney Transplantable Left',
  `ReasonNotTransplantable` varchar(500) DEFAULT NULL COMMENT 'Reason Not Transplantable',
  `WashoutPerfusion` varchar(45) DEFAULT NULL COMMENT 'Washout Perfusion (Homogenous/Patchy/Blue)',
  `RemovalDate` date DEFAULT NULL COMMENT 'Removal Time (hh:mm)',
  `RemovalTime` time DEFAULT NULL COMMENT 'Removal Time (hh:mm)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (free text)',
  `PreservationModality_R` varchar(45) DEFAULT NULL COMMENT 'Preservation modality (HMP with oxygen/ HMP without oxygen ) Right Kidney',
  `RandomisationComplete_R` varchar(45) DEFAULT NULL COMMENT 'YES/NO valve left open or closed and gauge concealed Right Kidney',
  `NumberRenalArteries_R` varchar(45) DEFAULT NULL COMMENT 'Number of renal arteries  Right Kidney',
  `ArterialProblems_R` varchar(300) DEFAULT NULL COMMENT 'Arterial damage (YES/NO); Venous damage (YES/NO); Ureteral damage (YES/NO); Parenchymal damage (YES/NO) Right Kidney',
  `KidneyTransplantable_R` varchar(45) DEFAULT NULL COMMENT 'Kidney Transplantable Right',
  `ReasonNotTransplantable_R` varchar(500) DEFAULT NULL COMMENT 'Reason Not Transplantable',
  `WashoutPerfusion_R` varchar(45) DEFAULT NULL COMMENT 'Washout Perfusion (Homogenous/Patchy/Blue) Right Kidney',
  `Removal_RDate` date DEFAULT NULL COMMENT 'Removal Time (hh:mm) Right Kidney',
  `Removal_RTime` time DEFAULT NULL COMMENT 'Removal Time (hh:mm) Right Kidne',
  `Comments_R` varchar(500) DEFAULT NULL COMMENT 'General Comments Right Kidney (free text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '27',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`KidneyInspectionID`) USING BTREE,
  KEY `Index_3` (`DonorID`),
  KEY `Index_2` (`TrialID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1 COMMENT='Donor operation kidney data';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`kidneyinspection_AFTER_UPDATE` AFTER UPDATE ON `kidneyinspection` FOR EACH ROW
    INSERT INTO  copy_kidneyinspection
(parentrow_action, KidneyInspectionID, TrialID, DonorID, Side, PreservationModality, RandomisationComplete, NumberRenalArteries,
ArterialProblems, WashoutPerfusion, RemovalDate, RemovalTime, Comments, PreservationModality_R, RandomisationComplete_R, NumberRenalArteries_R,
ArterialProblems_R, WashoutPerfusion_R, Removal_RDate, Removal_RTime, Comments_R, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.KidneyInspectionID, OLD.TrialID, OLD.DonorID, OLD.Side, OLD.PreservationModality, OLD.RandomisationComplete, OLD.NumberRenalArteries,
OLD.ArterialProblems, OLD.WashoutPerfusion, OLD.RemovalDate, OLD.RemovalTime, OLD.Comments, OLD.PreservationModality_R, OLD.RandomisationComplete_R,
OLD.NumberRenalArteries_R, OLD.ArterialProblems_R, OLD.WashoutPerfusion_R, OLD.Removal_RDate, OLD.Removal_RTime, OLD.Comments_R, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`kidneyinspection_AFTER_DELETE` AFTER DELETE ON `kidneyinspection` FOR EACH ROW
    INSERT INTO  copy_kidneyinspection
(parentrow_action, KidneyInspectionID, TrialID, DonorID, Side, PreservationModality, RandomisationComplete, NumberRenalArteries,
ArterialProblems, WashoutPerfusion, RemovalDate, RemovalTime, Comments, PreservationModality_R, RandomisationComplete_R, NumberRenalArteries_R,
ArterialProblems_R, WashoutPerfusion_R, Removal_RDate, Removal_RTime, Comments_R, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete',OLD.KidneyInspectionID, OLD.TrialID, OLD.DonorID, OLD.Side, OLD.PreservationModality, OLD.RandomisationComplete, OLD.NumberRenalArteries,
OLD.ArterialProblems, OLD.WashoutPerfusion, OLD.RemovalDate, OLD.RemovalTime, OLD.Comments, OLD.PreservationModality_R, OLD.RandomisationComplete_R,
OLD.NumberRenalArteries_R, OLD.ArterialProblems_R, OLD.WashoutPerfusion_R, OLD.Removal_RDate, OLD.Removal_RTime, OLD.Comments_R, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `kidneyproceduredata`
--

DROP TABLE IF EXISTS `kidneyproceduredata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `kidneyproceduredata` (
  `KidneyProcedureDataID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(45) NOT NULL COMMENT 'TrialID',
  `Side` varchar(45) NOT NULL COMMENT 'Side of Kindey (Left/Right)',
  `TransplantTechnician` varchar(100) DEFAULT NULL COMMENT 'Name of transplant technician',
  `DonorTechnicianPhoneDate` date DEFAULT NULL COMMENT 'Date phone call received from colleague technician involved in donor procedure ',
  `DonorTechnicianPhoneTime` time DEFAULT NULL COMMENT 'Time phone call received from colleague technician involved in donor procedure ',
  `TransplantHospital` varchar(100) DEFAULT NULL COMMENT 'Transplant hospital',
  `TechnicianDonorProcedure` varchar(100) DEFAULT NULL COMMENT 'Name of colleague technician involved in donor procedure',
  `TransplantHospitalContact` varchar(100) DEFAULT NULL COMMENT 'Name of recipient hospital’s operating theatre contact person/supervisor ',
  `TransplantHospitalContactPhone` varchar(100) DEFAULT NULL COMMENT 'Telephone number of recipient hospital’s operating theatre contact person/supervisor ',
  `ScheduledTransplantStartDate` date DEFAULT NULL COMMENT 'Transplant Start Date',
  `ScheduledTransplantStartTime` time DEFAULT NULL COMMENT 'Transplant Start Time',
  `TechnicianArrivalPerfusionCentreDate` date DEFAULT NULL COMMENT 'Arrival date of technician at perfusion centre ',
  `TechnicianArrivalPerfusionCentreTime` time DEFAULT NULL COMMENT 'Arrival time of technician at perfusion centre ',
  `TechnicianDeparturePerfusionCentreDate` date DEFAULT NULL COMMENT 'Date of departure from perfusion centre ',
  `TechnicianDeparturePerfusionCentreTime` time DEFAULT NULL COMMENT 'Time of departure from perfusion centre ',
  `ArrivalTransplantHospitalDate` date DEFAULT NULL COMMENT 'Date of arrival at transplant hospital ',
  `ArrivalTransplantHospitalTime` time DEFAULT NULL COMMENT 'Time of arrival at transplant hospital ',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Remarks',
  `Reallocated` varchar(100) DEFAULT NULL COMMENT 'Kidney re-allocated to another transplant centre YES/NO',
  `ReasonReallocated` varchar(100) DEFAULT NULL COMMENT 'Reason for re-allocation if positive crossmatch',
  `ReasonReallocatedOther` varchar(100) DEFAULT NULL COMMENT 'Reason for re-allocation if Other',
  `NewRecipientHospitalContact` varchar(100) DEFAULT NULL COMMENT 'Name of new recipient hospital’s operating theatre contact person/supervisor ',
  `NewRecipientHospitalContactPhone` varchar(100) DEFAULT NULL COMMENT 'Telephone number of new recipient hospital’s operating theatre contact person/supervisor ',
  `NewTransplantHospital` varchar(100) DEFAULT NULL COMMENT 'New transplant hospital',
  `NewScheduledTransplantStartDate` date DEFAULT NULL COMMENT 'New scheduled date for start transplant procedure ',
  `NewScheduledTransplantStartTime` time DEFAULT NULL COMMENT 'New scheduled time for start transplant procedure ',
  `DepartureFirstTransplantHospitalDate` date DEFAULT NULL COMMENT 'Date of departure from first transplant hospital ',
  `DepartureFirstTransplantHospitalTime` time DEFAULT NULL COMMENT 'Time of departure from first transplant hospital ',
  `ArrivalNewTransplantHospitalDate` date DEFAULT NULL COMMENT 'Date of arrival at new transplant hospital ',
  `ArrivalNewTransplantHospitalTime` time DEFAULT NULL COMMENT 'Time of arrival at new transplant hospital ',
  `NewComments` varchar(500) DEFAULT NULL COMMENT 'Additional Comments',
  `TechnicianDepartureTransplantDate` date DEFAULT NULL COMMENT 'Date of technician’s departure from transplant hospital',
  `TechnicianDepartureTransplantTime` time DEFAULT NULL COMMENT 'Time of technician’s departure from transplant hospital',
  `FinalArrivalPerfusionCentreDate` date DEFAULT NULL COMMENT 'Date of technician’s arrival at perfusion centre',
  `FinalArrivalPerfusionCentreTime` time DEFAULT NULL COMMENT 'Time of technician’s arrival at perfusion centre ',
  `TechnicianEndProcessDate` date DEFAULT NULL COMMENT 'End time of entire procedure for this technician',
  `TechnicianEndProcessTime` time DEFAULT NULL COMMENT 'End time of entire procedure for this technician',
  `TechnicianDepartureComments` varchar(500) DEFAULT NULL COMMENT 'Technician Departure Remarks',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `EventCode` tinyint(3) DEFAULT '30' COMMENT 'Machine Perfusion Dara default event code is 20',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`KidneyProcedureDataID`),
  KEY `Index_2` (`TrialID`) USING BTREE,
  KEY `Index_3` (`Side`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1 COMMENT='Recipient General Procedure Data';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`kidneyproceduredata_AFTER_UPDATE` AFTER UPDATE ON `kidneyproceduredata` FOR EACH ROW
 INSERT INTO  copy_kidneyproceduredata
(parentrow_action, KidneyProcedureDataID, TrialID, Side, TransplantTechnician, DonorTechnicianPhoneDate, DonorTechnicianPhoneTime, TransplantHospital,
TechnicianDonorProcedure, TransplantHospitalContact, TransplantHospitalContactPhone, ScheduledTransplantStartDate, ScheduledTransplantStartTime,
TechnicianArrivalPerfusionCentreDate, TechnicianArrivalPerfusionCentreTime, TechnicianDeparturePerfusionCentreDate, TechnicianDeparturePerfusionCentreTime,
ArrivalTransplantHospitalDate, ArrivalTransplantHospitalTime, Comments, Reallocated, ReasonReallocated, ReasonReallocatedOther, NewRecipientHospitalContact,
NewRecipientHospitalContactPhone, NewTransplantHospital, NewScheduledTransplantStartDate, NewScheduledTransplantStartTime, DepartureFirstTransplantHospitalDate,
DepartureFirstTransplantHospitalTime, ArrivalNewTransplantHospitalDate, ArrivalNewTransplantHospitalTime, NewComments, TechnicianDepartureTransplantDate,
TechnicianDepartureTransplantTime, FinalArrivalPerfusionCentreDate, FinalArrivalPerfusionCentreTime, TechnicianEndProcessDate, TechnicianEndProcessTime,
TechnicianDepartureComments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.KidneyProcedureDataID, OLD.TrialID, OLD.Side, OLD.TransplantTechnician, OLD.DonorTechnicianPhoneDate, OLD.DonorTechnicianPhoneTime, OLD.TransplantHospital,
OLD.TechnicianDonorProcedure, OLD.TransplantHospitalContact, OLD.TransplantHospitalContactPhone, OLD.ScheduledTransplantStartDate, OLD.ScheduledTransplantStartTime,
OLD.TechnicianArrivalPerfusionCentreDate, OLD.TechnicianArrivalPerfusionCentreTime, OLD.TechnicianDeparturePerfusionCentreDate, OLD.TechnicianDeparturePerfusionCentreTime,
OLD.ArrivalTransplantHospitalDate, OLD.ArrivalTransplantHospitalTime, OLD.Comments, OLD.Reallocated, OLD.ReasonReallocated, OLD.ReasonReallocatedOther,
OLD.NewRecipientHospitalContact, OLD.NewRecipientHospitalContactPhone, OLD.NewTransplantHospital, OLD.NewScheduledTransplantStartDate,
OLD.NewScheduledTransplantStartTime, OLD.DepartureFirstTransplantHospitalDate, OLD.DepartureFirstTransplantHospitalTime, OLD.ArrivalNewTransplantHospitalDate,
OLD.ArrivalNewTransplantHospitalTime, OLD.NewComments, OLD.TechnicianDepartureTransplantDate, OLD.TechnicianDepartureTransplantTime, OLD.FinalArrivalPerfusionCentreDate,
 OLD.FinalArrivalPerfusionCentreTime, OLD.TechnicianEndProcessDate, OLD.TechnicianEndProcessTime, OLD.TechnicianDepartureComments,
 OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`kidneyproceduredata_AFTER_DELETE` AFTER DELETE ON `kidneyproceduredata` FOR EACH ROW
    INSERT INTO  copy_kidneyproceduredata
(parentrow_action, KidneyProcedureDataID, TrialID, Side, TransplantTechnician, DonorTechnicianPhoneDate, DonorTechnicianPhoneTime, TransplantHospital,
TechnicianDonorProcedure, TransplantHospitalContact, TransplantHospitalContactPhone, ScheduledTransplantStartDate, ScheduledTransplantStartTime,
TechnicianArrivalPerfusionCentreDate, TechnicianArrivalPerfusionCentreTime, TechnicianDeparturePerfusionCentreDate, TechnicianDeparturePerfusionCentreTime,
ArrivalTransplantHospitalDate, ArrivalTransplantHospitalTime, Comments, Reallocated, ReasonReallocated, ReasonReallocatedOther, NewRecipientHospitalContact,
NewRecipientHospitalContactPhone, NewTransplantHospital, NewScheduledTransplantStartDate, NewScheduledTransplantStartTime, DepartureFirstTransplantHospitalDate,
DepartureFirstTransplantHospitalTime, ArrivalNewTransplantHospitalDate, ArrivalNewTransplantHospitalTime, NewComments, TechnicianDepartureTransplantDate,
TechnicianDepartureTransplantTime, FinalArrivalPerfusionCentreDate, FinalArrivalPerfusionCentreTime, TechnicianEndProcessDate, TechnicianEndProcessTime,
TechnicianDepartureComments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update',OLD.KidneyProcedureDataID, OLD.TrialID, OLD.Side, OLD.TransplantTechnician, OLD.DonorTechnicianPhoneDate, OLD.DonorTechnicianPhoneTime, OLD.TransplantHospital,
OLD.TechnicianDonorProcedure, OLD.TransplantHospitalContact, OLD.TransplantHospitalContactPhone, OLD.ScheduledTransplantStartDate, OLD.ScheduledTransplantStartTime,
OLD.TechnicianArrivalPerfusionCentreDate, OLD.TechnicianArrivalPerfusionCentreTime, OLD.TechnicianDeparturePerfusionCentreDate, OLD.TechnicianDeparturePerfusionCentreTime,
OLD.ArrivalTransplantHospitalDate, OLD.ArrivalTransplantHospitalTime, OLD.Comments, OLD.Reallocated, OLD.ReasonReallocated, OLD.ReasonReallocatedOther,
OLD.NewRecipientHospitalContact, OLD.NewRecipientHospitalContactPhone, OLD.NewTransplantHospital, OLD.NewScheduledTransplantStartDate,
OLD.NewScheduledTransplantStartTime, OLD.DepartureFirstTransplantHospitalDate, OLD.DepartureFirstTransplantHospitalTime, OLD.ArrivalNewTransplantHospitalDate,
OLD.ArrivalNewTransplantHospitalTime, OLD.NewComments, OLD.TechnicianDepartureTransplantDate, OLD.TechnicianDepartureTransplantTime, OLD.FinalArrivalPerfusionCentreDate,
 OLD.FinalArrivalPerfusionCentreTime, OLD.TechnicianEndProcessDate, OLD.TechnicianEndProcessTime, OLD.TechnicianDepartureComments,
 OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `kidneyr`
--

DROP TABLE IF EXISTS `kidneyr`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `kidneyr` (
  `KidneyRID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique TrialID',
  `DonorID` varchar(45) DEFAULT NULL,
  `LeftKidneyDonate` varchar(45) DEFAULT NULL COMMENT 'Left Kidney Donated',
  `RightKidneyDonate` varchar(45) DEFAULT NULL COMMENT 'Right Kidney Donates',
  `InclusionCriteriaChecked` varchar(45) DEFAULT NULL COMMENT 'Inclusion Criteria Checked (YES/NO)',
  `ExclusionCriteriaChecked` varchar(45) DEFAULT NULL COMMENT 'Exclusion Criteria Checked (YES/NO)',
  `ConsentChecked` varchar(45) DEFAULT NULL COMMENT 'Consent Checked (YES/NO)',
  `LeftRanCategory` varchar(45) DEFAULT NULL COMMENT 'Left Kidney Randomised To',
  `LeftRandomisationArm` varchar(100) DEFAULT NULL,
  `RightRanCategory` varchar(45) DEFAULT NULL COMMENT 'Right Kidney Randomised To',
  `RightRandomisationArm` varchar(100) DEFAULT NULL,
  `WPFour_RandomID` int(10) unsigned DEFAULT NULL COMMENT 'Unique Identifier from Randomisation Table',
  `Comments` varchar(500) DEFAULT NULL,
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Time stamp',
  `EventCode` tinyint(3) unsigned NOT NULL DEFAULT '25' COMMENT 'Default Event Code for the Table',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`KidneyRID`) USING BTREE,
  KEY `Index_2` (`TrialID`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`kidneyr_AFTER_UPDATE` AFTER UPDATE ON `kidneyr` FOR EACH ROW
  INSERT INTO  copy_kidneyr
(parentrow_action, KidneyRID, TrialID, DonorID, LeftKidneyDonate, RightKidneyDonate, InclusionCriteriaChecked,
ExclusionCriteriaChecked, ConsentChecked, LeftRanCategory, LeftRandomisationArm, RightRanCategory, RightRandomisationArm,
WPFour_RandomID, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update', OLD.KidneyRID, OLD.TrialID, OLD.DonorID, OLD.LeftKidneyDonate, OLD.RightKidneyDonate, OLD.InclusionCriteriaChecked,
OLD.ExclusionCriteriaChecked, OLD.ConsentChecked, OLD.LeftRanCategory, OLD.LeftRandomisationArm, OLD.RightRanCategory, OLD.RightRandomisationArm,
OLD.WPFour_RandomID, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`kidneyr_AFTER_DELETE` AFTER DELETE ON `kidneyr` FOR EACH ROW
    INSERT INTO  copy_kidneyr
(parentrow_action, KidneyRID, TrialID, DonorID, LeftKidneyDonate, RightKidneyDonate, InclusionCriteriaChecked,
ExclusionCriteriaChecked, ConsentChecked, LeftRanCategory, LeftRandomisationArm, RightRanCategory, RightRandomisationArm,
WPFour_RandomID, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete', OLD.KidneyRID, OLD.TrialID, OLD.DonorID, OLD.LeftKidneyDonate, OLD.RightKidneyDonate, OLD.InclusionCriteriaChecked,
OLD.ExclusionCriteriaChecked, OLD.ConsentChecked, OLD.LeftRanCategory, OLD.LeftRandomisationArm, OLD.RightRanCategory, OLD.RightRandomisationArm,
OLD.WPFour_RandomID, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `lstcentres`
--

DROP TABLE IF EXISTS `lstcentres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `lstcentres` (
  `CentresID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `CountryCode` tinyint(3) unsigned NOT NULL,
  `CentreCode` tinyint(3) unsigned NOT NULL COMMENT 'Two digit centre code, First digit denotes country',
  `CentreName` varchar(100) NOT NULL COMMENT 'Name of Centre',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`CentresID`),
  KEY `Index_2` (`CountryCode`) USING BTREE,
  KEY `Index_3` (`CountryCode`,`CentreCode`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=latin1 COMMENT='List of centres';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `lstcountries`
--

DROP TABLE IF EXISTS `lstcountries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `lstcountries` (
  `CountryID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `CountryCode` tinyint(3) unsigned NOT NULL,
  `Country` varchar(100) DEFAULT NULL,
  `DateCreated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`CountryID`),
  KEY `Index_2` (`CountryCode`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `machineperfusion`
--

DROP TABLE IF EXISTS `machineperfusion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `machineperfusion` (
  `MachinePerfusionID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Identifier for the table',
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Foreign Key from donor_identification table',
  `Side` varchar(45) DEFAULT NULL COMMENT 'Side of Kidney (Left/Right)',
  `KidneyOnMachine` varchar(45) DEFAULT NULL COMMENT 'Is Machine Perfusion Possible (YES/NO)',
  `KidneyOnMachineNo` varchar(500) DEFAULT NULL COMMENT 'If kidney can be placed on machine NO, details (Free Text) ',
  `PerfusionStartDate` date DEFAULT NULL COMMENT 'Date Perfusion Started (dd/mm/yyyy)',
  `PerfusionStartTime` time DEFAULT NULL COMMENT 'Time Perfusion Started (hh:mm)',
  `MachineSerialNumber` varchar(45) DEFAULT NULL COMMENT 'Machine Seria lNumber',
  `MachineReferenceModelNumber` varchar(45) DEFAULT NULL COMMENT 'Reference Mode lNumber for the machine',
  `LotNumberPerfusionSolution` varchar(100) DEFAULT NULL,
  `LotNumberDisposables` varchar(100) DEFAULT NULL COMMENT 'Lot Number of Disposables',
  `Cannulae` varchar(45) DEFAULT NULL COMMENT 'Type of Cannulae Used (Sealring/Straight Cannula)',
  `CannulaeNumber` varchar(45) DEFAULT NULL COMMENT 'Cannulae Number Used 1/2/3',
  `UsedPatchHolder` varchar(45) DEFAULT NULL COMMENT 'Small/ Large/ Double Artery',
  `ArtificialPatchUsed` varchar(45) DEFAULT NULL COMMENT 'YES/NO',
  `ArtificialPatchSize` varchar(45) DEFAULT NULL COMMENT 'Small/ Large',
  `ArtificialPatchNumber` varchar(45) DEFAULT NULL COMMENT '1/2',
  `OxygenBottleFull` varchar(45) DEFAULT NULL,
  `OxygenBottleOpened` varchar(45) DEFAULT NULL,
  `OxygenTankChanged` varchar(45) DEFAULT NULL COMMENT 'Oxygen tank changed',
  `OxygenTankChangedDate` date DEFAULT NULL,
  `OxygenTankChangedTime` time DEFAULT NULL,
  `IceContainerReplenished` varchar(45) DEFAULT NULL COMMENT 'Oxygen tank changed',
  `IceContainerReplenishedDate` date DEFAULT NULL,
  `IceContainerReplenishedTime` time DEFAULT NULL,
  `LogisticallyPossibleMeasurepO2Perfusate` varchar(45) DEFAULT NULL COMMENT 'Logistically possible to measure pO2 of perfusate (YES/NO)',
  `ValuepO2Perfusate` varchar(45) DEFAULT NULL COMMENT 'Value pO2 of Perfusate',
  `ValuepO2PerfusateMeasured` varchar(100) DEFAULT NULL COMMENT 'How was pO2 measured',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments (Free Text); Not Mandatory',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the table',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `EventCode` tinyint(3) unsigned NOT NULL DEFAULT '29' COMMENT 'Default Event Code for the Table',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`MachinePerfusionID`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1 COMMENT='Machine Perfusion';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`machineperfusion_AFTER_UPDATE` AFTER UPDATE ON `machineperfusion` FOR EACH ROW
 INSERT INTO  copy_machineperfusion
(parentrow_action, MachinePerfusionID, TrialID, DonorID, Side, KidneyOnMachine, KidneyOnMachineNo, PerfusionStartDate, PerfusionStartTime, MachineSerialNumber,
MachineReferenceModelNumber, LotNumberPerfusionSolution, LotNumberDisposables, Cannulae, CannulaeNumber, UsedPatchHolder, 
ArtificialPatchUsed, ArtificialPatchSize,ArtificialPatchNumber, OxygenBottleFull, OxygenBottleOpened,
OxygenTankChanged, OxygenTankChangedDate, OxygenTankChangedTime, IceContainerReplenished, IceContainerReplenishedDate,
IceContainerReplenishedTime, LogisticallyPossibleMeasurepO2Perfusate, ValuepO2Perfusate, ValuepO2PerfusateMeasured, Comments, DataLocked, DateLocked, LockedBy,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, ReasonModified, EventCode, Source)
VALUES
('update', OLD.MachinePerfusionID, OLD.TrialID, OLD.DonorID, OLD.Side, OLD.KidneyOnMachine, OLD.KidneyOnMachineNo, OLD.PerfusionStartDate, OLD.PerfusionStartTime, OLD.MachineSerialNumber,
OLD.MachineReferenceModelNumber, OLD.LotNumberPerfusionSolution, OLD.LotNumberDisposables, OLD.Cannulae, OLD.CannulaeNumber, OLD.UsedPatchHolder, 
OLD.ArtificialPatchUsed, OLD.ArtificialPatchSize, OLD.ArtificialPatchNumber, OLD.OxygenBottleFull, OLD.OxygenBottleOpened, 
OLD.OxygenTankChanged, OLD.OxygenTankChangedDate, OLD.OxygenTankChangedTime, OLD.IceContainerReplenished, OLD.IceContainerReplenishedDate,
OLD.IceContainerReplenishedTime, OLD.LogisticallyPossibleMeasurepO2Perfusate, OLD.ValuepO2Perfusate, OLD.ValuepO2PerfusateMeasured, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.ReasonModified, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`machineperfusion_AFTER_DELETE` AFTER DELETE ON `machineperfusion` FOR EACH ROW
    INSERT INTO  copy_machineperfusion
(parentrow_action, MachinePerfusionID, TrialID, DonorID, Side, KidneyOnMachine, KidneyOnMachineNo, PerfusionStartDate, PerfusionStartTime, MachineSerialNumber,
MachineReferenceModelNumber, LotNumberPerfusionSolution, LotNumberDisposables, Cannulae, CannulaeNumber, UsedPatchHolder, 
ArtificialPatchUsed, ArtificialPatchSize, ArtificialPatchNumber, OxygenBottleFull, OxygenBottleOpened,
OxygenTankChanged, OxygenTankChangedDate, OxygenTankChangedTime, IceContainerReplenished, IceContainerReplenishedDate,
IceContainerReplenishedTime, LogisticallyPossibleMeasurepO2Perfusate, ValuepO2Perfusate, ValuepO2PerfusateMeasured, Comments, DataLocked, DateLocked, LockedBy,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, ReasonModified, EventCode, Source)
VALUES
('delete', OLD.MachinePerfusionID, OLD.TrialID, OLD.DonorID, OLD.Side, OLD.KidneyOnMachine, OLD.KidneyOnMachineNo, OLD.PerfusionStartDate, OLD.PerfusionStartTime, OLD.MachineSerialNumber,
OLD.MachineReferenceModelNumber, OLD.LotNumberPerfusionSolution, OLD.LotNumberDisposables, OLD.Cannulae, OLD.CannulaeNumber, OLD.UsedPatchHolder, 
OLD.ArtificialPatchUsed, OLD.ArtificialPatchSize, OLD.ArtificialPatchNumber, OLD.OxygenBottleFull, OLD.OxygenBottleOpened, 
OLD.OxygenTankChanged, OLD.OxygenTankChangedDate, OLD.OxygenTankChangedTime, OLD.IceContainerReplenished, OLD.IceContainerReplenishedDate,
OLD.IceContainerReplenishedTime, OLD.LogisticallyPossibleMeasurepO2Perfusate, OLD.ValuepO2Perfusate, OLD.ValuepO2PerfusateMeasured, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.ReasonModified, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `r_ae`
--

DROP TABLE IF EXISTS `r_ae`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `r_ae` (
  `AEID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'TrialIDRecipient',
  `DateAE` date NOT NULL COMMENT 'Date of Adverse Event (Date)',
  `AdverseEventType` varchar(100) DEFAULT NULL COMMENT 'Adverse Event Type (Recipient infection/ Biopsy proven acute rejection/ Bile leak/ Biliary stricture (anastomotic)/ Biliary stricture (non-anastomotic)/\nBleeding/ Hepatic artery thrombosis/ Portal vein thrombosis/ Hepatic artery stenosis/ Portal vein stenosis/ Re-operation/ Other)',
  `RecipientInfectionType` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Type (Pneumonia/ Surgical Site Infection/ Intra-abdominal Collection/ Other)',
  `RecipientInfectionTypeOther` varchar(100) DEFAULT NULL COMMENT 'If Recipient Infection Type is Other (Free Text)',
  `RecipientInfectionOrganismBacteria` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Organism Bacteria (YES/NO)',
  `RecipientInfectionOrganismBacteriaDetails` varchar(100) DEFAULT NULL COMMENT 'Details if Recipient Infection Organism Bacteria is YES (Free text)',
  `RecipientInfectionOrganismViral` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Organism Viral (YES/NO)',
  `RecipientInfectionOrganismViralDetails` varchar(100) DEFAULT NULL COMMENT 'Details if Recipient Infection Organism Viral is YES (Free text)',
  `RecipientInfectionOrganismFungal` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Organism Fungal (YES/NO)',
  `RecipientInfectionOrganismFungalDetails` varchar(100) DEFAULT NULL COMMENT 'Details if Recipient Infection Organism Fungal is YES (Free text)',
  `RecipientInfectionCG` varchar(100) DEFAULT NULL COMMENT 'Recipient Infection Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BiopsyProvenAcuteRejectionBG` varchar(100) DEFAULT NULL COMMENT 'Biopsy Proven Acute Rejection Banff Grading (Indeterminate/ Mild/ Moderate/ Severe)',
  `BiopsyProvenAcuteRejectionCG` varchar(100) DEFAULT NULL COMMENT 'Biopsy Proven Acute Rejection Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BileLeakCG` varchar(100) DEFAULT NULL COMMENT 'Bile Leak Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BiliaryStrictureAnastomoticCG` varchar(100) DEFAULT NULL COMMENT 'Biliary Stricture Anastomotic Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BiliaryStrictureNonAnastomoticCG` varchar(100) DEFAULT NULL COMMENT 'Biliary Stricture NonA nastomotic Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `BleedingCG` varchar(100) DEFAULT NULL COMMENT 'Bleeding Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `HepaticArteryThrombosisCG` varchar(100) DEFAULT NULL COMMENT 'Hepatic Artery Thrombosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `PortalVeinThrombosisCG` varchar(100) DEFAULT NULL COMMENT 'Portal Vein Thrombosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `HepaticArteryStenosisCG` varchar(100) DEFAULT NULL COMMENT 'Hepatic Artery Stenosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `PortalVeinStenosisCG` varchar(100) DEFAULT NULL COMMENT 'Portal VeinStenosis Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `ReOperationCG` varchar(100) DEFAULT NULL COMMENT 'ReOperation Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `ReOperationDetails` varchar(500) DEFAULT NULL COMMENT 'ReOperation Details if Adverse Event Type= ReOperation',
  `OtherAdverseEvent` varchar(500) DEFAULT NULL COMMENT 'Other Adverse Event (Free Text)',
  `ClavienGrading` varchar(45) DEFAULT NULL COMMENT 'Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `OtherAdverseEventCG` varchar(100) DEFAULT NULL COMMENT 'Other Adverse Event Clavien Grading (I/II/IIa/IIb/IIIa/IIIb/IVa/IVb/V)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the Table',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`AEID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COMMENT='Adverse Events';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_ae_AFTER_UPDATE` AFTER UPDATE ON `r_ae` FOR EACH ROW
  INSERT INTO  copy_r_ae
(parentrow_action, AEID, TrialIDRecipient, DateAE, AdverseEventType, RecipientInfectionType, RecipientInfectionTypeOther,
RecipientInfectionOrganismBacteria, RecipientInfectionOrganismBacteriaDetails, RecipientInfectionOrganismViral,
RecipientInfectionOrganismViralDetails, RecipientInfectionOrganismFungal, RecipientInfectionOrganismFungalDetails,
RecipientInfectionCG, BiopsyProvenAcuteRejectionBG, BiopsyProvenAcuteRejectionCG, BileLeakCG, BiliaryStrictureAnastomoticCG,
BiliaryStrictureNonAnastomoticCG, BleedingCG, HepaticArteryThrombosisCG, PortalVeinThrombosisCG, HepaticArteryStenosisCG,
PortalVeinStenosisCG, ReOperationCG, ReOperationDetails, OtherAdverseEvent, ClavienGrading, OtherAdverseEventCG, Comments, DataLocked, DateLocked,
LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('update', OLD.AEID, OLD.TrialIDRecipient, OLD.DateAE, OLD.AdverseEventType, OLD.RecipientInfectionType, OLD.RecipientInfectionTypeOther,
OLD.RecipientInfectionOrganismBacteria, OLD.RecipientInfectionOrganismBacteriaDetails, OLD.RecipientInfectionOrganismViral,
OLD.RecipientInfectionOrganismViralDetails, OLD.RecipientInfectionOrganismFungal, OLD.RecipientInfectionOrganismFungalDetails,
OLD.RecipientInfectionCG, OLD.BiopsyProvenAcuteRejectionBG, OLD.BiopsyProvenAcuteRejectionCG, OLD.BileLeakCG, OLD.BiliaryStrictureAnastomoticCG,
OLD.BiliaryStrictureNonAnastomoticCG, OLD.BleedingCG, OLD.HepaticArteryThrombosisCG, OLD.PortalVeinThrombosisCG, OLD.HepaticArteryStenosisCG,
OLD.PortalVeinStenosisCG, OLD.ReOperationCG, OLD.ReOperationDetails, OLD.OtherAdverseEvent, OLD.ClavienGrading, OLD.OtherAdverseEventCG, OLD.Comments, OLD.DataLocked, OLD.DateLocked,
OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_ae_AFTER_DELETE` AFTER DELETE ON `r_ae` FOR EACH ROW
    INSERT INTO  copy_r_ae
(parentrow_action, AEID, TrialIDRecipient, DateAE, AdverseEventType, RecipientInfectionType, RecipientInfectionTypeOther,
RecipientInfectionOrganismBacteria, RecipientInfectionOrganismBacteriaDetails, RecipientInfectionOrganismViral,
RecipientInfectionOrganismViralDetails, RecipientInfectionOrganismFungal, RecipientInfectionOrganismFungalDetails,
RecipientInfectionCG, BiopsyProvenAcuteRejectionBG, BiopsyProvenAcuteRejectionCG, BileLeakCG, BiliaryStrictureAnastomoticCG,
BiliaryStrictureNonAnastomoticCG, BleedingCG, HepaticArteryThrombosisCG, PortalVeinThrombosisCG, HepaticArteryStenosisCG,
PortalVeinStenosisCG, ReOperationCG, ReOperationDetails, OtherAdverseEvent, ClavienGrading, OtherAdverseEventCG, Comments, DataLocked, DateLocked,
LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('delete', OLD.AEID, OLD.TrialIDRecipient, OLD.DateAE, OLD.AdverseEventType, OLD.RecipientInfectionType, OLD.RecipientInfectionTypeOther,
OLD.RecipientInfectionOrganismBacteria, OLD.RecipientInfectionOrganismBacteriaDetails, OLD.RecipientInfectionOrganismViral,
OLD.RecipientInfectionOrganismViralDetails, OLD.RecipientInfectionOrganismFungal, OLD.RecipientInfectionOrganismFungalDetails,
OLD.RecipientInfectionCG, OLD.BiopsyProvenAcuteRejectionBG, OLD.BiopsyProvenAcuteRejectionCG, OLD.BileLeakCG, OLD.BiliaryStrictureAnastomoticCG,
OLD.BiliaryStrictureNonAnastomoticCG, OLD.BleedingCG, OLD.HepaticArteryThrombosisCG, OLD.PortalVeinThrombosisCG, OLD.HepaticArteryStenosisCG,
OLD.PortalVeinStenosisCG, OLD.ReOperationCG, OLD.ReOperationDetails, OLD.OtherAdverseEvent, OLD.ClavienGrading, OLD.OtherAdverseEventCG, OLD.Comments, OLD.DataLocked, OLD.DateLocked,
OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `r_deceased`
--

DROP TABLE IF EXISTS `r_deceased`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `r_deceased` (
  `RDeceasedID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(10) NOT NULL COMMENT 'Unique ID for Liver being transplanted (ABCCC -A is country code, B Centre number, where the Transplant is done, CCC running number)',
  `DeathDate` date DEFAULT NULL COMMENT 'Date of Death (Date)',
  `DeathTime` time DEFAULT NULL COMMENT 'Date of Death (Time)',
  `CauseDeath` varchar(100) DEFAULT NULL COMMENT 'Tx/Non Tx',
  `CauseDeathDetails` varchar(500) DEFAULT NULL COMMENT 'From Multiple Selection',
  `CauseDeathDetailsOther` varchar(100) DEFAULT NULL COMMENT 'If other choses in cause of death details (Free Text)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments Withdrawn',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Time stamp',
  `Source` varchar(45) DEFAULT '0',
  PRIMARY KEY (`RDeceasedID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`),
  KEY `Index_3` (`DeathDate`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COMMENT='Deceased Details';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_deceased_AFTER_UPDATE` AFTER UPDATE ON `r_deceased` FOR EACH ROW
    INSERT INTO  copy_r_deceased
(parentrow_action, RDeceasedID, TrialIDRecipient, DeathDate, DeathTime, CauseDeath, CauseDeathDetails,
CauseDeathDetailsOther, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('update', OLD.RDeceasedID, OLD.TrialIDRecipient, OLD.DeathDate, OLD.DeathTime, OLD.CauseDeath, OLD.CauseDeathDetails,
OLD.CauseDeathDetailsOther, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_deceased_AFTER_DELETE` AFTER DELETE ON `r_deceased` FOR EACH ROW
    INSERT INTO  copy_r_deceased
(parentrow_action, RDeceasedID, TrialIDRecipient, DeathDate, DeathTime, CauseDeath, CauseDeathDetails,
CauseDeathDetailsOther, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('delete', OLD.RDeceasedID, OLD.TrialIDRecipient, OLD.DeathDate, OLD.DeathTime, OLD.CauseDeath, OLD.CauseDeathDetails,
OLD.CauseDeathDetailsOther, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `r_fuposttreatment`
--

DROP TABLE IF EXISTS `r_fuposttreatment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `r_fuposttreatment` (
  `RFUPostTreatmentID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `RIdentificationID` varchar(45) DEFAULT NULL COMMENT 'RecipientID',
  `Occasion` varchar(45) NOT NULL COMMENT '3 Months/ 6 months/ 1 Year',
  `FollowUpDate` date NOT NULL COMMENT 'Date of Follow Up',
  `GraftFailure` varchar(45) NOT NULL COMMENT 'Graft Failure (YES/NO)',
  `DateGraftFailure` date DEFAULT NULL COMMENT 'Date of Graft Failure (Date)',
  `PrimaryCause` varchar(100) DEFAULT NULL COMMENT 'Primary Cause (Immunologic/ Preservation/ Technical - Arterial/ Technical - Venous/ Infection - Bacterial/ Infection - Viral/ Other)',
  `PrimaryCauseOther` varchar(100) DEFAULT NULL COMMENT 'If Primary Cause if Other',
  `GraftRemoval` varchar(45) DEFAULT NULL COMMENT 'Graft Removal (YES/NO)',
  `DateGraftRemoval` date DEFAULT NULL COMMENT 'Date of Graft Removal (Date)',
  `Death` varchar(45) DEFAULT NULL COMMENT 'Death (YES/NO)',
  `DateDeath` date DEFAULT NULL COMMENT 'Date of Death (Date)',
  `CauseDeath` varchar(45) DEFAULT NULL COMMENT 'Cause of Death (Tx Related/ Non Tx Related)',
  `RequiredHyperkalemia` varchar(100) DEFAULT NULL COMMENT 'Required for hyperkalemia or fluid overload, If only 1 dialysis session  (YES/NO)',
  `HypotensivePeriod1` varchar(100) DEFAULT NULL COMMENT 'Hypotensive period I First 24 Hours Post Transplant (YES/NO)',
  `HypotensivePeriod1Duration` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period I Duration (Minutes)',
  `HypotensivePeriod1LowestSystolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period I Lowest Systolic Blood Pressure (mmHg)',
  `HypotensivePeriod1LowestDiastolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period I LowestDiastolic Blood Pressure (mmHg)',
  `HypotensivePeriod2` varchar(100) DEFAULT NULL COMMENT 'Hypotensive period II First 24 Hours Post Transplant (YES/NO)',
  `HypotensivePeriod2Duration` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period II Duration (Minutes)',
  `HypotensivePeriod2LowestSystolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period II Lowest Systolic Blood Pressure (mmHg)',
  `HypotensivePeriod2LowestDiastolicBloodPressure` varchar(100) DEFAULT NULL COMMENT 'Hypotensive Period II LowestDiastolic Blood Pressure (mmHg)',
  `SerumCreatinine` varchar(300) DEFAULT NULL COMMENT 'Serum Creatinine (µmol/l or mg/dL)',
  `CreatinineUnit` varchar(45) DEFAULT NULL COMMENT 'Creatine Unit (µmol/l or mg/dL)',
  `UrineCreatinine` varchar(300) DEFAULT NULL COMMENT 'UrineCreatinine (mmol/l or mg/dL)',
  `UrineUnit` varchar(45) DEFAULT NULL COMMENT 'Urine Unit (mmol/l or mg/dL)',
  `CreatinineClearance` varchar(300) DEFAULT NULL COMMENT 'Creatinine Clearance (ml/min)',
  `CreatinineClearanceUnit` varchar(45) DEFAULT NULL COMMENT 'CreatinineClearanceUnit ml/min',
  `EGFR` varchar(500) DEFAULT NULL COMMENT 'eGFR (Derived Value)',
  `CurrentlyDialysis` varchar(45) DEFAULT NULL COMMENT 'Currently on Dialysis (YES/NO)',
  `DialysisType` varchar(45) DEFAULT NULL COMMENT 'Dialysis Type (CAPD/Hemodialysis)',
  `DateDialysisSince` date DEFAULT NULL COMMENT 'Date Last Dialysis Since, if currently on Dialysis (Date)',
  `DateLastDialysis` date DEFAULT NULL COMMENT 'Date of last dialysis (Date)',
  `DialysisSessions` varchar(45) DEFAULT NULL COMMENT 'Number of Dialysis Session Since Last Follow Up (Number)',
  `InductionTherapy` varchar(45) DEFAULT NULL COMMENT 'Induction Therapy',
  `NumberRejectionPeriods` varchar(45) DEFAULT NULL COMMENT 'Number of rejection period (Number)',
  `PostTxImmunosuppressive` varchar(500) DEFAULT NULL COMMENT 'Post Tx Immunosuppressive Drugs (Azathioprine/Cyclosporin/MMF/Prednisolone/Sirolomus/Tacrolimus/Other)',
  `PostTxImmunosuppressiveOther` varchar(100) DEFAULT NULL COMMENT 'Post Tx Immunosuppressive Drugs If Other',
  `RejectionTreatmentsPostTx` varchar(45) DEFAULT NULL COMMENT 'Number of treatments for rejection post transplant, since last follow up (Number)',
  `Rejection` varchar(45) DEFAULT NULL COMMENT 'Rejection (YES/NO)',
  `PostTxPrednisolon` varchar(45) DEFAULT NULL COMMENT 'Post TX Rejection Treated with Prednisolon (YES/NO)',
  `PostTxOther` varchar(300) DEFAULT NULL COMMENT 'Post TX Rejection Treated with Other Drug (YES/NO)',
  `PostTxOtherDetails` varchar(100) DEFAULT NULL COMMENT 'Post TX Rejection Treated with Other Drug Details (Free Text)',
  `RejectionBiopsyProven` varchar(45) DEFAULT NULL COMMENT ' Rejection Biopsy Proven (YES/NO)',
  `CalcineurinInhibitorToxicity` varchar(45) DEFAULT NULL COMMENT 'Calcineurin Inhibitor Toxicity (YES/NO)',
  `NeedDialysis` varchar(300) DEFAULT NULL COMMENT 'Need Dialysis For First Follow Up, Days 1-7, 10 and 14 (YES/NO)',
  `DatePrimaryPostTxDischarge` date DEFAULT NULL COMMENT 'Date of primary post-Tx discharge ',
  `ComplicationsGraftFunction` varchar(500) DEFAULT NULL COMMENT 'Complications interfering with graft function not mentioned above',
  `QOLFilledAt` varchar(45) DEFAULT NULL,
  `Mobility` varchar(45) DEFAULT NULL COMMENT 'Mobility Quality of Life (1-5, 9 missing)',
  `SelfCare` varchar(45) DEFAULT NULL COMMENT 'Self Care Quality of Life (1-5, 9 missing)',
  `UsualActivities` varchar(45) DEFAULT NULL COMMENT 'Usual Activities Quality of Life (1-5, 9 missing)',
  `PainDiscomfort` varchar(45) DEFAULT NULL COMMENT 'Pain/Discomfort of Life (1-5, 9 missing)',
  `AnxietyDepression` varchar(45) DEFAULT NULL COMMENT 'Anxiety Depression Quality of Life (1-5, 9 missing)',
  `VASScore` varchar(45) DEFAULT NULL COMMENT 'VAS Score Quality of Life (0-100, 999 missing)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (Free Text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the Table',
  `EventCode` tinyint(3) unsigned DEFAULT NULL COMMENT 'Default Event Code for the Table. Blank as mutliple event codes assigned',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`RFUPostTreatmentID`),
  KEY `Index_3` (`RIdentificationID`),
  KEY `Index_2` (`TrialIDRecipient`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=latin1 COMMENT='Follow up post transplant 1month, 3 months, 6 months, 1 year ';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_fuposttreatment_AFTER_UPDATE` AFTER UPDATE ON `r_fuposttreatment` FOR EACH ROW
 INSERT INTO  copy_r_fuposttreatment
(parentrow_action, RFUPostTreatmentID, TrialIDRecipient, RIdentificationID, Occasion, FollowUpDate, GraftFailure, DateGraftFailure,
PrimaryCause, PrimaryCauseOther, GraftRemoval, DateGraftRemoval, Death, DateDeath, CauseDeath, RequiredHyperkalemia,
HypotensivePeriod1, HypotensivePeriod1Duration, HypotensivePeriod1LowestSystolicBloodPressure,
HypotensivePeriod1LowestDiastolicBloodPressure, HypotensivePeriod2, HypotensivePeriod2Duration, HypotensivePeriod2LowestSystolicBloodPressure,
HypotensivePeriod2LowestDiastolicBloodPressure, SerumCreatinine, CreatinineUnit, UrineCreatinine, UrineUnit, CreatinineClearance,
CreatinineClearanceUnit, EGFR, CurrentlyDialysis, DialysisType, DateDialysisSince, DateLastDialysis, DialysisSessions,
InductionTherapy, NumberRejectionPeriods, PostTxImmunosuppressive, PostTxImmunosuppressiveOther, RejectionTreatmentsPostTx,
Rejection, PostTxPrednisolon, PostTxOther, PostTxOtherDetails, RejectionBiopsyProven, CalcineurinInhibitorToxicity, NeedDialysis, DatePrimaryPostTxDischarge,
ComplicationsGraftFunction, QOLFilledAt, Mobility, SelfCare, UsualActivities, PainDiscomfort, AnxietyDepression, VASScore, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update', OLD.RFUPostTreatmentID, OLD.TrialIDRecipient, OLD.RIdentificationID, OLD.Occasion, OLD.FollowUpDate, OLD.GraftFailure, OLD.DateGraftFailure,
OLD.PrimaryCause, OLD.PrimaryCauseOther, OLD.GraftRemoval, OLD.DateGraftRemoval, OLD.Death, OLD.DateDeath, OLD.CauseDeath, OLD.RequiredHyperkalemia,
OLD.HypotensivePeriod1, OLD.HypotensivePeriod1Duration, OLD.HypotensivePeriod1LowestSystolicBloodPressure, OLD.HypotensivePeriod1LowestDiastolicBloodPressure,
OLD.HypotensivePeriod2, OLD.HypotensivePeriod2Duration, OLD.HypotensivePeriod2LowestSystolicBloodPressure, OLD.HypotensivePeriod2LowestDiastolicBloodPressure,
OLD.SerumCreatinine, OLD.CreatinineUnit, OLD.UrineCreatinine, OLD.UrineUnit, OLD.CreatinineClearance, OLD.CreatinineClearanceUnit, OLD.EGFR, OLD.CurrentlyDialysis,
OLD.DialysisType, OLD.DateDialysisSince, OLD.DateLastDialysis, OLD.DialysisSessions, OLD.InductionTherapy, OLD.NumberRejectionPeriods, OLD.PostTxImmunosuppressive,
OLD.PostTxImmunosuppressiveOther, OLD.RejectionTreatmentsPostTx, OLD.Rejection, OLD.PostTxPrednisolon, OLD.PostTxOther, OLD.PostTxOtherDetails, OLD.RejectionBiopsyProven,
OLD.CalcineurinInhibitorToxicity, OLD.NeedDialysis, OLD.DatePrimaryPostTxDischarge, OLD.ComplicationsGraftFunction, OLD.QOLFilledAt, OLD.Mobility, OLD.SelfCare,
OLD.UsualActivities, OLD.PainDiscomfort, OLD.AnxietyDepression, OLD.VASScore, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_fuposttreatment_AFTER_DELETE` AFTER DELETE ON `r_fuposttreatment` FOR EACH ROW
    INSERT INTO  copy_r_fuposttreatment
(parentrow_action, RFUPostTreatmentID, TrialIDRecipient, RIdentificationID, Occasion, FollowUpDate, GraftFailure, DateGraftFailure,
PrimaryCause, PrimaryCauseOther, GraftRemoval, DateGraftRemoval, Death, DateDeath, CauseDeath, RequiredHyperkalemia,
HypotensivePeriod1, HypotensivePeriod1Duration, HypotensivePeriod1LowestSystolicBloodPressure,
HypotensivePeriod1LowestDiastolicBloodPressure, HypotensivePeriod2, HypotensivePeriod2Duration, HypotensivePeriod2LowestSystolicBloodPressure,
HypotensivePeriod2LowestDiastolicBloodPressure, SerumCreatinine, CreatinineUnit, UrineCreatinine, UrineUnit, CreatinineClearance,
CreatinineClearanceUnit, EGFR, CurrentlyDialysis, DialysisType, DateDialysisSince, DateLastDialysis, DialysisSessions,
InductionTherapy, NumberRejectionPeriods, PostTxImmunosuppressive, PostTxImmunosuppressiveOther, RejectionTreatmentsPostTx,
Rejection, PostTxPrednisolon, PostTxOther, PostTxOtherDetails, RejectionBiopsyProven, CalcineurinInhibitorToxicity, NeedDialysis, DatePrimaryPostTxDischarge,
ComplicationsGraftFunction, QOLFilledAt, Mobility, SelfCare, UsualActivities, PainDiscomfort, AnxietyDepression, VASScore, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete', OLD.RFUPostTreatmentID, OLD.TrialIDRecipient, OLD.RIdentificationID, OLD.Occasion, OLD.FollowUpDate, OLD.GraftFailure, OLD.DateGraftFailure,
OLD.PrimaryCause, OLD.PrimaryCauseOther, OLD.GraftRemoval, OLD.DateGraftRemoval, OLD.Death, OLD.DateDeath, OLD.CauseDeath, OLD.RequiredHyperkalemia,
OLD.HypotensivePeriod1, OLD.HypotensivePeriod1Duration, OLD.HypotensivePeriod1LowestSystolicBloodPressure, OLD.HypotensivePeriod1LowestDiastolicBloodPressure,
OLD.HypotensivePeriod2, OLD.HypotensivePeriod2Duration, OLD.HypotensivePeriod2LowestSystolicBloodPressure, OLD.HypotensivePeriod2LowestDiastolicBloodPressure,
OLD.SerumCreatinine, OLD.CreatinineUnit, OLD.UrineCreatinine, OLD.UrineUnit, OLD.CreatinineClearance, OLD.CreatinineClearanceUnit, OLD.EGFR, OLD.CurrentlyDialysis,
OLD.DialysisType, OLD.DateDialysisSince, OLD.DateLastDialysis, OLD.DialysisSessions, OLD.InductionTherapy, OLD.NumberRejectionPeriods, OLD.PostTxImmunosuppressive,
OLD.PostTxImmunosuppressiveOther, OLD.RejectionTreatmentsPostTx, OLD.Rejection, OLD.PostTxPrednisolon, OLD.PostTxOther, OLD.PostTxOtherDetails, OLD.RejectionBiopsyProven,
OLD.CalcineurinInhibitorToxicity, OLD.NeedDialysis, OLD.DatePrimaryPostTxDischarge, OLD.ComplicationsGraftFunction, OLD.QOLFilledAt, OLD.Mobility, OLD.SelfCare,
OLD.UsualActivities, OLD.PainDiscomfort, OLD.AnxietyDepression, OLD.VASScore, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `r_identification`
--

DROP TABLE IF EXISTS `r_identification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `r_identification` (
  `RIdentificationID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(45) DEFAULT NULL COMMENT 'UniqueID',
  `KidneyReceived` varchar(45) DEFAULT NULL COMMENT 'Kidney received (LEFT/RIGHT)',
  `TrialIDRecipient` varchar(45) NOT NULL,
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Unique Donor ID',
  `RecipientID` varchar(45) DEFAULT NULL COMMENT 'Unique RecipientID',
  `RecipientCentre` varchar(45) DEFAULT NULL COMMENT 'Name recipient transplantation center (Free  Text)',
  `DateOfBirth` date DEFAULT NULL COMMENT 'Recipient date of birth ',
  `Sex` varchar(45) DEFAULT NULL COMMENT 'Recipient Sex: (Male/ Female)',
  `Weight` varchar(45) DEFAULT NULL COMMENT 'Donor weight kg',
  `Height` varchar(45) DEFAULT NULL COMMENT 'Donor Height in centimetre',
  `BMI` varchar(45) DEFAULT NULL COMMENT 'Donor BMI',
  `EthnicityBlack` varchar(45) DEFAULT NULL COMMENT 'If Ethnicity is Black',
  `RenalDisease` varchar(100) DEFAULT NULL COMMENT 'Renal disease (Glomerular diseases, Polycystic kidneys, Uncertain etiology, Tubular and interstitial diseases, Retransplant/Graft failure,Diabetes,Hypertensive nephroangiosclerosis, Congenital, rare familial/ metabolic disorders, Renovascular and other renal vascular diseases, Neoplasms, Other)',
  `RenalDiseaseOther` varchar(100) DEFAULT NULL COMMENT 'If Renal Disease Other',
  `NumberPreviousTransplants` varchar(45) DEFAULT NULL COMMENT 'Number of previous transplants Not required removed Sarah Email 20140418',
  `PreTransplantDiuresis` varchar(45) DEFAULT NULL COMMENT 'Pre-transplant diuresis (ml/24h)',
  `BloodGroup` varchar(45) DEFAULT NULL COMMENT 'Recipient blood group',
  `HLA_A` varchar(45) DEFAULT NULL COMMENT 'HLA Type A (HLA A)',
  `HLA_B` varchar(45) DEFAULT NULL COMMENT 'HLA Type B (HLA B)',
  `HLA_DR` varchar(45) DEFAULT NULL COMMENT 'HLA Type DR (HLA DR)',
  `ET_Urgency` varchar(45) DEFAULT NULL COMMENT 'ETUrgency 0/1/2/4',
  `HLA_A_Mismatch` varchar(45) DEFAULT NULL COMMENT 'Number of HLA mismatches (HLA A)',
  `HLA_B_Mismatch` varchar(45) DEFAULT NULL COMMENT 'Number of HLA mismatches (HLA B)',
  `HLA_DR_Mismatch` varchar(45) DEFAULT NULL COMMENT 'Number of HLA mismatches (HLA DR)',
  `Occasion` varchar(45) DEFAULT NULL COMMENT 'For Quality of Life Data',
  `DateQOLFIlled` varchar(45) DEFAULT NULL,
  `QOLFilledAt` varchar(45) DEFAULT NULL,
  `Mobility` varchar(45) DEFAULT NULL COMMENT 'Mobility Quality of Life (1-5, 9 missing)',
  `SelfCare` varchar(45) DEFAULT NULL COMMENT 'Self Care Quality of Life (1-5, 9 missing)',
  `UsualActivities` varchar(45) DEFAULT NULL COMMENT 'Usual Activities Quality of Life (1-5, 9 missing)',
  `PainDiscomfort` varchar(45) DEFAULT NULL COMMENT 'Pain/Discomfort of Life (1-5, 9 missing)',
  `AnxietyDepression` varchar(45) DEFAULT NULL COMMENT 'Anxiety Depression Quality of Life (1-5, 9 missing)',
  `VASScore` varchar(45) DEFAULT NULL COMMENT 'VAS Score Quality of Life (0-100, 999 missing)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'General Comments (Free Text)',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'DateTime Created By',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the table',
  `EventCode` tinyint(4) DEFAULT '31',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`RIdentificationID`),
  KEY `Index_3` (`DonorID`),
  KEY `Index_4` (`RecipientID`),
  KEY `Index_2` (`TrialID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=latin1 COMMENT='Recipient identification';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_identification_BEFORE_UPDATE` BEFORE UPDATE ON `r_identification` FOR EACH ROW
INSERT INTO copy_r_identification
(parentrow_action, RIdentificationID, TrialID, KidneyReceived, TrialIDRecipient, DonorID, RecipientID, RecipientCentre, DateOfBirth,
Sex, Weight, Height, BMI, EthnicityBlack,RenalDisease, RenalDiseaseOther, NumberPreviousTransplants, PreTransplantDiuresis,
BloodGroup, HLA_A, HLA_B, HLA_DR, ET_Urgency,HLA_A_Mismatch, HLA_B_Mismatch, HLA_DR_Mismatch,
Occasion, DateQOLFIlled, QOLFilledAt, Mobility, SelfCare, UsualActivities, PainDiscomfort, AnxietyDepression, VASScore,
Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update', OLD.RIdentificationID, OLD.TrialID, OLD.KidneyReceived, OLD.TrialIDRecipient, OLD.DonorID, OLD.RecipientID, OLD.RecipientCentre, OLD.DateOfBirth,
OLD.Sex, OLD.Weight, OLD.Height, OLD.BMI, OLD.EthnicityBlack, OLD.RenalDisease, OLD.RenalDiseaseOther, OLD.NumberPreviousTransplants, OLD.PreTransplantDiuresis,
OLD.BloodGroup, OLD.HLA_A, OLD.HLA_B, OLD.HLA_DR, OLD.ET_Urgency, OLD.HLA_A_Mismatch, OLD.HLA_B_Mismatch, OLD.HLA_DR_Mismatch,
OLD.Occasion, OLD.DateQOLFIlled, OLD.QOLFilledAt, OLD.Mobility, OLD.SelfCare, OLD.UsualActivities, OLD.PainDiscomfort, OLD.AnxietyDepression, OLD.VASScore,
OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_identification_AFTER_DELETE` AFTER DELETE ON `r_identification` FOR EACH ROW
INSERT INTO copy_r_identification
(parentrow_action, RIdentificationID, TrialID, KidneyReceived, TrialIDRecipient, DonorID, RecipientID, RecipientCentre, DateOfBirth,
Sex, Weight, Height, BMI, EthnicityBlack,RenalDisease, RenalDiseaseOther, NumberPreviousTransplants, PreTransplantDiuresis,
BloodGroup, HLA_A, HLA_B, HLA_DR, ET_Urgency,HLA_A_Mismatch, HLA_B_Mismatch, HLA_DR_Mismatch,
Occasion, DateQOLFIlled, QOLFilledAt, Mobility, SelfCare, UsualActivities, PainDiscomfort, AnxietyDepression, VASScore,
Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete', OLD.RIdentificationID, OLD.TrialID, OLD.KidneyReceived, OLD.TrialIDRecipient, OLD.DonorID, OLD.RecipientID, OLD.RecipientCentre, OLD.DateOfBirth,
OLD.Sex, OLD.Weight, OLD.Height, OLD.BMI, OLD.EthnicityBlack, OLD.RenalDisease, OLD.RenalDiseaseOther, OLD.NumberPreviousTransplants, OLD.PreTransplantDiuresis,
OLD.BloodGroup, OLD.HLA_A, OLD.HLA_B, OLD.HLA_DR, OLD.ET_Urgency, OLD.HLA_A_Mismatch, OLD.HLA_B_Mismatch, OLD.HLA_DR_Mismatch,
OLD.Occasion, OLD.DateQOLFIlled, OLD.QOLFilledAt, OLD.Mobility, OLD.SelfCare, OLD.UsualActivities, OLD.PainDiscomfort, OLD.AnxietyDepression, OLD.VASScore,
OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `r_perioperative`
--

DROP TABLE IF EXISTS `r_perioperative`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `r_perioperative` (
  `RPerioperativeID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Unique ID of the transplant',
  `RIdentificationID` int(10) unsigned DEFAULT NULL COMMENT 'Foreign Key r_identification',
  `TransplantationDate` date DEFAULT NULL COMMENT 'Transplantation date (Date)',
  `MachinePerfusionStopDate` date DEFAULT NULL COMMENT 'Date Stop machine perfusion (dd-mm-yyyy hh:mm)',
  `MachinePerfusionStopTime` time DEFAULT NULL COMMENT 'Time Stop machine perfusion (dd-mm-yyyy hh:mm)',
  `TimeOnMachine` varchar(45) DEFAULT NULL COMMENT 'calculate from interval ‘time stop machine perfusion to time left/right kidney on machine in donor data connect left or right to Kidney received in recipient identification',
  `TapeBroken` varchar(45) DEFAULT NULL COMMENT 'Was the Tape over the reagulator Broken?',
  `KidneyRemovedFromIceDate` date DEFAULT NULL,
  `KidneyRemovedFromIceTime` time DEFAULT NULL,
  `OxygenBottleFullAndTurnedOpen` varchar(45) DEFAULT NULL,
  `ColdIschemiaPeriod` varchar(45) DEFAULT NULL,
  `ColdIschemiaPeriodHours` varchar(45) DEFAULT NULL COMMENT 'Cold Ischemia Period (Hours) `ReclampingTime` varchar(45) DEFAULT NULL COMMENT Reclamping time (minutes)',
  `ColdIschemiaPeriodMinutes` varchar(45) DEFAULT NULL COMMENT 'Cold Ischemia Period (Minutes)',
  `KidneyDiscarded` varchar(45) DEFAULT NULL COMMENT 'Kidney Discarded /untransplantable (YES/NO)',
  `KidneyDiscardedYes` varchar(500) DEFAULT NULL COMMENT 'If Kidney Discarded is YES, Reason???',
  `OperationStartDate` date DEFAULT NULL COMMENT 'Start Date operation (induction of anesthesia) (yyyy-mm-dd)',
  `OperationStartTime` time DEFAULT NULL COMMENT 'Start time operation (induction of anesthesia) (hh:mm)',
  `CVPReperfusion` varchar(45) DEFAULT NULL COMMENT 'CVP at Reperfusion (mmHg)',
  `Incision` varchar(45) DEFAULT NULL COMMENT 'Incision (Med. laparotomy/extraperitoneal ie hockey stick incision)',
  `TransplantSide` varchar(45) DEFAULT NULL COMMENT 'Transplant Side (Left/Right)',
  `ArterialProblems` varchar(45) DEFAULT NULL COMMENT 'Arterial problems (no/ligated polar artery/reconstructed polar or hilar artery/repaired intima dissection/other)',
  `ArterialProblemsOther` varchar(500) DEFAULT NULL COMMENT 'If Arterial Problems Other',
  `VenousProblems` varchar(45) DEFAULT NULL COMMENT 'Venous problems (YES/NO)',
  `StartAnastomosisDate` date DEFAULT NULL COMMENT 'Start anastomosis Date',
  `StartAnastomosisTime` time DEFAULT NULL COMMENT 'Start anastomosis Time(hh:mm)',
  `ReperfusionDate` date DEFAULT NULL COMMENT 'Time of reperfusion (hh:mm)',
  `ReperfusionTime` time DEFAULT NULL COMMENT 'Time of reperfusion (hh:mm)',
  `TotalAnastomosisTime` varchar(45) DEFAULT NULL COMMENT 'Total Anastomosis time (Minutes)',
  `MannitolUsed` varchar(45) DEFAULT NULL COMMENT 'Mannitol (YES/NO)',
  `DiureticsUsed` varchar(45) DEFAULT NULL COMMENT 'Diuretics (YES/NO)',
  `HypotensivePeriod` varchar(45) DEFAULT NULL COMMENT 'Hypotensive period - syst. < 100 mmHg (YES/NO)',
  `IntraoperativeDiuresis` varchar(45) DEFAULT NULL COMMENT 'Intra-operative diuresis (YES/ NO/ Unknown)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Remarks Second',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Record Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Record Last Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp',
  `EventCode` tinyint(4) DEFAULT '31',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`RPerioperativeID`) USING BTREE,
  KEY `Index_3` (`RIdentificationID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1 COMMENT='Recipient operation peri-operative data';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_perioperative_AFTER_UPDATE` AFTER UPDATE ON `r_perioperative` FOR EACH ROW
INSERT INTO copy_r_perioperative
(parentrow_action, RPerioperativeID, TrialIDRecipient, RIdentificationID, TransplantationDate, MachinePerfusionStopDate, MachinePerfusionStopTime,
TimeOnMachine, TapeBroken, KidneyRemovedFromIceDate, KidneyRemovedFromIceTime, OxygenBottleFullAndTurnedOpen, ColdIschemiaPeriod,
ColdIschemiaPeriodHours, ColdIschemiaPeriodMinutes, KidneyDiscarded, KidneyDiscardedYes, OperationStartDate, OperationStartTime,
CVPReperfusion, Incision, TransplantSide, ArterialProblems, ArterialProblemsOther, VenousProblems, StartAnastomosisDate,
StartAnastomosisTime, ReperfusionDate, ReperfusionTime, TotalAnastomosisTime, MannitolUsed, DiureticsUsed, HypotensivePeriod,
IntraoperativeDiuresis, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update', OLD.RPerioperativeID, OLD.TrialIDRecipient, OLD.RIdentificationID, OLD.TransplantationDate, OLD.MachinePerfusionStopDate, OLD.MachinePerfusionStopTime,
OLD.TimeOnMachine, OLD.TapeBroken, OLD.KidneyRemovedFromIceDate, OLD.KidneyRemovedFromIceTime, OLD.OxygenBottleFullAndTurnedOpen, OLD.ColdIschemiaPeriod,
OLD.ColdIschemiaPeriodHours, OLD.ColdIschemiaPeriodMinutes, OLD.KidneyDiscarded, OLD.KidneyDiscardedYes, OLD.OperationStartDate, OLD.OperationStartTime,
OLD.CVPReperfusion, OLD.Incision, OLD.TransplantSide, OLD.ArterialProblems, OLD.ArterialProblemsOther, OLD.VenousProblems, OLD.StartAnastomosisDate,
OLD.StartAnastomosisTime, OLD.ReperfusionDate, OLD.ReperfusionTime, OLD.TotalAnastomosisTime, OLD.MannitolUsed, OLD.DiureticsUsed, OLD.HypotensivePeriod,
OLD.IntraoperativeDiuresis, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_perioperative_AFTER_DELETE` AFTER DELETE ON `r_perioperative` FOR EACH ROW
 INSERT INTO copy_r_perioperative
(parentrow_action, RPerioperativeID, TrialIDRecipient, RIdentificationID, TransplantationDate, MachinePerfusionStopDate, MachinePerfusionStopTime,
TimeOnMachine, TapeBroken, KidneyRemovedFromIceDate, KidneyRemovedFromIceTime, OxygenBottleFullAndTurnedOpen, ColdIschemiaPeriod,
ColdIschemiaPeriodHours, ColdIschemiaPeriodMinutes, KidneyDiscarded, KidneyDiscardedYes, OperationStartDate, OperationStartTime,
CVPReperfusion, Incision, TransplantSide, ArterialProblems, ArterialProblemsOther, VenousProblems, StartAnastomosisDate,
StartAnastomosisTime, ReperfusionDate, ReperfusionTime, TotalAnastomosisTime, MannitolUsed, DiureticsUsed, HypotensivePeriod,
IntraoperativeDiuresis, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete', OLD.RPerioperativeID, OLD.TrialIDRecipient, OLD.RIdentificationID, OLD.TransplantationDate, OLD.MachinePerfusionStopDate, OLD.MachinePerfusionStopTime,
OLD.TimeOnMachine, OLD.TapeBroken, OLD.KidneyRemovedFromIceDate, OLD.KidneyRemovedFromIceTime, OLD.OxygenBottleFullAndTurnedOpen, OLD.ColdIschemiaPeriod,
OLD.ColdIschemiaPeriodHours, OLD.ColdIschemiaPeriodMinutes, OLD.KidneyDiscarded, OLD.KidneyDiscardedYes, OLD.OperationStartDate, OLD.OperationStartTime,
OLD.CVPReperfusion, OLD.Incision, OLD.TransplantSide, OLD.ArterialProblems, OLD.ArterialProblemsOther, OLD.VenousProblems, OLD.StartAnastomosisDate,
OLD.StartAnastomosisTime, OLD.ReperfusionDate, OLD.ReperfusionTime, OLD.TotalAnastomosisTime, OLD.MannitolUsed, OLD.DiureticsUsed, OLD.HypotensivePeriod,
OLD.IntraoperativeDiuresis, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `r_readmissions`
--

DROP TABLE IF EXISTS `r_readmissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `r_readmissions` (
  `RReadmissionsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Trial ID',
  `RecipientID` varchar(45) DEFAULT NULL COMMENT 'Recipient ID',
  `RFUPostTreatmentID` int(10) unsigned NOT NULL,
  `Occasion` varchar(45) DEFAULT NULL COMMENT 'Occasion',
  `DateAdmission` date DEFAULT NULL COMMENT 'Date of Admission (Date)',
  `DateDischarge` date DEFAULT NULL COMMENT 'Date of Discharge (Date)',
  `ICU` varchar(45) DEFAULT NULL COMMENT 'If admitted to ICU YES/NO',
  `NeedDialysis` varchar(45) DEFAULT NULL COMMENT 'if Dialysis Needed YES/NO',
  `BiopsyTaken` varchar(45) DEFAULT NULL COMMENT 'if Biopsy Taken YES/NO',
  `Surgery` varchar(45) DEFAULT NULL COMMENT 'If Surgery requried YES/NO',
  `ReasonAdmission` varchar(500) DEFAULT NULL COMMENT 'Reason for Admission',
  `DataLocked` tinyint(4) DEFAULT '0',
  `DateLocked` datetime DEFAULT NULL,
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `ReasonModified` varchar(10000) DEFAULT NULL,
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the Table',
  PRIMARY KEY (`RReadmissionsID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`),
  KEY `Index_3` (`RecipientID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COMMENT='Readmissions During Follow Up';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_readmissions_AFTER_UPDATE` AFTER UPDATE ON `r_readmissions` FOR EACH ROW
 INSERT INTO copy_r_readmissions
(parentrow_action, RReadmissionsID, TrialIDRecipient, RecipientID, RFUPostTreatmentID, Occasion, DateAdmission, DateDischarge, ICU,
NeedDialysis, BiopsyTaken, Surgery, ReasonAdmission, DataLocked, DateLocked, LockedBy,
DataFinal, DateFinal, FinalAssignedBy, ReasonModified, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded)
VALUES
('update', OLD.RReadmissionsID, OLD.TrialIDRecipient, OLD.RecipientID, OLD.RFUPostTreatmentID, OLD.Occasion, OLD.DateAdmission, OLD.DateDischarge, OLD.ICU,
OLD.NeedDialysis, OLD.BiopsyTaken, OLD.Surgery, OLD.ReasonAdmission, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.ReasonModified, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_readmissions_AFTER_DELETE` AFTER DELETE ON `r_readmissions` FOR EACH ROW
 INSERT INTO copy_r_readmissions
(parentrow_action, RReadmissionsID, TrialIDRecipient, RecipientID, RFUPostTreatmentID, Occasion, DateAdmission, DateDischarge, ICU,
NeedDialysis, BiopsyTaken, Surgery, ReasonAdmission, DataLocked, DateLocked, LockedBy,
DataFinal, DateFinal, FinalAssignedBy, ReasonModified, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded)
VALUES
('delete', OLD.RReadmissionsID, OLD.TrialIDRecipient, OLD.RecipientID, OLD.RFUPostTreatmentID, OLD.Occasion, OLD.DateAdmission, OLD.DateDischarge, OLD.ICU,
OLD.NeedDialysis, OLD.BiopsyTaken, OLD.Surgery, OLD.ReasonAdmission, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.ReasonModified, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `r_serae`
--

DROP TABLE IF EXISTS `r_serae`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `r_serae` (
  `SerAEID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'TrialIDRecipient',
  `DateOnset` date NOT NULL COMMENT 'Date of Onset (Date)',
  `SerialNumber` varchar(45) DEFAULT NULL COMMENT 'Sequential number of the Serious Adverse Event for the indivudal',
  `Ongoing` varchar(45) DEFAULT NULL COMMENT 'Still Ongoing (YES/NO)',
  `DateResolution` date DEFAULT NULL COMMENT 'Date of Resolution (Date)',
  `DescriptionEvent` varchar(500) DEFAULT NULL COMMENT 'Description of Event (Free Text)',
  `ActionTaken` varchar(500) DEFAULT NULL COMMENT 'Other Complications',
  `Outcome` varchar(500) DEFAULT NULL COMMENT 'Outcome (Free Text)',
  `ContactName` varchar(100) DEFAULT NULL COMMENT 'Name of the Contact Person',
  `ContactPhone` varchar(45) DEFAULT NULL COMMENT 'Phone Number of the Contact Person',
  `ContactEmail` varchar(100) DEFAULT NULL COMMENT 'Email of Contact Person',
  `OtherDetails` varchar(500) DEFAULT NULL COMMENT 'Is this AE serious YES/NO; Did it result in death YES/NO, Did it result in permanent disability YES/NO; Did it result in incapacity/inability to do work YES/NO; Did it result in a sign/symptom that interferes with subject’s usual activity YES/NO; Did it cause signs/symptoms that resolved with no sequelae YES/NO; Did this AE arise from device deficiency YES/NO; Did this AE arise from device user error YES/NO;',
  `ContactDetails` varchar(500) DEFAULT NULL COMMENT 'Name, Phone Number and Email of the person to Contact for this Serious Adverse Event',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the Table',
  PRIMARY KEY (`SerAEID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COMMENT='Serious Adverse Events';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_serae_AFTER_UPDATE` AFTER UPDATE ON `r_serae` FOR EACH ROW
 INSERT INTO copy_r_serae
(parentrow_action, SerAEID, TrialIDRecipient, DateOnset, SerialNumber, Ongoing, DateResolution, DescriptionEvent, ActionTaken, Outcome,
ContactName, ContactPhone, ContactEmail, OtherDetails, ContactDetails, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded)
VALUES
('update', OLD.SerAEID, OLD.TrialIDRecipient, OLD.DateOnset, OLD.SerialNumber, OLD.Ongoing, OLD.DateResolution, OLD.DescriptionEvent, OLD.ActionTaken, OLD.Outcome,
OLD.ContactName, OLD.ContactPhone, OLD.ContactEmail, OLD.OtherDetails, OLD.ContactDetails, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`r_serae_AFTER_DELETE` AFTER DELETE ON `r_serae` FOR EACH ROW
 INSERT INTO copy_r_serae
(parentrow_action, SerAEID, TrialIDRecipient, DateOnset, SerialNumber, Ongoing, DateResolution, DescriptionEvent, ActionTaken, Outcome,
ContactName, ContactPhone, ContactEmail, OtherDetails, ContactDetails, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded)
VALUES
('delete', OLD.SerAEID, OLD.TrialIDRecipient, OLD.DateOnset, OLD.SerialNumber, OLD.Ongoing, OLD.DateResolution, OLD.DescriptionEvent, OLD.ActionTaken, OLD.Outcome,
OLD.ContactName, OLD.ContactPhone, OLD.ContactEmail, OLD.OtherDetails, OLD.ContactDetails, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `resuse`
--

DROP TABLE IF EXISTS `resuse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `resuse` (
  `ResUseID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(45) NOT NULL COMMENT 'Trial ID for the Recipient',
  `DateEntered` date DEFAULT NULL COMMENT 'Date of Adverse Event (Date)',
  `Occasion` varchar(100) DEFAULT NULL COMMENT 'Occasion (Day 30/ Month 6)',
  `GPAppointment` varchar(100) DEFAULT NULL COMMENT 'GP Practice Appointment (0-20)',
  `GPHomeVisit` varchar(100) DEFAULT NULL COMMENT 'GP Home Visit (0-20)',
  `GPTelConversation` varchar(100) DEFAULT NULL COMMENT 'GP Telephone Conversation (0-20)',
  `SpecConsultantAppointment` varchar(100) DEFAULT NULL COMMENT 'Specialist/consultant appointment (0-20)',
  `AETreatment` varchar(100) DEFAULT NULL COMMENT 'Treated in A&E (0-20)',
  `AmbulanceAEVisit` varchar(100) DEFAULT NULL COMMENT 'Ambulance to A&E/hospital (0-20)',
  `NurseHomeVisit` varchar(100) DEFAULT NULL COMMENT 'Nurse Home Visit (0-20)',
  `NursePracticeAppointment` varchar(100) DEFAULT NULL COMMENT 'Nurse Practice Appointment (0-20)',
  `PhysiotherapistAppointment` varchar(100) DEFAULT NULL COMMENT 'Physiotherapist appointment (0-20)',
  `OccupationalTherapistAppointment` varchar(100) DEFAULT NULL COMMENT 'Occupational therapist appointment (0-20)',
  `PsychologistAppointment` varchar(100) DEFAULT NULL COMMENT 'Psychologist Appointment (0-20)',
  `CounsellorAppointment` varchar(100) DEFAULT NULL COMMENT 'Counsellor Appointment (0-20)',
  `AttendedDayHospital` varchar(100) DEFAULT NULL COMMENT 'Attended Day Hospital (0-20)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime NOT NULL COMMENT 'Date Time when the record was First Created',
  `CreatedBy` varchar(45) NOT NULL COMMENT 'Record Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when the record was Last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Record Last Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp for the Table',
  `EventCode` tinyint(4) DEFAULT NULL COMMENT '65 for Day 30 66 for Month 6',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`ResUseID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1 COMMENT='Resource Use';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`resuse_AFTER_UPDATE` AFTER UPDATE ON `resuse` FOR EACH ROW
 INSERT INTO copy_resuse
(parentrow_action, ResUseID, TrialIDRecipient, DateEntered, Occasion, GPAppointment, GPHomeVisit, GPTelConversation, SpecConsultantAppointment,
AETreatment, AmbulanceAEVisit, NurseHomeVisit, NursePracticeAppointment, PhysiotherapistAppointment, OccupationalTherapistAppointment,
PsychologistAppointment, CounsellorAppointment, AttendedDayHospital, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('update', OLD.ResUseID, OLD.TrialIDRecipient, OLD.DateEntered, OLD.Occasion, OLD.GPAppointment, OLD.GPHomeVisit, OLD.GPTelConversation, OLD.SpecConsultantAppointment,
OLD.AETreatment, OLD.AmbulanceAEVisit, OLD.NurseHomeVisit, OLD.NursePracticeAppointment, OLD.PhysiotherapistAppointment, OLD.OccupationalTherapistAppointment,
OLD.PsychologistAppointment, OLD.CounsellorAppointment, OLD.AttendedDayHospital, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`resuse_AFTER_DELETE` AFTER DELETE ON `resuse` FOR EACH ROW
 INSERT INTO copy_resuse
(parentrow_action, ResUseID, TrialIDRecipient, DateEntered, Occasion, GPAppointment, GPHomeVisit, GPTelConversation, SpecConsultantAppointment,
AETreatment, AmbulanceAEVisit, NurseHomeVisit, NursePracticeAppointment, PhysiotherapistAppointment, OccupationalTherapistAppointment,
PsychologistAppointment, CounsellorAppointment, AttendedDayHospital, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, EventCode, Source)
VALUES
('delete', OLD.ResUseID, OLD.TrialIDRecipient, OLD.DateEntered, OLD.Occasion, OLD.GPAppointment, OLD.GPHomeVisit, OLD.GPTelConversation, OLD.SpecConsultantAppointment,
OLD.AETreatment, OLD.AmbulanceAEVisit, OLD.NurseHomeVisit, OLD.NursePracticeAppointment, OLD.PhysiotherapistAppointment, OLD.OccupationalTherapistAppointment,
OLD.PsychologistAppointment, OLD.CounsellorAppointment, OLD.AttendedDayHospital, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `specimen`
--

DROP TABLE IF EXISTS `specimen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `specimen` (
  `SpecimenID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(15) DEFAULT NULL,
  `TrialIDRecipient` varchar(15) DEFAULT NULL,
  `Centre` tinyint(3) unsigned DEFAULT NULL COMMENT 'CentreCode',
  `Collected` tinyint(4) DEFAULT NULL COMMENT '0 No, 1 Yes',
  `Barcode` varchar(15) DEFAULT NULL COMMENT 'Unique Barcode',
  `SpecimenType` varchar(45) DEFAULT NULL COMMENT 'Serum, LHPST, K2E,WB, Citrate',
  `SpecimenTypeOther` varchar(45) DEFAULT NULL,
  `TissueSource` varchar(100) DEFAULT NULL COMMENT 'Left/ Right',
  `CollectedBy` varchar(100) DEFAULT NULL COMMENT 'Collected By',
  `CollectedByOther` varchar(100) DEFAULT NULL COMMENT 'If Other Collected By',
  `Occasion` varchar(100) DEFAULT NULL COMMENT 'Extra column to add when the sample was collected',
  `OccasionOther` varchar(100) DEFAULT NULL,
  `DateCollected` date DEFAULT NULL COMMENT 'Date Collected',
  `TimeCollected` time DEFAULT NULL COMMENT 'Time Collected',
  `DateCentrifugation` date DEFAULT NULL,
  `TimeCentrifugation` time DEFAULT NULL,
  `DateFrozen` date DEFAULT NULL COMMENT 'Date Frozen',
  `TimeFrozen` time DEFAULT NULL,
  `Protocol` varchar(45) DEFAULT NULL COMMENT 'Protocol',
  `State` varchar(45) DEFAULT NULL,
  `StateOther` varchar(45) DEFAULT NULL,
  `EstimatedVolume` decimal(10,2) DEFAULT NULL COMMENT 'Estimated Volume',
  `AliquoteNo` varchar(3) DEFAULT '1' COMMENT 'If more than One aliquote',
  `BoxType` varchar(10) DEFAULT NULL COMMENT 'BoxNumber 4 digit',
  `BoxState` varchar(3) DEFAULT NULL,
  `BoxOrder` varchar(3) DEFAULT NULL,
  `BoxDestination` varchar(2) DEFAULT NULL COMMENT 'S-Sheffield, C-Cambridge - Carryover from Protect',
  `BoxNumber` varchar(10) DEFAULT NULL COMMENT 'BoxNumber 4 digit',
  `Position` varchar(45) DEFAULT NULL COMMENT 'Position of Sample in the Box/Rack',
  `Status` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '0-Not Used, 1 - Processed Robot, 2-Data Marked as Deleted, 3- Destroyed, 4-Data Updated, 5-Perished, 10-Consumed',
  `Destroyed` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '0-No, 1 Yes',
  `CodeDestroyed` varchar(45) DEFAULT NULL,
  `CodeDestroyedOther` varchar(100) DEFAULT NULL,
  `DateDestroyed` date DEFAULT NULL COMMENT 'Date When the Sample Was Destroyed',
  `DestroyedBy` varchar(100) DEFAULT NULL COMMENT 'Peson who destroyed the samples',
  `DestroyedByAuto` varchar(45) DEFAULT NULL COMMENT 'User entering destroyed by data',
  `DestroyedByTimeStamp` datetime DEFAULT NULL,
  `NotDestroyedBy` varchar(100) DEFAULT NULL,
  `NotDestroyedByAuto` varchar(45) DEFAULT NULL,
  `NotDestroyedByTimestamp` datetime DEFAULT NULL,
  `ReasonDestroyed` varchar(300) DEFAULT NULL COMMENT 'Reason Specimen was Destroyed',
  `FreezeThaw` int(10) unsigned DEFAULT NULL COMMENT 'Number of freeze thaw cycles, default 0',
  `FreezeThawUnknown` varchar(45) DEFAULT NULL COMMENT 'if freeze thaw numbers are not accurate this column is used, e.g. 2+',
  `ManualProcessed` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '1 Yes was Processed by Hand',
  `RobotProcessed` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT '1 Was Processed by Robot',
  `Consumed` tinyint(3) unsigned DEFAULT '0' COMMENT '0-No, 1 Yes',
  `PageVersion` varchar(45) DEFAULT NULL COMMENT 'To keep track of the versions of page',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `EventCode` varchar(45) NOT NULL DEFAULT '50' COMMENT 'default eventcode for samples',
  `Source` tinyint(3) unsigned DEFAULT NULL,
  PRIMARY KEY (`SpecimenID`),
  KEY `Index_Barcode` (`Barcode`),
  KEY `Index_TrialID` (`TrialID`),
  KEY `Index_TrialIDRecipient` (`TrialIDRecipient`),
  KEY `Index_DateCollectedl` (`DateCollected`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=latin1 COMMENT='Specimen Data ';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`specimen_AFTER_UPDATE` AFTER UPDATE ON `specimen` FOR EACH ROW
 INSERT INTO copy_specimen
(parentrow_action, SpecimenID, TrialID, TrialIDRecipient, Centre, Collected, Barcode, SpecimenType, SpecimenTypeOther, TissueSource,
CollectedBy, CollectedByOther, Occasion, OccasionOther, DateCollected, TimeCollected, DateCentrifugation, TimeCentrifugation,
DateFrozen, TimeFrozen, Protocol, State, StateOther, EstimatedVolume, AliquoteNo, BoxType, BoxState, BoxOrder, BoxDestination,
BoxNumber, Position, Status, Destroyed, CodeDestroyed, CodeDestroyedOther, DateDestroyed, DestroyedBy, DestroyedByAuto,
DestroyedByTimeStamp, NotDestroyedBy, NotDestroyedByAuto, NotDestroyedByTimestamp, ReasonDestroyed, FreezeThaw,
FreezeThawUnknown, ManualProcessed, RobotProcessed, Consumed, PageVersion,
DateAdded, DateCreated, CreatedBy, DateUpdated, UpdatedBy, Comments, EventCode, Source)
VALUES
('update', OLD.SpecimenID, OLD.TrialID, OLD.TrialIDRecipient, OLD.Centre, OLD.Collected, OLD.Barcode, OLD.SpecimenType, OLD.SpecimenTypeOther, OLD.TissueSource,
OLD.CollectedBy, OLD.CollectedByOther, OLD.Occasion, OLD.OccasionOther, OLD.DateCollected, OLD.TimeCollected, OLD.DateCentrifugation, OLD.TimeCentrifugation,
OLD.DateFrozen, OLD.TimeFrozen, OLD.Protocol, OLD.State, OLD.StateOther, OLD.EstimatedVolume, OLD.AliquoteNo, OLD.BoxType, OLD.BoxState, OLD.BoxOrder, OLD.BoxDestination,
OLD.BoxNumber, OLD.Position, OLD.Status, OLD.Destroyed, OLD.CodeDestroyed, OLD.CodeDestroyedOther, OLD.DateDestroyed, OLD.DestroyedBy, OLD.DestroyedByAuto,
OLD.DestroyedByTimeStamp, OLD.NotDestroyedBy, OLD.NotDestroyedByAuto, OLD.NotDestroyedByTimestamp, OLD.ReasonDestroyed, OLD.FreezeThaw,
OLD.FreezeThawUnknown, OLD.ManualProcessed, OLD.RobotProcessed, OLD.Consumed, OLD.PageVersion,
OLD.DateAdded, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.Comments, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`specimen_AFTER_DELETE` AFTER DELETE ON `specimen` FOR EACH ROW
INSERT INTO copy_specimen
(parentrow_action, SpecimenID, TrialID, TrialIDRecipient, Centre, Collected, Barcode, SpecimenType, SpecimenTypeOther, TissueSource,
CollectedBy, CollectedByOther, Occasion, OccasionOther, DateCollected, TimeCollected, DateCentrifugation, TimeCentrifugation,
DateFrozen, TimeFrozen, Protocol, State, StateOther, EstimatedVolume, AliquoteNo, BoxType, BoxState, BoxOrder, BoxDestination,
BoxNumber, Position, Status, Destroyed, CodeDestroyed, CodeDestroyedOther, DateDestroyed, DestroyedBy, DestroyedByAuto,
DestroyedByTimeStamp, NotDestroyedBy, NotDestroyedByAuto, NotDestroyedByTimestamp, ReasonDestroyed, FreezeThaw,
FreezeThawUnknown, ManualProcessed, RobotProcessed, Consumed, PageVersion,
DateAdded, DateCreated, CreatedBy, DateUpdated, UpdatedBy, Comments, EventCode, Source)
VALUES
('delete', OLD.SpecimenID, OLD.TrialID, OLD.TrialIDRecipient, OLD.Centre, OLD.Collected, OLD.Barcode, OLD.SpecimenType, OLD.SpecimenTypeOther, OLD.TissueSource,
OLD.CollectedBy, OLD.CollectedByOther, OLD.Occasion, OLD.OccasionOther, OLD.DateCollected, OLD.TimeCollected, OLD.DateCentrifugation, OLD.TimeCentrifugation,
OLD.DateFrozen, OLD.TimeFrozen, OLD.Protocol, OLD.State, OLD.StateOther, OLD.EstimatedVolume, OLD.AliquoteNo, OLD.BoxType, OLD.BoxState, OLD.BoxOrder, OLD.BoxDestination,
OLD.BoxNumber, OLD.Position, OLD.Status, OLD.Destroyed, OLD.CodeDestroyed, OLD.CodeDestroyedOther, OLD.DateDestroyed, OLD.DestroyedBy, OLD.DestroyedByAuto,
OLD.DestroyedByTimeStamp, OLD.NotDestroyedBy, OLD.NotDestroyedByAuto, OLD.NotDestroyedByTimestamp, OLD.ReasonDestroyed, OLD.FreezeThaw,
OLD.FreezeThawUnknown, OLD.ManualProcessed, OLD.RobotProcessed, OLD.Consumed, OLD.PageVersion,
OLD.DateAdded, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.Comments, OLD.EventCode, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `specimenmaindetails`
--

DROP TABLE IF EXISTS `specimenmaindetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `specimenmaindetails` (
  `SpecimenMainDetailsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(15) DEFAULT NULL,
  `TrialIDRecipient` varchar(15) DEFAULT NULL,
  `ConsentQuod` varchar(45) DEFAULT NULL COMMENT 'changed to If the Donor is included in recipient',
  `ConsentAdditionalSamples` varchar(45) DEFAULT NULL COMMENT 'If Recipient Consent for Additional Samples (YES/NO)',
  `WorksheetBarcode` varchar(45) DEFAULT NULL COMMENT 'Barcode on the Worksheet',
  `ResearcherName` varchar(100) DEFAULT NULL COMMENT 'name of Researcher',
  `ResearcherTelephoneNumber` varchar(45) DEFAULT NULL COMMENT 'Telephone Number of Researcher',
  `SamplesStoredOxfordDate` date DEFAULT NULL COMMENT 'Date Sample stored in -80 freezer oxford',
  `SamplesStoredOxfordTime` time DEFAULT NULL COMMENT 'Time Sample stored in -80 freezer oxford',
  `InitiationNMPTime` time DEFAULT NULL COMMENT 'Time Initiation of normothermic machine preservation Time of placement of liver on NMP device (Time)',
  `LiverRemovedMachineTime` time DEFAULT NULL COMMENT 'Time If NMP Liver removed from machine Time of removal of liver from NMP device  (hh:mm)',
  `ReperfusionTime` time DEFAULT NULL COMMENT 'Time of reperfusion   (hh:mm)',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Comments',
  `AllDataAdded` varchar(45) DEFAULT NULL COMMENT 'if all data has been added and Lock routine executed',
  `AllDataAddedLogs` varchar(5000) DEFAULT NULL COMMENT 'Track if AllDataAdded status is changed',
  `DataLocked` tinyint(3) unsigned DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'if all data has been added and Lock routine executed',
  `ReasonModified` varchar(10000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(3) unsigned DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `DateUpdated` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Source` tinyint(4) DEFAULT '0' COMMENT 'where the data has come from 0 is if entered in the server directly',
  `EventCode` varchar(45) DEFAULT NULL COMMENT 'default eventcode for samples',
  PRIMARY KEY (`SpecimenMainDetailsID`),
  KEY `Index_TrialID` (`TrialID`),
  KEY `Index_WorksheetBarcode` (`WorksheetBarcode`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COMMENT='Specimen Main Details';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`specimenmaindetails_AFTER_UPDATE` AFTER UPDATE ON `specimenmaindetails` FOR EACH ROW
 INSERT INTO copy_specimenmaindetails
(parentrow_action, SpecimenMainDetailsID, TrialID, TrialIDRecipient, ConsentQuod, ConsentAdditionalSamples, WorksheetBarcode,
ResearcherName, ResearcherTelephoneNumber, SamplesStoredOxfordDate, SamplesStoredOxfordTime, InitiationNMPTime, LiverRemovedMachineTime,
ReperfusionTime, Comments, AllDataAdded, AllDataAddedLogs, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source, EventCode)
VALUES
('update', OLD.SpecimenMainDetailsID, OLD.TrialID, OLD.TrialIDRecipient, OLD.ConsentQuod, OLD.ConsentAdditionalSamples, OLD.WorksheetBarcode,
OLD.ResearcherName, OLD.ResearcherTelephoneNumber, OLD.SamplesStoredOxfordDate, OLD.SamplesStoredOxfordTime, OLD.InitiationNMPTime, OLD.LiverRemovedMachineTime,
OLD.ReperfusionTime, OLD.Comments, OLD.AllDataAdded, OLD.AllDataAddedLogs, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source, OLD.EventCode) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`specimenmaindetails_AFTER_DELETE` AFTER DELETE ON `specimenmaindetails` FOR EACH ROW
INSERT INTO copy_specimenmaindetails
(parentrow_action, SpecimenMainDetailsID, TrialID, TrialIDRecipient, ConsentQuod, ConsentAdditionalSamples, WorksheetBarcode,
ResearcherName, ResearcherTelephoneNumber, SamplesStoredOxfordDate, SamplesStoredOxfordTime, InitiationNMPTime, LiverRemovedMachineTime,
ReperfusionTime, Comments, AllDataAdded, AllDataAddedLogs, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source, EventCode)
VALUES
('delete', OLD.SpecimenMainDetailsID, OLD.TrialID, OLD.TrialIDRecipient, OLD.ConsentQuod, OLD.ConsentAdditionalSamples, OLD.WorksheetBarcode,
OLD.ResearcherName, OLD.ResearcherTelephoneNumber, OLD.SamplesStoredOxfordDate, OLD.SamplesStoredOxfordTime, OLD.InitiationNMPTime, OLD.LiverRemovedMachineTime,
OLD.ReperfusionTime, OLD.Comments, OLD.AllDataAdded, OLD.AllDataAddedLogs, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source, OLD.EventCode) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `tblfileuploads`
--

DROP TABLE IF EXISTS `tblfileuploads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tblfileuploads` (
  `RowIndex` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(45) NOT NULL COMMENT 'Unique TrialID',
  `Side` varchar(45) NOT NULL COMMENT 'Side of Kidney',
  `FileName` varchar(100) DEFAULT NULL COMMENT 'File Name',
  `IPAddress` varchar(100) DEFAULT NULL COMMENT 'IP address from where it was uploaded.',
  `Comments` varchar(500) DEFAULT NULL COMMENT 'Free Text',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime when file was uploaded to the server',
  `CreatedBy` varchar(100) DEFAULT NULL COMMENT 'User who uplaoded the file',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'Date Time when Keyword/Description was last updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'User who updated Keyword/Description',
  `IPAddressUpdatedBy` varchar(100) DEFAULT NULL,
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Timestamp',
  `Source` tinyint(4) DEFAULT '0' COMMENT 'where the data has come from 0 is if entered in the server directly',
  PRIMARY KEY (`RowIndex`),
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`FileName`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COMMENT='Perfusion File Upload';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`tblfileuploads_AFTER_UPDATE` AFTER UPDATE ON `tblfileuploads` FOR EACH ROW
 INSERT INTO copy_tblfileuploads
(parentrow_action, RowIndex, TrialID, Side, FileName, IPAddress, Comments,
DateCreated, CreatedBy, DateUpdated, UpdatedBy, IPAddressUpdatedBy, DateAdded, Source)
VALUES
('update', OLD.RowIndex, OLD.TrialID, OLD.Side, OLD.FileName, OLD.IPAddress, OLD.Comments,
OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.IPAddressUpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`tblfileuploads_AFTER_DELETE` AFTER DELETE ON `tblfileuploads` FOR EACH ROW
INSERT INTO copy_tblfileuploads
(parentrow_action, RowIndex, TrialID, Side, FileName, IPAddress, Comments,
DateCreated, CreatedBy, DateUpdated, UpdatedBy, IPAddressUpdatedBy, DateAdded, Source)
VALUES
('delete', OLD.RowIndex, OLD.TrialID, OLD.Side, OLD.FileName, OLD.IPAddress, OLD.Comments,
OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.IPAddressUpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `trialdetails`
--

DROP TABLE IF EXISTS `trialdetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `trialdetails` (
  `TrialDetailsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(10) NOT NULL COMMENT 'Unique ID for Kidney being transplanted',
  `CentreCode` varchar(45) DEFAULT NULL COMMENT 'Centre where donor was added',
  `DonorID` varchar(45) DEFAULT NULL COMMENT 'Unique ID of Donor ACCC (A is country code, CCC running number',
  `AgeOrDateOfBirth` varchar(45) DEFAULT NULL COMMENT 'If Age selected or Date Of Birth selected',
  `DonorAge` varchar(45) DEFAULT NULL COMMENT 'Age of Donor',
  `DateOfBirthDonor` date DEFAULT NULL COMMENT 'Unique ID of Donor',
  `DonorAccept` enum('YES','NO') DEFAULT NULL COMMENT 'Check if accepted',
  `KidneySide` enum('Left','Right') DEFAULT NULL COMMENT 'Check if accepted',
  `RanCategory` varchar(45) DEFAULT NULL COMMENT 'Randomised To',
  `WP3RandomID` int(10) unsigned DEFAULT NULL COMMENT 'Unique Identifier from Randomisation Table',
  `Active` tinyint(3) unsigned NOT NULL DEFAULT '1' COMMENT '0 Inactive, 1 Active',
  `Comments` varchar(500) DEFAULT NULL,
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Time stamp',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`TrialDetailsID`) USING BTREE,
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`DonorID`)
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=latin1 COMMENT='Main Details';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`trialdetails_AFTER_UPDATE` AFTER UPDATE ON `trialdetails` FOR EACH ROW
 INSERT INTO copy_trialdetails
(parentrow_action, TrialDetailsID, TrialID, CentreCode, DonorID, AgeOrDateOfBirth, DonorAge,
DateOfBirthDonor, DonorAccept, KidneySide, RanCategory, WP3RandomID, Active,
Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('update', OLD.TrialDetailsID, OLD.TrialID, OLD.CentreCode, OLD.DonorID, OLD.AgeOrDateOfBirth, OLD.DonorAge,
OLD.DateOfBirthDonor, OLD.DonorAccept, OLD.KidneySide, OLD.RanCategory, OLD.WP3RandomID, OLD.Active, OLD.Comments,
OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`trialdetails_AFTER_DELETE` AFTER DELETE ON `trialdetails` FOR EACH ROW
INSERT INTO copy_trialdetails
(parentrow_action, TrialDetailsID, TrialID, CentreCode, DonorID, AgeOrDateOfBirth, DonorAge,
DateOfBirthDonor, DonorAccept, KidneySide, RanCategory, WP3RandomID, Active,
Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('delete', OLD.TrialDetailsID, OLD.TrialID, OLD.CentreCode, OLD.DonorID, OLD.AgeOrDateOfBirth, OLD.DonorAge,
OLD.DateOfBirthDonor, OLD.DonorAccept, OLD.KidneySide, OLD.RanCategory, OLD.WP3RandomID, OLD.Active, OLD.Comments,
OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `trialdetails_recipient`
--

DROP TABLE IF EXISTS `trialdetails_recipient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `trialdetails_recipient` (
  `TrialDetails_RecipientID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialID` varchar(10) NOT NULL COMMENT 'Unique ID for Kidney being transplanted',
  `KidneyReceived` varchar(45) DEFAULT NULL COMMENT 'Kidney received (LEFT/RIGHT)',
  `TrialIDRecipient` varchar(45) DEFAULT NULL,
  `TransplantCentre` varchar(100) DEFAULT NULL COMMENT 'Name of recipient transplantation center (Free  Text)',
  `RecipientInformedConsent` varchar(45) DEFAULT NULL COMMENT 'Has the recipient signed an informed consent form? (YES/NO)',
  `Recipient18Year` varchar(45) DEFAULT NULL COMMENT 'Is the recipient >18y old? (YES/NO)',
  `RecipientMultipleDualTransplant` varchar(45) DEFAULT NULL COMMENT 'Recipient will not undergo a multiple/dual transplant (YES/NO)',
  `Active` tinyint(3) unsigned NOT NULL DEFAULT '1' COMMENT '0 Inactive, 1 Active',
  `Comments` varchar(500) DEFAULT NULL,
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT 'If Data Complte 0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'User Locking the data/marking as complete',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT NULL COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User marking the data as Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Time stamp',
  `Source` tinyint(3) DEFAULT '0',
  PRIMARY KEY (`TrialDetails_RecipientID`) USING BTREE,
  KEY `Index_2` (`TrialID`),
  KEY `Index_3` (`TrialIDRecipient`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COMMENT='Main Details';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`trialdetails_recipient_AFTER_UPDATE` AFTER UPDATE ON `trialdetails_recipient` FOR EACH ROW
 INSERT INTO copy_trialdetails_recipient
(parentrow_action, TrialDetails_RecipientID, TrialID, KidneyReceived, TrialIDRecipient, TransplantCentre, RecipientInformedConsent,
Recipient18Year, RecipientMultipleDualTransplant, Active, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('update', OLD.TrialDetails_RecipientID, OLD.TrialID, OLD.KidneyReceived, OLD.TrialIDRecipient, OLD.TransplantCentre, OLD.RecipientInformedConsent,
OLD.Recipient18Year, OLD.RecipientMultipleDualTransplant, OLD.Active, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`trialdetails_recipient_AFTER_DELETE` AFTER DELETE ON `trialdetails_recipient` FOR EACH ROW
 INSERT INTO copy_trialdetails_recipient
(parentrow_action, TrialDetails_RecipientID, TrialID, KidneyReceived, TrialIDRecipient, TransplantCentre, RecipientInformedConsent,
Recipient18Year, RecipientMultipleDualTransplant, Active, Comments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('delete', OLD.TrialDetails_RecipientID, OLD.TrialID, OLD.KidneyReceived, OLD.TrialIDRecipient, OLD.TransplantCentre, OLD.RecipientInformedConsent,
OLD.Recipient18Year, OLD.RecipientMultipleDualTransplant, OLD.Active, OLD.Comments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `trialidwithdrawn`
--

DROP TABLE IF EXISTS `trialidwithdrawn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `trialidwithdrawn` (
  `TrialIDWithdrawnID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `TrialIDRecipient` varchar(10) NOT NULL COMMENT 'Unique ID for Kidney being transplanted (WP3ABCCC -A is country code, B Centre number, where the Transplant is done, CCC running number)',
  `DateWithdrawn` date DEFAULT NULL COMMENT 'If Withdrawn, Date Withdrawn from the Study',
  `ReasonWithdrawn` varchar(100) DEFAULT NULL COMMENT 'Reason person withdrew from teh Study',
  `ReasonWithdrawnOther` varchar(100) DEFAULT NULL COMMENT 'If Reason Withdrawn is Other',
  `WithdrawnComments` varchar(500) DEFAULT NULL COMMENT 'Comments Withdrawn',
  `DataLocked` tinyint(4) DEFAULT '0' COMMENT '0-No, 1- Yes',
  `DateLocked` datetime DEFAULT NULL COMMENT 'Date Time when the record was Locked',
  `LockedBy` varchar(45) DEFAULT NULL COMMENT 'Person who locked the record',
  `ReasonModified` varchar(1000) DEFAULT NULL COMMENT 'Reasons data modified',
  `DataFinal` tinyint(4) DEFAULT '0' COMMENT 'Data assigned as Final 0 No 1 Yes',
  `DateFinal` datetime DEFAULT NULL COMMENT 'Date Time Data assigned as Final',
  `FinalAssignedBy` varchar(45) DEFAULT NULL COMMENT 'User Assigning Data As Final',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Time stamp',
  `Source` varchar(45) DEFAULT '0',
  PRIMARY KEY (`TrialIDWithdrawnID`) USING BTREE,
  KEY `Index_2` (`TrialIDRecipient`),
  KEY `Index_3` (`DateWithdrawn`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1 COMMENT='Withdrawn details';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`trialidwithdrawn_AFTER_UPDATE` AFTER UPDATE ON `trialidwithdrawn` FOR EACH ROW
  INSERT INTO copy_trialidwithdrawn
(parentrow_action, TrialIDWithdrawnID, TrialIDRecipient, DateWithdrawn, ReasonWithdrawn, ReasonWithdrawnOther,
WithdrawnComments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('update', OLD.TrialIDWithdrawnID, OLD.TrialIDRecipient, OLD.DateWithdrawn, OLD.ReasonWithdrawn, OLD.ReasonWithdrawnOther,
OLD.WithdrawnComments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cope_wp_four`.`trialidwithdrawn_AFTER_DELETE` AFTER DELETE ON `trialidwithdrawn` FOR EACH ROW
  INSERT INTO copy_trialidwithdrawn
(parentrow_action, TrialIDWithdrawnID, TrialIDRecipient, DateWithdrawn, ReasonWithdrawn, ReasonWithdrawnOther,
WithdrawnComments, DataLocked, DateLocked, LockedBy, ReasonModified,
DataFinal, DateFinal, FinalAssignedBy, DateCreated, CreatedBy, DateUpdated, UpdatedBy, DateAdded, Source)
VALUES
('delete', OLD.TrialIDWithdrawnID, OLD.TrialIDRecipient, OLD.DateWithdrawn, OLD.ReasonWithdrawn, OLD.ReasonWithdrawnOther,
OLD.WithdrawnComments, OLD.DataLocked, OLD.DateLocked, OLD.LockedBy, OLD.ReasonModified,
OLD.DataFinal, OLD.DateFinal, OLD.FinalAssignedBy, OLD.DateCreated, OLD.CreatedBy, OLD.DateUpdated, OLD.UpdatedBy, OLD.DateAdded, OLD.Source) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Dumping events for database 'cope_wp_four'
--

--
-- Dumping routines for database 'cope_wp_four'
--

--
-- Current Database: `cope_wpfour_random`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `cope_wpfour_random` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `cope_wpfour_random`;

--
-- Table structure for table `wp4_final`
--

DROP TABLE IF EXISTS `wp4_final`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `wp4_final` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `identifier` int(11) DEFAULT NULL,
  `StratID` int(11) DEFAULT NULL,
  `Country` int(11) DEFAULT NULL,
  `centreName` varchar(255) DEFAULT NULL,
  `block` int(11) DEFAULT NULL,
  `size` int(11) DEFAULT NULL,
  `SeqInBlk` int(11) DEFAULT NULL,
  `treat` varchar(255) DEFAULT NULL,
  `AllocationLeft` varchar(255) DEFAULT NULL,
  `AllocationRight` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `Index_2` (`identifier`)
) ENGINE=InnoDB AUTO_INCREMENT=455 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `wpfour_random`
--

DROP TABLE IF EXISTS `wpfour_random`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `wpfour_random` (
  `WPFour_RandomID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `RanTreat` varchar(45) DEFAULT NULL COMMENT 'from randomisation list sent by Virginia.',
  `BlockCodeLeft` varchar(1) NOT NULL COMMENT 'Block Code Left Kidney. A, B, A - HMP with oxygen , B - HMP without oxygen ',
  `TreatmentLeft` varchar(100) DEFAULT NULL,
  `BlockCodeRight` varchar(1) NOT NULL COMMENT 'Block Code Right Kidney. A, B , A - HMP with oxygen , B - HMP without oxygen ',
  `TreatmentRight` varchar(100) DEFAULT NULL,
  `CountryCode` varchar(45) NOT NULL COMMENT 'CountryCode',
  `CountryName` varchar(100) DEFAULT NULL COMMENT 'Country Name',
  `TrialID` varchar(45) DEFAULT NULL COMMENT 'Unique TrialID',
  `DonorID` varchar(100) DEFAULT NULL,
  `DonorDateOfBirth` date DEFAULT NULL COMMENT 'Date of Birth of the Donor',
  `DonorCentre` varchar(45) DEFAULT NULL COMMENT 'Centre Code where the Donor was randmosied',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Time stamp',
  PRIMARY KEY (`WPFour_RandomID`),
  KEY `Index_2` (`TrialID`)
) ENGINE=InnoDB AUTO_INCREMENT=449 DEFAULT CHARSET=latin1 COMMENT='Randomisation List';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER wpfour_random_added 
AFTER INSERT ON wpfour_random
FOR EACH ROW 

  INSERT INTO wpfour_random_copy (`parentrow_action`,    `WPFour_RandomID`,    `BlockCodeLeft`,    `BlockCodeRight`,
    `CountryCode`,    `TrialID`,   `DonorID`,  `DonorDateOfBirth`, `DonorCentre`,  
`DateCreated`,    `CreatedBy`,    `DateUpdated`,    `UpdatedBy`,    `DateAdded`)
  VALUES ('create',NEW.`WPFour_RandomID`,    NEW.`BlockCodeLeft`,    NEW.`BlockCodeRight`,
    NEW.`CountryCode`,    NEW.`TrialID`,  NEW.`DonorID`,   NEW.`DonorDateOfBirth`,  NEW.`DonorCentre`,  
NEW.`DateCreated`,    NEW.`CreatedBy`,    NEW.`DateUpdated`,    NEW.`UpdatedBy`,    NEW.`DateAdded`) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER wpfour_random_update  AFTER UPDATE ON wpfour_random 
FOR EACH ROW     INSERT INTO wpfour_random_copy 
(`parentrow_action`,    `WPFour_RandomID`,    `BlockCodeLeft`,    `BlockCodeRight`,     `CountryCode`,    
`TrialID`, `DonorID`,  `DonorDateOfBirth`, `DonorCentre`,   
`DateCreated`,    `CreatedBy`,    `DateUpdated`,    `UpdatedBy`,    `DateAdded`)   
VALUES 
('update',NEW.`WPFour_RandomID`,    NEW.`BlockCodeLeft`,    NEW.`BlockCodeRight`,     NEW.`CountryCode`,    
NEW.`TrialID`,  NEW.`DonorID`,   NEW.`DonorDateOfBirth`,  NEW.`DonorCentre`,  
NEW.`DateCreated`,    NEW.`CreatedBy`,    NEW.`DateUpdated`,    NEW.`UpdatedBy`,    NEW.`DateAdded`) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `wpfour_random_delete` AFTER DELETE ON `wpfour_random` 
FOR EACH ROW
INSERT INTO wpfour_random_copy (`parentrow_action`,    `WPFour_RandomID`,    `BlockCodeLeft`,    `BlockCodeRight`,
    `CountryCode`,    `TrialID`,   `DonorID`,  `DonorDateOfBirth`, `DonorCentre`,  
`DateCreated`,    `CreatedBy`,    `DateUpdated`,    `UpdatedBy`,    `DateAdded`)
  VALUES ('create',OLD.`WPFour_RandomID`,    OLD.`BlockCodeLeft`,    OLD.`BlockCodeRight`,
    OLD.`CountryCode`,    OLD.`TrialID`,  OLD.`DonorID`,   OLD.`DonorDateOfBirth`,  OLD.`DonorCentre`,  
OLD.`DateCreated`,    OLD.`CreatedBy`,    OLD.`DateUpdated`,    OLD.`UpdatedBy`,    OLD.`DateAdded`) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `wpfour_random_copy`
--

DROP TABLE IF EXISTS `wpfour_random_copy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `wpfour_random_copy` (
  `parentrow_action` enum('create','update','delete') DEFAULT NULL,
  `CurrentTimeStamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `WPFour_RandomID` int(10) unsigned NOT NULL,
  `BlockCodeLeft` varchar(1) NOT NULL COMMENT 'Block Code Left Kidney. A, B, A - HMP with oxygen , B - HMP without oxygen ',
  `BlockCodeRight` varchar(1) NOT NULL COMMENT 'Block Code Right Kidney. A, B , A - HMP with oxygen , B - HMP without oxygen ',
  `CountryCode` varchar(45) NOT NULL COMMENT 'CountryCode',
  `TrialID` varchar(45) DEFAULT NULL COMMENT 'Unique TrialID',
  `DonorID` varchar(100) DEFAULT NULL,
  `DonorDateOfBirth` date DEFAULT NULL COMMENT 'Date of Birth of the Donor',
  `DonorCentre` varchar(45) DEFAULT NULL COMMENT 'Centre Code where the Donor was randmosied',
  `DateCreated` datetime DEFAULT NULL COMMENT 'DateTime Created',
  `CreatedBy` varchar(45) DEFAULT NULL COMMENT 'Created By',
  `DateUpdated` datetime DEFAULT NULL COMMENT 'DateTime Updated',
  `UpdatedBy` varchar(45) DEFAULT NULL COMMENT 'Updated By',
  `DateAdded` datetime DEFAULT NULL COMMENT 'Time stamp',
  KEY `Index_1` (`WPFour_RandomID`),
  KEY `Index_2` (`TrialID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Randomisation List Insert/Update/Delete/Log';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'cope_wpfour_random'
--

--
-- Dumping routines for database 'cope_wpfour_random'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-02-12 10:53:19
