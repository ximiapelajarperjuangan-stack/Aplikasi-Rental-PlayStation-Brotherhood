-- phpMyAdmin SQL Dump
-- version 4.7.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 12, 2026 at 11:25 AM
-- Server version: 10.1.22-MariaDB
-- PHP Version: 7.0.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `rental_playstation`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_meja`
--

CREATE TABLE `tbl_meja` (
  `meja_id` int(20) NOT NULL,
  `ps_id` int(20) NOT NULL,
  `status` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_meja`
--

INSERT INTO `tbl_meja` (`meja_id`, `ps_id`, `status`) VALUES
(1, 1, 'Tersedia'),
(2, 2, 'Error'),
(3, 3, 'Tersedia'),
(4, 4, 'Tersedia'),
(5, 5, 'Tersedia'),
(6, 6, 'Tersedia'),
(7, 7, 'Tersedia'),
(8, 8, 'Tersedia'),
(9, 9, 'Tersedia'),
(10, 10, 'Tersedia');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_pembayaran`
--

CREATE TABLE `tbl_pembayaran` (
  `pembayaran_id` int(20) NOT NULL,
  `transaksi_id` varchar(20) NOT NULL,
  `waktu_bayar` datetime NOT NULL,
  `metode_pembayaran` varchar(10) NOT NULL,
  `jumlah_bayar` double NOT NULL,
  `kembalian` double NOT NULL,
  `status_bayar` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_playstation`
--

