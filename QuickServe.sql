-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: localhost
-- Létrehozás ideje: 2024. Máj 07. 17:22
-- Kiszolgáló verziója: 10.6.16-MariaDB-0ubuntu0.22.04.1
-- PHP verzió: 8.1.2-1ubuntu2.14

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `QuickServe`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `Rendeles`
--

CREATE TABLE `Rendeles` (
  `r_ID` int(11) NOT NULL,
  `v_ID` int(11) NOT NULL DEFAULT 1,
  `v_Etel` varchar(15) DEFAULT NULL,
  `v_Etelar` int(11) DEFAULT NULL,
  `v_Datum` datetime DEFAULT current_timestamp(),
  `v_Allapot` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `Vendeg`
--

CREATE TABLE `Vendeg` (
  `v_ID` int(11) NOT NULL,
  `v_Asztalszam` int(11) DEFAULT NULL,
  `v_Nev` varchar(25) DEFAULT NULL,
  `v_Email` varchar(25) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `Vendeg`
--

INSERT INTO `Vendeg` (`v_ID`, `v_Asztalszam`, `v_Nev`, `v_Email`) VALUES
(1, 1, 'Teszt User', 'teszt@email.com');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `Rendeles`
--
ALTER TABLE `Rendeles`
  ADD PRIMARY KEY (`r_ID`),
  ADD KEY `vendegIDCheck` (`v_ID`);

--
-- A tábla indexei `Vendeg`
--
ALTER TABLE `Vendeg`
  ADD PRIMARY KEY (`v_ID`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `Rendeles`
--
ALTER TABLE `Rendeles`
  MODIFY `r_ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `Vendeg`
--
ALTER TABLE `Vendeg`
  MODIFY `v_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `Rendeles`
--
ALTER TABLE `Rendeles`
  ADD CONSTRAINT `vendegIDCheck` FOREIGN KEY (`v_ID`) REFERENCES `Vendeg` (`v_ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
