CREATE DATABASE  IF NOT EXISTS `gasystemdb` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `gasystemdb`;
-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: gasystemdb
-- ------------------------------------------------------
-- Server version	5.7.16-log

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
-- Table structure for table `gspeed`
--

DROP TABLE IF EXISTS `gspeed`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gspeed` (
  `gspeed_id` int(11) NOT NULL AUTO_INCREMENT,
  `gender` char(1) DEFAULT NULL,
  `age_lower` int(2) DEFAULT NULL,
  `age_upper` int(2) DEFAULT NULL,
  `ci_lower` decimal(5,1) DEFAULT NULL,
  `ci_upper` decimal(5,1) DEFAULT NULL,
  `pi_lower` decimal(5,1) DEFAULT NULL,
  `pi_upper` decimal(5,1) DEFAULT NULL,
  PRIMARY KEY (`gspeed_id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gspeed`
--

LOCK TABLES `gspeed` WRITE;
/*!40000 ALTER TABLE `gspeed` DISABLE KEYS */;
INSERT INTO `gspeed` VALUES (1,'M',10,14,119.9,144.7,89.2,175.4),(2,'M',15,19,127.5,142.7,105.8,164.4),(3,'M',20,29,116.7,128.7,94.4,146.0),(4,'M',30,39,123.5,139.7,100.1,163.1),(5,'M',40,49,127.5,138.1,112.2,153.4),(6,'M',50,59,115.6,134.8,88.0,162.4),(7,'M',60,69,121.0,134.4,101.7,153.7),(8,'M',70,79,109.8,126.6,85.9,150.5),(9,'F',10,14,101.5,115.7,84.0,133.2),(10,'F',15,19,114.0,133.8,85.4,162.4),(11,'F',20,29,114.8,133.4,88.2,160.0),(12,'F',30,39,118.1,138.9,88.4,168.6),(13,'F',40,49,116.9,132.5,94.5,154.9),(14,'F',50,59,105.2,115.8,90.1,130.9),(15,'F',60,69,106.6,124.8,80.6,150.8),(16,'F',70,79,104.5,118.1,85.1,137.6);
/*!40000 ALTER TABLE `gspeed` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gstepfreq`
--

DROP TABLE IF EXISTS `gstepfreq`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gstepfreq` (
  `gstepfreq_id` int(11) NOT NULL,
  `gender` char(1) DEFAULT NULL,
  `age_lower` int(2) DEFAULT NULL,
  `age_upper` int(2) DEFAULT NULL,
  `ci_lower` decimal(5,2) DEFAULT NULL,
  `ci_upper` decimal(5,2) DEFAULT NULL,
  `pi_lower` decimal(5,2) DEFAULT NULL,
  `pi_upper` decimal(5,2) DEFAULT NULL,
  PRIMARY KEY (`gstepfreq_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gstepfreq`
--

LOCK TABLES `gstepfreq` WRITE;
/*!40000 ALTER TABLE `gstepfreq` DISABLE KEYS */;
INSERT INTO `gstepfreq` VALUES (1,'M',10,14,2.02,2.26,1.72,2.56),(2,'M',15,19,1.91,2.13,1.58,2.46),(3,'M',20,29,1.91,2.05,1.71,2.25),(4,'M',30,39,1.92,2.08,1.71,2.29),(5,'M',40,49,1.95,2.07,1.78,2.24),(6,'M',50,59,1.86,2.06,1.58,2.34),(7,'M',60,69,1.87,2.03,1.66,2.24),(8,'M',70,79,1.83,1.99,1.62,2.20),(9,'F',10,14,1.86,2.08,1.60,2.34),(10,'F',15,19,1.99,2.19,1.69,2.49),(11,'F',20,29,2.00,2.16,1.77,2.40),(12,'F',30,39,2.04,2.22,1.77,2.49),(13,'F',40,49,2.07,2.25,1.82,2.50),(14,'F',50,59,1.96,2.10,1.76,2.30),(15,'F',60,69,1.96,2.16,1.68,2.44),(16,'F',70,79,1.95,2.11,1.74,2.32);
/*!40000 ALTER TABLE `gstepfreq` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gsteplen`
--

DROP TABLE IF EXISTS `gsteplen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gsteplen` (
  `gsteplen_id` int(11) NOT NULL,
  `gender` char(1) DEFAULT NULL,
  `age_lower` int(2) DEFAULT NULL,
  `age_upper` int(2) DEFAULT NULL,
  `ci_lower` decimal(5,1) DEFAULT NULL,
  `ci_upper` decimal(5,1) DEFAULT NULL,
  `pi_lower` decimal(5,1) DEFAULT NULL,
  `pi_upper` decimal(5,1) DEFAULT NULL,
  PRIMARY KEY (`gsteplen_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gsteplen`
--

LOCK TABLES `gsteplen` WRITE;
/*!40000 ALTER TABLE `gsteplen` DISABLE KEYS */;
INSERT INTO `gsteplen` VALUES (1,'M',10,14,59.0,64.0,52.9,70.1),(2,'M',15,19,63.3,68.7,55.4,76.6),(3,'M',20,29,59.7,63.5,54.3,69.0),(4,'M',30,39,62.4,67.4,55.2,74.6),(5,'M',40,49,62.7,66.7,56.9,72.5),(6,'M',50,59,60.2,66.8,50.9,76.1),(7,'M',60,69,63.0,67.0,57.4,72.6),(8,'M',70,79,58.6,64.4,60.8,72.2),(9,'F',10,14,52.4,56.0,47.8,60.6),(10,'F',15,19,56.9,61.7,49.8,68.8),(11,'F',20,29,55.7,62.5,45.9,72.3),(12,'F',30,39,56.8,62.6,48.6,70.8),(13,'F',40,49,55.1,59.1,49.3,64.9),(14,'F',50,59,52.1,54.9,48.0,59.0),(15,'F',60,69,53.0,57.6,46.5,64.1),(16,'F',70,79,52.2,56.2,46.4,62.0);
/*!40000 ALTER TABLE `gsteplen` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-11-01 22:45:30