CREATE TABLE `tbl_playstation` (
  `ps_id` int(20) NOT NULL,
  `jenis_ps` varchar(10) NOT NULL,
  `tarif_per_jam` double NOT NULL,
  `status` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_playstation`
--

INSERT INTO `tbl_playstation` (`ps_id`, `jenis_ps`, `tarif_per_jam`, `status`) VALUES
(1, 'PS 3', 6000, 'Tersedia'),
(2, 'PS 3', 6000, 'Tersedia'),
(3, 'PS 3', 6000, 'Tersedia'),
(4, 'PS 3', 6000, 'Tersedia'),
(5, 'PS 3', 6000, 'Tersedia'),
(6, 'PS 4', 10000, 'Tersedia'),
(7, 'PS 4', 10000, 'Tersedia'),
(8, 'PS 4', 10000, 'Tersedia'),
(9, 'PS 4', 10000, 'Tersedia'),
(10, 'PS 5', 20000, 'Tersedia');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_transaksi`
--

CREATE TABLE `tbl_transaksi` (
  `transaksi_id` varchar(20) NOT NULL,
  `user_id` int(20) DEFAULT NULL,
  `meja_id` int(20) NOT NULL,
  `nama_pelanggan` varchar(20) NOT NULL,
  `jam_mulai` datetime NOT NULL,
  `jam_selesai` datetime NOT NULL,
  `durasi` varchar(20) NOT NULL,
  `total_biaya` double NOT NULL,
  `status` varchar(20) NOT NULL,
  `tanggal` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_transaksi`
--

INSERT INTO `tbl_transaksi` (`transaksi_id`, `user_id`, `meja_id`, `nama_pelanggan`, `jam_mulai`, `jam_selesai`, `durasi`, `total_biaya`, `status`, `tanggal`) VALUES
('TRX20260604001', 1, 3, 'prilly', '2026-04-06 14:48:00', '2026-04-06 15:48:00', '1.00000000000000000e', 6000, 'Selesai', '2026-06-04'),
('TRX20260604002', 1, 3, 'grace', '2026-04-06 15:06:00', '2026-04-06 16:06:00', '1.00000000000000000e', 6000, 'Selesai', '2026-06-04'),
('TRX20260605001', 1, 3, 'Grace', '2026-05-06 10:55:00', '2026-05-06 11:55:00', '1.00000000000000000e', 6000, 'Selesai', '2026-06-05'),
('TRX20260605002', 1, 7, 'Reza', '2026-05-06 16:36:00', '2026-05-06 18:36:00', '2', 20000, 'Selesai', '2026-06-05'),
('TRX20260606001', 1, 5, 'Surya', '2026-06-06 10:00:00', '2026-06-06 11:00:00', '1', 6000, 'Selesai', '2026-06-06'),
('TRX20260608001', 1, 1, 'Boni', '2026-06-08 08:19:00', '2026-06-08 11:19:00', '3', 18000, 'Selesai', '2026-06-08'),
('TRX20260608002', 1, 4, 'Nabil', '2026-06-08 11:42:00', '2026-06-08 12:42:00', '1', 6000, 'Selesai', '2026-06-08'),
('TRX20260608003', 1, 4, 'Grace', '2026-06-08 12:54:00', '2026-06-08 14:54:00', '2', 12000, 'Selesai', '2026-06-08'),
('TRX20260608004', 1, 5, 'Prilly', '2026-06-08 12:54:00', '2026-06-08 14:54:00', '2', 12000, 'Selesai', '2026-06-08'),
('TRX20260608005', 1, 6, 'Prilly', '2026-06-08 12:10:00', '2026-06-08 13:10:00', '1', 10000, 'Selesai', '2026-06-08'),
('TRX20260612001', 1, 10, 'Prilly', '2026-06-12 06:39:00', '2026-06-12 07:39:00', '1', 20000, 'Selesai', '2026-06-12'),
('TRX20260612003', 1, 5, 'Nikita', '2026-06-12 06:43:00', '2026-06-12 08:43:00', '2', 12000, 'Selesai', '2026-06-12'),
('TRX20260612004', 1, 7, 'Boni', '2026-06-12 08:43:00', '2026-06-12 10:43:00', '2', 20000, 'Selesai', '2026-06-12');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_transaksi_detail`
--

CREATE TABLE `tbl_transaksi_detail` (
  `detail_id` int(20) NOT NULL,
  `transaksi_id` varchar(20) NOT NULL,
  `ps_id` int(20) NOT NULL,
  `jumlah_jam` varchar(10) NOT NULL,
  `tarif_per_jam` double NOT NULL,
  `sub_total` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_user`
--

CREATE TABLE `tbl_user` (
  `user_id` int(20) NOT NULL,
  `nama` varchar(20) NOT NULL,
  `password` varchar(10) NOT NULL,
  `no_hp` varchar(20) NOT NULL,
  `role` varchar(20) NOT NULL,
  `foto` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbl_user`
--

INSERT INTO `tbl_user` (`user_id`, `nama`, `password`, `no_hp`, `role`, `foto`) VALUES
(1050905, 'Grace Sitohang', 'grace123', '082276458480', 'Admin', 'D:\\Akuntansi Keuangan Publik\\Pas Foto Kak Prilly.jpeg'),
(2010706, 'Prilly Sinaga', 'prilly123', '082284458135', 'Admin', 'D:\\piccc\\Screenshots\\Screenshot 2026-01-12 213618.png');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_meja`
--
ALTER TABLE `tbl_meja`
  ADD PRIMARY KEY (`meja_id`),
  ADD KEY `ps_id` (`ps_id`);

--
-- Indexes for table `tbl_pembayaran`
--
ALTER TABLE `tbl_pembayaran`
  ADD PRIMARY KEY (`pembayaran_id`),
  ADD KEY `transaksi_id` (`transaksi_id`);

--
-- Indexes for table `tbl_playstation`
--
ALTER TABLE `tbl_playstation`
  ADD PRIMARY KEY (`ps_id`);

--
-- Indexes for table `tbl_transaksi`
--
ALTER TABLE `tbl_transaksi`
  ADD PRIMARY KEY (`transaksi_id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `meja_id` (`meja_id`);

--
-- Indexes for table `tbl_transaksi_detail`
--
ALTER TABLE `tbl_transaksi_detail`
  ADD PRIMARY KEY (`detail_id`),
  ADD KEY `transaksi_id` (`transaksi_id`),
  ADD KEY `ps_id` (`ps_id`);

--
-- Indexes for table `tbl_user`
--
ALTER TABLE `tbl_user`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_meja`
--
ALTER TABLE `tbl_meja`
  MODIFY `meja_id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT for table `tbl_pembayaran`
--
ALTER TABLE `tbl_pembayaran`
  MODIFY `pembayaran_id` int(20) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `tbl_playstation`
--
ALTER TABLE `tbl_playstation`
  MODIFY `ps_id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT for table `tbl_transaksi_detail`
--
ALTER TABLE `tbl_transaksi_detail`
  MODIFY `detail_id` int(20) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `tbl_user`
--
ALTER TABLE `tbl_user`
  MODIFY `user_id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2010707;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_meja`
--
ALTER TABLE `tbl_meja`
  ADD CONSTRAINT `tbl_meja_ibfk_1` FOREIGN KEY (`ps_id`) REFERENCES `tbl_playstation` (`ps_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_pembayaran`
--
ALTER TABLE `tbl_pembayaran`
  ADD CONSTRAINT `tbl_pembayaran_ibfk_1` FOREIGN KEY (`transaksi_id`) REFERENCES `tbl_transaksi` (`transaksi_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_transaksi`
--
ALTER TABLE `tbl_transaksi`
  ADD CONSTRAINT `tbl_transaksi_ibfk_2` FOREIGN KEY (`meja_id`) REFERENCES `tbl_meja` (`meja_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_transaksi_detail`
--
ALTER TABLE `tbl_transaksi_detail`
  ADD CONSTRAINT `tbl_transaksi_detail_ibfk_1` FOREIGN KEY (`ps_id`) REFERENCES `tbl_playstation` (`ps_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_transaksi_detail_ibfk_2` FOREIGN KEY (`transaksi_id`) REFERENCES `tbl_transaksi` (`transaksi_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
