USE [master]
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DHBW-Experts-database')
BEGIN
  CREATE DATABASE [DHBW-Experts-database];
END;
GO