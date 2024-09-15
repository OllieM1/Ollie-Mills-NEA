-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 02, 2024 at 11:51 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `school_sign_in_data`
--

-- --------------------------------------------------------

--
-- Table structure for table `timeofscan`
--

CREATE TABLE `timeofscan` (
  `ID` int(11) NOT NULL,
  `DayOfScan` date NOT NULL,
  `TimeOfScan` time NOT NULL,
  `UserID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `timeofscan`
--

INSERT INTO `timeofscan` (`ID`, `DayOfScan`, `TimeOfScan`, `UserID`) VALUES
(1, '2024-08-29', '14:29:56', 1),
(2, '2024-08-29', '14:30:01', 1),
(3, '2024-08-29', '14:30:06', 1),
(4, '2024-08-29', '14:30:11', 1),
(5, '2024-08-29', '14:30:16', 1),
(6, '2024-08-29', '14:30:21', 1),
(7, '2024-08-29', '18:21:12', 1),
(8, '2024-08-29', '18:22:44', 1),
(9, '2024-08-29', '18:22:47', 1),
(10, '2024-08-29', '18:29:17', 1),
(11, '2024-08-29', '18:29:35', 1),
(12, '2024-08-29', '18:29:47', 1),
(13, '2024-08-29', '18:29:50', 1),
(14, '2024-09-02', '20:49:21', 1),
(15, '2024-09-02', '20:49:34', 1),
(16, '2024-09-02', '20:51:47', 1),
(17, '2024-09-02', '20:51:51', 1),
(18, '2024-09-02', '21:42:16', 2),
(19, '2024-09-02', '21:42:21', 2),
(20, '2024-09-02', '21:42:24', 2),
(21, '2024-09-02', '21:42:29', 2),
(22, '2024-09-02', '21:48:22', 2),
(23, '2024-09-02', '21:52:17', 1),
(24, '2024-09-02', '21:59:47', 2),
(25, '2024-09-02', '22:01:37', 2),
(26, '2024-09-02', '22:01:43', 1),
(27, '2024-09-02', '22:12:03', 2),
(28, '2024-09-02', '22:12:09', 1),
(29, '2024-09-02', '22:13:16', 2),
(30, '2024-09-02', '22:13:19', 1),
(31, '2024-09-02', '22:15:10', 2),
(32, '2024-09-02', '22:19:56', 2),
(33, '2024-09-02', '22:20:04', 1),
(34, '2024-09-02', '22:20:08', 1),
(35, '2024-09-02', '22:20:21', 1),
(36, '2024-09-02', '22:22:03', 2),
(37, '2024-09-02', '22:24:15', 2),
(38, '2024-09-02', '22:24:24', 2),
(39, '2024-09-02', '22:33:00', 2),
(40, '2024-09-02', '22:33:06', 2),
(41, '2024-09-02', '22:33:12', 2),
(42, '2024-09-02', '22:33:21', 1),
(43, '2024-09-02', '22:34:25', 2),
(44, '2024-09-02', '22:34:28', 1),
(45, '2024-09-02', '22:34:33', 1),
(46, '2024-09-02', '22:34:37', 2),
(47, '2024-09-02', '22:35:31', 1),
(48, '2024-09-02', '22:36:15', 2),
(49, '2024-09-02', '22:36:18', 1),
(50, '2024-09-02', '22:36:24', 2),
(51, '2024-09-02', '22:36:27', 1),
(52, '2024-09-02', '22:36:31', 1),
(53, '2024-09-02', '22:36:35', 1),
(54, '2024-09-02', '22:36:39', 1),
(55, '2024-09-02', '22:36:42', 2),
(56, '2024-09-02', '22:38:29', 2),
(57, '2024-09-02', '22:38:33', 2),
(58, '2024-09-02', '22:38:37', 2),
(59, '2024-09-02', '22:38:41', 2),
(60, '2024-09-02', '22:38:44', 2),
(61, '2024-09-02', '22:38:48', 1),
(62, '2024-09-02', '22:38:56', 1),
(63, '2024-09-02', '22:43:45', 1),
(64, '2024-09-02', '22:43:48', 1),
(65, '2024-09-02', '22:43:51', 1),
(66, '2024-09-02', '22:43:54', 2),
(67, '2024-09-02', '22:43:57', 2),
(68, '2024-09-02', '22:44:00', 2);

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `ID` int(11) NOT NULL,
  `firstName` varchar(30) NOT NULL,
  `secondName` varchar(30) NOT NULL,
  `CardNum` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`ID`, `firstName`, `secondName`, `CardNum`) VALUES
(1, 'Ollie', 'Mills', '10 7B 24 AC'),
(2, 'Joey', 'Tinfina', 'F3 7A 9C 92');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `timeofscan`
--
ALTER TABLE `timeofscan`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `timeofscan`
--
ALTER TABLE `timeofscan`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=69;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
